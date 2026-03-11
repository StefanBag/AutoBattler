using System.Collections;
using TMPro;
using UnityEngine;

public class TextScript : MonoBehaviour
{
    public string[] texts;   
    TMP_Text tmp;
    string text;
    void Awake()
    {
        tmp = GetComponent<TMP_Text>();

        int index = Random.Range(0, texts.Length);
        text = texts[index];
    }
    

    public void LoadText()
    {
        tmp.text = "";
        StartCoroutine(SpaceText(0.05f));

    }

    IEnumerator SpaceText(float timeout)
    {
        foreach(char ch in text)
        {
            tmp.text += ch;
            yield return new WaitForSeconds(timeout); 
        }
        
  
    }
    
    void Update()
    {
        
    }
}
