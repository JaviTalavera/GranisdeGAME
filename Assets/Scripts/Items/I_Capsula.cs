using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class I_Capsula : Item {
    
    public float time;
    public GameObject explosion;

    public override void Action()
    {
        base.Action();
        Instantiate(explosion, transform.position, Quaternion.identity);
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().SetInmunity(time);
    }
}
