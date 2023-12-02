using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Behavior : MonoBehaviour
{
    public float lookRadius = 10f;  // Detection range for player

    public Transform target;   // Reference to the player
    public bool id = true;
    public Animator animator;
    interact enemyInteract;
    NavMeshAgent agent; // Reference to the NavMeshAgent
                        // 
    [Header("Enemy Settings")]
    public int attackSpan;
    public int attackSpanMax = 1500;
    public int attacks = 1;
    void Start()
    {

        agent = GetComponent<NavMeshAgent>();
        enemyInteract = GetComponent<interact>();

        attackSpan = attackSpanMax;
    }

    // Update is called once per frame
    void Update()
    {
        // Distance to the target
        float distance = Vector3.Distance(target.position, transform.position);

        // If inside the lookRadius
        if (distance <= lookRadius)
        {
            // Move towards the target
            agent.SetDestination(target.position);
           

            // If within attacking distance
            if (distance <= agent.stoppingDistance)
            {
                // Make sure to face towards the target
                if (attackSpan > 0 && !enemyInteract.isInteracting)
                {
                    attackSpan--;
                }
                else
                {
                    if (attackSpan <= 0)
                    {
                        Attack();
                    }

                }
            }


        }

        FaceTarget();


    }

    void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(-direction.x, 0, -direction.z));
        transform.rotation = lookRotation;
    }

    void Attack()
    {
        if (attacks == 1)
        {
            animator.SetTrigger("Attack");
        }

        if (attacks == 2)
        {
            if (id)
            {
                var attackChoice = Random.Range(0, 5);
                print(attackChoice);
                if (attackChoice == 1 || attackChoice == 0)
                {
                    animator.SetTrigger("Attack");
                    id = false;
                }
                else
                {
                    animator.SetTrigger("Attack2");
                    id = false;
                }
            }
            

        }
        StartCoroutine(Cooldown());
    }

    IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(1);
        animator.ResetTrigger("Attack");
        id = true;
        attackSpan = attackSpanMax;
    }

    // Show the lookRadius in editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }
}
