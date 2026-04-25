using System;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class Character : MonoBehaviour
{
    private static int currentLevel = -1;

    public int level = 0;
    public int money = 90;
    public GameObject holding = null;
    public Boolean active = true;
    [SerializeField] private AudioClip selectClip;

    private AudioSource audioSource;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetProgress()
    {
        currentLevel = -1;
    }

    void Awake()
    {
        if (currentLevel < 0)
            currentLevel = level;
        else
            level = currentLevel;
    }

    public static void SetCurrentLevel(int level)
    {
        currentLevel = level;
    }

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
