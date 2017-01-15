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

    private Direction currentDir;
    private Direction oldDirection;
    private Vector2 input;
    private bool isMoving = false, collided = false, startedTheGame = true;
    private Vector3 startPos;
    private Vector3 endPos;
    private float t;

    public GameObject PolygonTerminalCell;

    public InputField chatInput;

    public Sprite NorthSprite;
    public Sprite EastSprite;
    public Sprite SouthSprite;
    public Sprite WestSprite;


    public float walkSpeed = 3f;
    public bool isAllowedToMove = true;

    void Awake()
    {
        chatInput = GameObject.Find("ChatCanvas")
            .transform.FindChild("Scroll View")
            .FindChild("InputField")
            .GetComponent<InputField>();
    }

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        isAllowedToMove = true;
    }

    void Update()
    {
        if (GameObject.Find("Network Manager").GetComponent<MyNetworkManager>().PolygonTerminalCell != null)
            CheckIfIsInFrontOfPolygonTerminal();
        transform.SetParent(GameObject.Find("Players").transform);
        isAllowedToMove = !chatInput.isFocused;
        ProcessMovement();
    }

    void ProcessMovement()
    {
        if (!isLocalPlayer)
            return;
        GameObject.Find("Camera").GetComponent<Camera>().transform.position = transform.position;
        if (!isMoving && isAllowedToMove)
        {
            oldDirection = currentDir;
            //Debug.Log("input X" + input.y + " Input Y " + input.y);
            input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            if (Mathf.Abs(input.x) > Mathf.Abs(input.y))
            {
                input.y = 0;
            }
            else
            {
                input.x = 0;
            }
            //Debug.Log("input X" + input.y + " Input Y " + input.y);

            if (input != Vector2.zero)
            {
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

                switch (currentDir)
                {
                    case Direction.North:
                        gameObject.GetComponent<SpriteRenderer>().sprite = NorthSprite;
                        break;
                    case Direction.East:
                        gameObject.GetComponent<SpriteRenderer>().sprite = EastSprite;
                        break;
                    case Direction.South:
                        gameObject.GetComponent<SpriteRenderer>().sprite = SouthSprite;
                        break;
                    case Direction.West:
                        gameObject.GetComponent<SpriteRenderer>().sprite = WestSprite;

                        break;
                }

                //start corountine

                StartCoroutine(Move(transform));
            }
        }
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

    public void OnTriggerEnter2D()
    {

            collided = true;
           // endPos = startPos;

    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(isServer)
            return;
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
        isAllowedToMove = true;
        Debug.Log("Existe!!");
    }

    public IEnumerator Move(Transform entity)
    {
        isMoving = true;
        startPos = entity.position;
        t = 0;

        //Debug.Log("Old direction : " + oldDirection + " currentDir " + currentDir);
        if (isAllowedToMove)
        {
            endPos = new Vector3(startPos.x + System.Math.Sign(input.x), startPos.y + System.Math.Sign(input.y),
                startPos.z);
            isAllowedToMove = true;
            //Debug.Log("is allowed to move");

        }
        else
        {

            Debug.Log("is not allowed to move");

        }

        //if same direction and last 

        if (!collided)
        {

            while (t < 1f)
            {
                t += Time.deltaTime * walkSpeed;
                entity.position = Vector3.Lerp(startPos, endPos, t);
                //Debug.Log("entity.position");
                yield return null;
            }

        }
        else
        {
            if (currentDir != oldDirection)
            {
                collided = false;
            }
            //entity.position = entity.position;
        }

        isMoving = false;
        yield return 0;
    }

}
