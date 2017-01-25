using UnityEngine;
using System.Collections;

public class callTrain : MonoBehaviour
{

    public GameObject Terminal1;
    public GameObject Terminal2;
    private bool trainWasCalled = false;

	// Update is called once per frame
	void Update () {
	    if (Terminal1.GetComponent<PoligonHackTerminal>().IsMinigameDone &&
	        Terminal2.GetComponent<triggerSimplificacao>().IsMinigameDone&&!trainWasCalled)
	    {
	        trainWasCalled = true;
            GameObject train = GameObject.FindGameObjectWithTag("Train");
            // endPos = startPos;
            train.GetComponent<MoveTrain>().triggerMove = true;
        }
	}
}
