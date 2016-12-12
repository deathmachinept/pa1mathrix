using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class PolygonHandler : MonoBehaviour
{
    public List<GameObject> _shadowPolygonList;
    public List<GameObject> _userPolygonList;
    public GameObject _currentlySelectedPolygon;
    public GameObject StaticPolygons;
    public GameObject UserPolygons;

    public bool figureCreated;
    private float camHeight;
    private float camWidth;

    private Camera c;

    private Vector2 A1;
    private Vector2 A2;
    private Vector2 A3;

    private Vector2 B1;
    private Vector2 B2;
    private Vector2 B3;

    private Vector2 C1;
    private Vector2 C2;
    private Vector2 C3;

    private Vector2 D1;
    private Vector2 D2;
    private Vector2 D3;

    private Vector2 E1;
    private Vector2 E2;
    private Vector2 E3;

    private Vector2 F1;
    private Vector2 F2;
    private Vector2 F3;

    private Vector2 G1;
    private Vector2 G2;
    private Vector2 G3;

    private Vector2 H1;
    private Vector2 H2;
    private Vector2 H3;

    private Vector2 I1;
    private Vector2 I2;
    private Vector2 I3;

    void Start()
    {
        _shadowPolygonList = new List<GameObject>();
        _userPolygonList = new List<GameObject>();
        StaticPolygons = new GameObject();
        UserPolygons = new GameObject();
        StaticPolygons.name = "Static Polygons";
        UserPolygons.name = "User Polygons";

        StaticPolygons.transform.SetParent(transform);
        UserPolygons.transform.SetParent(transform);
        figureCreated = false;

        AddControllablePolygon();
        AddControllablePolygon();
        _currentlySelectedPolygon = UserPolygons.transform.GetChild(0).gameObject;

        c = GameObject.Find("PolygonGame").transform.FindChild("Camera").gameObject.GetComponent<Camera>();
        camHeight = c.orthographicSize;
        camWidth = c.aspect * camHeight;

        AddPolygon();

        //for (int i = 0; i < 9; i++)
        //{
        //    AddPolygon();
        //}

    }

    void InputHandling()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray r = c.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(r, out hit))
            {
                if (hit.transform.parent.transform.name == "User Polygons")
                {
                    _currentlySelectedPolygon = hit.transform.gameObject;
                    foreach (GameObject g in _userPolygonList)
                    {
                        if(g!=_currentlySelectedPolygon)
                            g.GetComponent<Polygon>().IsSelected = false;
                        else
                        {
                            g.GetComponent<Polygon>().IsSelected = false;
                        }
                    }
                }
            }
        }
    }

    void Update()
    {
        _currentlySelectedPolygon.GetComponent<Polygon>().IsSelected = true;
        if (_currentlySelectedPolygon != null)
        {
            if (ShadowGame())
            {
                Debug.Log("You're Winner!");
            }
        }
    }

    void SimplePolygon()
    {
        if (StaticPolygons.transform.GetChild(0).gameObject.GetComponent<Polygon>().hasInitialized)
        {
            StaticPolygons.transform.GetChild(0).gameObject.GetComponent<Polygon>().Replace_Points(new Vector3(0, 0, 140), new Vector3(0, 30, 140), new Vector3(30, 0, 140));
        }
    }

    void RabbitFigure()
    {
        if (StaticPolygons.transform.GetChild(0).gameObject.GetComponent<Polygon>().hasInitialized)
        {
            StaticPolygons.transform.GetChild(0).gameObject.GetComponent<Polygon>().Replace_Points(new Vector3(0, -10, 140), new Vector3(0, 20, 140), new Vector3(30, -10, 140));
        }
        if (StaticPolygons.transform.GetChild(1).gameObject.GetComponent<Polygon>().hasInitialized)
        {
            StaticPolygons.transform.GetChild(1).gameObject.GetComponent<Polygon>().Replace_Points(new Vector3(0, 0, 140), new Vector3(-10, -10, 140), new Vector3(0, -20, 140));
        }
        if (StaticPolygons.transform.GetChild(2).gameObject.GetComponent<Polygon>().hasInitialized)
        {
            StaticPolygons.transform.GetChild(2).gameObject.GetComponent<Polygon>().Replace_Points(new Vector3(0, -10, 140), new Vector3(30, -10, 140), new Vector3(30, -40, 140));
        }

        if (StaticPolygons.transform.GetChild(3).gameObject.GetComponent<Polygon>().hasInitialized)
        {
            StaticPolygons.transform.GetChild(3).gameObject.GetComponent<Polygon>().Replace_Points(new Vector3(10, -20, 140), new Vector3(0, -30, 140), new Vector3(10, -40, 140));
        }
        if (StaticPolygons.transform.GetChild(4).gameObject.GetComponent<Polygon>().hasInitialized)
        {
            StaticPolygons.transform.GetChild(4).gameObject.GetComponent<Polygon>().Replace_Points(new Vector3(10, -20, 140), new Vector3(10, -40, 140), new Vector3(30, -40, 140));
        }
        if (StaticPolygons.transform.GetChild(5).gameObject.GetComponent<Polygon>().hasInitialized)
        {
            StaticPolygons.transform.GetChild(5).gameObject.GetComponent<Polygon>().Replace_Points(new Vector3(0, 20, 140), new Vector3(-20, 20, 140), new Vector3(-20, 40, 140));
        }

        if (StaticPolygons.transform.GetChild(6).gameObject.GetComponent<Polygon>().hasInitialized)
        {
            StaticPolygons.transform.GetChild(6).gameObject.GetComponent<Polygon>().Replace_Points(new Vector3(0, 20, 140), new Vector3(-20, 40, 140), new Vector3(0, 40, 140));
        }
        if (StaticPolygons.transform.GetChild(7).gameObject.GetComponent<Polygon>().hasInitialized)
        {
            StaticPolygons.transform.GetChild(7).gameObject.GetComponent<Polygon>().Replace_Points(new Vector3(0, 40, 140), new Vector3(-10, 40, 140), new Vector3(0, 60, 140));
        }
        if (StaticPolygons.transform.GetChild(8).gameObject.GetComponent<Polygon>().hasInitialized)
        {
            StaticPolygons.transform.GetChild(8).gameObject.GetComponent<Polygon>().Replace_Points(new Vector3(0, 40, 140), new Vector3(0, 60, 140), new Vector3(10, 60, 140));
        }

        UserPolygons.transform.GetChild(0).gameObject.GetComponent<Polygon>().Replace_Points(A1,A2,A3);
        UserPolygons.transform.GetChild(1).gameObject.GetComponent<Polygon>().Replace_Points(B1,B2,B3);
        UserPolygons.transform.GetChild(2).gameObject.GetComponent<Polygon>().Replace_Points(C1,C2,C3);
        UserPolygons.transform.GetChild(3).gameObject.GetComponent<Polygon>().Replace_Points(D1,D2,D3);
        UserPolygons.transform.GetChild(4).gameObject.GetComponent<Polygon>().Replace_Points(E1,E2,E3);
        UserPolygons.transform.GetChild(5).gameObject.GetComponent<Polygon>().Replace_Points(F1,F2,F3);
        UserPolygons.transform.GetChild(6).gameObject.GetComponent<Polygon>().Replace_Points(G1,G2,G3);
        UserPolygons.transform.GetChild(7).gameObject.GetComponent<Polygon>().Replace_Points(H1,H2,H3);
        UserPolygons.transform.GetChild(8).gameObject.GetComponent<Polygon>().Replace_Points(I1,I2,I3);

    }

    void AddPolygon()
    {
        GameObject poly =
            Instantiate(Resources.Load("PolygonGameResources/Prefabs/StaticPolygon"), Vector3.zero, Quaternion.identity) as GameObject;
        poly.transform.SetParent(StaticPolygons.transform);
        poly.transform.name = "ShadowPoly " + _shadowPolygonList.Count;
        _shadowPolygonList.Add(poly);
    }

    void AddControllablePolygon()
    {
        GameObject poly =
    Instantiate(Resources.Load("PolygonGameResources/Prefabs/UserPolygon"), Vector3.zero, Quaternion.identity) as GameObject;
        poly.transform.SetParent(UserPolygons.transform);
        poly.transform.name = "UserPoly " + _userPolygonList.Count;
        _userPolygonList.Add(poly);
    }

    public void ApplyTranslationToCurrentPolygon()
    {
        Vector3 translationValues =
            GameObject.Find("TranslationMenu").GetComponent<TranslationMenuController>().GiveTranslationValues();
        _currentlySelectedPolygon.GetComponent<Polygon>().ApplyTranslation(translationValues.x,translationValues.y);
        GameObject.Find("OperationButtonsHolder").GetComponent<OperationsMenuController>().Reset();
    }

    public void ApplyRotationToCurrentPolygon()
    {
        double angle = GameObject.Find("RotationMenu").GetComponent<RotationMenuController>().GiveRotationAngle();
        _currentlySelectedPolygon.GetComponent<Polygon>().ApplyRotation(angle,false);
        GameObject.Find("OperationButtonsHolder").GetComponent<OperationsMenuController>().Reset();
    }

    public void ApplyScalingToCurrentPolygon()
    {
        float scale = GameObject.Find("ScalingMenu").GetComponent<ScalingMenuController>().GiveScale();
        _currentlySelectedPolygon.GetComponent<Polygon>().ApplyScaling(scale);
        GameObject.Find("OperationButtonsHolder").GetComponent<OperationsMenuController>().Reset();
    }

    private bool ShadowGame()
    {
        bool allPointsCovered = true;
        foreach (GameObject staticPolygon in _shadowPolygonList)
        {
            foreach (Vector2 shadowPoint in staticPolygon.GetComponent<Polygon>().InsertedPoints)
            {
                bool foundMatch = false;
                foreach (Vector2 userPolygon in _currentlySelectedPolygon.GetComponent<Polygon>().InsertedPoints)
                {
                    if (shadowPoint == userPolygon)
                    {
                        foundMatch = true;
                    }
                }
                if (!foundMatch)
                    allPointsCovered = false;
            }
        }
        if (allPointsCovered)
            return true;
        else
        {
            return false;
        }
    }
}
