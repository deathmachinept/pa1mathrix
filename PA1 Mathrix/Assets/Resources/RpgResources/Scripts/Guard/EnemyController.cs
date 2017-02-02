using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine.Networking;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour {
    public int Health;
    public GameObject Player;
    public float AttackRange;
    public int AttackStrength;
    public int TimeBetweenAttacks;

    public int CurrentGuardPointIndex=0;
    public int ClosestGuardPointIndex;
    public int SecondsToGuardPoint;
    public bool HasGuardedPoint = false;

    public bool PlayerVisible;

    public GameObject Players;
    public Text textbox;
    public GuardFSM sm;
    public void Awake()
    {
        Player = GameObject.Find("Player");
        textbox = transform.FindChild("Canvas").FindChild("Text").GetComponent<Text>();

        gameObject.AddComponent<GuardFSM_Action_GuardPoint>();
        GetComponent<GuardFSM_Action_GuardPoint>().enemy=this;
        GetComponent<GuardFSM_Action_GuardPoint>().CoroutineIsRunning= false;
        GetComponent<GuardFSM_Action_GuardPoint>().Seconds = 3;

        gameObject.AddComponent<GuardFSM_Action_MoveToAPoint>();
        GetComponent<GuardFSM_Action_MoveToAPoint>().enemy = this;
        GetComponent<GuardFSM_Action_MoveToAPoint>().TimeToTake = 3;
        GetComponent<GuardFSM_Action_MoveToAPoint>().isMoving = false;
    }

    public void Start()
    {
        sm = new GuardFSM(this);
    }

    public void Update()
    {
        if (Player != null)
        {
            GetComponent<GuardFSM_Action_MoveToAPoint>().Destination = Player.transform.position;
        }
        else
        {
            GetComponent<GuardFSM_Action_MoveToAPoint>().Destination =
                GlobalVariables.singleton.GuardPoints[CurrentGuardPointIndex];
        }

        if (Input.GetKeyDown(KeyCode.P) && !GetComponent<GuardFSM_Action_GuardPoint>().CoroutineIsRunning)
        {
            GetComponent<GuardFSM_Action_GuardPoint>().DoAction();
        }
        textbox.text = (!gameObject.GetComponent<GuardFSM_Action_GuardPoint>().CoroutineIsRunning).ToString();
        sm.update();
    }
}