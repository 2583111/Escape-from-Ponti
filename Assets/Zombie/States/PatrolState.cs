using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : BaseState
{
    //track which waypoint we are targeting.
    public int waypointIndex;

    public override void Enter()
    {

    }

    public override void Perform()
    {
        PatrolCycle();
        if (zombie.CanSeePlayer())
        {
            stateMachine.ChangeState(new AttackState());
        }
    }

    public override void Exit()
    {

    }

    public void PatrolCycle()
    {
        //implement our patrol logic
        if (zombie.Agent.remainingDistance < 0.2f)
        {
            if (waypointIndex < zombie.path.waypoints.Count - 1)
                waypointIndex++;
            else
                waypointIndex = 0;
            zombie.Agent.SetDestination(zombie.path.waypoints[waypointIndex].position);
        }
    }

}

