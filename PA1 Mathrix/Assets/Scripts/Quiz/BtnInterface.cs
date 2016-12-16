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

    GM_Object findObjQuestion;

    void Start () {



        //if (this.transform)
        //factText.text = findObjQuestion.;

    }
	
	// Update is called once per frame
	void Update () {
	//if(runOnce == 0)
 //       {
 //           runOnce = 1;

 //           findObjQuestion = GameObject.FindGameObjectWithTag("ListaQuizQuestions").GetComponent<GM_Object>();

 //           Question questao = findObjQuestion.currentQuestion;
 //           //Question findObjQuestion = GameObject.Find("ListaQuizQuestions").GetComponent(GM_Object).currentQuestion;
 //           Debug.Log("Tag this transform " + transform.parent.parent.tag);
 //           if (transform.parent.parent.tag == "OptionA")
 //           {
 //               factText.text = questao.solutionA;
 //           }
 //           if (transform.parent.parent.tag == "OptionB")
 //           {
 //               factText.text = questao.solutionB;

 //           }
 //           if (transform.parent.parent.tag == "OptionC")
 //           {
 //               factText.text = questao.solutionC;

 //           }
 //           if (transform.parent.parent.tag == "OptionD")
 //           {
 //               factText.text = questao.solutionD;

 //           }
 //       }
	}
}
