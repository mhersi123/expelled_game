using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatTest : MonoBehaviour, IDamageReceiver
{
    public void TakeDamage(float dmg)
    {
        Debug.Log("Took Damage!");
        transform.Rotate(new Vector3(0, 1, 0));
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
