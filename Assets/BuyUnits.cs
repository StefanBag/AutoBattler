using System.Collections.Generic;
using UnityEngine;

public class BuyUnits : MonoBehaviour
{
    
    List<BenchSlot> bench_slots = new List<BenchSlot>();
    public GameObject unit;
    public AudioSource audioSource;

    void Start()
    {
        GameObject Bench = transform.parent.parent.Find("FriendlyBench").gameObject;
        Debug.Log(Bench);
        foreach (Transform slot in Bench.transform)
        {
            bench_slots.Add(slot.gameObject.GetComponent<BenchSlot>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BuyUnit()
    {
        if(unit != null){
            BenchSlot available_slot = CheckAvailableSlots();
            Debug.Log(available_slot);
            available_slot.AddUnit(unit);
            audioSource.Play();
            unit = null;
        }
    }

    BenchSlot CheckAvailableSlots()
    {
        foreach(BenchSlot slot in bench_slots)
        {
            if(slot.unit == null)
            {
                return slot;
            }
        }
        return null;
    }
}
