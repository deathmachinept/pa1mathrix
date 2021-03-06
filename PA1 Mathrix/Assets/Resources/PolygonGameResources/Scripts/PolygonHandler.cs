﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

public class PolygonHandler : MonoBehaviour
{
    public GameObject ShadowPolygonHolder;
    public GameObject UserPolygonHolder;
    public GameObject Timer;

    public GameObject TranslationValueA;
    public GameObject TranslationValueB;
    public GameObject RotationValue;


    public Polygon CurrentlySelectedPolygon;

    public int SelectedFigure;
    public float time;
    public int PreviouslySelectedFigure;
    public bool IsPointInputEnabled;

    void Awake()
    {
        TranslationValueA =
            GameObject.Find("TranslationValuesHolder").transform.FindChild("Input").FindChild("ValueA").gameObject;
        TranslationValueB =
            GameObject.Find("TranslationValuesHolder").transform.FindChild("Input").FindChild("ValueB").gameObject;
        RotationValue = GameObject.Find("Angle");

        GameObject.Find("TranslationMenu").SetActive(false);
        GameObject.Find("RotationMenu").SetActive(false);

        ShadowPolygonHolder = new GameObject("Shadow Polygon Holder");
        ShadowPolygonHolder.transform.SetParent(transform);

        UserPolygonHolder = new GameObject("User Polygon Holder");
        UserPolygonHolder.transform.SetParent(transform);
        UserPolygonHolder.transform.position=new Vector3(UserPolygonHolder.transform.position.x, UserPolygonHolder.transform.position.y, UserPolygonHolder.transform.position .z- 1);
        
        Timer=GameObject.Find("Timer");
    }

    void Start()
    {
        IsPointInputEnabled = false;
        SelectedFigure = Random.Range(0, 5);
        PreviouslySelectedFigure = SelectedFigure;
        StartShadowGame();
        time = 100;
    }

    void Update()
    {
        PulsateSelectedPolygon();
        if (time > 0)
        {
            time -= Time.deltaTime;
            Timer.GetComponent<Text>().text = ((int) time).ToString();
            SelectionChecking();
            if (ShadowFigureCheck())
            {
                GameObject.Find("Network Manager").GetComponent<ReturnScript>().ReturnToMainScene("Polygon Game", true);
            }
        }
        else
        {
            GameObject.Find("Network Manager").GetComponent<ReturnScript>().ReturnToMainScene("Polygon Game",false);
        }
    }

    private void PulsateSelectedPolygon()
    {
        CurrentlySelectedPolygon.gameObject.GetComponent<MeshRenderer>().material.color=new Color(CurrentlySelectedPolygon.gameObject.GetComponent<MeshRenderer>().material.color.r, CurrentlySelectedPolygon.gameObject.GetComponent<MeshRenderer>().material.color.g, CurrentlySelectedPolygon.gameObject.GetComponent<MeshRenderer>().material.color.b,Mathf.PingPong(Time.time,1));
        CurrentlySelectedPolygon.UpdatePolygon();
    }

    void StartShadowGame()
    {
        CurrentlySelectedPolygon = null;
        while (ShadowPolygonHolder.transform.childCount > 0)
        {
            Destroy(ShadowPolygonHolder.transform.GetChild(0));
        }
        while (UserPolygonHolder.transform.childCount > 0)
        {
            Destroy(UserPolygonHolder.transform.GetChild(0));
        }

        switch (SelectedFigure)
        {
            case 0:
                DrawRabbit();
                break;
            case 1:
                DrawCat();
                break;
            case 2:
                DrawServant();
                break;
            case 3:
                DrawSwan();
                break;
            case 4:
                DrawHorse();
                break;
        }
        foreach (Transform t in ShadowPolygonHolder.transform)
        {
            CloneShadowToInteractablePolygon(t.GetComponent<Polygon>());
        }
        foreach (Transform t in UserPolygonHolder.transform)
        {
            GenerateUserPolygonTransformations(t.GetComponent<Polygon>());
        }
    }

