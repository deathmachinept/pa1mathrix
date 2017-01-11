using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class ReturnScript : MonoBehaviour
{
    public void ReturnToMainScene()
    {
        //SceneManager.LoadSceneAsync("TestBuild", LoadSceneMode.Single);
        SceneManager.UnloadScene(GameObject.Find("NetworkManagerHolder").GetComponent<MyNetworkManager>().CurrentSceneName);
        GameObject.Find("NetworkManagerHolder").GetComponent<MyNetworkManager>().MainSceneObjects.SetActive(true);
        GameObject.Find("NetworkManagerHolder").GetComponent<MyNetworkManager>().players.SetActive(true);
        GameObject.Find("NetworkManagerHolder").GetComponent<MyNetworkManager>().Chat.SetActive(true);
        GameObject.Find("NetworkManagerHolder").GetComponent<MyNetworkManager>().CurrentSceneName = "Rpg";
    }
}
