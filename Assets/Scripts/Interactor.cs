using UnityEngine;



public abstract class Interactor:MonoBehaviour
{
    public abstract void Interact(Character character);
    public virtual void Hover(Character character)
    {
        return;
    }
    private void Awake() {
        transform.gameObject.layer = LayerMask.NameToLayer("Interactable");
    }

    
}
