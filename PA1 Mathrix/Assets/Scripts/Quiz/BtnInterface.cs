using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class BtnInterface : MonoBehaviour {

    // Use this for initialization
    [SerializeField]
    public Text factText;
    int runOnce = 0;
    int questionID = 0;


    GM_Object findObjQuestion;

    void Start () {
        findObjQuestion = GameObject.FindGameObjectWithTag("ListaQuizQuestions").GetComponent<GM_Object>();


    }
	
	// Update is called once per frame
	void Update () {

        if (runOnce == 0)
        {
            runOnce = 1;
            Question questao = findObjQuestion.currentQuestion;
            //Question findObjQuestion = GameObject.Find("ListaQuizQuestions").GetComponent(GM_Object).currentQuestion;
            if (transform.parent.parent.tag == "OptionA")
            {
                //string textFact = "A)";
                //textFact = textFact + questao.solutionA;
                string textFact = string.Concat("A) " + questao.solutionA);

                factText.text = textFact;
                questionID = 1;
            }
            if (transform.parent.parent.tag == "OptionB")
            {
               // string textFact = "A)";
                string textFact = string.Concat("B) " + questao.solutionB);

                factText.text = textFact;
                questionID = 2;

            }
            if (transform.parent.parent.tag == "OptionC")
            {
                string textFact = string.Concat("C) " + questao.solutionC);

                factText.text = textFact;
                questionID = 3;


            }
            if (transform.parent.parent.tag == "OptionD")
            {
                string textFact = string.Concat("D) " + questao.solutionD);

                factText.text = textFact;
                questionID = 4;
            }
        }

        if (findObjQuestion)
        {
            runOnce = 0;
             if (questionID == 1)
            {
                findObjQuestion.updatedA = true;
            }
            if (questionID == 2)
            {
                findObjQuestion.updatedB = true;

            }
            if (questionID == 3)
            {
                findObjQuestion.updatedC = true;

            }
            if (questionID == 4)
            {
                findObjQuestion.updatedD = true;
            }
        }

	}
}
