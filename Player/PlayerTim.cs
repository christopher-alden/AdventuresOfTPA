using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTim : PlayerManager
{
    private void InitSounds()
    {
        footstepAudioController = GetComponent<FootstepAudioController>();
        footstepAudioController.FootstepSource = footstepSource;
        footstepAudioController.FootstepSounds = footstepSounds;
        footstepAudioController.FootstepFadeOffTime = footstepFadeOffTime;
        footstepAudioController.FootstepVolume = footstepVolume;
    }
    private void InitAnimation()
    {
        animator = GetComponent<Animator>();
        animationController = GetComponent<AnimationController>();
        animationController.Animator = animator;
        animationController.AnimationVelocity = animationVelocity;
        animationController.AnimationDeccel = animationDeccel;
        animationController.AnimationAccel = animationAccel;
    }
    protected void InitStat()
    {
        health = 350f;
        lightAttackDmg = 15f;
        heavyAttackDmg = 25f;
        ultAttackDmg = 40f;
        secondaryStat = 20f;
        heavyAttackLength = 1f;
        lightAttackLength = 0.6f;
    }
    protected override void ChildStart()
    {
        isMainCharacter = true;
        InitStat();
        GetPlayerCamera();
        InitMovementDefault();
        InitAnimation();
        InitSounds();
        InitHealthBar();
        InitAStar();
        ToggleAStar();
        nextEnabledTime = 0.7f;
    }
    protected override void ChildUpdate()
    {
        if (!CheckIsAlive())
        {
            arenaManager.RemovePlayer(this);
        }
        CheckIsGrounded();
        CheckIsMainCharacter();
        if (isMainCharacter)
        {
            CheckIsInventoryOpen();
            CheckIsAttacking();
            playerMovement.Movement(forwardKey, backwardKey, leftKey, rightKey, jumpKey, runKey);
            animationController.Animate(forwardKey, backwardKey, leftKey, rightKey, jumpKey, runKey, leftClick, rightClick);
        }
        else
        {
            animationController.AutoAnimate();
            RotateTowardsEnemy();
        }

    }

}
