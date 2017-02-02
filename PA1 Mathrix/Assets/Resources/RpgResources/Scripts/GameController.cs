using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
public class GameController : MonoBehaviour
{
    public void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void Update()
    {

    }
}
