using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;


public class GameController : NetworkBehaviour
{
    public GameObject PlayersHolder;

    public void Awake()
    {
        PlayersHolder = transform.FindChild("Players").gameObject;
    }

    public void Update()
    {

    }

    void UpdatePlayerNames()
    {
        if(!isServer)
        { return;}
        foreach (var VARIABLE in PlayersHolder.transform)
        {
            
        }
    }
}