    bool ShadowFigureCheck()
    {
        //return (AllControllableHaveCorrespondence() && AllShadowsHaveCorrespondence())||AllPointsHaveMatch();
        return AllPointsHaveMatch();
    }

    bool AllShadowsHaveCorrespondence()
    {
        foreach (Transform shadow in ShadowPolygonHolder.transform)
        {
            bool hasPolygonMatch = false;
            foreach (Transform controllable in UserPolygonHolder.transform)
            {
                if (PolygonMatchChecking(shadow.GetComponent<Polygon>(), controllable.GetComponent<Polygon>()))
                {
                    hasPolygonMatch = true;
                }
            }
            if (!hasPolygonMatch)
            {
                return false;
            }
        }
        return true;
    }

    bool AllControllableHaveCorrespondence()
    {
        foreach (Transform controllable in ShadowPolygonHolder.transform)
        {
            bool hasPolygonMatch = false;
            foreach (Transform shadow in UserPolygonHolder.transform)
            {
                if (PolygonMatchChecking(controllable.GetComponent<Polygon>(), shadow.GetComponent<Polygon>()))
                {
                    hasPolygonMatch = true;
                }
            }
            if (!hasPolygonMatch)
            {
                return false;
            }
        }
        return true;
    }

    bool PolygonMatchChecking(Polygon A, Polygon B)
    {
        foreach (Vector2 pointA in A.InsertedPoints)
        {
            bool foundMatch = false;
            foreach (Vector2 pointB in B.InsertedPoints)
            {
                if (pointA == pointB)
                    foundMatch = true;
            }
            if (!foundMatch)
            {
                return false;
            }
        }
        return true;
    }

    bool AllPointsHaveMatch()
    {
        foreach (Transform t in UserPolygonHolder.transform)
        {
            bool foundMatch=false;
            foreach (Vector2 point in t.GetComponent<Polygon>().InsertedPoints)
            {
                foreach (Transform st in ShadowPolygonHolder.transform)
                {
                    foreach (Vector2 spoint in st.GetComponent<Polygon>().InsertedPoints)
                    {
                        if (point == spoint)
                            foundMatch=true;
                    }
                }
            }
            if (!foundMatch)
            {
                return false;
            }
        }
        return true;
    }

