using System.Collections;
using UnityEngine;

public class GuardarJugador : MonoBehaviour {

    private string NuevaPuntuacionURL = "granisde.square7.ch//RankingScripts//NuevaPuntuacion.php?";
    private string claveSecreta = "dcbkx12fjea59";

	void Start () {
        //StartCoroutine(EnviarJugadores("Cristian", 2, 0));
	}
	
    public void GuardarDatos(string nombreJugador, int puntuacionJugador, int penya)
    {
        StartCoroutine(EnviarJugadores(nombreJugador, puntuacionJugador, penya));
    }


    IEnumerator EnviarJugadores(string nombreJugador, int puntuacionJugador, int penya)
    {
    
        string hash = Md5Sum(nombreJugador + puntuacionJugador + penya + claveSecreta);
        string postURL = NuevaPuntuacionURL + "nombre=" + WWW.EscapeURL(nombreJugador) + "&puntuacion=" + puntuacionJugador + "&penya=" + penya + "&hash=" + hash;

        WWW DataPost = new WWW("http://" + postURL);
        yield return DataPost;

        if (DataPost.error != null)
        {
            print("Problema al insertar en la base de datos: " + DataPost.error);
        }
        else
            Debug.Log((System.Text.Encoding.UTF8.GetString(DataPost.bytes)));
    }

    public string Md5Sum(string strToEncrypt)
    {
        System.Text.UTF8Encoding ue = new System.Text.UTF8Encoding();
        byte[] bytes = ue.GetBytes(strToEncrypt);
        System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
        byte[] hashBytes = md5.ComputeHash(bytes);
        string hashString = "";
        for (int i = 0; i < hashBytes.Length; i++)
        {
            hashString += System.Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');
        }
        return hashString.PadLeft(32, '0');
    }
}
