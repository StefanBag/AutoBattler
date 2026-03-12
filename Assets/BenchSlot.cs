using UnityEngine;

public class BenchSlot : MonoBehaviour
{
    public GameObject unit;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddUnit(GameObject new_unit)
    {
        unit = new_unit;
        new_unit.transform.parent = transform;
        new_unit.transform.localPosition = new Vector3(0, 2, 0);
    }
}
