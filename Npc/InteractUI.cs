using TMPro;
using UnityEngine;

public class InteractUI : MonoBehaviour
{
    [SerializeField] private PlayerInteractionController playerInteract;
    [SerializeField] private TextMeshProUGUI interactTMP;


    private void Update()
    {
        if (playerInteract.GetInteractableObject() != null && playerInteract.GetInteracting()==false) Show(playerInteract.GetInteractableObject());
        else Hide();
    }
    private void Show(InteractableInterface interactable)
    {
        interactTMP.enabled = true;
        interactTMP.text = interactable.GetInteractText();
        
    }
    private void Hide()
    {
        interactTMP.enabled = false;
    }
}
