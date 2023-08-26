using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTower : EnemyManager
{
    private Cystal currTarget;

    private float attackRange = 3f;

    protected override void ChildStart()
    {
        // base init
        targetTag = "CrystalHitbox";
        health = 150;
        attackDmg = 15;

       
        
        InitArenaManager();
        arenaManager.AddTowerAttacker();

        initDefault();
        //crystalnya object
        currTarget = arenaManager.Crystal;

        //transform
        InitTarget();

    }

    protected override void ChildUpdate()
    {
        // alignment
        RotateTowardsTarget();
        if (!IsAlive())
        {
            arenaManager.DecrementTowerAttacker();
            arenaManager.RemoveEnemy(this);
        }
    }


    protected override void InitTarget()
    {
        AStar.Target = currTarget.gameObject.transform;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("CrystalHitbox") && !isAttacking)
        {
            StartCoroutine(PreAttackDelay());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("CrystalHitbox"))
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

            yield return new WaitForSeconds(attackInterval);
        }
    }
}
