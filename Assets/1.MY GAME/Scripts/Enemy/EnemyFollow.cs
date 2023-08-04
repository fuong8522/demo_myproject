using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyFollow : MonoBehaviour
{

    private Transform player;
    private NavMeshAgent agent;
    private CapsuleCollider capsuleCollider;
    private Animator animator;
    public bool deadth;
    private float health = 3f;
    private float lastPositionZ;
    public static bool attacked;

    public bool checkAnimationStart;

    public GameObject coinObject;
    public bool checkObstacle = false;

    void Start()
    {
        checkObstacle = false;
        checkAnimationStart = false;
        attacked = false;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        lastPositionZ = transform.position.z;
        animator = GetComponent<Animator>();
        deadth = false;
    }

    void Update()
    {
        OnAnimationZombieWalk();
        OnAnimationAttack();
        CheckPunch();
        if(!checkObstacle)
        {
            NavMove();
        }


        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 1))
        {
            if ((hit.collider.gameObject.name == "Player" ) && checkAnimationStart)
            {
                MovementPlayer.instance.health -= 5;
            }

            if(hit.collider.gameObject.name == "Obstacle")
            {
 
            }

        }

        if (MovementPlayer.instance.health == 0 && !MovementPlayer.instance.death)
        {
            MovementPlayer.instance.animator.SetTrigger("Death");
            MovementPlayer.instance.death = true;
        }

    }

    public void OnAnimationZombieWalk()
    {
        if (transform.position.z != lastPositionZ)
        {
            animator.SetBool("Walk", true);
        }
        else
        {
            animator.SetBool("Walk", false);
        }

        lastPositionZ = transform.position.z;
    }
    public void OnAnimationAttack()
    {
        //Zombie attack player
        if ((animator.GetBool("Walk") == false ) && !deadth)
        {
            transform.forward = player.transform.position - transform.position;

            animator.SetTrigger("Collision");
        }
    }


    private void OnTriggerStay(Collider other)
    {
        if ( (other.gameObject.CompareTag("BallWeapon" ) && !deadth ) || (other.gameObject.CompareTag("Weapon") && MovementPlayer.instance.checkPunch && !attacked && !deadth))
        {
            animator.SetTrigger("IsHitted");
            health--;
            attacked = true;
            OnDeadth();
        }
        if(other.gameObject.CompareTag("Obstacle"))
        {
            checkObstacle = true;
            agent.SetDestination(transform.position);

        }
    }


    //Delay zombie disappear
    IEnumerator DelayDisActiveZombie()
    {
        yield return new WaitForSeconds(15f);
        gameObject.SetActive(false);
    }

    public void OnDeadth()
    {
        if (health == 0)
        {
            deadth = true;
            Invoke("SpawnCoin", 0.5f);
            agent.SetDestination(transform.position);
            animator.SetTrigger("Death");
            capsuleCollider.isTrigger = true;
            StartCoroutine(DelayDisActiveZombie());
            this.gameObject.tag = "Untagged";
        }
    }

    public void SpawnCoin()
    {
        Vector3 posCoin = transform.position;
        posCoin.y += 0.5f;
        Instantiate(coinObject, posCoin, Quaternion.identity);
    }
    public void NavMove()
    {
        if (!deadth)
        {
            if(!checkObstacle)
            {
                agent.SetDestination(player.position);
            }
        }
    }

    void CheckPunch()
    {
        if (MovementPlayer.instance.checkPunch == false)
        {
            attacked = false;
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if(collision.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("check obstackle");
        }
    }
}
