using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ChatController : NetworkBehaviour
{
    //[SyncVar(hook = "OnChatUpdate")]
    //private string chatString;

    public SyncListString chatString=new SyncListString();

    private InputField input;
    private Text textBox;

    public void Awake()
    {
        input = transform.FindChild("Scroll View").FindChild("InputField").gameObject.GetComponent<InputField>();
        textBox = transform.FindChild("Scroll View").FindChild("Viewport").FindChild("Content").FindChild("Text").GetComponent<Text>();
    }

    public void AddMessage()
    {
        //chatString += input.text;
        chatString.Add(input.text);
        input.text = "";
    }

    void OnChatUpdate(string message)
    {
        textBox.text = message + "\n";
    }
}
