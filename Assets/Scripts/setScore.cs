using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class setScore : MonoBehaviour {

    public GameObject Nombre;
    public GameObject Puntuacion;
    public Color[] colors;

    public void SetScore(string nombreJugador, string puntuacionJugador, int iEquipo)
    {
        Nombre.GetComponent<Text>().text = nombreJugador;
        Puntuacion.GetComponent<Text>().text = puntuacionJugador;
        GetComponent<Image>().color = colors[iEquipo];
    }
}
