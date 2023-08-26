using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Transform cam;
    private Rigidbody target;

    private float jumpSpeed;
    private float verticalSpeed;
    private float runSpeed;
    private float walkSpeed;
    private float e = 1f;
    private float characterSpeed;

    public bool isGrounded = true;

    private float turnSmoothing;
    private float turnSmoothVelocity;

    private LayerMask groundLayer;
    private AnimationCurve aniCurve;
    private float alignmentTime;

    private string collidedObjectTag;

    private float alignmentSpeed = 10f;

    public Transform Cam
    {
        get { return cam; }
        set { cam = value; }
    }
    public Rigidbody Target
    {
        get { return target; }
        set { target = value; }
    }
    public float JumpSpeed
    {
        get { return jumpSpeed; }
        set { jumpSpeed = value; }
    }
    public float RunSpeed
    {
        get { return runSpeed; }
        set { runSpeed = value; }
    }
    public float WalkSpeed
    {
        get { return walkSpeed; }
        set { walkSpeed = value; }
    }
    public float E
    {
        get { return e; }
        set { e = value; }
    }
    public bool IsGrounded
    {
        get { return isGrounded; }
        set { isGrounded = value; }
    }
    public float TurnSmoothing
    {
        get { return turnSmoothing; }
        set { turnSmoothing = value; }
    }
    public LayerMask GroundLayer
    {
        get { return groundLayer; }
        set { groundLayer = value; }
    }
    public AnimationCurve AniCurve
    {
        get { return aniCurve; }
        set { aniCurve = value; }
    }
    public float AlignmentTime
    {
        get { return alignmentTime; }
        set { alignmentTime = value; }
    }
    public string CollidedObjectTag
    {
        get { return collidedObjectTag; }
        set { collidedObjectTag = value; }
    }

    public void Movement(bool forwardKey, bool backwardKey,bool leftKey,bool rightKey,bool jumpKey, bool runKey)
    {
        if (runKey) characterSpeed = runSpeed;
        else if (!runKey) characterSpeed = walkSpeed;

        float x = 0f;
        float y = 0f;

        if (forwardKey) y = 1f;
        else if (backwardKey) y = -1f;
        if (leftKey) x = -1f;
        else if (rightKey) x = 1f;
        
        if (verticalSpeed>0) verticalSpeed += Physics.gravity.y * Time.deltaTime;
        if (jumpKey && isGrounded)
        {
            verticalSpeed = -0.5f;
            verticalSpeed = jumpSpeed;
            target.AddForce(transform.up * jumpSpeed, ForceMode.Impulse);
            isGrounded = false;
        }


        Vector3 direction = new Vector3(x, 0f, y).normalized;
       
        if (direction != Vector3.zero)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothing);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            Vector3 counterMovement = new Vector3(-target.velocity.x, 0, -target.velocity.z);

            target.AddForce(moveDir * characterSpeed);
            target.AddForce(counterMovement * e);
            //SurfaceAlignment();
        }
    }
    public void SurfaceAlignment()
    {
        Ray ray = new Ray(transform.position, -transform.up);

        if (Physics.Raycast(ray, out RaycastHit hit, groundLayer))
        {
            Quaternion targetRotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * alignmentSpeed);
        }
    }


}
