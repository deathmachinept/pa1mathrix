using UnityEngine;
using System.Collections;
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

    void Awake()
    {
        MainSceneObjects = GameObject.Find("MainSceneObjectsHolder");
        players = transform.FindChild("Players").gameObject;
    }

    public void Start()
    {
        Address = "localhost";
        Port = 7777;
    }

    public void ChangeAddressFromInput(Text address)
    {
        Address = address.text;
    }

    public void Update()
    {
        OrganizePlayers();
        IdentifyPlayers();
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


    void IdentifyPlayers()
    {
        foreach (Transform t in transform.FindChild("Players").transform)
        {
            t.FindChild("Canvas").FindChild("Text").GetComponent<Text>().text =
                t.GetComponent<PlayerController>().PLAYERNAME;
        }
    }

    public void Pressed_Solo_Button()
    {
        if (PlayerPrefs.GetString("Player Name") != "")
        {
            networkPort = Port;
            StartHost();
            HideNetworkingHUD();
        }
    }

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        var player = (GameObject) GameObject.Instantiate(singleton.playerPrefab, Vector3.zero, Quaternion.identity);
        player.transform.SetParent(players.transform);
        player.GetComponent<PlayerController>().PLAYERNAME = PlayerPrefs.GetString("Player Name");
        player.tag = "Player";
        NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
    }

    public void Pressed_Client_Button()
    {
        if (PlayerPrefs.GetString("Player Name") != "")
        {
            networkAddress = Address;
            networkPort = Port;
            StartClient();
            HideNetworkingHUD();
        }
    }

    public void Pressed_Host_Button()
    {
        networkPort = Port;
        StartServer();
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