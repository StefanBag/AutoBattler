using UnityEngine;
using System.Collections;
public class GameManager : MonoBehaviour
{
    public GameObject PlayerHUD;
    public GameObject Text;
    public BuyTimer buy_phase;
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

            Text.SetActive(false);
            buy_phase.StartBuyPhase();
            
        }

    }
}
