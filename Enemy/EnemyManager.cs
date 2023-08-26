using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyManager : MonoBehaviour
{
    //a*
    protected Units AStar;

    //alignment
    protected float rotationSpeed = 5f;

    //animation
    protected Animator animator;
    protected bool isAttacking = false;

    //attack logic
    protected float attackInterval = 1f;

    //collision
    protected string targetTag;

    //stats
    protected float health;
    protected float attackDmg;

    //health bar
    protected HealthBar healthBar;

    //arena manager
    protected ArenaManager arenaManager;

    protected abstract void ChildStart();
    protected abstract void ChildUpdate();
    protected abstract void InitTarget();

    protected bool IsAlive()
    {
        if (health > 0) return true;
        else return false;
    }

    protected void InitArenaManager()
    {
        arenaManager = ArenaManager.Instance;
    }
    //default initialization for a* and animation
    protected void initDefault()
    {
        AStar = GetComponent<Units>();
        AStar.AntiCollisionTag = "Enemy";
        AStar.EnableUpdate = true;
        animator = GetComponent<Animator>();
        healthBar = GetComponentInChildren<HealthBar>();
        healthBar.SetMaxHealth(health);
    }

    //alignment towards target
    protected void RotateTowardsTarget()
    {
        Vector3 lookAtDirection = AStar.GetLookAt();
        if (lookAtDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(lookAtDirection, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        healthBar.SetHealth(health);
    }

    //=======================================================================
    private void Start()
    {
        ChildStart();
    }
    private void Update()
    {
        ChildUpdate();
        
    }
}

