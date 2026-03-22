using UnityEngine;

public class FieldSlot : Interactor
{
    

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
        throw new System.NotImplementedException();
    }

    public override void Hover(Character character)
    {
        hovered = true;
    }
}
