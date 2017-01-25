using UnityEngine;
using System.Collections;

public class callTrain : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void OnTriggerEnter2D()
    {

        GameObject train = GameObject.FindGameObjectWithTag("Train");
        // endPos = startPos;
        train.GetComponent<MoveTrain>().triggerMove = true;
    }
}
