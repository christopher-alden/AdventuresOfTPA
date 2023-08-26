using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class CAInteractionController : MonoBehaviour, InteractableInterface
{
    [SerializeField] private List<AudioClip> soundList;
    [SerializeField] private AudioSource soundSource;
    [SerializeField, Range(0f, 1f)] private float soundVolume = 0.1f;
    [SerializeField] private PostProcessVolume normalVolume;
    [SerializeField] private GameObject HorrorVolume;
    [SerializeField] private PostProcessVolume transitionVolume;
    [SerializeField] private string interactText;
    private float transitionDuration = 0.5f;
    private float enableDuration = 3.5f;

    private void Start()
    {
        HorrorVolume.SetActive(true);
        transitionVolume.enabled = false;
    }

    public void Interact()
    {
        AudioManager.Instance.PlayAudioClip(soundSource, soundList, 0, soundVolume, false);
        StartCoroutine(EnableTransitionVolume());
    }

    private IEnumerator EnableTransitionVolume()
    {
        transitionVolume.enabled = true;

        float elapsedTime = 0f;
        while (elapsedTime < transitionDuration)
        {
            float t = elapsedTime / transitionDuration;

            normalVolume.weight = Mathf.Lerp(1f, 0f, t);
            transitionVolume.weight = Mathf.Lerp(0f, 1f, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        normalVolume.weight = 0f;
        transitionVolume.weight = 1f;

        yield return new WaitForSeconds(enableDuration);

        StartCoroutine(TransitionVolumes());
    }

    private IEnumerator TransitionVolumes()
    {
        float elapsedTime = 0f;
        while (elapsedTime < transitionDuration)
        {
            float t = elapsedTime / transitionDuration;

            normalVolume.weight = Mathf.Lerp(0f, 1f, t);
            transitionVolume.weight = Mathf.Lerp(1f, 0f, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        normalVolume.weight = 1f;
        transitionVolume.weight = 0f;

        if (transitionVolume.enabled) transitionVolume.enabled = false;
    }

    public Transform GetTransform()
    {
        return transform;
    }

    public string GetInteractText()
    {
        return interactText;
    }
    public void StopInteract()
    {

    }

}
