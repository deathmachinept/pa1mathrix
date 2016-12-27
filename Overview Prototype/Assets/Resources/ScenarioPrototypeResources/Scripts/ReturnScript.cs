using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class ReturnScript : NetworkBehaviour
{
    public void Awake()
    {
        DontDestroyOnLoad(GameObject.Find("Networkmanager"));
    }

    public void ReturnToMainScene()
    {
        SceneManager.LoadSceneAsync("TestBuild", LoadSceneMode.Single);
    }
}
