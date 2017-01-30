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

    public List<MovimentoJogador> Players;
    public Text textbox;
    
    //public Guard_SM sm;
    public void Awake()
    {
        Players = GameObject.Find("Players").transform.GetComponentsInChildren<MovimentoJogador>().ToList();
        textbox = transform.FindChild("Canvas").FindChild("Text").GetComponent<Text>();
        //sm = new Test_SM(this, Player);
    }

    public void Update()
    {
        //if (sm.CurrentState != null)
        //    textbox.text = sm.CurrentState.Name;
        //sm.update();
    }
}
