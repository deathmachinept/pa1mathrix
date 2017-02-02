using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Xml.Serialization;
using JetBrains.Annotations;
using UnityEngine.Networking;

//Componentes Principais das Máquina de Estados
//Interfaces
public interface IAction
{
    void DoAction();
}
public interface ITransition
{
    bool CanPerformTransition();
}
public interface ICondition
{
    bool test();
}

//Classes
public class Transition : ITransition
{
    public SM parentMachine;
    public List<IAction> Actions;
    public List<ICondition> ConditionsList;
    public State targetState;

    public bool CanPerformTransition()
    {
        foreach (ICondition condition in ConditionsList)
        {
            if (!condition.test())
                return false;
        }
        return true;
    }
}

public class State
{
    public string Name;
    public SM parentMachine;
    public List<IAction> Actions;
    public IAction EntryAction;
    public IAction ExitAction;
    public List<Transition> transitions;

    public Transition CheckForTriggeredTransition()
    {
        foreach (Transition t in transitions)
        {
            bool AllConditionsAreMet = true;
            foreach (ICondition condition in t.ConditionsList)
            {
                if (!condition.test())
                    AllConditionsAreMet = false;
            }
            if (AllConditionsAreMet)
            {
                return t;
            }
        }
        return null;
    }
}

public class HSMTransition : ITransition
{
    public int TransitionLevel;
    public HSMState Parent;
    public List<IAction> Actions;
    public List<ICondition> ConditionsList;
    public HSMState targetState;

    public bool CanPerformTransition()
    {
        foreach (ICondition condition in ConditionsList)
        {
            if (!condition.test())
                return false;
        }
        return true;
    }
}

public class HSMController
{
    public EnemyController enemy;
    public PlayerController player;

    public HSM MainHSM;//Guarda-se a Máquina de Estados Mãe
    public HSMState currentState;//Para se saber onde se está neste momento

    public void DoActions()
    {
        //Vão-se buscar as acções a ser executadas a partir da máquina inicial/principal
        List<IAction> actionsToExecute = MainHSM.update();
        foreach (IAction action in actionsToExecute)
        {
            if (action != null)
            {
                action.DoAction();
            }
        }
    }
}

public class HSMState
{
    public string Name;
    public int StateLevel;
    public HSMState ParentState;
    public List<IAction> Actions;
    public IAction EntryAction;
    public IAction ExitAction;
    public List<HSMTransition> transitions;

    public HSMTransition CheckForTriggeredTransition()
    {
        foreach (HSMTransition t in transitions)
        {
            bool AllConditionsAreMet = true;
            foreach (ICondition condition in t.ConditionsList)
            {
                if (!condition.test())
                    AllConditionsAreMet = false;
            }
            if (AllConditionsAreMet)
            {
                return t;
            }
        }
        return null;
    }
}

public class SM
{
    public string Name;
    public List<State> States;
    public State CurrentState;
    public State InitialState;

    public List<IAction> GetActions()
    {
        List<IAction> actions = new List<IAction>();
        foreach (Transition t in CurrentState.transitions)
        {
            if (t.CanPerformTransition())
            {
                CurrentState = t.targetState;
                return actions;
            }
        }
        actions.AddRange(CurrentState.Actions);
        return actions;
    }

    public void update()
    {
        if (CurrentState == null)
        {
            CurrentState = InitialState;
        }
        List<IAction> actionsToRun = GetActions();
        foreach (IAction action in actionsToRun)
        {
            if(action!=null)
                action.DoAction();
        }
    }
}

public class HSM : HSMState
{
    public int HSMLevel;//(Será mesmo necessário?) 0 significa o nível principal da HSM
    public List<HSMState> states;//Estados ou outras sub-HSMs
    public HSMState initialState;//Para saber o estado inicial da máquina
    public HSMState currentState;//Para saber em que estado é que esta máquina se encontra

