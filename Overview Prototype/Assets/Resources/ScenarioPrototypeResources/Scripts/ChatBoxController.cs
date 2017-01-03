using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ChatBoxController : NetworkBehaviour
{

    SyncListString ChatMessagesList=new SyncListString();

    private NetworkClient client;
    private GameObject contentHolder;
    private GameObject inputField;
    public void Awake()
    {
        contentHolder = transform.FindChild("Scroll View").FindChild("Viewport").FindChild("Content").gameObject;
        inputField = transform.FindChild("InputField").gameObject;
    }

    public void Start()
    {
        client = NetworkManager.singleton.client;

    }

    [Client]
    void OnNewMessage()
    {
        while (contentHolder.transform.childCount != 0)
        {
            Destroy(contentHolder.transform.GetChild(0).gameObject);
        }

        for (int i = 0; i < ChatMessagesList.Count; i++)
        {
            GameObject message = Resources.Load("ScenarioPrototypeResources/Prefabs/Text") as GameObject;
            message.transform.SetParent(contentHolder.transform);
            message.transform.localPosition = new Vector3(0, -(i * message.GetComponent<RectTransform>().rect.height), 0);
            message.GetComponent<Text>().text = ChatMessagesList[i];
        }
        transform.FindChild("Scroll View")
            .FindChild("Scrollbar Vertical")
            .GetComponent<ScrollRect>()
            .verticalNormalizedPosition = 0.5f;
    }

    [Server]
    void ServerOnNewMessage()
    {

    }

    public void AddString()
    {
        ChatMessagesList.Add(inputField.transform.FindChild("Text").gameObject.GetComponent<Text>().text);
        inputField.transform.FindChild("Text").gameObject.GetComponent<Text>().text = "";
    }
}
