using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameScore : MonoBehaviour {
    public enum EQUIPOS { GRANOTA = 0, EQUISDE = 1 };
    public enum CONTROLES { MOVIMIENTO = 1, TOUCH = 0, ARRASTRAR = 2 };

    public Sprite[] imgLogo;
    public GameObject[] pfEnemigos;
    public GameObject[] pfPlayers;
    public Color[] colors;

    public static int iEquipo = (int)EQUIPOS.EQUISDE;
    public static int iControles = (int)CONTROLES.MOVIMIENTO;
    private bool bPuntuaciones = false;
    private bool bEmpezado = false;
    private static int iScore = 0;
    public static int iVidas = 100;

    public Text lblContador;
    public Text lblScore;
    public Text lblVida;
    public Image barraVidaMovil;
    public Image barraVidaFija;

    public RectTransform pnlLeyenda;
    public RectTransform pnlGameOver;
    public RectTransform pnlGame;
    public RectTransform pnlPause;

    public AudioClip[] audioContador;

    public static void AddScore(int iPuntos)
    {
        iScore += iPuntos;
    }

    public static int GetScore()
    {
        return iScore;
    }


    public static int DivScore(float iDiv)
    {
        int resta = (int)(iScore / iDiv);
        iScore = iScore - resta;
        return resta;
    }

    public static void DecVida(int idamage = 1)
    {
        iVidas += idamage;
        if (iVidas > 100) iVidas = 100;
    }

    private void Start()
    {
        pnlLeyenda.Find("imgBando").GetComponent<Image>().sprite = imgLogo[iEquipo];
        pnlLeyenda.GetComponent<Image>().color = colors[iEquipo];
        GetComponent<Spawn>().items[4] = pfEnemigos[iEquipo].GetComponent<Item>();
        GameObject.Instantiate(pfPlayers[iEquipo], pfPlayers[iEquipo].transform.position, Quaternion.identity);
        StartCoroutine(OcultarNormas());
    }

    private void Update()
    {
        if (bPuntuaciones && !bEmpezado && (Input.GetKeyDown(KeyCode.Return) || Input.touchCount > 0))
        {
            bEmpezado = true;
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().enabled = true;
            pnlLeyenda.gameObject.SetActive(false);
            pnlGame.gameObject.SetActive(true);
            StartCoroutine("SetContador");
        }
    }

    static public void PlaySound(AudioClip clip)
    {
        var obj = Resources.Load("Prefabs/AudioSource") as GameObject;
        var go = Instantiate(obj);
        var audio = go.GetComponent<AudioSource>();
        audio.clip = clip;
        audio.Play();
    }

    public IEnumerator OcultarNormas()
    {
        yield return new WaitForSeconds(1f);
        bPuntuaciones = true;
    }

    public IEnumerator SetContador()
    {
        int i = 3;
        lblContador.GetComponent<Animator>().Play("TextSplash");
        while (i > 0)
        {
            GetComponent<AudioSource>().clip = audioContador[i-1];
            GetComponent<AudioSource>().Play();
            yield return new WaitForSeconds(1);
            i--;
            if (i == 0)
            {
                lblContador.text = "GO!";
                GetComponent<AudioSource>().clip = audioContador[3];
                GetComponent<AudioSource>().Play();
            }
            else
            {
                lblContador.text = i + "";
            }

            lblContador.GetComponent<Animator>().Play("TextSplash");
        }
        GetComponent<Spawn>().enabled = true;
        Camera.main.GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(1);
        lblContador.gameObject.SetActive(false);
    }
    private void FixedUpdate()
    {
        lblScore.text = iScore + "";
        if (iVidas < 0)
            GameOver();
        barraVidaMovil.fillAmount = iVidas / 100f;
        lblVida.text = iVidas + "%";
    }

    public void Pause(bool bPause)
    {
        if (bPause)
        {
            Time.timeScale = 0;
            pnlPause.gameObject.SetActive(true);
        }
        else
        {
            pnlPause.gameObject.SetActive(false);
            Time.timeScale = 1;
        }
    }

    public static void ResetGame (int iEquipo)
    {
        Time.timeScale = 1;
        GameScore.iVidas = 100;
        GameScore.iScore = 0;
        GameScore.iEquipo = iEquipo;
    }

    public static void ResetGameHardMode(int iEquipo)
    {
        Time.timeScale = 2;
        GameScore.iVidas = 1;
        GameScore.iScore = 0;
        GameScore.iEquipo = iEquipo;
    }

    public void GameOver()
    {
        //pnlGame.gameObject.SetActive(false);
        pnlGameOver.gameObject.SetActive(true);
        pnlGameOver.GetChild(0).GetComponent<Text>().text = iScore + "";
        Camera.main.GetComponent<AudioSource>().Stop();
        Time.timeScale = 0;
    }
}
