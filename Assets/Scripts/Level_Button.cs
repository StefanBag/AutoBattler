using UnityEngine;
using UnityEngine.UI;

public class Level_Button : MonoBehaviour
{
    public int level = 0;

    void Start()
    {
        Button button = GetComponent<Button>();
        Image image = GetComponent<Image>();

        Colors colors = transform.parent.GetComponent<Colors>();
        Character character = transform.parent.parent.parent.Find("Character").GetComponent<Character>();
        int characterLevel = character.level;

        if (characterLevel < level)
        {
            image.color = colors.Incomplete;
            button.interactable = false;
        }
        else if (characterLevel > level)
        {
            image.color = colors.Complete;
            button.interactable = true;
        }
        else if (characterLevel  == level)
        {
            image.color = colors.Active;
            button.interactable = true;
        }
    }
}
