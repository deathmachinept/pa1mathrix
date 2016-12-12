using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RotationMenuController : MonoBehaviour
{
    private GameObject AA;
    private GameObject AB;
    private GameObject BA;
    private GameObject BB;
    private GameObject AngleDisplay;
    private int[] angleList;
    private int angle;
    private int ListPosition;

    public void Start()
    {
        AA = transform.FindChild("Matrix").FindChild("RotationValuesHolder").FindChild("ColumnA").FindChild("ValueA").gameObject;
        AB = transform.FindChild("Matrix").FindChild("RotationValuesHolder").FindChild("ColumnA").FindChild("ValueB").gameObject;
        BA = transform.FindChild("Matrix").FindChild("RotationValuesHolder").FindChild("ColumnB").FindChild("ValueA").gameObject;
        BB = transform.FindChild("Matrix").FindChild("RotationValuesHolder").FindChild("ColumnB").FindChild("ValueB").gameObject;
        AngleDisplay = transform.FindChild("Angle").gameObject;
        angleList = new int[] { 0, 90, 180, 270 };
        ListPosition = 0;
    }

    public int GiveRotationAngle()
    {
        return angleList[ListPosition];
    }

    public void Update()
    {
        AngleDisplay.GetComponent<Text>().text = angleList[ListPosition].ToString();
        AA.GetComponent<Text>().text ="cos "+ AngleDisplay.GetComponent<Text>().text;
        AB.GetComponent<Text>().text = "sin " + AngleDisplay.GetComponent<Text>().text;
        BA.GetComponent<Text>().text = "-sin " + AngleDisplay.GetComponent<Text>().text;
        BB.GetComponent<Text>().text = "cos " + AngleDisplay.GetComponent<Text>().text;
    }

    public void NextAngle()
    {
        if(ListPosition<angleList.Length-1)
            ListPosition++;
    }

    public void PreviousAngle()
    {
        if (ListPosition > 0)
            ListPosition--;
    }
}
