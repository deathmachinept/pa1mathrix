﻿using System;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using UnityEngine.Networking.Types;
using UnityEngine.UI;

public class PlayerController : NetworkBehaviour
{
    public bool isPlayingMinigame;

    [SyncVar]
    public string PLAYERNAME;

    public GameObject PlayersHolder;
    public float posGridX;
    public float posGridY;
    private GameObject TA, TB;

    void Awake()
    {
        PlayersHolder = GameObject.Find("Players");
    }

    void Start()
    {
        isPlayingMinigame = false;
        transform.FindChild("Canvas").FindChild("Text").GetComponent<Text>().text = PLAYERNAME;
    }

    void Update()
    {
        if (!GameObject.Find("InputField").GetComponent<InputField>().isFocused)
        {
            ProcessMovement();
            CheckForExit();
        }
    }

    public void ChangeToSimplificationGame()
    {
        if (isLocalPlayer)
        {
            SceneManager.LoadSceneAsync("SimplificacaoMatrizes", LoadSceneMode.Additive);
            GameObject.Find("MainSceneObjectsHolder").SetActive(false);
            GameObject.Find("Network Manager").GetComponent<MyNetworkManager>().CurrentSceneName =
                "SimplificacaoMatrizes";
            this.transform.position = Vector3.zero;
            GameObject.Find("Network Manager").GetComponent<MyNetworkManager>().players.SetActive(false);
        }
    }

    public void ChangeToPolygonGame()
    {
        if (isLocalPlayer)
        {
            SceneManager.LoadSceneAsync("Desenho Polígono", LoadSceneMode.Additive);
            GameObject.Find("MainSceneObjectsHolder").SetActive(false);
            GameObject.Find("Network Manager").GetComponent<MyNetworkManager>().CurrentSceneName = "Desenho Polígono";
            this.transform.position = Vector3.zero;
            GameObject.Find("Network Manager").GetComponent<MyNetworkManager>().players.SetActive(false);
        }
    }

    void ProcessMovement()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        var x = Input.GetAxis("Horizontal") * Time.deltaTime * 150.0f;
        var y = Input.GetAxis("Vertical") * Time.deltaTime * 150.0f;

        transform.Translate(new Vector3(x, y, 0));
        posGridX = (int)(transform.position.x * 0.1f) * 10;
        posGridY = (int)(transform.position.y * 0.1f) * 10;

        transform.FindChild("GridPos").position = new Vector3(posGridX, posGridY, 0);
    }
    
    void CheckForExit()
    {
        TA = GameObject.Find("Terminal A");
        TB = GameObject.Find("Terminal B");
        if (posGridX == TA.transform.position.x &&
            posGridY == TA.transform.position.y)
        {
            ChangeToSimplificationGame();
        }

        if (posGridX == TB.transform.position.x &&
            posGridY == TB.transform.position.y)
        {
            ChangeToPolygonGame();
        }
    }
}