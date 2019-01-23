using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class VerJugadores : MonoBehaviour {


    private string URLVerPuntuacion = "granisde.square7.ch//RankingScripts//VerPuntuacion.php";
    private List<Jugador> rankingJugadores = new List<Jugador>();
    private string[] CurrentArray = null;
    public Transform tfPanelCargarDatos;
    public GameObject panelPrefab;

	// Use this for initialization
	void Start () {
        StartCoroutine(ObtenerJugadores());
	}
	
    IEnumerator ObtenerJugadores()
    {
        WWW DataServer = new WWW("http://" + URLVerPuntuacion);
        yield return DataServer;

        if (DataServer.error != null)
        {
            Debug.Log("Problema al intentar obtener los jugadores " + DataServer.error);
        }
        else
        {
            ObtenerRegistros(DataServer);
            VerRegistros();
        }
    }

    void ObtenerRegistros(WWW DataServer)
    {
        CurrentArray = System.Text.Encoding.UTF8.GetString(DataServer.bytes).Split(";"[0]);
        for (int i = 0; i <= CurrentArray.Length - 4; i = i + 3)
            rankingJugadores.Add(new Jugador(CurrentArray[i], CurrentArray[i + 1], Convert.ToInt32(CurrentArray[i + 2])));
    }

	// Update is called once per frame
	void VerRegistros () {
		foreach (Jugador j in rankingJugadores)
        {
            GameObject obj = Instantiate(panelPrefab);
            obj.GetComponent<setScore>().SetScore(j.nombre, j.puntuacion, j.penya);
            obj.transform.SetParent(tfPanelCargarDatos);
            obj.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        }
	}
}

public class Jugador
{
    public string puntuacion;
    public string nombre;
    public int penya;

    public Jugador(string nombre, string puntuacion, int penya)
    {
        this.nombre = nombre;
        this.puntuacion = puntuacion;
        this.penya = penya;
    }
}
