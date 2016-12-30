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
        //SceneManager.LoadSceneAsync("TestBuild", LoadSceneMode.Single);
        SceneManager.UnloadScene(GameObject.Find("Network Manager").GetComponent<MyNetworkManager>().CurrentSceneName);
        GameObject.Find("Network Manager").GetComponent<MyNetworkManager>().MainSceneObjects.SetActive(true);
        GameObject.Find("Network Manager").GetComponent<MyNetworkManager>().PlayerList.SetActive(true);
        GameObject.Find("Network Manager").GetComponent<MyNetworkManager>().CurrentSceneName = "TestBuild";
    }
}
