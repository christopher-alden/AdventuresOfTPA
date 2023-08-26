using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class NpcInteractionController : MonoBehaviour, InteractableInterface
{
    [SerializeField] private List<AudioClip> soundList;
    [SerializeField] private AudioSource soundSource;
    [SerializeField, Range(0f, 1f)] private float soundVolume = 0.1f;
    [SerializeField] private string interactText;
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] GameObject questContainer;
    private CursorManager cursorManager;
    private AudioManager audioManager;
    private void Start()
    {
        Hide();
        audioManager = AudioManager.Instance;
        cursorManager = CursorManager.Instance;
    }
    public void Interact()
    {

        audioManager.PlayAudioClip(soundSource, soundList, 0, soundVolume, false);
        virtualCamera.Priority = 50;
        cursorManager.EnableCursor();
        Show();
    }
    public void Show()
    {
        questContainer.SetActive(true);
    }
    public void Hide()
    {
        questContainer.SetActive(false);
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
        Hide();
        cursorManager.DisableCursor();
        virtualCamera.Priority = 1;
        
    }
}
