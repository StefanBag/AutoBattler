using UnityEngine;

public class BenchSlot : Interactor
{
    public GameObject unit;    
    Color ogColor;
    Color newColor;
    new Renderer renderer;
    bool hovered = false;

    void Start()
    {
        
        renderer = GetComponent<Renderer>();

        
        ogColor = renderer.material.color;
        newColor = new Color(0.0f, 1.0f, 0.0f, 1.0f);

        
        
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
        if(unit != null)
        {
            character.holding = unit;
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
