using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Random=UnityEngine.Random;

public class ShadowPolygon : MonoBehaviour
{

    private Mesh _msh;
    private MeshFilter _filter;

    public List<Vector2> InsertedPoints;
    public Vector2[] Vertices2D;
    private Triangulator _triangulator;
    public List<List<Vector2>> TransformationMatrices;
    private float camHeight;
    private float camWidth;
    private int DecimalCases = 0;
    private bool poligonDrawn;

    private Camera c;

    void Start()
    {
        c = GameObject.Find("PolygonGame").transform.FindChild("Camera").gameObject.GetComponent<Camera>();

        camHeight = c.orthographicSize;
        camWidth = c.aspect * camHeight;

        InsertedPoints = new List<Vector2>();
        TransformationMatrices = new List<List<Vector2>>();

        gameObject.AddComponent(typeof(MeshRenderer));
        gameObject.GetComponent<MeshRenderer>().material = Resources.Load("PolygonGameResources/Materials/ShadowPolygon") as Material;
        //gameObject.GetComponent<MeshRenderer>().material.color = Random.ColorHSV();
        //poligonDrawn = false;

        CreateRandomTriangle();
        DrawPolygon();
    }

    public void Update()
    {
        if(InsertedPoints.Count>=3 && !poligonDrawn)
            DrawPolygon();
    }

    //bool AllPointsAreInside()
    //{
    //    foreach (Vector2 point in InsertedPoints)
    //    {
    //        if (point.x > 90 || point.y > 240 || point.x < 0 || point.y < 0)
    //        {
    //            return false;
    //        }
    //    }
    //    return true;
    //}

    void CreateRandomTriangle()
    {
        List<Vector2> pointList = new List<Vector2>();
        Vector3 newPoint = new Vector3(0, 0, 0);
        newPoint.x = Random.Range(-camWidth, camWidth);
        newPoint.y = Random.Range(-camHeight, camHeight);
        pointList.Add(newPoint);
        if (newPoint.x + 40 < camWidth)
        {
            newPoint.x += 20;
            pointList.Add(newPoint);
            if (newPoint.y + 20 < camHeight)
            {
                newPoint.y += 20;
                pointList.Add(newPoint);
            }
            else
            {
                newPoint.y -= 20;
                pointList.Add(newPoint);
            }
        }
        else
        {
            newPoint.x -= 20;
            pointList.Add(newPoint);
            if (newPoint.y + 20 < camHeight)
            {
                newPoint.y += 20;
                pointList.Add(newPoint);
            }
            else
            {
                newPoint.y -= 20;
                pointList.Add(newPoint);
            }
        }

        for (int i = 0; i < pointList.Count; i++)
        {
            pointList[i] = ProcessPoint(pointList[i]);
            AddPoint(pointList[i]);
        }
    }

    public void ApplyTranslation(float x, float y)
    {
        x = Arredondar(x, DecimalCases);
        y = Arredondar(y, DecimalCases);
        for (int i = 0; i < InsertedPoints.Count; i++)
        {
            Vector2 newpoint = InsertedPoints[i];
            newpoint.x += x;
            newpoint.y += y;
            InsertedPoints[i] = newpoint;
        }
        Vertices2D = InsertedPoints.ToArray();
        if (TransformationMatrices.Count != 0)
        {
            //Se a última matriz transformação aplicada tiver mais que um elemento, será uma matriz rotação, logo terá de ser criada uma nova translacção
            if (TransformationMatrices[TransformationMatrices.Count - 1].Count != 1)
            {
                List<Vector2> newList = new List<Vector2>();
                newList.Add(new Vector2(x, y));
                TransformationMatrices.Add(newList);
            }
            else
            {
                Vector2 newVector = new Vector2(TransformationMatrices[TransformationMatrices.Count - 1][0].x, TransformationMatrices[TransformationMatrices.Count - 1][0].y);
                newVector.x += x;
                newVector.y += y;
                TransformationMatrices[TransformationMatrices.Count - 1].Clear();
                TransformationMatrices[TransformationMatrices.Count - 1].Add(newVector);
            }
        }
        else
        {
            TransformationMatrices.Add(new List<Vector2>());
            Vector2 newVector = Vector2.zero;
            newVector.x += x;
            newVector.y += y;
            TransformationMatrices[0].Add(newVector);
        }
    }

    public void ApplyRotation(double givenAngle, bool radians)
    {
        double angle = givenAngle;
        if (radians == false)
        {
            angle = givenAngle * (Math.PI / 180);
        }
        for (int i = 0; i < InsertedPoints.Count; i++)
        {
            Vector2 newPoint = InsertedPoints[i];
            newPoint.x = (float)(Math.Cos(angle) * InsertedPoints[i].x - Math.Sin(angle) * InsertedPoints[i].y);
            newPoint.y = (float)(Math.Sin(angle) * InsertedPoints[i].x + Math.Cos(angle) * InsertedPoints[i].y);
            InsertedPoints[i] = newPoint;
        }
        Vertices2D = InsertedPoints.ToArray();

        //Adicionar a matriz rotação à lista de transformações
        List<Vector2> rotMat = new List<Vector2>();
        rotMat.Add(new Vector2(Arredondar((float)Math.Cos(angle), 2), Arredondar((float)Math.Sin(angle), 2)));
        rotMat.Add(new Vector2(Arredondar((float)-Math.Sin(angle), 2), Arredondar((float)Math.Cos(angle), 2)));
        TransformationMatrices.Add(rotMat);

        Debug.Log(TransformationMatrices.Count);
    }

    public void ApplyScaling(float scale)
    {
        for (int i = 0; i < InsertedPoints.Count; i++)
        {
            InsertedPoints[i] *= scale;
        }
    }

    public void DrawPolygon()
    {
        Vertices2D = InsertedPoints.ToArray();
        _triangulator = new Triangulator(Vertices2D);

        int[] indices = _triangulator.Triangulate();

        Vector3[] vertices = new Vector3[Vertices2D.Length];

        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = new Vector3(Vertices2D[i].x, Vertices2D[i].y, 140);
        }

        _msh = new Mesh();
        _msh.vertices = vertices;
        _msh.triangles = indices;
        _msh.RecalculateNormals();
        _msh.RecalculateBounds();

        _filter = gameObject.AddComponent(typeof(MeshFilter)) as MeshFilter;
        _filter.mesh = _msh;
        poligonDrawn = true;
    }

    public void AddPoint(Vector3 input)
    {
        Vector2 SelectedPoint = new Vector2(input.x, input.y);
        InsertedPoints.Add(SelectedPoint);
    }

    private Vector3 ProcessPoint(Vector3 p)
    {
        p.x = Mathf.RoundToInt(p.x);
        p.x *= 0.1f;
        p.x = Mathf.RoundToInt(p.x);
        p.x *= 10f;

        p.y = Mathf.RoundToInt(p.y);
        p.y *= 0.1f;
        p.y = Mathf.RoundToInt(p.y);
        p.y *= 10f;
        return p;
    }

    static float Arredondar(float num, int numero_casas)
    {
        return (float)(((int)(num * Math.Pow(10, numero_casas)) * Math.Pow(0.1f, numero_casas)));
    }
}
