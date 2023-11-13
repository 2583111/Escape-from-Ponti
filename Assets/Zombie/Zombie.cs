using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour
{
    private StateMachine stateMachine;
    private NavMeshAgent agent;

    public int health = 100;

    public NavMeshAgent Agent { get => agent; }



    [SerializeField]
    private string currentState;

    public Path path;

    private GameObject player;
    public float sightDistance = 20f;
    public float fieldOfView = 85f;
    public float eyeHeight;

    public float sphereCastRadius = 1f;


    

    // Start is called before the first frame update
    void Start()
    {
        stateMachine = GetComponent<StateMachine>();
        agent = GetComponent<NavMeshAgent>();
        stateMachine.Initialize();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        CanSeePlayer();
        currentState = stateMachine.activeState.ToString();



    }


    public bool CanSeePlayer()
    {
        if (player != null)
        {
            Vector3 toPlayer = player.transform.position - transform.position;

            // Check if the player is within sightDistance and within the field of view.
            if (toPlayer.sqrMagnitude <= sightDistance * sightDistance)
            {
                float angleToPlayer = Vector3.Angle(transform.forward, toPlayer);

                if (angleToPlayer <= fieldOfView * 0.5f)
                {
                    RaycastHit hitInfo;
                    Vector3 start = transform.position + Vector3.up * 0.5f; // Adjust the height as needed.

                    // Perform a sphere cast.
                    if (Physics.SphereCast(start, sphereCastRadius, toPlayer.normalized, out hitInfo, sightDistance))
                    {
                        if (hitInfo.transform.gameObject == player)
                        {
                            // The player is within sight and not blocked by obstacles.
                            return true;
                        }
                    }
                }
            }
        }

        return false;
    }


    public void TakeDamage(int damageAmount)
    {
        health -= damageAmount;
        
        if(health <= 0)
        {
            Destroy(gameObject);
        }

    }

}
