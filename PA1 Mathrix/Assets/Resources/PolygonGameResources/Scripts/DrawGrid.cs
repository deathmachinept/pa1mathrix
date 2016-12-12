using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DrawGrid : MonoBehaviour
{

    public int tileNumber = 5;
    public int spacing = 40;

    private Camera c;

    void Start()
    {
        c = GameObject.Find("PolygonGame").transform.FindChild("Camera").gameObject.GetComponent<Camera>();
        DrawPoints();
    }

    void DrawPoints()
    {
        GameObject GridDotsHolder = new GameObject("GridDotsHolder");
        GridDotsHolder.transform.SetParent(GameObject.Find("PolygonGame").transform);

        for (int x = -50; x < 50; x++)
        {
            for (int y = -50; y < 50; y++)
            {
                if (PointIsWithinScreen(new Vector3(x * 10, y * 10, 140)))
                {
                    GameObject point = GameObject.CreatePrimitive(PrimitiveType.Quad);
                    point.transform.position = new Vector3(x*10, y*10, 140);
                    if (x == 0 || y == 0)
                    {
                        if (x == 0 && y == 0)
                            point.transform.localScale *= 5;
                        else
                        {
                            point.transform.localScale *= 3;
                        }
                        point.GetComponent<MeshRenderer>().material = Resources.Load("PolygonGameResources/Materials/MainPoints") as Material;
                    }
                    else
                    {
                        point.transform.localScale *= 3;
                        point.GetComponent<MeshRenderer>().material = Resources.Load("PolygonGameResources/Materials/Points") as Material;
                    }
                    point.transform.SetParent(GridDotsHolder.transform);
                }
            }

        }
    }

    private bool PointIsWithinScreen(Vector3 p)
    {
        if (c.pixelRect.Contains(c.WorldToScreenPoint(p)))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
