using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScalingMenuController : MonoBehaviour
{

    private GameObject ScaleDisplay;
    private float ScaleValue;

    public void Start()
    {
        ScaleDisplay = transform.FindChild("Scale").gameObject;
        ScaleValue = 1;
    }

    void Update()
    {
        ScaleDisplay.GetComponent<Text>().text = ScaleValue + "x";
    }

    public float GiveScale()
    {
        return ScaleValue;
    }

    public void DivideScale()
    {
        if(ScaleValue>=1f)
            ScaleValue /= 2f;
    }

    public void MultiplyScale()
    {
        ScaleValue *= 2f;
    }

}
