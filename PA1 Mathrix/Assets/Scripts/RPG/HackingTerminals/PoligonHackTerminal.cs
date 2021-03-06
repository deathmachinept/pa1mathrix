﻿using UnityEngine;
using System.Collections;
using JetBrains.Annotations;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PoligonHackTerminal : NetworkBehaviour
{

    // Use this for initialization
    public NetworkIdentity interactingPlayerIdentity;

    [SyncVar(hook = "MinigameWasDone")]
    public bool IsMinigameDone;
    public int ID;
    public bool podeCarregar = false;
    private bool carregou = false;
    private bool loadCameraOnce = false;
    public GameObject[] camerasOnScene;
    private AsyncOperation op;
    
    public void TriggerTrain(bool input)
    {
        IsMinigameDone = input;
    }

    public void Start()
    {
        if (isServer)
        {
            IsMinigameDone = false;
        }
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log("Colision");
        interactingPlayerIdentity = collider.gameObject.GetComponent<NetworkIdentity>();
        podeCarregar = true;
    }

    [ClientRpc]
    public void RpcSwitch(bool value)
    {
        IsMinigameDone = value;
    }

    void MinigameWasDone(bool value)
    {
        IsMinigameDone = value;
    }

    public void OnTriggerExit2D()
    {
        interactingPlayerIdentity = null;
        podeCarregar = false;
    }

    void Update()
    {
        if (podeCarregar && !carregou && !IsMinigameDone)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                //Camera.main.enabled = false;
                //camerasOnScene[0].tag = "Untagged";
                if (interactingPlayerIdentity.isLocalPlayer)
                {
                    SceneManager.LoadSceneAsync("Desenho Polígono", LoadSceneMode.Additive);
                    GameObject.Find("MainSceneObjectsHolder").SetActive(false);
                    GameObject.Find("Players").SetActive(false);
                    GameObject.Find("Network Manager").GetComponent<MyNetworkManager>().CurrentSceneName =
                        "Desenho Polígono";
                    transform.position = Vector3.zero;
                    GameObject.Find("Network Manager").GetComponent<MyNetworkManager>().players.SetActive(false);
                    GameObject.Find("ChatCanvas").SetActive(false);
                }

                //loadCameraOnce = true;
                //carregou = true;
                Debug.Log("Corre!");
            }
        }
        else
        {
            if (podeCarregar && IsMinigameDone)
            {
                interactingPlayerIdentity.GetComponent<MovimentoJogador>().SolvedHackID = ID;
            }
        }

        if (loadCameraOnce && carregou)
        {
            if (op.isDone) //load completo
            {
                //camerasOnScene[1] = GameObject.FindGameObjectWithTag("MainCamera");
                //loadCameraOnce = false;
            }
        }
    }
}
