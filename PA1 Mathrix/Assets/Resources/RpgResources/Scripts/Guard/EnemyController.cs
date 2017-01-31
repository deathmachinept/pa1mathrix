using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine.Networking;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour {
    public int Health;
    public float SightRange;
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
    public void Awake()
    {
        Players = GameObject.Find("Players");
        textbox = transform.FindChild("Canvas").FindChild("Text").GetComponent<Text>();
        gameObject.AddComponent<GuardFSM_Action_GuardPoint>();
        GetComponent<GuardFSM_Action_GuardPoint>().enemy=this;
        GetComponent<GuardFSM_Action_GuardPoint>().CoroutineIsRunning= false;
        GetComponent<GuardFSM_Action_GuardPoint>().Seconds = 5;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            GetComponent<GuardFSM_Action_GuardPoint>().DoAction();
        }
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
//CLASSE DA ACÇÃO COM COROTINA
//public class GuardFSM_Action_GuardPoint : MonoBehaviour, IAction
//{
//    public EnemyController enemy;
//    public int Seconds;
//    public bool CoroutineIsRunning;
//    public GuardFSM_Action_GuardPoint(EnemyController Character_That_Waits, int Seconds_To_Wait)
//    {
//        enemy = Character_That_Waits;
//        Seconds = Seconds_To_Wait;
//        CoroutineIsRunning = false;
//    }
//    public void DoAction()
//    {
//        CoroutineIsRunning = true;
//        StartCoroutine(WaitCoroutine(enemy, Seconds));
//    }

//    public IEnumerator WaitCoroutine(EnemyController Enemy, int secondsToWait)
//    {
//        Debug.Log("Entra na corotina");
//        Enemy.HasGuardedPoint = false;
//        yield return new WaitForSecondsRealtime(secondsToWait);
//        Enemy.HasGuardedPoint = true;
//        CoroutineIsRunning = false;
//    }
//}