    public List<IAction> update()
    {
        if (currentState == null)
        {
            currentState = initialState;
        }
        foreach (HSMTransition transition in transitions)
        {
            //Se todas as condições de transição se verificarem
            if (transition.CanPerformTransition())
            {
                List<IAction> actionsToReturn = new List<IAction>();
                //Serão adicionadas à lista de retorno as acções de saída do estado actual, as acções da transição e as acções de entrada do estado alvo da transição
                actionsToReturn.Add(currentState.ExitAction);
                actionsToReturn.AddRange(transition.Actions);
                actionsToReturn.Add(transition.targetState.EntryAction);
                //O estado actual passa a ser o que foi alvo da transição
                currentState = transition.targetState;
                //Retorna-se a lista de acções
                return actionsToReturn;
            }
        }
        if (currentState is HSM)
        {
            HSM returner = currentState as HSM;
            //Cria-se a lista que irá ser retornada por este método
            List<IAction> actionsToReturn = new List<IAction>();
            //Adicionam-se as próprias acções
            actionsToReturn.AddRange(Actions);
            //Sendo que isto não á apenas um estado, mas uma HSM que contem outros estados, adicionam-se também as acções dos estados actuais dentro dela
            actionsToReturn.AddRange(returner.update());
            //Retorna-se a lista com todas as acções
            return actionsToReturn;
        }
        else
        {
            //Caso seja apenas um estado simples (e tendo em conta que não foram accionadas transições), apenas se irão retornar as acções respectivas
            return currentState.Actions;
        }
    }

    public HSMState GetStateForTransition(HSMState stateToLookFor,HSMTransition transition)
    {
        foreach (HSMState s in states)
        {
            if (s.GetType() == states.GetType())
            {
                return s;
            }
        }
        return null;
    }
}

//Componentes da Máquina de Estados Simples
//FSM
public class GuardFSM : SM
{
    public EnemyController enemy;

    public GuardFSM(EnemyController thisEnemy)
    {
        enemy = thisEnemy;
        this.Name = "Máquina de Estados do Guarda";
        this.States = new List<State>();
        States.Add(new GuardFSM_State_Guard(enemy,this));
        States.Add(new GuardFSM_State_Move(enemy, this));
        States.Add(new GuardFSM_State_Pursuit(enemy, this));
        States.Add(new GuardFSM_State_Attack(enemy, this));

        States[0].transitions[0].targetState = States[1];
        //States[0].transitions[1].targetState = States[2];

        States[1].transitions[0].targetState = States[0];
        //States[1].transitions[1].targetState = States[2];

        States[2].transitions[0].targetState = States[1];
        States[2].transitions[1].targetState = States[3];

        States[3].transitions[0].targetState = States[1];
        States[3].transitions[1].targetState = States[2];
        this.InitialState = States[0];
        this.CurrentState = null;
    }
}
//States
public class GuardFSM_State_Guard : State
{
    public SM parentMachine;
    public EnemyController enemy;

    public GuardFSM_State_Guard(EnemyController thisEnemy, SM parent)
    {
        parentMachine = parent;
        enemy = thisEnemy;
        Name = "GUARD STATE";
        Actions = new List<IAction>();
        Actions.Add(new GuardFSM_Action_PointGuard(enemy));
        EntryAction = null;
        ExitAction = null;
        transitions = new List<Transition>();
        transitions.Add(new GuardFSM_Transition_TimeToRelocate(parentMachine, enemy));
        transitions.Add(new GuardFSM_Transition_PlayerWasDetected(parentMachine, enemy));
    }
}
public class GuardFSM_State_Move : State
{
    public SM parentMachine;
    public EnemyController enemy;

    public GuardFSM_State_Move(EnemyController thisEnemy, SM parent)
    {
        parentMachine = parent;
        enemy = thisEnemy;
        Name = "MOVE STATE";
        Actions = new List<IAction>();
        Actions.Add(new GuardFSM_Action_Move(enemy, GlobalVariables.singleton.GuardPoints[enemy.ClosestGuardPointIndex]));
        EntryAction = null;
        ExitAction = null;
        transitions = new List<Transition>();
        transitions.Add(new GuardFSM_Transition_ArrivedAtPost(parentMachine,enemy,enemy.Players));
        transitions.Add(new GuardFSM_Transition_PlayerWasDetected(parentMachine,enemy));
    }
}
public class GuardFSM_State_Pursuit : State
{
    public SM parentMachine;
    public EnemyController enemy;

