using System;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour
{
    public bool isPlayingMinigame;
    public float posGridX;
    public float posGridY;
    private GameObject TA, TB;

    public void Awake()
    {
        DontDestroyOnLoad(GameObject.Find("Player(Clone)"));
    }

    void Start()
    {
        isPlayingMinigame = false;
    }

    void Update()
    {
        ProcessMovement();
        CheckForExit();
    }

    public void ChangeToSimplificationGame()
    {
        if (isLocalPlayer)
        {
            PlayerPrefs.SetFloat("PosX", this.transform.position.x);
            PlayerPrefs.SetFloat("PosY", this.transform.position.y);
            PlayerPrefs.SetFloat("PosZ", this.transform.position.z);
            PlayerPrefs.Save();
            SceneManager.LoadSceneAsync("SimplificacaoMatrizes",LoadSceneMode.Single);
        }
    }

    public void ChangeToPolygonGame()
    {
        if (isLocalPlayer)
        {
            PlayerPrefs.SetFloat("PosX", this.transform.position.x);
            PlayerPrefs.SetFloat("PosY", this.transform.position.y);
            PlayerPrefs.SetFloat("PosZ", this.transform.position.z);
            PlayerPrefs.Save();
            SceneManager.LoadSceneAsync("Desenho Polígono",LoadSceneMode.Single);
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
