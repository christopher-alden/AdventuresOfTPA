using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepAudioController : MonoBehaviour
{
    private AudioSource footstepSource;
    private List<AudioClip> footstepSounds;
    private float footstepFadeOffTime;
    private float footstepVolume;

    public bool isGrounded = true;

    private float stepInterval;
    private bool isWalking = false;

    private string collidedTag;
    private int[] grass = { 0, 1 ,2, 3, 4, 5, 6, 7 };
    private int[] path = { 8, 9, 10 ,11, 12, 13, 14, 15};

    public List<AudioClip> FootstepSounds
    {
        get { return footstepSounds; }
        set { footstepSounds = value; }
    }
    public AudioSource FootstepSource
    {
        get { return footstepSource; }
        set { footstepSource = value; }
    }
    public float FootstepFadeOffTime
    {
        get { return footstepFadeOffTime; }
        set { footstepFadeOffTime = value; }
    }
    public float FootstepVolume
    {
        get { return footstepVolume; }
        set { footstepVolume = value; }
    }
    public string CollidedTag
    {
        get { return collidedTag; }
        set { collidedTag = value; }
    }
    public bool IsGrounded
    {
        get { return isGrounded; }
        set { isGrounded = value; }
    }

    public void Footstep(bool forwardKey, bool backwardKey, bool leftKey, bool rightKey, bool jumpKey, bool runKey)
    {
        if (runKey)
        {
            stepInterval = 0.2f;
        }
        if (!runKey)
        {
            stepInterval = 0.27f;
        }
        if ((forwardKey || backwardKey || leftKey || rightKey) && !isWalking)
        {
            StartFootstepLoop();
        }
        else if (!(forwardKey || backwardKey || leftKey || rightKey) && isWalking)
        {
            StopFootstepLoop();
        }
    }

    private void StartFootstepLoop()
    {
        isWalking = true;
        StartCoroutine(PlayFootstepLoop());
    }

    private void StopFootstepLoop()
    {
        isWalking = false;
        StartCoroutine(FadeOffAudio());
    }

    private IEnumerator PlayFootstepLoop()
    {
        while (isWalking)
        {
            if (collidedTag == "Terrain")
            {
                footstepSource.clip = footstepSounds[Random.Range(grass[0], grass[7])];
            }
            else if (collidedTag == "Path")
            {
                footstepSource.clip = footstepSounds[Random.Range(path[0], path[7])];
            }
            footstepSource.loop = false;
            footstepSource.Play();

            yield return new WaitForSeconds(stepInterval);
        }
    }

    private IEnumerator FadeOffAudio()
    { 
        float timer = 0f;

        while (timer < footstepFadeOffTime)
        {
            timer += Time.deltaTime;
            footstepSource.volume = Mathf.Lerp(footstepVolume, 0f, timer / footstepFadeOffTime);
            yield return null;
        }

        footstepSource.Stop();
        footstepSource.volume = footstepVolume;
    }


}
