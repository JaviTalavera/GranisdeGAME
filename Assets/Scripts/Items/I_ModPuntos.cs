using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class I_ModPuntos : Item {

    public int score;

    public override void Action()
    {
        base.Action();
        if (score > 0)
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().AddPoints(score);
    }

    public override void Die()
    {
        base.Die();
        if (damage != 0 )
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().SetDamage(damage);
    }
}
