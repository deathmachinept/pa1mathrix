using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class triggerSimplificacao : MonoBehaviour {

    private bool podeCarregar = false;
    private bool carregou = false;
    private bool loadCameraOnce = false;
    public GameObject[] camerasOnScene;
    private AsyncOperation op;
    public bool IsMinigameDone = true;

	// Use this for initialization
    public void OnTriggerEnter2D()
    {
        podeCarregar = true;
    }

    public void OnTriggerExit2D()
    {
        podeCarregar = false;
    }

    void Update()
    {
        if (podeCarregar && !carregou)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {

                    Camera.main.enabled = false;
                    camerasOnScene[0].tag = "Untagged";
                    op = SceneManager.LoadSceneAsync("SimplificacaoMatrizes", LoadSceneMode.Additive);
                    GameObject.FindGameObjectWithTag("ChatCanvas").SetActive(false);

                loadCameraOnce = true;
                    carregou = true;
                Debug.Log("Corre!");
            }
        }else if (Input.GetKeyDown(KeyCode.F))
            {
                if (carregou)
                {
                    camerasOnScene[1].GetComponent<Camera>().enabled = false;
                    camerasOnScene[0].tag = "MainCamera";

                    camerasOnScene[0].GetComponent<Camera>().enabled = true;
                    carregou = false;
                }

            }

        if (loadCameraOnce && carregou)
        {
            if (op.isDone)
            {
                camerasOnScene[1] = GameObject.FindGameObjectWithTag("MainCamera");
                loadCameraOnce = false;
            }
        }
    }

}