    public GuardFSM_State_Pursuit(EnemyController thisEnemy, SM parent)
    {
        parentMachine = parent;
        enemy = thisEnemy;
        Name = "PURSUIT STATE";
        Actions = new List<IAction>();
        Actions.Add(new GuardFSM_Action_PursuePlayer(enemy));
        EntryAction = null;
        ExitAction = null;
        transitions = new List<Transition>();
        transitions.Add(new GuardFSM_Transition_NoPlayersDetected(parentMachine,enemy));
        transitions.Add(new GuardFSM_Transition_PlayerInAttackRange(parentMachine,enemy));
    }
}
public class GuardFSM_State_Attack : State
{
    public SM parentMachine;
    public EnemyController enemy;

    public GuardFSM_State_Attack(EnemyController thisEnemy, SM parent)
    {
        parentMachine = parent;
        enemy = thisEnemy;
        Name = "ATTACK STATE";
        Actions = new List<IAction>();
        Actions.Add(new GuardFSM_Action_AttackAPlayer(enemy));
        EntryAction = null;
        ExitAction = null;
        transitions = new List<Transition>();
        transitions.Add(new GuardFSM_Transition_PlayerWasKilled(parentMachine,enemy,enemy.Players));
        transitions.Add(new GuardFSM_Transition_PlayerIsOutOfAttackRange(parentMachine,enemy));
    }
}
//Transitions
public class GuardFSM_Transition_TimeToRelocate : Transition
{
    public EnemyController Enemy;

    public GuardFSM_Transition_TimeToRelocate(SM parent, EnemyController enemy)
    {
        parentMachine = parent;
        Enemy = enemy;
        Actions = new List<IAction>();
        ConditionsList = new List<ICondition>();
        ConditionsList.Add(new GuardFSM_Condition_HasTheWaitingTimePassed(Enemy));
        targetState = null;
    }
}
public class GuardFSM_Transition_ArrivedAtPost : Transition
{
    public EnemyController Enemy;
    public GameObject PlayersHolder;

    public GuardFSM_Transition_ArrivedAtPost(SM parent, EnemyController enemy, GameObject Players)
    {
        parentMachine = parent;
        Enemy = enemy;
        PlayersHolder = Players;
        Actions = new List<IAction>();
        ConditionsList = new List<ICondition>();
        ConditionsList.Add(new GuardFSM_Condition_HaveIArrivedAtPost(Enemy));
        targetState = null;
    }
}
public class GuardFSM_Transition_PlayerWasDetected : Transition
{
    public EnemyController Enemy;

    public GuardFSM_Transition_PlayerWasDetected(SM parent, EnemyController enemy)
    {
        parentMachine = parent;
        Enemy = enemy;
        Actions = new List<IAction>();
        ConditionsList = new List<ICondition>();
        ConditionsList.Add(new GuardFSM_Condition_IsAPlayerVisible(Enemy));
        targetState = null;
    }
}
public class GuardFSM_Transition_NoPlayersDetected : Transition
{
    public EnemyController Enemy;

    public GuardFSM_Transition_NoPlayersDetected(SM parent, EnemyController enemy)
    {
        parentMachine = parent;
        Enemy = enemy;
        Actions = new List<IAction>();
        ConditionsList = new List<ICondition>();
        ConditionsList.Add(new GuardFSM_Condition_AreThereNoPlayersInRange(Enemy.gameObject));
        targetState = null;
    }
}
public class GuardFSM_Transition_PlayerInAttackRange : Transition
{
    public EnemyController Enemy;

    public GuardFSM_Transition_PlayerInAttackRange(SM parent, EnemyController enemy)
    {
        parentMachine = parent;
        Enemy = enemy;
        Actions = new List<IAction>();
        ConditionsList = new List<ICondition>();
        ConditionsList.Add(new GuardFSM_Condition_IsAPlayerInsideARange(Enemy,Enemy.AttackRange));
        targetState = null;
    }
}
public class GuardFSM_Transition_PlayerIsOutOfAttackRange:Transition
{
    public EnemyController Enemy;

