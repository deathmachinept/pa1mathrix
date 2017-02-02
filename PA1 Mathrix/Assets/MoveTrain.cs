using UnityEngine;
using System.Collections;

public class MoveTrain : MonoBehaviour {

	// Use this for initialization
    public bool triggerMove = false;
    private Vector3 trainPos;
    private Animator animTrain;
    private bool PlayOpenDoorAnimation;
    private bool podeCarregar, carregouOnce, onceAnimationDoors, checkNofPlayerOnce;
    private GameObject playerTriggered;
    private bool partir;
    private float timerToLeave;
    private int children;
    float countMovement = 0f;
	// Update is called once per frame

    public void OnTriggerEnter2D(Collider2D other)
    {

        if(other.tag == "Player"){
        playerTriggered = other.gameObject;
        podeCarregar = true;
        Debug.Log("Pode Carregar!");
            }
    }

    public void OnTriggerExit2D()
    {
        podeCarregar = false;
        checkNofPlayerOnce = false;
        carregouOnce = false;
    }


    void Start()
    {
        trainPos = this.transform.localPosition;
        animTrain = this.GetComponent<Animator>();
        PlayOpenDoorAnimation = false; onceAnimationDoors = false;
        podeCarregar = false;
        carregouOnce = false;
        partir = false;
        timerToLeave = 3f;

    }



	void Update () {

        if (!checkNofPlayerOnce)
        {
            children = GameObject.FindGameObjectWithTag("ManagerPlayers").transform.childCount;
            checkNofPlayerOnce = true;
        }

        if (podeCarregar && !carregouOnce)
	    {
	        if (Input.GetKeyDown(KeyCode.F))
	        {
                Debug.Log("Pos " + playerTriggered.transform.localPosition + " " + playerTriggered.GetComponent<MovimentoJogador>().isAllowedToMove);
	            playerTriggered.GetComponent<MovimentoJogador>().isMoving = true;
	            playerTriggered.GetComponent<MovimentoJogador>().inputAuto.y = 3f;
	            playerTriggered.GetComponent<MovimentoJogador>().automatic = true;



                Debug.Log("Pos " + playerTriggered.transform.localPosition + " " + playerTriggered.GetComponent<MovimentoJogador>().isAllowedToMove);

	                carregouOnce = true;
	 
                Debug.Log("Carregou!!");
	        }

	    }


	    if (triggerMove)
	    {

	        if (countMovement < 17)
	        {
	            trainPos = new Vector3(trainPos.x - 0.05f, trainPos.y, trainPos.z);
	            this.transform.localPosition = trainPos;
                countMovement+= 0.05f;
	        }
	        else
	        {
                GameObject.FindGameObjectWithTag("TrainDoorsLeft").GetComponent<DoorLeftOpenAnimScript>().executeOpenAnim = true;
                GameObject.FindGameObjectWithTag("TrainDoorsRight").GetComponent<DoorRightOpenAnimScript>().executeOpenAnim = true;
                Debug.Log("antes de contar");
                triggerMove = false;
	        }
	    }

	    if (partir)
	    {

	        if (onceAnimationDoors)
	        {
	            if (timerToLeave <= 0)
	            {
	                trainPos = new Vector3(trainPos.x - 0.05f, trainPos.y, trainPos.z);
	                this.transform.localPosition = trainPos;
	                foreach (Transform player in GameObject.FindGameObjectWithTag("ManagerPlayers").transform)
	                {
	                    if (player.gameObject.name == "Player(Clone)")
	                    {
	                        player.gameObject.GetComponent<SpriteRenderer>().enabled = false;
	                    }
	                }
	                GameObject.FindGameObjectWithTag("Win").transform.position =
	                    GameObject.FindGameObjectWithTag("MainCamera").transform.position;
	                Animator play = GameObject.FindGameObjectWithTag("Win").transform.GetComponent<Animator>();
	                play.SetBool("Venceu", true);


	            }

	            timerToLeave -= Time.deltaTime;
	        }

	        else if (!onceAnimationDoors)
	        {
	            GameObject.FindGameObjectWithTag("TrainDoorsLeft").GetComponent<DoorLeftOpenAnimScript>().executeCloseAnim
	                = true;
	            GameObject.FindGameObjectWithTag("TrainDoorsRight")
	                .GetComponent<DoorRightOpenAnimScript>()
	                .executeCloseAnim = true;
	            onceAnimationDoors = true;
	        }

	    }


	}

    public void checkPlayerInTrain()
    {
        Debug.Log("Devia executar aqui!1 " + children);
        int contar = 0;

        if (children ==1)
        {
            if (
                GameObject.FindGameObjectWithTag("ManagerPlayers")
                    .transform.GetChild(0)
                    .gameObject.GetComponent<MovimentoJogador>()
                    .estaDentroDoComboio)
            {
            Debug.Log("Devia executar aqui!");
                partir = true;
            }
        }
        else
        {
            foreach (Transform player in GameObject.FindGameObjectWithTag("ManagerPlayers").transform)
            {
                if (player.gameObject.name == "Player(Clone)")
                {

                    if (player.gameObject.GetComponent<MovimentoJogador>().estaDentroDoComboio)
                    {
                        contar++;
                           
                        
                    }
                }
            }
            if(contar == children)
            {
                partir = true;
            }
        }


    }
}
