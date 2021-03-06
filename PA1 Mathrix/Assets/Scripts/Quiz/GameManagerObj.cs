﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GameManager : MonoBehaviour {

    // Use this for initialization
    public List<Question> listaQuestoes; //= new List<Question>();
    public static List<Question> unansweredQuestions;
    public Question currentQuestion;
    private int userAnswer;

    [SerializeField]
    private Text factText;

    [SerializeField]
    private float timeBetweenQuestion = 1f;

    void Start()
    {
        if (unansweredQuestions == null || unansweredQuestions.Count == 0)
        {
            unansweredQuestions = listaQuestoes.ToList<Question>();
        }
        getRandomQuestion();
    }

    void getRandomQuestion()
    {
        int randomQuestionIndex = Random.Range(0, unansweredQuestions.Count);
        currentQuestion = unansweredQuestions[randomQuestionIndex];
        factText.text = currentQuestion.fact;

    }

    IEnumerator TransitionToNextQuestion()
    {
        unansweredQuestions.Remove(currentQuestion);
        yield return new WaitForSeconds(timeBetweenQuestion);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void UserAnsweredCorrectly(int userAnswer)
    {
        if (currentQuestion.answer != userAnswer)
        {
            Debug.Log(false);

        }
        if (currentQuestion.answer == userAnswer)
        {
            Debug.Log(true);
            StartCoroutine(TransitionToNextQuestion());
        }

    }

    public void OptionA()
    {
        userAnswer = 1;
        UserAnsweredCorrectly(userAnswer);
    }
    public void OptionB()
    {
        userAnswer = 2;
        UserAnsweredCorrectly(userAnswer);

    }
    public void OptionC()
    {
        userAnswer = 3;
        UserAnsweredCorrectly(userAnswer);

    }
    public void OptionD()
    {
        userAnswer = 4;
        UserAnsweredCorrectly(userAnswer);

    }
}
