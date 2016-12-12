using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UIController : MonoBehaviour
{
    private GameObject X_Representation;
    private GameObject Y_Representation;
    private Vector2[] vectorList;
	// Use this for initialization
	void Start ()
	{
	    X_Representation = GameObject.Find("Coordenadas_X");
        Y_Representation = GameObject.Find("Coordenadas_Y");
    }
	
	// Update is called once per frame
	void Update ()
	{
	    vectorList = GameObject.Find("MyPolygonDraw").GetComponent<Polygon>().Vertices2D;
	    if (vectorList != null && vectorList.Length>=3)
	    {
	        string textoX = "X: ";
	        string textoY = "Y: ";
	        for (int i = 0; i < vectorList.Length; i++)
	        {
	            textoX += vectorList[i].x + " ";
	            textoY += vectorList[i].y + " ";
	        }
	        X_Representation.GetComponent<Text>().text = textoX;
	        Y_Representation.GetComponent<Text>().text = textoY;
	    }
	}
}
