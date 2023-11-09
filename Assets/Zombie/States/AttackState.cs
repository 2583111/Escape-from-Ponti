using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AttackState : BaseState
{
    private float moveTimer;
    private float losePlayerTimer;
    [SerializeField] GameObject Player;
    
    void Start()
    {
       

    }
    public override void Enter()
    {

    }

    public override void Exit()
    {

    }

    public override void Perform()
    {
        if (zombie.CanSeePlayer())
        {
            losePlayerTimer = 0;
            moveTimer += Time.deltaTime;
            if (moveTimer > Random.Range(3, 7))
            {
                Player = GameObject.FindGameObjectWithTag("Player");
                Debug.Log(Player);
                //zombie.Agent.SetDestination(zombie.transform.position + (Random.insideUnitSphere * 5));
                zombie.Agent.SetDestination(Player.transform.position);
                //To DO:
                //Get the Difference between Player and This Zombie
                //Set a threshold, stop zombie when within threshold (use difference calculated)
                //Call actual Attack Function for damage dealt
                moveTimer = 0;
            }

        }
        else
        {
            losePlayerTimer += Time.deltaTime;
            if (losePlayerTimer > 8)
            {
                //Change to the search state.
                stateMachine.ChangeState(new PatrolState());
            }
        }
    }

}


