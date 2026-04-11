using UnityEngine;

public class Character : MonoBehaviour
{
    public int level = 0;
    public int money = 90;
    public GameObject holding = null;

    [SerializeField] private AudioClip selectClip;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null && Camera.main != null)
        {
            audioSource = Camera.main.GetComponent<AudioSource>();
        }
    }

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

            if (item != null)
            {
                item.Hover(this);

                if (Input.GetMouseButtonDown(0))
                {
                    item.Interact(this);
                }
            }
        }
    }
}
