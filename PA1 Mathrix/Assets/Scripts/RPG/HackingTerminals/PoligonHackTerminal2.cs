using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PoligonHackTerminal2 : MonoBehaviour
{

    // Use this for initialization
    // Use this for initialization

    public bool IsMinigameDone;
    public int ID;
    public bool podeCarregar = false;
    private bool carregou = false;
    private bool loadCameraOnce = false;
    private AsyncOperation op;

    public void TriggerTrain(bool input)
    {
        IsMinigameDone = input;
    }

    public void Start()
    {
        IsMinigameDone = false;
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log("Colision");
        podeCarregar = true;
    }

    public void RpcSwitch(bool value)
    {
        IsMinigameDone = value;
    }

    void MinigameWasDone(bool value)
    {
        IsMinigameDone = value;
    }

    public void OnTriggerExit2D()
    {
        podeCarregar = false;
    }

    void Update()
    {
        if (podeCarregar && !carregou && !IsMinigameDone)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                SceneManager.LoadSceneAsync("Desenho Polígono", LoadSceneMode.Additive);
                GameObject.Find("MainSceneObjectsHolder").SetActive(false);
                transform.position = Vector3.zero;
            }
        }
        else
        {
            if (podeCarregar && IsMinigameDone)
            {
                GameObject.Find("Player").GetComponent<MovimentoJogador>().SolvedHackID = ID;
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
