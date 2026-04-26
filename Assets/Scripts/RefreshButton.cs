using UnityEngine;

public class RefreshButton : MonoBehaviour
{
    public Character character;
    public void Refresh()
    {
        Transform shopButtons = transform.parent.Find("ShopButtons");
        character.money -= 10;
        foreach (Transform child in shopButtons)
        {
            child.gameObject.SetActive(true);
            BuyUnits buyUnit = child.GetComponent<BuyUnits>();
            if (buyUnit != null)
                buyUnit.Button_Setup();
        }
    }
}
