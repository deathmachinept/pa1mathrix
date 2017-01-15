using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class triggerSimplificacao : MonoBehaviour {

    bool podeCarregar = false;
    private bool carregou = false;


	// Use this for initialization
    public void OnTriggerEnter2D()
    {

        podeCarregar = true;
        // endPos = startPos;

    }

    void Update()
    {
        if (podeCarregar && !carregou)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                SceneManager.LoadSceneAsync("SimplificacaoMatrizes", LoadSceneMode.Additive);
                carregou = true;
            }
        }
    }

}
