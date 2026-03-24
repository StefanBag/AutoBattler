using UnityEngine;

public class Character : MonoBehaviour
{
    public int level = 0;
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

                    Transform hitRoot = hit.collider.transform.root;
                    Transform hitParent = hit.collider.GetComponentInParent<Transform>();

                    bool clickedFieldSlot = item is FieldSlot;
                    bool clickedAllyUnit =
                        hit.collider.CompareTag("AllyUnit") ||
                        (hitParent != null && hitParent.CompareTag("AllyUnit")) ||
                        (hitRoot != null && hitRoot.CompareTag("AllyUnit"));

                    if ((clickedFieldSlot || clickedAllyUnit) && audioSource != null && selectClip != null)
                    {
                        audioSource.PlayOneShot(selectClip);
                    }
                }
            }
        }
    }
}