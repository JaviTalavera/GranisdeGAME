using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class I_Flash : Item {

    public float speedMultiplier;
    public float time;

    public override void Action()
    {
        base.Action();
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<Player>().UpdateSpeed(speedMultiplier, time);

        GameObject spawn = GameObject.FindGameObjectWithTag("Spawn");
        spawn.GetComponent<Spawn>().UpdateFrecuencia(speedMultiplier, time);
    }
}
