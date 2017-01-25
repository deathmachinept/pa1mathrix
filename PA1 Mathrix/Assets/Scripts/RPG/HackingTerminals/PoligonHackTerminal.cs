using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PoligonHackTerminal : MonoBehaviour
{

    // Use this for initialization
    public NetworkIdentity interactingPlayerIdentity;
    public bool IsMinigameDone=false;
    public bool podeCarregar = false;
    private bool carregou = false;
    private bool loadCameraOnce = false;
    public GameObject[] camerasOnScene;
    private AsyncOperation op;

    // Use this for initialization

    public void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log("Colision");
        interactingPlayerIdentity = collider.gameObject.GetComponent<NetworkIdentity>();
        podeCarregar = true;
    }



    public void OnTriggerExit2D()
    {
        interactingPlayerIdentity = null;
        podeCarregar = false;
    }

    void Update()
    {
        if (podeCarregar && !carregou)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {

                //Camera.main.enabled = false;
                //camerasOnScene[0].tag = "Untagged";
                if (interactingPlayerIdentity.isLocalPlayer)
                {
                    SceneManager.LoadSceneAsync("Desenho Polígono", LoadSceneMode.Additive);
                    GameObject.Find("MainSceneObjectsHolder").SetActive(false);
                    GameObject.Find("Players").SetActive(false);
                    GameObject.Find("Network Manager").GetComponent<MyNetworkManager>().CurrentSceneName =
                        "Desenho Polígono";
                    transform.position = Vector3.zero;
                    GameObject.Find("Network Manager").GetComponent<MyNetworkManager>().players.SetActive(false);
                    GameObject.Find("ChatCanvas").SetActive(false);
                }

                //loadCameraOnce = true;
                //carregou = true;
                Debug.Log("Corre!");
            }
        }
        else if (Input.GetKeyDown(KeyCode.F))
        {
            if (carregou) // pressionar uma segunda fez sai do jogo
            {
                //camerasOnScene[1].GetComponent<Camera>().enabled = false;
                //camerasOnScene[0].tag = "MainCamera";

                //camerasOnScene[0].GetComponent<Camera>().enabled = true;
                //carregou = false;
            }

        }

        if (loadCameraOnce && carregou)
        {
            if (op.isDone) //load completo
            {
                //camerasOnScene[1] = GameObject.FindGameObjectWithTag("MainCamera");
                //loadCameraOnce = false;
            }
        }
    }
}
