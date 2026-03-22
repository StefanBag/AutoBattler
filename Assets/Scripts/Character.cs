using UnityEngine;


public class Character : MonoBehaviour
{
    public int level = 0;
    

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Interact();
    }

    

    public void Interact()
    {
        RaycastHit hit;
        int layerMask = 1 << LayerMask.NameToLayer("Interactable");
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            Interactor item = hit.collider.GetComponentInParent<Interactor>();
            Debug.Log("Hit object: " + hit.collider.name); 

            
            item.Hover(this);
        }
    }


}
