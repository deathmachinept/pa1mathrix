using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour {
    public void StartExpressionGame()
    {
        SceneManager.LoadSceneAsync("SimplificacaoMatrizes");
    }

    public void StartPolygonGame()
    {
        SceneManager.LoadSceneAsync("Desenho Polígono");
    }
}
