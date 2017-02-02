using System;
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MovimentoJogador : NetworkBehaviour
{

    // Use this for initialization
    enum Direction
    {
        North,
        East,
        South,
        West
    }

    [SyncVar]
    public string PLAYERNAME;

    [SyncVar] public int SolvedHackID=-1;

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

    public InputField chatInput;

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

    void Awake()
    {
        chatInput = GameObject.Find("ChatCanvas")
            .transform.FindChild("Scroll View")
            .FindChild("InputField")
            .GetComponent<InputField>();
        SolvedHackID = -1;
    }

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        anim = this.GetComponent<Animator>();
        automatic = false;
        inputAuto = Vector2.zero;
        inputEspecial = Vector3.zero;
        OneMovement = true;
        timer = 0.1f;
        if (!isLocalPlayer)
        {
            NotLocalPlayerPositionLastPosition = transform.position;
            currentPosition = transform.position;
        }

    }

    [Command]
    public void CmdGiveStatusOfPolygonTerminalHack()
    {
        NetworkIdentity terminalNetID = GameObject.Find("HackObect").GetComponent<NetworkIdentity>();
        terminalNetID.AssignClientAuthority(terminalNetID.connectionToClient);
        terminalNetID.GetComponent<PoligonHackTerminal>().RpcSwitch(true);
        SolvedHackID = terminalNetID.GetComponent<PoligonHackTerminal>().ID;
        terminalNetID.RemoveClientAuthority(terminalNetID.connectionToClient);
    }


    void Update()
    {
        if (isClient)
        {
            if (Input.GetKeyDown(KeyCode.P) && GameObject.Find("HackObect").GetComponent<PoligonHackTerminal>().interactingPlayerIdentity==gameObject.GetComponent<NetworkIdentity>())
            {
                CmdGiveStatusOfPolygonTerminalHack();
            }
        }
        if (GameObject.Find("Network Manager").GetComponent<MyNetworkManager>().PolygonTerminalCell != null)
            CheckIfIsInFrontOfPolygonTerminal();
        transform.SetParent(GameObject.Find("Players").transform);
        isAllowedToMove = !chatInput.isFocused;
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
        if (!isLocalPlayer)
        {

            currentPosition = transform.position;
            timer -= Time.deltaTime;
            if (timer < 0) { 
                if (currentPosition != NotLocalPlayerPositionLastPosition) // sempre que o local position é diferente calcula o input
                {
                    anim.SetBool("Stop", false);

                    inputEspecial = currentPosition - NotLocalPlayerPositionLastPosition;
                    NotLocalPlayerPositionLastPosition = currentPosition;
                    anim.SetFloat("Xvalue", inputEspecial.x);
                    anim.SetFloat("Yvalue", inputEspecial.y);
                }
                else
                {
                    if (!anim.GetBool("Stop")) //false
                    {
                        anim.SetBool("Stop", true);
                        inputEspecial = Vector3.zero;

                        Debug.Log("Entrie uma vez!");
                        NotLocalPlayerPositionLastPosition = currentPosition;
                        anim.SetFloat("Xvalue", 0f);
                        anim.SetFloat("Yvalue", 0f);
                    }

                }
                timer = 0.1f;
            }

            //if (inputEspecial != Vector3.zero)
            //{
            //    anim.SetBool("Stop", false);
            //    anim.SetBool("OnceStop", false);


            //    Debug.Log("Especial Devia estar a andar " + inputEspecial + inputEspecial + anim.GetFloat("Xvalue") + anim.GetFloat("Yvalue"));

            //    //anim.SetFloat("Xvalue", gameObject.GetComponent<NetworkTransform>().transform.position.x);

            //    NotLocalPlayerPositionLastPosition = currentPosition;
            //    //NotLocalPlayerPositionLastPosition = NotLocalPlayerPositionLastPosition - gameObject.GetComponent<NetworkTransform>().
            //}


        }
        else
        {
            GameObject.Find("Camera").GetComponent<Camera>().transform.position = transform.position;
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
            if (isLocalPlayer)
            {
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
        if(isServer)
            return;

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
        if(!orderToStop)
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

            endPos = new Vector3(startPos.x + inputAuto.x, startPos.y +2f ,startPos.z);

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
                t += 0.007f* walkSpeed;
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
