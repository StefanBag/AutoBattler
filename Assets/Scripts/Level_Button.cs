using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Level_Button : MonoBehaviour
{
    public int level = 0;
    TMP_Text levelTextNum;
    Button button;
    Image image;
    Colors colors;
    Character character;
    int characterLevel;
    void Start()
    {
        
        levelTextNum = transform.Find("LevelTextNum").GetComponent<TMP_Text>();
        button = GetComponent<Button>();
        image = GetComponent<Image>();
        colors = transform.parent.GetComponent<Colors>();
        Character character = transform.parent.parent.parent.Find("Character").GetComponent<Character>();
        characterLevel = character.level;

        colorSquares();
        
    }

    public void colorSquares()
    {
        Transform LineGO = transform.Find("Line");
        Image Line = null;
        bool found = false;
        if(LineGO != null){ 
            Line = LineGO.GetComponent<Image>();
            found = true;
        }

        
        if(LineGO != null){ 
            Line = LineGO.GetComponent<Image>();
            found = true;
        }

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
                Line.color = colors.Complete;
            }
            button.interactable = true;
        }
        else if (characterLevel  == level)
        {
            image.color = colors.Active;
            levelTextNum.color = colors.Active;
            if(found)
            {
                Line.color = colors.Active;
            }
            button.interactable = true;
        }
    }

    public void switch_scene()
    {
        SceneManager.LoadScene(level);
    }
}
