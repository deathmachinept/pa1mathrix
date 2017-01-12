using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MaterialUI;


public class GM_Object : MonoBehaviour {

    public List<Question> listaQuestoes; //= new List<Question>();
    public static List<Question> unansweredQuestions;
    public Question currentQuestion;
    private int userAnswer;
    public bool acertou = false, updatedA = false, updatedB = false, updatedC = false, updatedD = false;
    private GameObject findObjQuestion;
    [SerializeField]
    private Text factText;

    [SerializeField]
    private float timeBetweenQuestion = 2f;

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
            acertou = true;
            if (userAnswer == 1)
            {

                GameObject Original = GameObject.Find("Canvas").transform.FindChild("OptionB").FindChild("ButtonLayer").gameObject;
                GameObject optionC = (GameObject)Instantiate(Original).gameObject;

                optionC.GetComponentInChildren<Image>().color = Color.green;

            }
            if (userAnswer == 2)
            {
                GameObject Original = GameObject.Find("Canvas").transform.FindChild("OptionB").FindChild("ButtonLayer").gameObject;
                GameObject optionC = (GameObject)Instantiate(Original).gameObject;

                optionC.GetComponentInChildren<Image>().color = Color.green;

                //GameObject.Find("Canvas").transform.FindChild("OptionB").FindChild("ButtonLayer").GetComponent<Image>().color = Color.Green;

            }
            if (this.userAnswer == 3)
            {
                GameObject Original = GameObject.Find("Canvas").transform.FindChild("OptionB").FindChild("ButtonLayer").gameObject;
                GameObject optionC = (GameObject) Instantiate(Original).gameObject;

                optionC.GetComponentInChildren<Image>().color = Color.green;

        //        GameObject tempMembro = (GameObject)Instantiate(MatrizPrint[i], currentPosition, Quaternion.identity);

        //        Image Test = GameObject.Find("Canvas")
        //.transform.FindChild("OptionB")
        //.FindChild("ButtonLayer")
        //.GetComponent<Image>();
        //        Test.color = new Color(110, 23, 0);

               // GameObject.Find("Canvas").transform.FindChild("OptionC").FindChild("ButtonLayer").GetComponent<Image>().color = Color.green;

            }
            if (this.userAnswer == 4)
            {
                GameObject Original = GameObject.Find("Canvas").transform.FindChild("OptionB").FindChild("ButtonLayer").gameObject;
                GameObject optionC = (GameObject)Instantiate(Original).gameObject;

                optionC.GetComponentInChildren<Image>().color = Color.green;
            }
            StartCoroutine(TransitionToNextQuestion());

            Debug.Log(true);
        }

    }

    void Update()
    {
        if (updatedA)
            if(updatedB)
                if(updatedC)
                   if(updatedD){
                       acertou = false;
                       updatedA = false;
                       updatedB = false;
                       updatedC = false;
                       updatedD = false;
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
