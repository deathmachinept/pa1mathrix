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

	        if (trainPos.x > -18.3)
	        {
	            trainPos = new Vector3(trainPos.x - 0.05f, trainPos.y, trainPos.z);
	            this.transform.localPosition = trainPos;
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

        if (children == 2)
        {
            if (
                GameObject.FindGameObjectWithTag("ManagerPlayers")
                    .transform.GetChild(1)
                    .gameObject.GetComponent<MovimentoJogador>()
                    .estaDentroDoComboio)
            {
            Debug.Log("Devia executar aqui!");
                partir = true;
            }
        }
        else
        {
            int contar = 0;
            foreach (Transform player in GameObject.FindGameObjectWithTag("ManagerPlayers").transform)
            {
                if (player.gameObject.name == "Player(Clone)")
                {

                    if (player.gameObject.GetComponent<MovimentoJogador>().estaDentroDoComboio)
                    {
                        contar++;
                        if (contar + 1 == children) // por causa de jogador no NetworkManagers Player
                        {
                            partir = true;
                        }
                    }
                }
            }
        }


    }
}
