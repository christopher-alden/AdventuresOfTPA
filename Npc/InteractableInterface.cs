using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface InteractableInterface
{
    void Interact();
    void StopInteract();
    Transform GetTransform();
    string GetInteractText();
    
}
