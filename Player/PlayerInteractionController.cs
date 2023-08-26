using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractionController : MonoBehaviour
{
    private bool interacting = false;
    private InteractableInterface lastInteractable;
    public void PlayerInteraction(bool interactKey)
    {
        if (interactKey)
        {
            InteractableInterface interactable = GetInteractableObject();

            if (interactable != null)
            {
                if (interacting)
                {
                    interactable.StopInteract();
                    interacting = false;
                    lastInteractable = null;
                }
                else
                {
                    interactable.Interact();
                    interacting = true;
                    lastInteractable = interactable;
                }
            }
            else
            {
                if(lastInteractable != null)
                {
                    lastInteractable.StopInteract();
                    interacting = false;
                }
            }
                
            
        }
    }

    public InteractableInterface GetInteractableObject()
    {
        List<InteractableInterface> interactableList = new List<InteractableInterface>();
        float interactRange = 2f;
        Collider[] colliderArray = Physics.OverlapSphere(transform.position, interactRange);
        foreach (Collider collider in colliderArray)
        {
            if (collider.TryGetComponent(out InteractableInterface interactable))
            {
                interactableList.Add(interactable);
            }
        }
        InteractableInterface closestInteractable = null;
        foreach (InteractableInterface interactable in interactableList)
        {
            if (closestInteractable == null)
            {
                closestInteractable = interactable;
            }
            else
            {
                if (Vector3.Distance(transform.position, interactable.GetTransform().position) < Vector3.Distance(transform.position, closestInteractable.GetTransform().position))
                {
                    closestInteractable = interactable;
                }
            }
        }
        return closestInteractable;
    }
    public bool GetInteracting()
    {
        return interacting;
    }

}
