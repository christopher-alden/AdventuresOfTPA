using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    private Animator animator;
    private float animationVelocity = 0.0f;
    private float animationAccel = 0.1f;
    private float animationDeccel = 0.4f;
    private bool isGrounded = true;
    private float nextEnabledTime = 0.5f;
    public static int noOfClicks = 0;
    float lastClickedTime = 0;
    float maxComboDelay = 0.6f;

    //attack reference
    private bool isLightAttack;
    private bool isHeavyAttack;
    private bool isUltAttack;

    public Animator Animator
    {
        get { return animator; }
        set { animator = value; }
    }

    public float AnimationVelocity
    {
        get { return animationVelocity; }
        set { animationVelocity = value; }
    }

    public float AnimationAccel
    {
        get { return animationAccel; }
        set { animationAccel = value; }
    }

    public float AnimationDeccel
    {
        get { return animationDeccel; }
        set { animationDeccel = value; }
    }

    public bool IsGrounded
    {
        get { return isGrounded; }
        set { isGrounded = value; }
    }
    public float NextEnabledTime
    {
        get { return nextEnabledTime; }
        set { nextEnabledTime = value; }
    }

    public bool IsLightAttack
    {
        get { return isLightAttack; }
        set { isLightAttack = value; }
    }
    public bool IsHeavyAttack
    {
        get { return isHeavyAttack; }
    }
    public bool IsUltAttack
    {
        get { return isUltAttack; }
    }

    private bool isAutoMoving;

    public bool IsAutoMoving
    {
        get { return isAutoMoving; }
        set { isAutoMoving = value; }
        
    }

    public void AutoAnimate()
    {
        if (IsAutoMoving)
        {
            animator.SetBool("isRunning", true);
        }
        else if (!IsAutoMoving)
        {
            animator.SetBool("isRunning", false);
        }
    }
    public void Animate(bool forwardKey, bool backwardKey, bool leftKey, bool rightKey, bool jumpKey, bool runKey, bool leftClick, bool rightClick)
    {
        bool isWalking = animator.GetBool("isWalking");
        bool isRunning = animator.GetBool("isRunning");
        bool isJumping = animator.GetBool("isJumping");

        if (jumpKey && isGrounded && !isJumping)
        {
            animator.SetBool("isJumping", true);
        }
        else if (!jumpKey || !isGrounded)
        {
            animator.SetBool("isJumping", false);
        }

        if (!isWalking && (forwardKey || backwardKey || leftKey || rightKey))
        {
            animator.SetBool("isWalking", true);
            if (animationVelocity < 1.0f)
            {
                animationVelocity += Time.deltaTime * animationAccel;
            }
        }
        else if (!forwardKey && !backwardKey && !leftKey && !rightKey)
        {
            animator.SetBool("isWalking", false);
            if (animationVelocity > 0.0f)
            {
                animationVelocity -= Time.deltaTime * animationDeccel;
            }
        }

        if (!isRunning && isWalking && runKey)
        {
            animator.SetBool("isRunning", true);
        }
        else if (!isWalking || !runKey)
        {
            animator.SetBool("isRunning", false);
        }

        animator.SetFloat("Velocity", animationVelocity);

        if (rightClick && !isHeavyAttack)
        {
            animator.SetBool("heavy", true);
            isHeavyAttack = true;
        }

        if (!rightClick && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.6f && animator.GetCurrentAnimatorStateInfo(0).IsName("heavy"))
        {
            animator.SetBool("heavy", false);
            isHeavyAttack = false;
        }



        //light attack

        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.6f && animator.GetCurrentAnimatorStateInfo(0).IsName("hit1"))
        {
            animator.SetBool("hit1", false);
            isLightAttack = false;
        }
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.6f && animator.GetCurrentAnimatorStateInfo(0).IsName("hit2"))
        {
            animator.SetBool("hit2", false);
            isLightAttack = false;
        }
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.6f && animator.GetCurrentAnimatorStateInfo(0).IsName("hit3"))
        {
            animator.SetBool("hit3", false);
            isLightAttack = false;
            noOfClicks = 0;
        }
        if(Time.time - lastClickedTime > maxComboDelay)
        {
            noOfClicks = 0;
        }
        if(Time.time > nextEnabledTime)
        {
            if (leftClick)
            {
                OnClick();
                isLightAttack = true;

            }
        }

    }

    public void Animate(bool forwardKey, bool backwardKey, bool leftKey, bool rightKey, bool jumpKey, bool runKey)
    {
        bool isWalking = animator.GetBool("isWalking");
        bool isRunning = animator.GetBool("isRunning");
        bool isJumping = animator.GetBool("isJumping");

        if (jumpKey && isGrounded && !isJumping)
        {
            animator.SetBool("isJumping", true);
        }
        else if (!jumpKey || !isGrounded)
        {
            animator.SetBool("isJumping", false);
        }

        if (!isWalking && (forwardKey || backwardKey || leftKey || rightKey))
        {
            animator.SetBool("isWalking", true);
            if (animationVelocity < 1.0f)
            {
                animationVelocity += Time.deltaTime * animationAccel;
            }
        }
        else if (!forwardKey && !backwardKey && !leftKey && !rightKey)
        {
            animator.SetBool("isWalking", false);
            if (animationVelocity > 0.0f)
            {
                animationVelocity -= Time.deltaTime * animationDeccel;
            }
        }

        if (!isRunning && isWalking && runKey)
        {
            animator.SetBool("isRunning", true);
        }
        else if (!isWalking || !runKey)
        {
            animator.SetBool("isRunning", false);
        }

        animator.SetFloat("Velocity", animationVelocity);
    }

    public void WizAnimate(bool forwardKey, bool backwardKey, bool leftKey, bool rightKey, bool jumpKey, bool runKey, bool leftClick, bool rightClic)
    {
        bool isWalking = animator.GetBool("isWalking");
        bool isRunning = animator.GetBool("isRunning");
        bool isJumping = animator.GetBool("isJumping");

        if (jumpKey && isGrounded && !isJumping)
        {
            animator.SetBool("isJumping", true);
        }
        else if (!jumpKey || !isGrounded)
        {
            animator.SetBool("isJumping", false);
        }

        if (!isWalking && (forwardKey || backwardKey || leftKey || rightKey))
        {
            animator.SetBool("isWalking", true);
            if (animationVelocity < 1.0f)
            {
                animationVelocity += Time.deltaTime * animationAccel;
            }
        }
        else if (!forwardKey && !backwardKey && !leftKey && !rightKey)
        {
            animator.SetBool("isWalking", false);
            if (animationVelocity > 0.0f)
            {
                animationVelocity -= Time.deltaTime * animationDeccel;
            }
        }

        if (!isRunning && isWalking && runKey)
        {
            animator.SetBool("isRunning", true);
        }
        else if (!isWalking || !runKey)
        {
            animator.SetBool("isRunning", false);
        }

        animator.SetFloat("Velocity", animationVelocity);

        if(leftClick)
        {
            animator.SetBool("hit1", true);
            isLightAttack = true;
        }
        else
        {
            animator.SetBool("hit1", false);
            isLightAttack = false;
        }
    }

    public void OnClick()
    {
        lastClickedTime = Time.time;
        noOfClicks++;
        if (noOfClicks == 1)
        {
            animator.SetBool("hit1", true);
        }
        noOfClicks = Mathf.Clamp(noOfClicks, 0, 3);
        if (noOfClicks >=2 && animator.GetCurrentAnimatorStateInfo(0).normalizedTime>0.4f && animator.GetCurrentAnimatorStateInfo(0).IsName("hit1"))
        {
            animator.SetBool("hit1", false);
            animator.SetBool("hit2", true);
        }
        if (noOfClicks >= 3 && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.4f && animator.GetCurrentAnimatorStateInfo(0).IsName("hit2"))
        {
            animator.SetBool("hit2", false);
            animator.SetBool("hit3", true);
            noOfClicks = 0;
            isLightAttack = false;
        }
    }
    public void DisableAnimation()
    {
        animator.SetBool("isJumping", false);
        animator.SetBool("isWalking", false);
        animator.SetBool("isRunning", false);
        animator.SetBool("hit1", false);
        animator.SetBool("hit2", false);
        animator.SetBool("hit3", false);
        animator.SetBool("heavy", false);
    }

}
