using UnityEngine;
using System.Collections;

public class QuizSingleTon : MonoBehaviour {

    public static QuizSingleTon quizData;
	// Use this for initialization
	void Awake () {

        if (quizData == null)
        {
            DontDestroyOnLoad(gameObject);
            quizData = this;
        }
        else if (quizData != null)
        {
            Destroy(gameObject);
        }

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
