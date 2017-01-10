using System;
using UnityEngine;
using System.Collections;
using System.Threading;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Xml.Serialization;
using UnityEngine.EventSystems;

public class Polygon : MonoBehaviour
{
    public Mesh _msh;
    private MeshFilter _filter;

    public int DecimalCases = 0;
    public double rotationAngle = 90;
    public float scale = 1.5f;
    public List<List<Vector2>> TransformationMatrices;
    public List<Vector2> InsertedPoints;
    public Vector2[] Vertices2D;
    public Material SelectedMaterial;
    private Triangulator _triangulator;
    private int _previousPointCount;

    public bool Interactable;
    public bool hasInitialized;
    public bool IsSelected;

    private float camHeight;
    private float camWidth;

    private Camera c;

    private bool addedPoint = false;

    void Awake()
    {
        c = GameObject.Find("PolygonGame").transform.FindChild("Camera").gameObject.GetComponent<Camera>();
        camHeight = c.orthographicSize;
        camWidth = c.aspect * camHeight;
        _previousPointCount = 0;

        InsertedPoints = new List<Vector2>();
        TransformationMatrices = new List<List<Vector2>>();
        hasInitialized = false;
        IsSelected = false;
    }



    void Update()
    {
        if (hasInitialized)
        {
            if (Interactable && IsSelected)
            {
                InputHandler();
            }
            if (InsertedPoints.Count != _previousPointCount && InsertedPoints.Count >= 3)
            {
                _previousPointCount = InsertedPoints.Count;
                UpdatePolygon();
            }
        }
        else
        {
            if (InsertedPoints.Count>=3)
                InitializePolygon();
        }
    }

    public void InitializePolygon()
    {
        if (Interactable)
        {
            SelectedMaterial = Resources.Load("PolygonGameResources/Materials/UserPolygonMaterial") as Material;
        }
        else
        {
            SelectedMaterial = Resources.Load("PolygonGameResources/Materials/ShadowPolygon") as Material;
        }
        gameObject.AddComponent(typeof(MeshRenderer));
        gameObject.GetComponent<MeshRenderer>().material = SelectedMaterial;
        gameObject.AddComponent<MeshCollider>();
        gameObject.GetComponent<MeshCollider>().sharedMesh = _msh;
        _filter = gameObject.AddComponent(typeof(MeshFilter)) as MeshFilter;
        //_filter.mesh = _msh;
        hasInitialized = true;
    }

    public void AddPoint(Vector3 input)
    {
        Vector2 SelectedPoint = new Vector2(Arredondar(input.x, DecimalCases), Arredondar(input.y, DecimalCases));
        InsertedPoints.Add(SelectedPoint);
    }

    public void CreateRandomTriangle()
    {
        List<Vector2> pointList = new List<Vector2>();
        Vector3 newPoint = new Vector3(0, 0, 0);
        newPoint.x = UnityEngine.Random.Range(-camWidth, camWidth);
        newPoint.y = UnityEngine.Random.Range(-camHeight, camHeight);
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

    private void InputHandler()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 _input = new Vector3(c.ScreenToWorldPoint(Input.mousePosition).x, c.ScreenToWorldPoint(Input.mousePosition).y, 140);
            Ray r = c.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            if (Physics.Raycast(r, out hit))
            {
                if (hit.collider.transform.parent.transform.name == "GridDotsHolder")
                {
                    _input = new Vector3(hit.collider.transform.position.x, hit.collider.transform.position.y,
                        hit.collider.transform.position.z);
                    AddPoint(_input);
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            ApplyTranslation(-10f * (float)Math.Pow(0.1f, DecimalCases), 0);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            ApplyTranslation(10f * (float)Math.Pow(0.1f, DecimalCases), 0);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            ApplyTranslation(0, 10f * (float)Math.Pow(0.1f, DecimalCases));
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            ApplyTranslation(0, -10f * (float)Math.Pow(0.1f, DecimalCases));
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            ApplyRotation(-rotationAngle, false);
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ApplyRotation(+rotationAngle, false);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            ApplyScaling(scale);
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            ApplyScaling(Mathf.Pow(scale,-1));
        }
    }

    private Vector3 ProcessPoint(Vector3 p)
    {
        p.x=Mathf.RoundToInt(p.x);
        p.x *= 0.1f;
        p.x = Mathf.RoundToInt(p.x);
        p.x *= 10f;

        p.y = Mathf.RoundToInt(p.y);
        p.y *= 0.1f;
        p.y = Mathf.RoundToInt(p.y);
        p.y *= 10f;
        return p;
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
        UpdatePolygon();

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
        UpdatePolygon();

        //Adicionar a matriz rotação à lista de transformações
        List<Vector2> rotMat = new List<Vector2>();
        rotMat.Add(new Vector2(Arredondar((float)Math.Cos(angle), 2), Arredondar((float)Math.Sin(angle), 2)));
        rotMat.Add(new Vector2(Arredondar((float)-Math.Sin(angle), 2), Arredondar((float)Math.Cos(angle), 2)));
        TransformationMatrices.Add(rotMat);
    }

    public void ApplyScaling(float scale)
    {
        for (int i = 0; i < InsertedPoints.Count; i++)
        {
            if(InsertedPoints[i].x!=0 || InsertedPoints[i].y != 0)
                InsertedPoints[i] *= scale;

            InsertedPoints[i] = ProcessPoint(InsertedPoints[i]);
        }
        UpdatePolygon();
    }

    public void UpdatePolygon()
    {
        Vertices2D = null;
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

        if (GetComponent<MeshCollider>() == null)
        {
            gameObject.AddComponent<MeshCollider>();
        }
        else
        {
            gameObject.GetComponent<MeshCollider>().sharedMesh = _msh;
        }
        if (_filter != null)
        {
            _filter.mesh = _msh;
        }
    }

    public void Replace_Points(Vector2 point1, Vector2 point2, Vector2 point3)
    {
        InsertedPoints[0] = point1;
        InsertedPoints[1] = point2;
        InsertedPoints[2] = point3;
        UpdatePolygon();
    }

    static float Arredondar(float num, int numero_casas)
    {
          return (float)(((int)(num * Math.Pow(10, numero_casas)) * Math.Pow(0.1f, numero_casas)));
    }
}
