using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public abstract class PlayerManager : MonoBehaviour
{
    protected PlayerMovement playerMovement;
    protected FootstepAudioController footstepAudioController;
    protected AnimationController animationController;
    protected PlayerInteractionController playerInteractionController;
    protected Units AStar;

    //movement
    [SerializeField] protected Transform cam;
    [SerializeField] protected Rigidbody target;

    [SerializeField] protected float moveSpeed = 2f;
    [SerializeField] protected float e = 1f;
    [SerializeField] protected float jumpSpeed = 1f;
    protected float runSpeed;
    protected float walkSpeed;

    [SerializeField] protected bool isGrounded = true;

    [SerializeField] protected float turnSmoothing = 0.1f;

    [SerializeField] protected LayerMask groundLayer;
    [SerializeField] protected AnimationCurve aniCurve;
    [SerializeField] protected float alignmentTime = 0.1f;
    protected string collidedObjectTag;

    //footsteps
    [SerializeField] protected List<AudioClip> footstepSounds;
    [SerializeField] protected AudioSource footstepSource;
    [SerializeField] protected float footstepFadeOffTime = 0.5f;
    [SerializeField] protected float footstepVolume = 0.9f;

    //animation
    protected Animator animator;
    [SerializeField] protected float animationVelocity = 0.0f;
    [SerializeField] protected float animationAccel = 0.1f;
    [SerializeField] protected float animationDeccel = 0.4f;
    [SerializeField] protected float nextEnabledTime = 0.5f;
    //interaction
    protected bool isInteracting;

    //menu
    protected bool isOnMenu;

    //inventory
    protected bool isInventoryOpen = false;

    //switchy
    protected bool isMainCharacter = false;

    //input
    protected bool forwardKey;
    protected bool backwardKey;
    protected bool leftKey;
    protected bool rightKey;
    protected bool jumpKey;
    protected bool runKey;
    protected bool interactKey;
    protected bool inventoryOpenKey;
    protected bool leftClick;
    protected bool rightClick;
    protected bool switchKey;

    //stats
    protected float health;
    protected float lightAttackDmg;
    protected float heavyAttackDmg;
    protected float ultAttackDmg;
    protected float secondaryStat;

    //attack
    protected bool isLightAttack = false;
    protected bool isHeavyAttack = false;
    protected bool isUltAttack = false;
    //hardcode
    protected float heavyAttackLength;
    protected bool heavyAttackCooldown = false;
    protected float lightAttackLength;
    protected bool lightAttackCooldown = false;

    //camera
    protected Cinemachine.CinemachineFreeLook camera;

    //enemy tracker
    protected ArenaManager arenaManager;

    protected HealthBar healthBar;

    protected abstract void ChildStart();
    protected abstract void ChildUpdate();

    public bool IsMainCharacter
    {
        get { return isMainCharacter; }
    }

    //INITIALIZATIONS===============================

    //default movement
    protected void InitMovementDefault()
    {
        runSpeed = moveSpeed * 2;
        walkSpeed = moveSpeed;
        playerMovement = GetComponent<PlayerMovement>();
        playerMovement.Cam = cam;
        playerMovement.Target = target;
        playerMovement.WalkSpeed = walkSpeed;
        playerMovement.RunSpeed = runSpeed;
        playerMovement.JumpSpeed = jumpSpeed;
        playerMovement.E = e;
        playerMovement.TurnSmoothing = turnSmoothing;
        playerMovement.GroundLayer = groundLayer;
        playerMovement.AniCurve = aniCurve;
        playerMovement.AlignmentTime = alignmentTime;
    }

    //use this to be able to interact
    protected void InitInteraction()
    {
        playerInteractionController = GetComponent<PlayerInteractionController>();
    }

    //dependencies: InitInteraction, CheckIsInteracting. Use this to enable player interaction
    protected void EnableInteraction()
    {
        playerInteractionController.PlayerInteraction(interactKey);
    }

    //use this to get player camera
    protected void GetPlayerCamera()
    {
        camera = GetComponentInChildren<Cinemachine.CinemachineFreeLook>();
    }
    protected void InitArenaManager()
    {
        arenaManager = ArenaManager.Instance;
    }
    //use this to init AStar
    protected void InitAStar()
    {
        InitArenaManager();
        AStar = GetComponent<Units>();
        AStar.AntiCollisionTag = "MyPlayer";
        InitEnemy();
        AStar.enabled = false;
    }

    protected void InitHealthBar()
    {
        healthBar = GetComponentInChildren<HealthBar>();
        healthBar.SetMaxHealth(health);
    }

    //STATUS CHECKING=============================================

    //use this to check if player is on menu
    protected void CheckIsOnMenu()
    {
        isOnMenu = MainMenu.Instance.GetIsOnMenu();
    }

    //use this to check if player is grounded
    protected void CheckIsGrounded()
    {
        isGrounded = playerMovement.isGrounded;
    }

    //use this to check if player is opening inventory
    protected void CheckIsInventoryOpen()
    {
        if (inventoryOpenKey)
        {
            isInventoryOpen = !isInventoryOpen;
            InventoryUI.Instance.ToggleInventory(isInventoryOpen);
        }
    }

    //use this to check if player is interacting
    protected void CheckIsInteracting()
    {
        isInteracting = playerInteractionController.GetInteracting();
    }

    //use this to check for main character. that involves action and camera
    protected void CheckIsMainCharacter()
    {
        if(camera != null)
        {
            if (isMainCharacter)
            {
                camera.Priority = 19;
            }
            else
            {
                camera.Priority = 10;
            }
        }
        
    }
    //use this to set the state of main character
    public void SetIsMainCharacter(bool state)
    {
        isMainCharacter = state;
    }

    IEnumerator HeavyAttackCooldown()
    {
        heavyAttackCooldown = true;
        yield return new WaitForSeconds(heavyAttackLength);
        heavyAttackCooldown = false;
    }

    IEnumerator LightAttackCooldown()
    {
        lightAttackCooldown = true;
        yield return new WaitForSeconds(lightAttackLength);
        lightAttackCooldown = false;
    }

    //WIP attack signal
    protected void CheckIsAttacking()
    {
        if (!isLightAttack && !lightAttackCooldown && leftClick && !animationController.IsLightAttack)
        {
            isLightAttack = true;
            StartCoroutine("LightAttackCooldown");
        }
        else
        {
            isLightAttack = false;
        }

        if (!isHeavyAttack && !heavyAttackCooldown && rightClick && !animationController.IsHeavyAttack)
        {
            isHeavyAttack = true;
            StartCoroutine("HeavyAttackCooldown");
        }
        else
        {
            isHeavyAttack = false;
        }
    }

    protected bool CheckIsAlive()
    {
        if (health > 0) return true;
        else return false;
    }

    //UTILITIES===============================
    private void HandleInput()
    {
        forwardKey = Input.GetKey(KeyCode.W);
        backwardKey = Input.GetKey(KeyCode.S);
        leftKey = Input.GetKey(KeyCode.A);
        rightKey = Input.GetKey(KeyCode.D);
        jumpKey = Input.GetKeyDown(KeyCode.Space);
        runKey = Input.GetKey(KeyCode.LeftShift);
        interactKey = Input.GetKeyDown(KeyCode.F);
        inventoryOpenKey = Input.GetKeyDown(KeyCode.I);
        leftClick = Input.GetMouseButtonDown(0);
        rightClick = Input.GetMouseButtonDown(1);
        switchKey = Input.GetKeyDown(KeyCode.Q);
    }

    private void SetGroundedStatus(bool grounded)
    {
        isGrounded = grounded;
        playerMovement.isGrounded = grounded;
        animationController.IsGrounded = grounded;
        footstepAudioController.IsGrounded = grounded;
    }

    //use this to handicap the player movement based on a condition
    protected void MovementHandicap()
    {
        if (isInteracting || isOnMenu)
        {
            forwardKey = backwardKey = leftKey = rightKey = jumpKey = runKey = inventoryOpenKey = false;
        }
    }

    //use this to disable all animation, preferablly for switching chars
    public void DisableAnimation()
    {
        animationController.DisableAnimation();
    }

    //use this for oncollision damage
    public void TakeDamage(float value)
    {
        health -= value;

        if(healthBar != null)
        {
            healthBar.SetHealth(health);
        }
    }

    //use this for attack signal
    public float Attack()
    {
        if (isLightAttack) return lightAttackDmg;
        else if (isHeavyAttack) return heavyAttackDmg;
        else if (!animationController.IsAutoMoving && !isMainCharacter) return lightAttackDmg;
        return 0f;
    }


    //Player A* Logic =====================================================================

    //use this to toggle AStar, use this in main character check, main logic
    //ini di control ama arenaManager
    //karna switchkey disono oops
    public void ToggleAStar()
    {
        if (isMainCharacter)
        {
            AStar.EnableUpdate = false;
            AStar.StopAllCoroutines();
            AStar.enabled = false;
            StopAllCoroutines();
        }
        else
        {
            AStar.EnableUpdate = true;
            AStar.enabled = true;
            StartCoroutine("CheckPositionRoutine");
            StartCoroutine("ScanForEnemy");
        }
    }

    //use this to find the closest enemy
    private Transform GetClosestEnemy()
    {
        EnemyManager closestEnemy = null;
        float shortestDistance = Mathf.Infinity;

        foreach (EnemyManager enemy in arenaManager.EnemyList)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);

            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                closestEnemy = enemy;
            }
        }

        if (closestEnemy == null)
        {
            return null;
            //Debug.Log("no enemies");
        }
        else return closestEnemy.transform;
    }

    //initilaize closest enemy
    protected void InitEnemy()
    {
        Transform closestEnemy = GetClosestEnemy();

        if (closestEnemy != null)
        {
            AStar.Target = closestEnemy;
        }
    }

    //interval scanning | if mainCharacter then stop
    IEnumerator ScanForEnemy()
    {
        while (true)
        {
            
            yield return new WaitForSeconds(2f);
            Transform closestPlayerTransform = GetClosestEnemy();

            if(closestPlayerTransform != null && AStar.Target != null)
            {
                if (Vector3.Distance(transform.position, closestPlayerTransform.position) < Vector3.Distance(transform.position, AStar.Target.position))
                {
                    AStar.Target = closestPlayerTransform;
                }
            }
            else if (AStar.Target == null)
            {
                AStar.Target = closestPlayerTransform;
            }
            
        }
    }

    //alignment
    protected float rotationSpeed = 5f;

    //automatic rotation
    protected void RotateTowardsEnemy()
    {
        Vector3 lookAtDirection = AStar.GetLookAt();
        lookAtDirection.y = 0;
        if (lookAtDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(lookAtDirection, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
    }
    private IEnumerator CheckPositionRoutine()
    {
        while (true)
        {
            CheckIsAutoMoving();

            yield return new WaitForSeconds(0.5f);
        }
    }
    private Vector3 lastPosition;
    protected void CheckIsAutoMoving()
    {
        if (Vector3.Distance(transform.position, lastPosition) > 0.1f)
        {
            lastPosition = transform.position;
            animationController.IsAutoMoving = true;
        }
        else
        {
            animationController.IsAutoMoving = false;
        }
    }

    //=======================================================================
    protected void Start()
    {
        ChildStart();
        
    }
    private void Update()
    {
        if (isMainCharacter)
        {
            HandleInput();
        }
        else
        {
            forwardKey = backwardKey = leftKey = rightKey = jumpKey = runKey = interactKey = inventoryOpenKey = leftClick = rightClick = false;
        }
        
        ChildUpdate();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 3)
        {
            SetGroundedStatus(true);
        }
        else
        {
            SetGroundedStatus(false);
        }
        collidedObjectTag = collision.gameObject.tag;
        footstepAudioController.CollidedTag = collidedObjectTag;
        playerMovement.CollidedObjectTag = collidedObjectTag;
    }

    private void OnTriggerStay(Collider other)
    {
        if (leftClick || rightClick)
        {
            if (other.gameObject.CompareTag("Enemy"))
            {
                float radius = 1.5f;
                Collider[] colliderArray = Physics.OverlapSphere(transform.position, radius);
                foreach (var collider in colliderArray)
                {
                    if (collider.TryGetComponent(out EnemyManager enemyManager))
                    {
                        enemyManager.TakeDamage(Attack());
                    }
                }
            }
        }
        else if (!IsMainCharacter)
        {
            if (!animationController.IsAutoMoving)
            {
                if (other.gameObject.CompareTag("Enemy"))
                {
                    float radius = 1.5f;
                    Collider[] colliderArray = Physics.OverlapSphere(transform.position, radius);
                    foreach (var collider in colliderArray)
                    {
                        if (collider.TryGetComponent(out EnemyManager enemyManager))
                        {
                            StartCoroutine("LightAttackCooldown");
                            animationController.IsLightAttack = true;
                            enemyManager.TakeDamage(Attack());
                        }
                    }
                }
            }
        }

    }
}
