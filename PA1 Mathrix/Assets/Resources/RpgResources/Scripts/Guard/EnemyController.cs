using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine.Networking;
using UnityEngine.UI;

public class EnemyController : NetworkBehaviour {
    public int Health;
    public NetworkIdentity PlayerToAttack;
    public float AttackRange;
    public int AttackStrength;
    public int TimeBetweenAttacks;

    public int CurrentGuardPointIndex=0;
    public int ClosestGuardPointIndex;
    public int SecondsToGuardPoint;
    public bool HasGuardedPoint = false;

    public GameObject Players;
    public Text textbox;
    public GuardFSM sm;

    public void Awake()
    {
        Players = GameObject.Find("Players");
        textbox = transform.FindChild("Canvas").FindChild("Text").GetComponent<Text>();

        gameObject.AddComponent<GuardFSM_Action_GuardPoint>();
        GetComponent<GuardFSM_Action_GuardPoint>().enemy=this;
        GetComponent<GuardFSM_Action_GuardPoint>().CoroutineIsRunning= false;
        GetComponent<GuardFSM_Action_GuardPoint>().Seconds = 5;

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
        if (PlayerToAttack != null)
        {
            GetComponent<GuardFSM_Action_MoveToAPoint>().Destination = PlayerToAttack.transform.position;
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
        //textbox.text = GetComponent<GuardFSM_Action_GuardPoint>().CoroutineIsRunning.ToString();

        sm.update();
        textbox.text = sm.CurrentState.Name;
    }

    public GameObject GetPlayerToAttackFromPlayerList()
    {
        foreach (Transform player in Players.transform)
        {
            if (player.gameObject.GetComponent<NetworkIdentity>() == PlayerToAttack)
            {
                return player.gameObject;
            }
        }
        return null;
    }
}