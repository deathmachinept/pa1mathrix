using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class MatrixDisplayBackgroundManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool isOver = false;
    public void FixedUpdate()
    {
        if (isOver)
        {
            StartCoroutine(Smooth.Fade(GetComponent<Image>(),0, 1));

            StartCoroutine(Smooth.Fade(transform.FindChild("Text").GetComponent<Text>(), 0, 1));

            StartCoroutine(Smooth.Fade(transform.FindChild("LeftFrame").FindChild("MatrixFrame1").GetComponent<SpriteRenderer>(),0,1));
            StartCoroutine(Smooth.Fade(transform.FindChild("LeftFrame").FindChild("MatrixFrame4").GetComponent<SpriteRenderer>(), 0, 1));

            StartCoroutine(Smooth.Fade(transform.FindChild("RightFrame").FindChild("MatrixFrame3").GetComponent<SpriteRenderer>(), 0, 1));
            StartCoroutine(Smooth.Fade(transform.FindChild("RightFrame").FindChild("MatrixFrame2").GetComponent<SpriteRenderer>(), 0, 1));

            StartCoroutine(Smooth.Fade(transform.FindChild("ValuesHolder").FindChild("SlotA").FindChild("SlotX").GetComponent<Text>(), 0, 1));
            StartCoroutine(Smooth.Fade(transform.FindChild("ValuesHolder").FindChild("SlotA").FindChild("SlotY").GetComponent<Text>(), 0, 1));

            StartCoroutine(Smooth.Fade(transform.FindChild("ValuesHolder").FindChild("SlotB").FindChild("SlotX").GetComponent<Text>(), 0, 1));
            StartCoroutine(Smooth.Fade(transform.FindChild("ValuesHolder").FindChild("SlotB").FindChild("SlotY").GetComponent<Text>(), 0, 1));

            StartCoroutine(Smooth.Fade(transform.FindChild("ValuesHolder").FindChild("SlotC").FindChild("SlotX").GetComponent<Text>(), 0, 1));
            StartCoroutine(Smooth.Fade(transform.FindChild("ValuesHolder").FindChild("SlotC").FindChild("SlotY").GetComponent<Text>(), 0, 1));
        }
        else
        {
            StartCoroutine(Smooth.Fade(GetComponent<Image>(), 0.5f, 1));

            StartCoroutine(Smooth.Fade(transform.FindChild("Text").GetComponent<Text>(), 1, 1));

            StartCoroutine(Smooth.Fade(transform.FindChild("LeftFrame").FindChild("MatrixFrame1").GetComponent<SpriteRenderer>(), 1, 1));
            StartCoroutine(Smooth.Fade(transform.FindChild("LeftFrame").FindChild("MatrixFrame4").GetComponent<SpriteRenderer>(), 1, 1));

            StartCoroutine(Smooth.Fade(transform.FindChild("RightFrame").FindChild("MatrixFrame3").GetComponent<SpriteRenderer>(), 1, 1));
            StartCoroutine(Smooth.Fade(transform.FindChild("RightFrame").FindChild("MatrixFrame2").GetComponent<SpriteRenderer>(), 1, 1));

            StartCoroutine(Smooth.Fade(transform.FindChild("ValuesHolder").FindChild("SlotA").FindChild("SlotX").GetComponent<Text>(), 1, 1));
            StartCoroutine(Smooth.Fade(transform.FindChild("ValuesHolder").FindChild("SlotA").FindChild("SlotY").GetComponent<Text>(), 1, 1));

            StartCoroutine(Smooth.Fade(transform.FindChild("ValuesHolder").FindChild("SlotB").FindChild("SlotX").GetComponent<Text>(), 1, 1));
            StartCoroutine(Smooth.Fade(transform.FindChild("ValuesHolder").FindChild("SlotB").FindChild("SlotY").GetComponent<Text>(), 1, 1));

            StartCoroutine(Smooth.Fade(transform.FindChild("ValuesHolder").FindChild("SlotC").FindChild("SlotX").GetComponent<Text>(), 1, 1));
            StartCoroutine(Smooth.Fade(transform.FindChild("ValuesHolder").FindChild("SlotC").FindChild("SlotY").GetComponent<Text>(), 1, 1));
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
