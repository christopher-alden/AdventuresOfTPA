using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ShopkeeperInteractController : MonoBehaviour, InteractableInterface
{
    [SerializeField] private List<AudioClip> soundList;
    [SerializeField] private AudioSource soundSource;
    [SerializeField, Range(0f, 1f)] private float soundVolume = 0.1f;
    [SerializeField] private string interactText;
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    private AudioManager audioManager;
    private CursorManager cursorManager;
    private ShopManager shopManager;

    private void Start()
    {
        shopManager = ShopManager.Instance;
        cursorManager = CursorManager.Instance;
        audioManager = AudioManager.Instance;
    }
    public void Interact()
    {
        audioManager.PlayAudioClip(soundSource, soundList, 0, soundVolume, false);
        shopManager.Show();
        cursorManager.EnableCursor();
        virtualCamera.Priority = 50;
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
        shopManager.Hide();
        cursorManager.DisableCursor();
        virtualCamera.Priority = 1;
    }

}
