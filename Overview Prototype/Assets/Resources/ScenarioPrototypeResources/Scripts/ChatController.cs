using UnityEngine;
using System;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ChatController : NetworkBehaviour
{
    //[SyncVar(hook = "OnChatUpdate")]
    //private string chatString;

    public SyncListString chatMessages=new SyncListString();

    public InputField input;
    public Text textBox;

    public void Awake()
    {
        input = transform.FindChild("Scroll View").FindChild("InputField").gameObject.GetComponent<InputField>();
        textBox = transform.FindChild("Scroll View").FindChild("Viewport").FindChild("Content").FindChild("Text").GetComponent<Text>();
    }

    public void Start()
    {
        chatMessages.Callback = OnChatMessagesChanged;
    }

    private void OnChatMessagesChanged(SyncListString.Operation op, int index)
    {
        Debug.Log(op + " at index of " + index);
        if (op == SyncListString.Operation.OP_ADD)
        {
            textBox.text += chatMessages[index] + "\n";
        }
    }

    public void AddMessage()
    {
        chatMessages.Add(input.text);
        //textBox.text += input.text+"\n";
        input.text = "";
    }

    void OnChatUpdate(string message)
    {
        textBox.text = message + "\n";
    }
}
