using UnityEngine;

public class I_Casalla : Item
{

    public float time;
    public float pointsMultiplier;

    public override void Action()
    {
        base.Action();
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().SetBorracho(time);
    }
    public override void Die()
    {
        base.Die();
        int iDamage = (int)(GameScore.GetScore() / pointsMultiplier);
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().AddPoints(-iDamage);
    }
}
