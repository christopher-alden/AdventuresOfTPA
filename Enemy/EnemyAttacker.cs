using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttacker : EnemyManager
{
    private List<PlayerManager> PlayerList;
    
    private float scanInterval = 3f;
    private PlayerManager currTarget;

    private float attackRange = 3f;

    protected override void ChildStart()
    {
        // base init
        targetTag = "PlayerAttackBox";
        health = 150;
        attackDmg = 15;

        InitArenaManager();
        arenaManager.AddPlayerAttacker();
        // get all player data
        PlayerList = arenaManager.PlayerList;

        // init unit animator target
        initDefault();
        InitTarget();
        
        //scanf player
        StartCoroutine("ScanForTarget");
    }

    protected override void ChildUpdate()
    {
        // alignment
        RotateTowardsTarget();
        PlayerList = arenaManager.PlayerList;
        if (!IsAlive())
        {
            arenaManager.DecrementPlayerAttacker();
            arenaManager.RemoveEnemy(this);
        }
    }

    protected override void InitTarget()
    {
        Transform closestPlayer = GetClosestPlayer();

        if (closestPlayer != null)
        {
            AStar.Target = closestPlayer;
        }
    }

    private Transform GetClosestPlayer()
    {
        PlayerManager closestPlayer = null;
        float shortestDistance = Mathf.Infinity;

        foreach (PlayerManager player in PlayerList)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

            if (distanceToPlayer < shortestDistance)
            {
                shortestDistance = distanceToPlayer;
                currTarget = closestPlayer = player;
            }
        }

        if (closestPlayer == null) return null;
        else return closestPlayer.transform;
    }

    IEnumerator ScanForTarget()
    {
        while (true)
        {
            yield return new WaitForSeconds(scanInterval);
            Transform closestPlayerTransform = GetClosestPlayer();

            
            if(closestPlayerTransform != null && AStar.Target != null)
            {
                if (Vector3.Distance(transform.position, closestPlayerTransform.position) < Vector3.Distance(transform.position, AStar.Target.position))
                {
                    AStar.Target = closestPlayerTransform;
                }
            }
            else if (AStar.Target == null) AStar.Target = GetClosestPlayer();

        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerAttackBox") && !isAttacking)
        {
            StartCoroutine(PreAttackDelay());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerAttackBox"))
        {
            isAttacking = false;
            animator.SetBool("isAttacking", false);
        }
    }

    IEnumerator PreAttackDelay()
    {
        float preAttackDelay = 1f;
        yield return new WaitForSeconds(preAttackDelay);

        if (!isAttacking)
        {
            isAttacking = true;
            animator.SetBool("isAttacking", true);
            StartCoroutine(AttackInterval());
        }
    }
    IEnumerator AttackInterval()
    {
        while (isAttacking)
        {
            if (currTarget != null)
            {
                if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.4f && animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
                {
                    float distanceToTarget = Vector3.Distance(transform.position, currTarget.transform.position);
                    if (distanceToTarget <= attackRange)
                    {
                        currTarget.TakeDamage(attackDmg);
                    }

                    animator.SetBool("isAttacking", false);
                    isAttacking = false;
                }
            }
            yield return new WaitForSeconds(attackInterval);
        }
    }
}
