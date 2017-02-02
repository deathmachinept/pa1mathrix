using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
public class GameController : MonoBehaviour
{
    public GameObject MainSceneObjects;
    public GameObject Guard;

    public void Awake()
    {
        DontDestroyOnLoad(gameObject);
        MainSceneObjects=GameObject.Find("MainSceneObjectsHolder");
        DontDestroyOnLoad(GameObject.Find("Guard"));
        DontDestroyOnLoad(MainSceneObjects);
    }

    public void Update()
    {

    }
}
