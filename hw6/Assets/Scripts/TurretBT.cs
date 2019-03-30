using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Panda;



public class TurretBT : MonoBehaviour
{

    //Attack mechanism
    public float minimumDistance =5.0f;
    public float maximumDistance = 8.0f;
    public float viewingAngle = 70.0f;
    public GameObject target;
    public float turnSpeed;
    public GameObject bullet;
    public GameObject gun;

        private void Start()
    {
        turnSpeed = 3f;
    }






    [Task]
    public void DetectIntruderT()
    {
        //distance
        Vector3 distance = target.transform.position - transform.position;

        //view angle
        float angle = Vector3.Angle(distance, transform.forward);

        //line of sight condition
        if ((distance.magnitude < maximumDistance) && (angle < viewingAngle))
        {   
                //Look at target
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(distance), turnSpeed * Time.deltaTime);

                //Attack

                Task.current.Succeed();
            
        }
        //out of line of sight
        else
        {
            Task.current.Fail();
        }
    }



    [Task]
    public void AttackIntruderT()

    {

        if (IsPlayerInSight())
        {
            Vector3 distance = target.transform.position - transform.position;
            float angle = Vector3.Angle(distance, transform.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(distance), turnSpeed * Time.deltaTime);

            GameObject cur = Instantiate(bullet, gun.transform.position, gun.transform.rotation);
            Vector3 direction = gun.transform.position - target.transform.position;
           // cur.transform.rotation = Quaternion.LookRotation(target.transform.position - transform.position);
            cur.GetComponent<Rigidbody>().AddForce(transform.forward * 70);
            Destroy(cur, 0.4f);
        }
        else
        {
            Task.current.Fail();
        }



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



    [Task]
    public void RotateTurret()
    {

        if (IsPlayerInSight())
        {
            Task.current.Fail();
        }
        transform.Rotate(0, 2, 0);

      
    }





}
