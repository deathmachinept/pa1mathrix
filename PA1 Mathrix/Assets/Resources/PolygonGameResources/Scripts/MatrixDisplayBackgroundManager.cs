using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class MatrixDisplayBackgroundManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool isOver = false;

    public void Update()
    {
        if (isOver)
        {
            Mathf.Lerp(GetComponent<Image>().color.a, 100, 2);
        }
        else
        {
            Mathf.Lerp(GetComponent<Image>().color.a, 255, 2);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isOver = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isOver = false;
    }
}
