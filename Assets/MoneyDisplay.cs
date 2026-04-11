using UnityEngine;
using TMPro;

public class MoneyDisplay : MonoBehaviour
{
    public Character character;
    private TextMeshProUGUI moneyText;

    void Start()
    {
        moneyText = GetComponentInChildren<TextMeshProUGUI>();
        
    }

    void Update()
    {
        if (character != null && moneyText != null)
            moneyText.text = $"Money: ${character.money}";
    }
}
