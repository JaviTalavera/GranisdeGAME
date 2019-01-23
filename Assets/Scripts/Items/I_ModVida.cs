using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class I_ModVida : Item {

    public GameObject explosion;

    public override void Action()
    {
        base.Action();
        if (explosion != null)
            Instantiate(explosion, transform.position, Quaternion.identity);
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().SetDamage(damage);
    }
}
