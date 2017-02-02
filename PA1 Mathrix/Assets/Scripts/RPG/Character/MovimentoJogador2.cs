using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class MovimentoJogador2 : MonoBehaviour {

    // Use this for initialization
    enum Direction
    {
        North,
        East,
        South,
        West
    }

    public string PLAYERNAME;

    public int SolvedHackID = -1;

    private Animator anim;
    private string layerTemporario, tagColliderTemporario;
    private Direction currentDir;
    private Direction oldDirection;
    private Vector2 input;
    public bool isMoving = false, collided = false, startedTheGame = true;
    private Vector3 startPos;
    private Vector3 endPos;
    private float t;
    public bool automatic;
    public Vector2 inputAuto;
    public bool OneMovement;
    public GameObject PolygonTerminalCell;


    public Sprite NorthSprite;
    public Sprite EastSprite;
    public Sprite SouthSprite;
    public Sprite WestSprite;

    public bool orderToStop = false;
    public float walkSpeed = 3f;
    public bool isAllowedToMove;

    public bool estaDentroDoComboio = false;

    public Vector3 NotLocalPlayerPositionLastPosition;
    public Vector3 currentPosition;
    public Vector3 inputEspecial;
    public float timer;

    public int Health = 100;



    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        anim = this.GetComponent<Animator>();
        automatic = false;
        inputAuto = Vector2.zero;
        inputEspecial = Vector3.zero;
        OneMovement = true;
        timer = 0.1f;
        isAllowedToMove = true;
        //if (!isLocalPlayer)
        //{
        //    NotLocalPlayerPositionLastPosition = transform.position;
        //    currentPosition = transform.position;
        //}

    }




    void Update()
    {
        //if (isClient)
        //{
        //    if (Input.GetKeyDown(KeyCode.P) && GameObject.Find("HackObect").GetComponent<PoligonHackTerminal>().interactingPlayerIdentity == gameObject.GetComponent<NetworkIdentity>())
        //    {
        //        CmdGiveStatusOfPolygonTerminalHack();
        //    }
        //}
        //if (GameObject.Find("Network Manager").GetComponent<MyNetworkManager>().PolygonTerminalCell != null)
        //    CheckIfIsInFrontOfPolygonTerminal();
        transform.SetParent(GameObject.FindGameObjectWithTag("Player").transform);

        if (!automatic)
        {
            ProcessMovement();
        }
        else if (automatic && OneMovement)
        {
            automaticMovement();
            OneMovement = false;
        }
    }

    void ProcessMovement()
    {


            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().transform.position = transform.position;
            if (!isMoving && isAllowedToMove)
            {
                oldDirection = currentDir;
                input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

                if (Mathf.Abs(input.x) > Mathf.Abs(input.y)) //escolhe a direção se for um em X nao move no Y
                {
                    input.y = 0;
                }
                else
                {
                    input.x = 0;
                }

                anim.SetFloat("Xvalue", input.x);
                anim.SetFloat("Yvalue", input.y);

                if (input != Vector2.zero)
                {
                    //anim.SetTrigger("Moving");
                    anim.SetBool("Stop", false);
                    anim.SetBool("OnceStop", false);

                    if (input.x < 0)
                    {
                        currentDir = Direction.West;
                    }
                    if (input.x > 0)
                    {
                        currentDir = Direction.East;
                    }
                    if (input.y < 0)
                    {
                        currentDir = Direction.South;
                    }
                    if (input.y > 0)
                    {
                        currentDir = Direction.North;
                    }

                    StartCoroutine(Move(transform));
                }
                else
                {
                    if (!anim.GetBool("Stop")) //false
                    {
                        anim.SetBool("Stop", true);
                        //anim.SetBool("OnceStop",true);
                    }

                }

            }
    }

    public void automaticMovement()
    {
        anim.SetBool("Stop", false);
        anim.SetFloat("Xvalue", inputAuto.x);
        anim.SetFloat("Yvalue", inputAuto.y);

        StartCoroutine(MoveAuto(transform));

    }

    public void CheckIfIsInFrontOfPolygonTerminal()
    {
        GameObject cell = PolygonTerminalCell;
        if (transform.position.x > cell.transform.position.x - (cell.GetComponent<SpriteRenderer>().bounds.size.x / 2) &&
            transform.position.x < cell.transform.position.x + (cell.GetComponent<SpriteRenderer>().bounds.size.x / 2) &&
            transform.position.y > cell.transform.position.y - (cell.GetComponent<SpriteRenderer>().bounds.size.y / 2) &&
            transform.position.y < cell.transform.position.y + (cell.GetComponent<SpriteRenderer>().bounds.size.y / 2))
        {

                SceneManager.LoadSceneAsync("Desenho Polígono", LoadSceneMode.Additive);
                GameObject.Find("MainSceneObjectsHolder").SetActive(false);
                GameObject.FindGameObjectWithTag("Player").SetActive(false);
                //GameObject.Find("Network Manager").GetComponent<MyNetworkManager>().CurrentSceneName =
                //    "Desenho Polígono";
                transform.position = Vector3.zero;
                //GameObject.Find("Network Manager").GetComponent<MyNetworkManager>().players.SetActive(false);
                //GameObject.Find("ChatCanvas").SetActive(false);
            
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Teste 1");

        //layerTemporario = other.transform.GetComponent<SpriteRenderer>().sortingLayerName;
        //tagColliderTemporario = other.transform.GetComponent<SpriteRenderer>().tag;

        //if (tagColliderTemporario == "Train")
        //{
        //    other.transform.GetComponent<Animator>().set
        //}

        if (other.transform.GetComponent<SpriteRenderer>() != null)
        {
            if (other.transform.GetComponent<SpriteRenderer>().sortingLayerName != "Floor")
            {
                Debug.Log("Pare!");
                collided = true;

            }
        }

        // endPos = startPos;
        //Debug.Log("OntriggerEnter2D " + other.transform.GetComponent<SpriteRenderer>().sortingLayerName);

    }



    public void OnCollisionEnter2D(Collision2D collision)
    {


        Debug.Log("Teste 2");

        if (collision.transform.GetComponent<SpriteRenderer>().sortingLayerName != "Floor")
        {
            collided = true;

            Debug.Log("Teste 3");
        }

        if (collision.collider.gameObject.tag == "PolygonTerminal")
        {
            Debug.Log("MiniJogo");

            SceneManager.LoadSceneAsync("Desenho Polígono", LoadSceneMode.Additive);
            GameObject.Find("MainSceneObjectsHolder").SetActive(false);
            GameObject.Find("Players").SetActive(false);
            GameObject.Find("Network Manager").GetComponent<MyNetworkManager>().CurrentSceneName =
                "Desenho Polígono";
            transform.position = Vector3.zero;
            GameObject.Find("Network Manager").GetComponent<MyNetworkManager>().players.SetActive(false);
            GameObject.Find("ChatCanvas").SetActive(false);
        }

    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        collided = false;
        if (!orderToStop)
            isAllowedToMove = true;
        Debug.Log("Existe!!");
    }

    public IEnumerator Move(Transform entity)
    {
        isMoving = true;
        startPos = entity.position;
        t = 0;

        if (isAllowedToMove)
        {
            endPos = new Vector3(startPos.x + System.Math.Sign(input.x), startPos.y + System.Math.Sign(input.y),
                startPos.z);
            if (!orderToStop)
                isAllowedToMove = true;

        }
        else
        {
            Debug.Log("is not allowed to move");
        }

        if (!collided)
        {
            while (t < 1f)
            {
                t += Time.deltaTime * walkSpeed;
                entity.position = Vector3.Lerp(startPos, endPos, t);
                yield return null;
            }
        }
        else
        {
            if (currentDir != oldDirection)
            {
                collided = false;
            }
        }

        isMoving = false;
        yield return 0;
    }


    public IEnumerator MoveAuto(Transform entity)
    {
        isMoving = true;
        startPos = entity.position;
        t = 0;

        //Debug.Log("Old direction : " + oldDirection + " currentDir " + currentDir);
        if (isAllowedToMove)
        {
            Debug.Log("Allowed to move!");
            //endPos = new Vector3(startPos.x + System.Math.Sign(inputAuto.x), startPos.y + System.Math.Sign(inputAuto.y),
            //    startPos.z);

            endPos = new Vector3(startPos.x + inputAuto.x, startPos.y + 2f, startPos.z);

            if (!orderToStop)
                isAllowedToMove = true;
        }
        else
        {
            Debug.Log("is not allowed to move");
        }

        collided = false;
        if (!collided)
        {
            while (t < 1f)
            {
                t += 0.007f * walkSpeed;
                entity.position = Vector3.Lerp(startPos, endPos, t);
                if (entity.position == endPos)
                {
                    inputAuto = Vector2.zero;

                    estaDentroDoComboio = true;
                    anim.SetBool("Stop", true);
                    anim.SetFloat("Xvalue", 0);
                    anim.SetFloat("Yvalue", 0);
                    this.GetComponentInChildren<Canvas>().enabled = false;
                    GameObject.FindGameObjectWithTag("Train").GetComponent<MoveTrain>().checkPlayerInTrain();
                }
                yield return null;
            }
        }
        else
        {
            if (currentDir != oldDirection)
            {
                collided = false;
            }
        }

        isMoving = false;

        yield return 0;
    }

}
