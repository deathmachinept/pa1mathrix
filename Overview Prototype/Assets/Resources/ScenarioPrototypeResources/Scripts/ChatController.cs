using UnityEngine;
using System;
using System.Collections;
using System.Security.Principal;
using System.Xml.Serialization;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine.UI;

public class ChatController : NetworkBehaviour
{
    private const short chatMessage = 100;

    [SerializeField]
    public InputField input;

    [SerializeField]
    public Text textBox;

    public void Awake()
    {
        input = transform.FindChild("Scroll View").FindChild("InputField").gameObject.GetComponent<InputField>();
        textBox = transform.FindChild("Scroll View").FindChild("Viewport").FindChild("Content").FindChild("Text").GetComponent<Text>();
    }

    public void CreateMessage()
    {
        string PlayerName = "";
        bool isAPlayer = false;
        foreach (Transform t in GameObject.Find("Players").transform)
        {
            if (t.GetComponent<NetworkIdentity>().isLocalPlayer)
            {
                isAPlayer = true;
                PlayerName = t.GetComponent<PlayerController>().PLAYERNAME;
            }
        }
        if (!isAPlayer)
        {
            PlayerName = "Server";
        }
        input.text = "";
    }



}
