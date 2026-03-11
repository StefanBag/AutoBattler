using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Level_Button : MonoBehaviour
{
    public int level = 0;

    void Start()
    {
        
        TMP_Text levelTextNum = transform.Find("LevelTextNum").GetComponent<TMP_Text>();
        Button button = GetComponent<Button>();
        Image image = GetComponent<Image>();
        
    
        
        Transform LineGO = transform.Find("Line");
        Image Line = null;
        bool found = false;
        if(LineGO != null){ 
            Line = LineGO.GetComponent<Image>();
            found = true;
        }

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
        SceneManager.LoadScene("BattleScene");
    }
}
