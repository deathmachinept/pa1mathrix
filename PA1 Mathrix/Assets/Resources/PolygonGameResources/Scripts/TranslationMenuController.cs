using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TranslationMenuController : MonoBehaviour
{
    private GameObject ValueA;
    private GameObject ValueB;

    public void Start()
    {
        ValueA = GameObject.Find("TranslationValuesHolder").transform.GetChild(0).transform.GetChild(0).gameObject;
        ValueB = GameObject.Find("TranslationValuesHolder").transform.GetChild(0).transform.GetChild(1).gameObject;
    }

    public void IncrementA()
    {
        int value=int.Parse(ValueA.GetComponent<Text>().text);
        value += 10;
        ValueA.GetComponent<Text>().text = value.ToString();
    }

    public void DecrementA()
    {
        int value = int.Parse(ValueA.GetComponent<Text>().text);
        value -= 10;
        ValueA.GetComponent<Text>().text = value.ToString();
    }

    public void IncrementB()
    {
        int value = int.Parse(ValueB.GetComponent<Text>().text);
        value += 10;
        ValueB.GetComponent<Text>().text = value.ToString();
    }

    public void DecrementB()
    {
        int value = int.Parse(ValueB.GetComponent<Text>().text);
        value -= 10;
        ValueB.GetComponent<Text>().text = value.ToString();
    }

    public Vector3 GiveTranslationValues()
    {
        int x = int.Parse(ValueA.GetComponent<Text>().text);
        int y = int.Parse(ValueB.GetComponent<Text>().text);
        ValueA.GetComponent<Text>().text = "0";
        ValueB.GetComponent<Text>().text = "0";
        return new Vector3(x, y, 140);
    }
}