    public GuardFSM_Transition_PlayerIsOutOfAttackRange(SM parent, EnemyController enemy)
    {
        parentMachine = parent;
        Enemy = enemy;
        Actions = new List<IAction>();
        ConditionsList = new List<ICondition>();
        ConditionsList.Add(new GuardFSM_Condition_IsAPlayerInsideARange(Enemy, Enemy.AttackRange));
        targetState = null;
    }
}
public class GuardFSM_Transition_PlayerWasKilled : Transition
{
    public EnemyController Enemy;
    public GameObject PlayersHolder;

    public GuardFSM_Transition_PlayerWasKilled(SM parent, EnemyController enemy, GameObject Players)
    {
        parentMachine = parent;
        Enemy = enemy;
        PlayersHolder = Players;
        Actions = new List<IAction>();
        ConditionsList = new List<ICondition>();
        //IMPORTANTE: ACTUALIZAR O JOGADOR EM QUESTÃO SEMPRE QUE O GUARDA VAI ATACAR
        foreach (Transform player in PlayersHolder.transform)
        {
            if (player.GetComponent<NetworkIdentity>()==Enemy.PlayerToAttack)
            {
                ConditionsList.Add(new GuardFSM_Condition_IsPlayerDead(player.GetComponent<MovimentoJogador>()));
                break;
            }
        }
        targetState = null;
    }
}
//Conditions
public class GuardFSM_Condition_HaveIArrivedAtPost : ICondition
{
    public EnemyController Enemy;

    public GuardFSM_Condition_HaveIArrivedAtPost(EnemyController enemy)
    {
        Enemy = enemy;
    }

    public bool test()
    {
        return !Enemy.HasGuardedPoint;
    }
}
public class GuardFSM_Condition_IsAPlayerInsideARange : ICondition
{
    public EnemyController Enemy;
    public float Range;

    public GuardFSM_Condition_IsAPlayerInsideARange(EnemyController enemy,float rangeToTest)
    {
        Enemy = enemy;
        Range = rangeToTest;
    }

    public bool test()
    {
        if (Enemy.PlayerToAttack != null)
        {
            if (Vector2.Distance(Enemy.transform.position, Enemy.PlayerToAttack.transform.position) < Range)
            {
                Enemy.PlayerToAttack = Enemy.PlayerToAttack.transform.GetComponent<NetworkIdentity>();
                return true;
            }
        }
        return false;
    }
}
public class GuardFSM_Condition_IsAPlayerVisible:ICondition
{
    public EnemyController Enemy;
    public float Range;

    public GuardFSM_Condition_IsAPlayerVisible(EnemyController enemy)
    {
        Enemy = enemy;
    }

    public bool test()
    {
        return Enemy.PlayerToAttack == null;
    }
}
public class GuardFSM_Condition_AreThereNoPlayersInRange : ICondition
{
    public GameObject Enemy;

    public GuardFSM_Condition_AreThereNoPlayersInRange(GameObject enemy)
    {
        Enemy = enemy;
    }

    public bool test()
    {
        return Enemy.GetComponent<EnemyController>().PlayerToAttack==null;
    }
}
public class GuardFSM_Condition_IsPlayerDead : ICondition
{
    public MovimentoJogador Player;

    public GuardFSM_Condition_IsPlayerDead(MovimentoJogador player)
    {
        Player = player;
    }

    public bool test()
    {
        return Player.Health >= 0;
    }
}
public class GuardFSM_Condition_HasTheWaitingTimePassed : ICondition
{
    public EnemyController enemy;

    public GuardFSM_Condition_HasTheWaitingTimePassed(EnemyController Enemy)
    {
        enemy = Enemy;
    }

    public bool test()
    {
        return !enemy.gameObject.GetComponent<GuardFSM_Action_GuardPoint>().CoroutineIsRunning;
    }
}
//Actions (Adicionar aqui o despoletar das animações)
public class GuardFSM_Action_MoveToAPoint : MonoBehaviour, IAction
{
    public EnemyController enemy;
    public Vector2 Destination;
    public float TimeToTake;
    public bool isMoving;
    public int previousClosestPoint=0;

    public GuardFSM_Action_MoveToAPoint(EnemyController Enemy,Vector2 Destination)
    {
        enemy = Enemy;
    }

    public void DoAction()
    {
        enemy.HasGuardedPoint=false;
        if (!isMoving)
        {
            Destination = GlobalVariables.singleton.GuardPoints[GetClosestPointIndex()];
            StartCoroutine(Move());
        }
    }

