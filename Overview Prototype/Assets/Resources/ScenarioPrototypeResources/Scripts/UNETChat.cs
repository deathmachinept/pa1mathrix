using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class UNETChat : Chat
{
	private const short chatMessageID = 131;

	private void Start()
	{
		//if the client is also the server
		if (NetworkServer.active) 
		{
			//registering the server handler
			NetworkServer.RegisterHandler(chatMessageID, ServerReceiveMessage);
		}

		//registering the client handler
        NetworkManager.singleton.client.RegisterHandler(chatMessageID, ReceiveMessage);
	}

	private void ReceiveMessage(NetworkMessage message)
	{
		//reading message
		string text = message.ReadMessage<StringMessage> ().value;

		AddMessage (text);
	}

	private void ServerReceiveMessage(NetworkMessage message)
	{
		StringMessage myMessage = new StringMessage ();
		myMessage.value = message.ReadMessage<StringMessage> ().value;
        
		NetworkServer.SendToAll (chatMessageID, myMessage);
	}

	public override void SendMessage (UnityEngine.UI.InputField input)
	{
		StringMessage myMessage = new StringMessage ();
	    string PlayerName="";

	    foreach (Transform t in GameObject.Find("Players").transform)
	    {
	        if (t.GetComponent<PlayerController>().isLocalPlayer)
	        {
	            PlayerName = t.GetComponent<PlayerController>().PLAYERNAME;
	        }
	    }

		//getting the value of the input
		myMessage.value = PlayerName+"-"+input.text;

		//sending to server
		NetworkManager.singleton.client.Send (chatMessageID, myMessage);
	}
}