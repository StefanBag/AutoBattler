using UnityEngine;
using System.Collections;
public class GameManager : MonoBehaviour
{
    public GameObject PlayerHUD;
    public GameObject Text;
    
    void Start()
    {
        StartCoroutine(HUDSequence());
    }

    IEnumerator HUDSequence()
    {
        PlayerHUD.SetActive(false);   
        Text.SetActive(true);       

        yield return new WaitForSeconds(2f); 

        PlayerHUD.SetActive(true);  
        Text.SetActive(false);        
    }
    
    void Update()
    {
        
    }
}
