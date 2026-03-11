using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Level_Button : MonoBehaviour
{
    public int level = 0;

    void Start()
    {
        
        TMP_Text levelTextNum = transform.Find("LevelTextNum").GetComponent<TMP_Text>();
        Button button = GetComponent<Button>();
        Image image = GetComponent<Image>();
        
    
        bool found = transform.Find("Line").TryGetComponent<Image>(out Image Line);
    
        Colors colors = transform.parent.GetComponent<Colors>();
        Character character = transform.parent.parent.parent.Find("Character").GetComponent<Character>();
        int characterLevel = character.level;

        if (characterLevel < level)
        {
            image.color = colors.Incomplete;
            levelTextNum.color = colors.Incomplete;
            if(found)
            {
                Line.color = colors.Incomplete;
            }
            button.interactable = false;
        }
        else if (characterLevel > level)
        {
            image.color = colors.Complete;
            levelTextNum.color = colors.Complete;
            if(found)
            {
                Line.color = colors.Active;
            }
            button.interactable = true;
        }
        else if (characterLevel  == level)
        {
            image.color = colors.Active;
            levelTextNum.color = colors.Active;
            button.interactable = true;
        }
    }
}
