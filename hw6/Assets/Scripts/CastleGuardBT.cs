using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Panda;
using UnityEngine.AI;

public class CastleGuardBT : MonoBehaviour
{
    NavMeshAgent myAgent;
    Vector3 randomPosition;
    Animator anim;

    //Attack mechanism
    public float minimumDistance = 2.0f;
    public float maximumDistance = 8.0f;
    public float viewingAngle = 50.0f;
    public GameObject target;
    public float turnSpeed;

    // Start is called before the first frame update
    void Start()
    {
        myAgent = this.gameObject.GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        turnSpeed = 2.0f;
    }


    [Task] 
    public void GetRandomPositionAndMove()
    {
        //set reandom position
        randomPosition = new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));

        //Move toward the position
        myAgent.SetDestination(randomPosition);

        if (IsPlayerInSight())
        {
            Task.current.Fail();
          //  Task.current.Succeed();
        }

        //Send success back to tree
        Task.current.Succeed();

    }

    [Task]
    public void CheckForDestinationReach()
    {
        if (IsPlayerInSight())
        {
            Task.current.Fail();
        }

        // to check if my agent has reached the destination
        if (myAgent.remainingDistance < myAgent.stoppingDistance)
        {
            Task.current.Succeed();
        }
    }

    [Task]
    public void DetectIntruder()
    {
        //distance
        Vector3 distance = target.transform.position - transform.position;

        //view angle
        float angle = Vector3.Angle(distance, transform.forward);

        //line of sight condition
        if ((distance.magnitude < maximumDistance) && (angle < viewingAngle))
        {
            myAgent.isStopped = false;
            // I have selected the player
            anim.SetBool("Charge", true);
            anim.SetBool("Patrol", false);
            anim.SetBool("Attack", false);

            myAgent.SetDestination(target.transform.position);
            //Attack
            if (distance.magnitude <= minimumDistance)
            {
                //stop agent while attacking
                myAgent.isStopped = true;

                //Look at target
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(distance), turnSpeed * Time.deltaTime);

                //Attack
                Task.current.Succeed();
            }
        }
        //out of line of sight
        else
        {
            anim.SetBool("Charge", false);
            anim.SetBool("Patrol", true);
            anim.SetBool("Attack", false);
            Task.current.Fail();
        }
    }

    [Task]
    public void AttackIntruder()
    {
        anim.SetBool("Charge", false);
        anim.SetBool("Patrol", false);
        anim.SetBool("Attack", true);
        Task.current.Fail();
    }

    public bool IsPlayerInSight()
    {
        //distance
        Vector3 distance = target.transform.position - transform.position;

        //view angle
        float angle = Vector3.Angle(distance, transform.forward);

        //line of sight condition
        return ((distance.magnitude < maximumDistance) && (angle < viewingAngle));
    }
}
