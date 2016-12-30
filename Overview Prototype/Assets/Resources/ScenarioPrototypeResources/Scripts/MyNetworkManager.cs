using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class MyNetworkManager : NetworkManager
{
    public GameObject MainSceneObjects;
    public GameObject PlayerList;
    public string Address;
    public int Port;
    public string CurrentSceneName = "TestBuild";

    public void Start()
    {
        MainSceneObjects = GameObject.Find("MainSceneObjectsHolder");
        PlayerList = transform.FindChild("Players").gameObject;
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
                obj.transform.SetParent(PlayerList.transform);
            }
        }
    }

    public void Pressed_Solo_Button()
    {
        singleton.networkPort = Port;
        singleton.StartHost();
        HideNetworkingHUD();
    }

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        var player = (GameObject) GameObject.Instantiate(singleton.playerPrefab, Vector3.zero, Quaternion.identity);
        player.transform.SetParent(PlayerList.transform);
        NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
    }

    public void Pressed_Client_Button()
    {
        singleton.networkAddress = Address;
        singleton.networkPort = Port;
        singleton.StartClient();
        HideNetworkingHUD();
    }

    public void Pressed_Host_Button()
    {
        singleton.networkPort = Port;
        singleton.StartServer();
        HideNetworkingHUD();
    }

    public void HideNetworkingHUD()
    {
        MainSceneObjects.transform.FindChild("NetworkCanvas").gameObject.SetActive(false);
    }
}