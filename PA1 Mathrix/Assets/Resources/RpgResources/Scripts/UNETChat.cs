using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class UNETChat : Chat
{
	private const short chatMessageID = 131;

	private void Start()
	{
		if (NetworkServer.active) 
		{
			NetworkServer.RegisterHandler(chatMessageID, ServerReceiveMessage);
		}
        
        NetworkManager.singleton.client.RegisterHandler(chatMessageID, ReceiveMessage);
	}

	private void ReceiveMessage(NetworkMessage message)
	{
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
	        if (t.GetComponent<MovimentoJogador>().isLocalPlayer)
	        {
	            PlayerName = t.GetComponent<MovimentoJogador>().PLAYERNAME;
	        }
	    }

		//getting the value of the input
		myMessage.value = PlayerName+" - "+input.text;

        //sending to server
        NetworkManager.singleton.client.Send (chatMessageID, myMessage);
	}
}