    void SelectionChecking()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(r, out hit))
            {
                if (hit.transform.tag == "Polygon")
                {
                    if (hit.transform.GetComponent<Polygon>().Interactable)
                    {
                        if (CurrentlySelectedPolygon != null)
                        {
                            CurrentlySelectedPolygon.IsSelected = false;
                            CurrentlySelectedPolygon.SelectedMaterial= Resources.Load("PolygonGameResources/Materials/UserPolygonMaterial") as Material;
                            StartCoroutine(Smooth.Fade(CurrentlySelectedPolygon.GetComponent<MeshRenderer>().material, 1,2));
                            CurrentlySelectedPolygon.GetComponent<Polygon>().UpdatePolygon();
                        }
                        CurrentlySelectedPolygon = hit.transform.GetComponent<Polygon>();
                        hit.transform.GetComponent<Polygon>().IsSelected = true;
                        CurrentlySelectedPolygon.SelectedMaterial = Resources.Load("PolygonGameResources/Materials/SelectedUserPolygon") as Material;
                    }
                }
            }
        }
    }

    public void TranslateCurrentPolyFromMatrixPoints()
    {
        if (CurrentlySelectedPolygon != null)
        {
            CurrentlySelectedPolygon.GetComponent<Polygon>().ApplyTranslation(int.Parse(TranslationValueA.GetComponent<Text>().text), int.Parse(TranslationValueB.GetComponent<Text>().text));
            GameObject.Find("OperationButtonsHolder").GetComponent<OperationsMenuController>().Reset();
        }
    }

    public void RotateCurrentPolyFromAngleValu()
    {
        if (CurrentlySelectedPolygon != null)
        {
            CurrentlySelectedPolygon.GetComponent<Polygon>().ApplyRotation(int.Parse(RotationValue.GetComponent<Text>().text),false);
            GameObject.Find("OperationButtonsHolder").GetComponent<OperationsMenuController>().Reset();
        }
    }

    void AddShadowPolygon()
    {
        GameObject newPolygon = new GameObject();
        newPolygon.tag = "Polygon";
        newPolygon.transform.SetParent(ShadowPolygonHolder.transform, false);
        newPolygon.name = "Shadow Polygon " + ShadowPolygonHolder.transform.childCount;
        newPolygon.AddComponent<Polygon>();
        newPolygon.GetComponent<Polygon>().CreateRandomTriangle();
        newPolygon.GetComponent<MeshCollider>().sharedMesh = newPolygon.GetComponent<Polygon>()._msh;
    }

    void AddShadowPolygon(Vector2 PointA, Vector2 PointB, Vector2 PointC)
    {
        GameObject newPolygon = new GameObject();
        newPolygon.tag = "Polygon";
        newPolygon.transform.SetParent(ShadowPolygonHolder.transform, false);
        newPolygon.name = "Shadow Polygon " + ShadowPolygonHolder.transform.childCount;
        newPolygon.AddComponent<Polygon>();
        newPolygon.GetComponent<Polygon>().InsertedPoints.Add(PointA);
        newPolygon.GetComponent<Polygon>().InsertedPoints.Add(PointB);
        newPolygon.GetComponent<Polygon>().InsertedPoints.Add(PointC);
    }

    void AddShadowPolygon(Vector2 PointA, Vector2 PointB, Vector2 PointC, Vector2 PointD)
    {
        GameObject newPolygon = new GameObject();
        newPolygon.tag = "Polygon";
        newPolygon.transform.SetParent(ShadowPolygonHolder.transform, false);
        newPolygon.name = "Shadow Polygon " + ShadowPolygonHolder.transform.childCount;
        newPolygon.AddComponent<Polygon>();
        newPolygon.GetComponent<Polygon>().InsertedPoints.Add(PointA);
        newPolygon.GetComponent<Polygon>().InsertedPoints.Add(PointB);
        newPolygon.GetComponent<Polygon>().InsertedPoints.Add(PointC);
        newPolygon.GetComponent<Polygon>().InsertedPoints.Add(PointD);
    }

    void AddControllablePolygon()
    {
        GameObject newPolygon = new GameObject();
        newPolygon.tag = "Polygon";
        newPolygon.transform.SetParent(UserPolygonHolder.transform, false);
        string newPolygonName = "User Polygon " + UserPolygonHolder.transform.childCount;
        newPolygon.name = newPolygonName;
        newPolygon.AddComponent<Polygon>();
        newPolygon.GetComponent<Polygon>().Interactable = true;
        if (UserPolygonHolder.transform.childCount == 1)
        {
            newPolygon.GetComponent<Polygon>().IsSelected = true;
            CurrentlySelectedPolygon = newPolygon.GetComponent<Polygon>();
        }
    }

    void AddControllablePolygon(Vector2 PointA, Vector2 PointB, Vector2 PointC)
    {
        GameObject newPolygon = new GameObject();
        newPolygon.tag = "Polygon";
        newPolygon.transform.SetParent(UserPolygonHolder.transform, false);
        newPolygon.name = "User Polygon " + UserPolygonHolder.transform.childCount;
        newPolygon.AddComponent<Polygon>();
        newPolygon.GetComponent<Polygon>().InsertedPoints.Add(PointA);
        newPolygon.GetComponent<Polygon>().InsertedPoints.Add(PointB);
        newPolygon.GetComponent<Polygon>().InsertedPoints.Add(PointC);
        newPolygon.GetComponent<Polygon>().Interactable = true;
        if (UserPolygonHolder.transform.childCount == 1)
        {
            newPolygon.GetComponent<Polygon>().IsSelected = true;
            CurrentlySelectedPolygon = newPolygon.GetComponent<Polygon>();
        }
    }

    void AddControllablePolygon(Vector2 PointA, Vector2 PointB, Vector2 PointC,Vector2 PointD)
    {
        GameObject newPolygon = new GameObject();
        newPolygon.tag = "Polygon";
        newPolygon.transform.SetParent(UserPolygonHolder.transform, false);
        newPolygon.name = "User Polygon " + UserPolygonHolder.transform.childCount;
        newPolygon.AddComponent<Polygon>();
        newPolygon.GetComponent<Polygon>().InsertedPoints.Add(PointA);
        newPolygon.GetComponent<Polygon>().InsertedPoints.Add(PointB);
        newPolygon.GetComponent<Polygon>().InsertedPoints.Add(PointC);
        newPolygon.GetComponent<Polygon>().InsertedPoints.Add(PointD);
        newPolygon.GetComponent<Polygon>().Interactable = true;
        if (UserPolygonHolder.transform.childCount == 1)
        {
            newPolygon.GetComponent<Polygon>().IsSelected = true;
            CurrentlySelectedPolygon = newPolygon.GetComponent<Polygon>();
        }
    }


    void CloneShadowToInteractablePolygon(Polygon p)
    {
        if (!p.Interactable)
        {
            AddControllablePolygon(p.InsertedPoints[0], p.InsertedPoints[1], p.InsertedPoints[2]);
        }
    }

    void GenerateUserPolygonTransformations(Polygon p)
    {
        Vector2 mainPointPosition = p.InsertedPoints[0];
        Vector2 nullifyingVector = -mainPointPosition;

        p.ApplyTranslation(nullifyingVector.x, nullifyingVector.y);

        if (Random.Range(0, 11) <= 5)
        {
            switch (Random.Range(0,4))
            {
                case 0:
                    p.ApplyRotation(90,false);
                    break;
                case 2:
                    p.ApplyRotation(180, false);
                    break;
                case 3:
                    p.ApplyRotation(270, false);
                    break;
            }
        }

        p.ApplyTranslation(Random.Range(-19,19)*10,Random.Range(-8,8)*10);
    }

    void DrawRabbit()
    {
        AddShadowPolygon(new Vector2(0, -10), new Vector2(0, 20), new Vector2(30, -10));
        AddShadowPolygon(new Vector2(0, 0), new Vector2(-10, -10), new Vector2(0, -20));
        AddShadowPolygon(new Vector2(0, -10), new Vector2(30, -10), new Vector2(30, -40));

        AddShadowPolygon(new Vector2(10, -20), new Vector2(0, -30), new Vector2(10, -40));
        AddShadowPolygon(new Vector2(10, -20), new Vector2(10, -40), new Vector2(30, -40));
        AddShadowPolygon(new Vector2(0, 20), new Vector2(-20, 20), new Vector2(-20, 40));

        AddShadowPolygon(new Vector2(0, 20), new Vector2(-20, 40), new Vector2(0, 40));
        AddShadowPolygon(new Vector2(0, 40), new Vector2(-10, 40), new Vector2(0, 60));
        AddShadowPolygon(new Vector2(0, 40), new Vector2(0, 60), new Vector2(10, 60));
    }

    void DrawCat()
    {
        AddShadowPolygon(new Vector2(-10, 50), new Vector2(-10, 30), new Vector2(0, 40));
        AddShadowPolygon(new Vector2(0, 40), new Vector2(10, 30), new Vector2(10, 50));
        AddShadowPolygon(new Vector2(0, 20), new Vector2(-10, 30), new Vector2(0, 40));

        AddShadowPolygon(new Vector2(0, 20), new Vector2(10, 30), new Vector2(0, 40));
        AddShadowPolygon(new Vector2(0, 0), new Vector2(-10, 10), new Vector2(0, 20));
        AddShadowPolygon(new Vector2(0, -20), new Vector2(20, 0), new Vector2(0, 20));

        AddShadowPolygon(new Vector2(-10, -30), new Vector2(20, 0), new Vector2(20, -30));
        AddShadowPolygon(new Vector2(20, -30), new Vector2(30, -10), new Vector2(30, -30));
        AddShadowPolygon(new Vector2(30, -10), new Vector2(30, -30), new Vector2(40, -10));
    }

    void DrawServant()
    {
        AddShadowPolygon(new Vector2(-20, 50), new Vector2(0, 70), new Vector2(20, 50));
        AddShadowPolygon(new Vector2(-10, 50), new Vector2(-10, 30), new Vector2(10, 30));
        AddShadowPolygon(new Vector2(-10, 50), new Vector2(10, 50), new Vector2(10, 30));
        AddShadowPolygon(new Vector2(-40, 10), new Vector2(-30, 10), new Vector2(20, 10));
        AddShadowPolygon(new Vector2(-30, 20), new Vector2(-20, 10), new Vector2(-10, 20));
        AddShadowPolygon(new Vector2(-30, 0), new Vector2(0, 0), new Vector2(0, 30));
        AddShadowPolygon(new Vector2(-30, 0), new Vector2(0, 0), new Vector2(0, -30));
        AddShadowPolygon(new Vector2(-20, -10), new Vector2(-20, -30), new Vector2(0, -30));
        AddShadowPolygon(new Vector2(-20, -40), new Vector2(-10, -30), new Vector2(-10, -40));
    }

    void DrawSwan()
    {
        AddShadowPolygon(new Vector2(-10, 30), new Vector2(0, 30), new Vector2(0, 40));
        AddShadowPolygon(new Vector2(0, 40), new Vector2(0, 20), new Vector2(10, 30));
        AddShadowPolygon(new Vector2(0, 20), new Vector2(10, 10), new Vector2(10, 30));

        AddShadowPolygon(new Vector2(0, 0), new Vector2(0, 20), new Vector2(10, 10));
        AddShadowPolygon(new Vector2(0, 0), new Vector2(-10, 10), new Vector2(0, 20));
        AddShadowPolygon(new Vector2(0, 0), new Vector2(-10, -10), new Vector2(-10, 10));

        AddShadowPolygon(new Vector2(0, 0), new Vector2(-10, -10), new Vector2(0, -20));
        AddShadowPolygon(new Vector2(0, 0), new Vector2(0, -20), new Vector2(20, -20));
        AddShadowPolygon(new Vector2(0, 0), new Vector2(20, -20), new Vector2(40, 0));
    }

    void DrawHorse()
    {
        AddShadowPolygon(new Vector2(-20, 30), new Vector2(-10, 40), new Vector2(0, 30));
        AddShadowPolygon(new Vector2(-10, 30), new Vector2(-10, 20), new Vector2(0, 20));
        AddShadowPolygon(new Vector2(-10, 30), new Vector2(0, 20), new Vector2(0, 30));

        AddShadowPolygon(new Vector2(-30, 10), new Vector2(-20, 20), new Vector2(-10, 10));
        AddShadowPolygon(new Vector2(-20, 20), new Vector2(0, 0), new Vector2(0, 20));
        AddShadowPolygon(new Vector2(0, 20), new Vector2(0, -20), new Vector2(20, 0));

        AddShadowPolygon(new Vector2(-10, -20), new Vector2(0, -20), new Vector2(0, -10));
        AddShadowPolygon(new Vector2(20, 0), new Vector2(20, -10), new Vector2(30, -10));
        AddShadowPolygon(new Vector2(20, -10), new Vector2(30, -20), new Vector2(30, -10));
    }

    void Draw_____()
    {
        AddShadowPolygon(new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0));
        AddShadowPolygon(new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0));
        AddShadowPolygon(new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0));

        AddShadowPolygon(new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0));
        AddShadowPolygon(new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0));
        AddShadowPolygon(new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0));

        AddShadowPolygon(new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0));
        AddShadowPolygon(new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0));
        AddShadowPolygon(new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0));
    }
}