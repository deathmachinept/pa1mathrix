using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PolygonHandler : MonoBehaviour
{
    public GameObject ShadowPolygonHolder;
    public GameObject UserPolygonHolder;

    public Polygon CurrentlySelectedPolygon;

    public int SelectedFigure;
    public int PreviouslySelectedFigure;

    void Awake()
    {
        ShadowPolygonHolder = new GameObject("Shadow Polygon Holder");
        ShadowPolygonHolder.transform.SetParent(transform);

        UserPolygonHolder = new GameObject("User Polygon Holder");
        UserPolygonHolder.transform.SetParent(transform);

        SelectedFigure = Random.Range(0, 2);
        PreviouslySelectedFigure = SelectedFigure;
    }

    void Start()
    {
        StartShadowGame();
    }

    void Update()
    {
        if (SelectedFigure != PreviouslySelectedFigure)
        {
            StartShadowGame();
        }
        SelectionChecking();
        //if (ShadowFigureCheck())
        //    Debug.Log("YOU'RE WINNER");
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
        }
        foreach (Transform t in ShadowPolygonHolder.transform)
        {
            CloneShadowToInteractablePolygon(t.GetComponent<Polygon>());
        }
    }

    bool ShadowFigureCheck()
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
            //Fazer Func para verificar se todos os pontos dos poígonos do utilizador estão a ocupar posições dos estáticos
            if (!hasPolygonMatch/* && !PointMatchChecking(shadow.GetComponent<Polygon>())*/)
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

    bool PointMatchChecking(Polygon shadow)
    {
        foreach (Vector2 shadowPoint in shadow.InsertedPoints)
        {
            bool foundMatch = false;
            foreach (Transform t in UserPolygonHolder.transform)
            {
                foreach (Vector2 point in t.GetComponent<Polygon>().InsertedPoints)
                {
                    if (point == shadowPoint)
                        foundMatch = true;
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
                Debug.Log("Bateu nalgo");
                if (hit.transform.tag == "Polygon")
                {
                    Debug.Log("Mais Especificamente um polygon");
                    if (hit.transform.GetComponent<Polygon>().Interactable)
                    {
                        Debug.Log("Que é interagível");
                        if (CurrentlySelectedPolygon != null)
                            CurrentlySelectedPolygon.IsSelected = false;
                        CurrentlySelectedPolygon = hit.transform.GetComponent<Polygon>();
                        hit.transform.GetComponent<Polygon>().IsSelected = true;
                    }
                }
            }
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
        Vector2 nullifyingVector = Vector2.zero;
        if (mainPointPosition.x > 0)
        {
            nullifyingVector.x = -mainPointPosition.x;
        }
        else
        {
            nullifyingVector.x = +mainPointPosition.x;
        }
        if (mainPointPosition.y > 0)
        {
            nullifyingVector.y = -mainPointPosition.y;
        }
        else
        {
            nullifyingVector.y = +mainPointPosition.y;
        }
        p.ApplyTranslation(nullifyingVector.x, nullifyingVector.y);
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
}