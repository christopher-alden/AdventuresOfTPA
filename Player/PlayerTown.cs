using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTown : PlayerManager
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

    protected override void ChildStart()
    {
        isMainCharacter = true;
        InitMovementDefault();
        InitAnimation();
        InitSounds();
        InitInteraction();
    }
    protected override void ChildUpdate()
    {
        EnableInteraction();
        CheckIsOnMenu();
        CheckIsGrounded();
        CheckIsInventoryOpen();
        CheckIsInteracting();
        MovementHandicap();
        animationController.Animate(forwardKey, backwardKey, leftKey, rightKey, jumpKey, runKey);
        playerMovement.Movement(forwardKey, backwardKey, leftKey, rightKey, jumpKey, runKey);
    }

}
