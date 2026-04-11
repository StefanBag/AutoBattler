using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BuyUnits : MonoBehaviour
{
    List<BenchSlot> bench_slots = new List<BenchSlot>();
    public UnitData unit;
    public AudioSource audioSource;
    private TextMeshProUGUI unitNameText;
    private UnitShopManager unitShopManager;

    void Start()
    {
        unitNameText = transform.Find("UnitName").GetComponent<TextMeshProUGUI>();
        unitShopManager = FindFirstObjectByType<UnitShopManager>();
        AssignRandomUnit();
        GameObject Bench = transform.parent.parent.Find("FriendlyBench").gameObject;
        

        foreach (Transform slot in Bench.transform)
        {
            bench_slots.Add(slot.gameObject.GetComponent<BenchSlot>());
        }
        unitNameText.text = unit.unit_name;
    }

    void Update()
    {
        
    }

    public void BuyUnit()
    {
        if (unit != null)
        {
            BenchSlot available_slot = CheckAvailableSlots();
            Debug.Log(available_slot);

            if (available_slot != null)
            {
                available_slot.AddUnit(Instantiate(unit.model));

                if (audioSource != null)
                {
                    audioSource.Play();
                }

                unit = null;
            }
        }
    }

    void AssignRandomUnit()
    {
        if (unitShopManager != null && unitShopManager.unitPool.Count > 0)
        {
            int index = Random.Range(0, unitShopManager.unitPool.Count);
            unit = unitShopManager.unitPool[index];
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
