using UnityEngine;
using System.Collections;
using System.Runtime.CompilerServices;

public class OperationsMenuController : MonoBehaviour
{
    private GameObject Rotation;
    private GameObject Translation;
    private GameObject Scaling;

    private GameObject TMenu;
    private GameObject RMenu;
    private GameObject SMenu;


    public void Start()
    {
        Rotation = transform.FindChild("RotationButton").gameObject;
        Translation = transform.FindChild("TranslationButton").gameObject;
        Scaling = transform.FindChild("ScalingButton").gameObject;

        TMenu = transform.FindChild("TranslationMenu").gameObject;
        TMenu.SetActive(false);

        RMenu = transform.FindChild("RotationMenu").gameObject;
        RMenu.SetActive(false);

        SMenu = transform.FindChild("ScalingMenu").gameObject;
        SMenu.SetActive(false);
    }

    public void TranslationSelected()
    {
        HideAllButtons();
        TMenu.SetActive(true);
    }

    public void RotationSelected()
    {
        HideAllButtons();
        RMenu.SetActive(true);
    }

    public void ScalingSelected()
    {
        HideAllButtons();
        SMenu.SetActive(true);
    }

    public void Reset()
    {
        TMenu.SetActive(false);
        RMenu.SetActive(false);
        //SMenu.SetActive(false);
        Translation.SetActive(true);
        Rotation.SetActive(true);
        //Scaling.SetActive(true);
    }

    public void HideAllButtons()
    {
        Rotation.SetActive(false);
        Translation.SetActive(false);
        Scaling.SetActive(false);
    }
}
