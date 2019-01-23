using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {

    public float speed = 4f;
    public GameObject textoFlotante;

    private float originalSpeed;
    private float maxSpeed;
    private Rigidbody2D rb2d;
    private Vector2 mov;
    private int direccion;
    private GameObject flashEffect;
    private GameObject capsulaEffect;
    private Animator anim;
    private Vector2 destino;
    private bool bInmune;

    //public AudioClip borracho;

	// Use this for initialization
	void Start () {

        anim = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        flashEffect = transform.GetChild(0).gameObject;
        capsulaEffect = transform.GetChild(1).gameObject;
        originalSpeed = speed;
        maxSpeed = speed * 2;
        direccion = 1;
        bInmune = false;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (GameScore.iControles == (int)GameScore.CONTROLES.MOVIMIENTO)
        {
            float movX = 0;
            if (Input.acceleration.x < -0.15)
                movX = -1;
            else if (Input.acceleration.x > 0.15)
                movX = 1;
            mov = new Vector2(
               direccion * (movX + Input.GetAxisRaw("Horizontal")),
               0
           );

        }
        else if (GameScore.iControles == (int)GameScore.CONTROLES.TOUCH)
        {
            /*if (Input.touchCount > 0)
                destino = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            float movX = (destino.x - transform.position.x) > 0 ? 1 : -1;
            if (Mathf.Abs(destino.x - transform.position.x) < 0.2f)
                movX = 0;
            mov = new Vector2(
                  direccion * (movX + Input.GetAxisRaw("Horizontal")),
                  0
              );*/
            float movX = 0;
            if (Input.touchCount > 0)
            {
                Vector2 touch = Input.GetTouch(0).position;
                if (touch.x < Screen.width / 20)
                    movX = -1;
                else if (touch.x > 8 * Screen.width / 10)
                    movX = 1;
                else
                {
                    destino = Camera.main.ScreenToWorldPoint(touch);
                    movX = (destino.x - transform.position.x) > 0 ? 1 : -1;
                    if (Mathf.Abs(destino.x - transform.position.x) < 0.2f)
                        movX = 0;
                }
            }
            mov = new Vector2(
                  direccion * (movX + Input.GetAxisRaw("Horizontal")),
                  0
              );
        }
        else if(GameScore.iControles == (int)GameScore.CONTROLES.ARRASTRAR)
        {
            float movX = 0;
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                Vector2 deltaPos = Input.GetTouch(0).deltaPosition;
                if (Mathf.Abs(deltaPos.sqrMagnitude) > 1)
                    deltaPos.Normalize();
                if (Mathf.Abs(deltaPos.x) > 0.15)
                    movX = deltaPos.x;
            }
            mov = new Vector2(
                  direccion * (movX + Input.GetAxisRaw("Horizontal")),
                  0
              );
        }
    }

    void FixedUpdate()
    {
        rb2d.MovePosition(rb2d.position + mov * speed * Time.deltaTime); 
    }

    public void SetDamage(int damage)
    {
        if (bInmune && damage < 0)
            damage = 0;
        GameScore.DecVida(damage);
        if (damage < 0)
            TextoFlotante( damage+ "VIDAS", Color.red);
        else
            TextoFlotante("+"+ damage + "VIDAS", Color.green);
    }
    public void AddPoints(int points)
    {
        GameScore.AddScore(points);
        if (points >= 0)
            TextoFlotante("+"+points, Color.green);
        else
            TextoFlotante(points.ToString(), Color.red);

    }

    public void TextoFlotante(string sTexto, Color color)
    {
        if (sTexto != "")
        {
            var go = Instantiate(textoFlotante, GameObject.FindGameObjectWithTag("Player").transform.position, Quaternion.identity);
            go.transform.SetParent(GameObject.Find("UI").transform);
            go.transform.localScale = new Vector3(1f, 1f, 1f);
            go.GetComponent<Text>().color = color;
            go.GetComponent<Text>().text = sTexto;
        }
    }

    public void UpdateSpeed(float multiplier, float time)
    {
        speed = Mathf.Min( speed * multiplier, maxSpeed);
        flashEffect.SetActive(true);
        StopCoroutine("SetOriginalSpeed");
        StartCoroutine("SetOriginalSpeed", time);
        TextoFlotante("VELOCIDAD!", Color.cyan);
    }

    public IEnumerator SetOriginalSpeed(float time)
    {
        yield return new WaitForSeconds(time);
        speed = originalSpeed;
        flashEffect.SetActive(false);
    }

    public void SetBorracho(float time)
    {
        if (!bInmune) {
            direccion = -1;
            anim.SetBool("Borracho", true);
            StopCoroutine("SetBorrachoFalse");
            StartCoroutine("SetBorrachoFalse", time);
            TextoFlotante("¡BORRACHO!", Color.red);
        }
        else
            TextoFlotante("¡INMUNE!", Color.cyan);
    }

    public IEnumerator SetBorrachoFalse(float time)
    {
        yield return new WaitForSeconds(time);
        direccion = 1;
        anim.SetBool("Borracho", false);
        TextoFlotante("¡SOBRIO!", Color.cyan);
        //GameScore.PlaySound(borracho);
    }

    public void SetInmunity(float time)
    {
        bInmune = true;
        StopCoroutine("SetInmunityFalse");
        StartCoroutine("SetInmunityFalse", time);
        TextoFlotante("¡YEAAAAAAH!", Color.cyan);
        capsulaEffect.SetActive(true);
    }

    public IEnumerator SetInmunityFalse(float time)
    {
        yield return new WaitForSeconds(time);
        bInmune = false;
        capsulaEffect.SetActive(false);
    }
}
