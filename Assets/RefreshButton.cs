using UnityEngine;

public class RefreshButton : MonoBehaviour
{
    public void Refresh()
    {
        Transform shopButtons = transform.parent.Find("ShopButtons");

        foreach (Transform child in shopButtons)
        {
            BuyUnits buyUnit = child.GetComponent<BuyUnits>();
            if (buyUnit != null)
                buyUnit.Button_Setup();
        }
    }
}
