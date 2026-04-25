// FieldSlot.cs
using UnityEngine;

public enum slotType
{
    Ally,
    Enemy
}

public class FieldSlot : Interactor
{
    Color ogColor;
    Color newColor;
    new Renderer renderer;
    bool hovered = false;
    GameObject unit = null;
    public slotType slot_type;
    private AudioSource audioSource;
    [SerializeField] private AudioClip selectClip;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null && Camera.main != null)
            audioSource = Camera.main.GetComponent<AudioSource>();

        renderer = GetComponent<Renderer>();
        ogColor = renderer.material.color;
        newColor = new Color(0.0f, 1.0f, 0.0f, 1.0f);

        if (slot_type == slotType.Enemy)
            newColor = new Color(1.0f, 0.0f, 0.0f, 1.0f);
    }

    void Update()
    {
        if (hovered)
        {
            renderer.material.color = newColor;
            hovered = false;
        }
        else
        {
            renderer.material.color = ogColor;
        }
    }

    public override void Interact(Character character)
    {
        if (slot_type == slotType.Ally)
        {
            if (character.holding != null && unit == null)
            {
                AddUnit(character.holding);
                character.holding = null;
                audioSource.PlayOneShot(selectClip);

                // Unit placed on board — refresh traits
                TraitSystem.Instance?.RefreshTraits();
            }
            else if (character.holding == null && unit != null)
            {
                character.holding = unit;
                unit = null;

                // Unit picked up from board — refresh traits
                TraitSystem.Instance?.RefreshTraits();
            }
        }
    }

    public override void Hover(Character character)
    {
        hovered = true;
    }

    public void AddUnit(GameObject new_unit)
    {
        unit = new_unit;
        new_unit.transform.parent = transform;
        new_unit.transform.localPosition = new Vector3(0, 2, 0);
    }
}