    public int GetClosestPointIndex()
    {
        int indexOfClosestPoint = 0;
        for (int i = 0; i < GlobalVariables.singleton.GuardPoints.Count; i++)
        {
            if (Vector2.Distance(enemy.transform.position, GlobalVariables.singleton.GuardPoints[i]) <
                Vector2.Distance(enemy.transform.position, GlobalVariables.singleton.GuardPoints[indexOfClosestPoint]) && i!=previousClosestPoint)
                indexOfClosestPoint = i;
        }
        return indexOfClosestPoint;
    }

    public IEnumerator Move()
    {
        float elapsedTime = 0;
        while (elapsedTime<TimeToTake)
        {
            isMoving = true;
            enemy.transform.position = Vector2.Lerp(enemy.transform.position, Destination, elapsedTime/TimeToTake);
            elapsedTime += Time.deltaTime;
        }
        enemy.transform.position = Destination;
        isMoving = false;
        yield return null;
    }
}//Não usar directamente
public class GuardFSM_Action_Move : IAction
{
    public EnemyController enemy;
    public Vector2 Destination;

    public GuardFSM_Action_Move(EnemyController character, Vector2 destination)
    {
        enemy = character;
        Destination = destination;
    }

    public void DoAction()
    {
        enemy.GetComponent<GuardFSM_Action_MoveToAPoint>().Destination = Destination;
        enemy.GetComponent<GuardFSM_Action_MoveToAPoint>().DoAction();
    }
}
public class GuardFSM_Action_PursuePlayer : IAction
{
    public EnemyController enemy;

    public GuardFSM_Action_PursuePlayer(EnemyController character)
    {
        enemy = character;
    }

    public void DoAction()
    {
        if (enemy.GetComponent<EnemyController>().PlayerToAttack != null)
        {
            enemy.GetComponent<GuardFSM_Action_MoveToAPoint>().Destination =
                enemy.GetComponent<EnemyController>().PlayerToAttack.transform.position;
            enemy.GetComponent<GuardFSM_Action_MoveToAPoint>().DoAction();
        }
    }
}
public class GuardFSM_Action_AttackAPlayer : IAction
{
    public EnemyController enemy;

    public GuardFSM_Action_AttackAPlayer(EnemyController character)
    {
        enemy = character;
    }

    public void DoAction()
    {
        if(enemy.PlayerToAttack!=null)
            enemy.PlayerToAttack.GetComponent<MovimentoJogador>().Health -= enemy.AttackStrength;
    }
}
public class GuardFSM_Action_ForgetPlayer : IAction
{
    public EnemyController enemy;

    public GuardFSM_Action_ForgetPlayer(EnemyController Enemy)
    {
        enemy = Enemy;
    }
    public void DoAction()
    {
        enemy.PlayerToAttack = null;
    }
}
public class GuardFSM_Action_GuardPoint : MonoBehaviour, IAction
{
    public EnemyController enemy;
    public int Seconds;
    public bool CoroutineIsRunning;
    public bool Arriving = true;
    public void DoAction()
    {
        if (Arriving)
        {
            Arriving = false;
            CoroutineIsRunning = true;
            StartCoroutine(WaitCoroutine(enemy, Seconds));
        }
    }

    public IEnumerator WaitCoroutine(EnemyController Enemy, int secondsToWait)
    {
        Enemy.HasGuardedPoint = false;
        yield return new WaitForSecondsRealtime(secondsToWait);
        Enemy.HasGuardedPoint = true;
        CoroutineIsRunning = false;
        Arriving = true;
    }
} //USAR A GUARDFSM_ACTION_POINTGUARD EM VEZ DESTA
public class GuardFSM_Action_PointGuard : IAction
{
    public EnemyController enemy;

    public GuardFSM_Action_PointGuard(EnemyController Enemy)
    {
        enemy = Enemy;
    }
    public void DoAction()
    {
        if (!enemy.GetComponent<GuardFSM_Action_GuardPoint>().CoroutineIsRunning)
        {
            enemy.GetComponent<GuardFSM_Action_GuardPoint>().DoAction();
        }
    }
}
//Classe Principal deste script (NÃO APAGAR)
public class EnemyHFSM : MonoBehaviour {}