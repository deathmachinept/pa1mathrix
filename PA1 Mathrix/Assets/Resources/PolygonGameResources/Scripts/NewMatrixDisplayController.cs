using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class NewMatrixDisplayController : MonoBehaviour
{
    public PolygonHandler polygonHandler;

    void Awake()
    {
        ClearDisplay();
        polygonHandler = GameObject.Find("PolygonHandlerHolder").GetComponent<PolygonHandler>();
    }
    void Update()
    {
        transform.FindChild("ValuesHolder").FindChild("SlotA").FindChild("SlotX").GetComponent<Text>().text =
            polygonHandler.CurrentlySelectedPolygon.InsertedPoints[0].x.ToString();
        transform.FindChild("ValuesHolder").FindChild("SlotA").FindChild("SlotY").GetComponent<Text>().text =
            polygonHandler.CurrentlySelectedPolygon.InsertedPoints[0].y.ToString();

        transform.FindChild("ValuesHolder").FindChild("SlotB").FindChild("SlotX").GetComponent<Text>().text =
            polygonHandler.CurrentlySelectedPolygon.InsertedPoints[1].x.ToString();
        transform.FindChild("ValuesHolder").FindChild("SlotB").FindChild("SlotY").GetComponent<Text>().text =
            polygonHandler.CurrentlySelectedPolygon.InsertedPoints[1].y.ToString();

        transform.FindChild("ValuesHolder").FindChild("SlotC").FindChild("SlotX").GetComponent<Text>().text =
            polygonHandler.CurrentlySelectedPolygon.InsertedPoints[2].x.ToString();
        transform.FindChild("ValuesHolder").FindChild("SlotC").FindChild("SlotY").GetComponent<Text>().text =
            polygonHandler.CurrentlySelectedPolygon.InsertedPoints[2].y.ToString();
    }

    public void ClearDisplay()
    {
        foreach (Transform t in transform.FindChild("ValuesHolder"))
        {
            t.FindChild("SlotX").GetComponent<Text>().text = 0.ToString();
            t.FindChild("SlotY").GetComponent<Text>().text = 0.ToString();
        }
    }
}
