using UnityEngine;
using System.Collections;
public class GameManager : MonoBehaviour
{
    public GameObject PlayerHUD;
    public GameObject Text;
    bool active_message;
    void Start()
    {
    
        Text.transform.Find("Panel").Find("EnemyText").GetComponent<TextScript>().LoadText();
        active_message = true;
    }

    
    void Update()
    {
        
        if (active_message && Input.GetKey(KeyCode.Space) )
        {
            active_message = false;
            PlayerHUD.SetActive(true);
            Text.SetActive(false);
        }

    }
}
