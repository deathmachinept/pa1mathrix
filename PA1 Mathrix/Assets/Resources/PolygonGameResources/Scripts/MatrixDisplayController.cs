using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;

public class MatrixDisplayController : MonoBehaviour
{
    private float _uiScale = 50;
    private GameObject _leftBrackets;
    private GameObject _rightBrackets;
    private Vector3 _initialRightBracketsPosition;
    private List<GameObject> _slots;
    public Polygon _PreviousPolygon;
    public Polygon currentPolygon;
    private int _previousVerticeCount=0;
	void Start ()
	{
        _slots=new List<GameObject>();
	    for (int i = 0; i < gameObject.transform.childCount; i++)
	    {
	        if (gameObject.transform.GetChild(i).transform.name == "LeftFrame")
	        {
	            _leftBrackets = gameObject.transform.GetChild(i).gameObject;
	        }
	        if (gameObject.transform.GetChild(i).transform.name == "RightFrame")
	        {
	            _rightBrackets = gameObject.transform.GetChild(i).gameObject;
	        }
	    }
	    _initialRightBracketsPosition = _rightBrackets.transform.position;
        _leftBrackets.SetActive(false);
        _rightBrackets.SetActive(false);
        if(GameObject.Find("PolygonHandlerHolder").GetComponent<PolygonHandler>().CurrentlySelectedPolygon!=null)
            currentPolygon = GameObject.Find("PolygonHandlerHolder").GetComponent<PolygonHandler>().CurrentlySelectedPolygon.GetComponent<Polygon>();
	}

    public void RemoveLastPoint()
    {
        currentPolygon.InsertedPoints.RemoveAt(currentPolygon.InsertedPoints.Count-1);
    }

	void Update () {
	    if (GameObject.Find("PolygonHandlerHolder").GetComponent<PolygonHandler>().CurrentlySelectedPolygon != null)
	    {
	        currentPolygon =
	            GameObject.Find("PolygonHandlerHolder")
	                .GetComponent<PolygonHandler>()
	                .CurrentlySelectedPolygon.GetComponent<Polygon>();
	        if (_PreviousPolygon == currentPolygon)
	        {
	            if (currentPolygon.InsertedPoints.Count > 0)
	            {
	                _leftBrackets.SetActive(true);
	                _rightBrackets.SetActive(true);
	            }
	            if (currentPolygon.InsertedPoints.Count > _previousVerticeCount)
	            {
	                AddNewSlot();
	            }
	            for (int i = 0; i < _slots.Count; i++)
	            {
	                _slots[i].transform.FindChild("SlotX").GetComponent<Text>().text =
	                    ((int) currentPolygon.InsertedPoints[i].x).ToString();
	                _slots[i].transform.FindChild("SlotY").GetComponent<Text>().text =
	                    ((int) currentPolygon.InsertedPoints[i].y).ToString();
	            }
	        }
	        else
	        {
	            _PreviousPolygon = currentPolygon;
	        }
	    }
	}

    void AddNewSlot()
    {
        GameObject newSlot = Instantiate(GameObject.Find("Slot"));
        newSlot.transform.SetParent(GameObject.Find("ValuesHolder").transform);
        newSlot.transform.position =
            new Vector3(
                GameObject.Find("Slot").transform.position.x + ((25 * currentPolygon.InsertedPoints.Count) - 25),
                GameObject.Find("Slot").transform.position.y, GameObject.Find("Canvas").transform.position.z);
        newSlot.transform.localScale = GameObject.Find("Slot").transform.localScale;

        newSlot.transform.FindChild("SlotX").gameObject.SetActive(true);
        newSlot.transform.FindChild("SlotY").gameObject.SetActive(true);

        _rightBrackets.transform.position = new Vector3(newSlot.transform.position.x,
            _rightBrackets.transform.position.y, GameObject.Find("Canvas").transform.position.z);
        _leftBrackets.transform.position = new Vector3(_leftBrackets.transform.position.x,
            _rightBrackets.transform.position.y, 95);

        _slots.Add(newSlot);

        _previousVerticeCount = currentPolygon.InsertedPoints.Count;
    }

    public void ClearDisplay()
    {
        _rightBrackets.transform.position = _initialRightBracketsPosition;
        for (int i = 0; i < _slots.Count; i++)
        {
            if (_slots[i].name == "Slot(Clone)")
            {
                Destroy(_slots[i]);
            }
        }
    }
}
