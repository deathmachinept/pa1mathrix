using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MyNetworkManager : NetworkManager
{
    public GameObject MainSceneObjects;
    public GameObject players;
    public string Address;
    public int Port;
    public string CurrentSceneName = "TestBuild";

    public void Awake()
    {
        MainSceneObjects = GameObject.Find("MainSceneObjectsHolder");
        players = transform.FindChild("Players").gameObject;
    }

    public void Start()
    {
        Address = "localhost";
        Port = 7777;
    }

    public void Update()
    {
        OrganizePlayers();
    }

    public void OrganizePlayers()
    {
        foreach (GameObject obj in SceneManager.GetActiveScene().GetRootGameObjects())
        {
            if (obj.tag=="Player")
            {
                obj.transform.SetParent(players.transform);
            }
        }
    }

    public void Pressed_Solo_Button()
    {
        if (PlayerPrefs.GetString("Player Name") != "")
        {
            singleton.networkPort = Port;
            singleton.StartHost();
            HideNetworkingHUD();
        }
    }

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        var player = (GameObject) GameObject.Instantiate(singleton.playerPrefab, Vector3.zero, Quaternion.identity);
        player.transform.SetParent(players.transform);
        NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
    }

    public void Pressed_Client_Button()
    {
        if (PlayerPrefs.GetString("Player Name") != "")
        {
            singleton.networkAddress = Address;
            singleton.networkPort = Port;
            singleton.StartClient();
            HideNetworkingHUD();
        }
    }

    public void Pressed_Host_Button()
    {
        singleton.networkPort = Port;
        singleton.StartServer();
        HideNetworkingHUD();
        PlayerPrefs.SetString("Player Name","Server");
        PlayerPrefs.Save();
    }

    public void InsertedName()
    {
        PlayerPrefs.SetString("Player Name", GameObject.Find("NameInput").GetComponent<InputField>().text);
        PlayerPrefs.Save();
    }

    public void HideNetworkingHUD()
    {
        MainSceneObjects.transform.FindChild("NetworkCanvas").gameObject.SetActive(false);
    }
}