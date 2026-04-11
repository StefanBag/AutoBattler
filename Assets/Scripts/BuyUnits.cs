using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BuyUnits : MonoBehaviour
{
    List<BenchSlot> bench_slots = new List<BenchSlot>();
    public UnitData unit;
    public AudioSource audioSource;
    private TextMeshProUGUI unitNameText;
    private TextMeshProUGUI priceText;
    public Character character;

    void Start()
    {
        character = FindFirstObjectByType<Character>();
        unitNameText = transform.Find("UnitName").GetComponent<TextMeshProUGUI>();
        priceText = transform.Find("Price").GetComponent<TextMeshProUGUI>();
        GameObject Bench = GameObject.Find("FriendlyBench");
        

        foreach (Transform slot in Bench.transform)
        {
            bench_slots.Add(slot.gameObject.GetComponent<BenchSlot>());
        }
        
        Button_Setup();
    }

    public void Button_Setup()
    {
        AssignRandomUnit();
        unitNameText.text = unit.unit_name;
        priceText.text = $"${unit.cost}";
    }

    void Update()
    {
        Debug.Log(character.money);
    }

    public void BuyUnit()
    {
        if (unit != null)
        {
            BenchSlot available_slot = CheckAvailableSlots();
            Debug.Log(available_slot);

            if (available_slot != null && character.money >= unit.cost)
            {
                
                available_slot.AddUnit(Instantiate(unit.model));

                if (audioSource != null)
                {
                    audioSource.Play();
                }
                character.money -= unit.cost;
                unit = null;
                this.gameObject.SetActive(false);
            }
        }
    }

    void AssignRandomUnit()
    {
        GameObject unitsFolder = GameObject.Find("Units");
        if (unitsFolder == null) return;

        List<GameObject> unitList = new();
        foreach (Transform child in unitsFolder.transform)
            unitList.Add(child.gameObject);

        if (unitList.Count > 0)
        {
            GameObject picked = unitList[Random.Range(0, unitList.Count)];
            unit = picked.GetComponent<UnitAI>().unit_data;
        }
    }

    BenchSlot CheckAvailableSlots()
    {
        foreach (BenchSlot slot in bench_slots)
        {
            if (slot.unit == null)
            {
                return slot;
            }
        }
        return null;
    }
}
