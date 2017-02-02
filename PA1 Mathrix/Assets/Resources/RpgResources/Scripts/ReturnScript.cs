using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class ReturnScript : MonoBehaviour
{
    public void ReturnToMainScene()
    {
        SceneManager.UnloadScene(GameObject.Find("Network Manager").GetComponent<MyNetworkManager>().CurrentSceneName);
        GameObject.Find("GameObjectManager").GetComponent<GameController>().MainSceneObjects.SetActive(true);
    }

    public void ReturnToMainScene(string MinigameName,bool isVictory)
    {
        SceneManager.UnloadScene(GameObject.Find("Network Manager").GetComponent<MyNetworkManager>().CurrentSceneName);
        GameObject.Find("GameObjectManager").GetComponent<MyNetworkManager>().MainSceneObjects.SetActive(true);
        if (isVictory)
        {
            if (MinigameName == "Polygon Game")
            {
                GameObject.Find("MainSceneObjectsHolder")
                    .transform.FindChild("TrainCallHolder")
                    .GetComponent<callTrain>()
                    .Terminal1.GetComponent<PoligonHackTerminal>()
                    .IsMinigameDone = true;
                GameObject.Find("MainSceneObjectsHolder")
                    .transform.FindChild("TrainCallHolder")
                    .GetComponent<callTrain>()
                    .Terminal1.GetComponent<PoligonHackTerminal>()
                    .podeCarregar=false;
            }
        }
    }
}
