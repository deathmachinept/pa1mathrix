using UnityEngine;
using System.Collections;

public class callTrain : MonoBehaviour
{

    public GameObject Terminal1;
    private bool trainWasCalled = false;

	// Update is called once per frame
	void Update () {
	    if (Terminal1.GetComponent<PoligonHackTerminal2>().IsMinigameDone && !trainWasCalled)
	    {
	        trainWasCalled = true;
	        GameObject train = GameObject.FindGameObjectWithTag("Train");
	        // endPos = startPos;
	        train.GetComponent<MoveTrain>().triggerMove = true;
	    }
	}

    bool CheckforGameCompletion()
    {
        GameObject playersHolder=GameObject.Find("Players");
        bool solvedA = false;
        bool solvedB = false;
        foreach (Transform t in playersHolder.transform)
        {
            if (t.GetComponent<MovimentoJogador>().SolvedHackID == 0)
                solvedA = true;
            if (t.GetComponent<MovimentoJogador>().SolvedHackID == 1)
                solvedB = true;
        }
        if ((playersHolder.transform.childCount==2 && (solvedA&&solvedB))|| (playersHolder.transform.childCount == 1 && (solvedA || solvedB)))
            return true;
        return false;
    }
}
