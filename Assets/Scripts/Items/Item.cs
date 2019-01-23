using UnityEngine;

public class Item : MonoBehaviour
{
    public int id;
    public int frecuencia;
    public float speed = 4f;
    public int damage = 0;
    /// <summary>
    /// Si está marcado entra dentro de la frecuencia del Spawner
    /// </summary>
    public bool bFrecuencia = true;
    public float iItemsBetweenFrecuenciaDec;
    public int iFrecuenciaPropia; 
    public int iMinFrecuenciaPropia;

    public AudioClip clip;


    private Rigidbody2D rb2d;
    private Vector2 mov;

    // Use this for initialization
    void Start()
    {
        mov = new Vector2(0, -1);
        rb2d = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        rb2d.MovePosition(rb2d.position + mov * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Finish")
        {
            Die();
            Destroy(this.gameObject);
        }
    }

    public AudioClip GetAudio()
    {
        return clip;
    }

    public virtual void Action () {
        if (clip != null)
            GameScore.PlaySound(clip);
    }
    public virtual void Die() { }
}
