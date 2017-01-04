using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


public class QuizSingleTon : MonoBehaviour {

    public static QuizSingleTon quizData;
    public List<Question> answeredQuestions = new List<Question>();
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

}
