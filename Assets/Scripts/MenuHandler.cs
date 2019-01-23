using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using GooglePlayGames;

public class MenuHandler : MonoBehaviour {

    public GameObject pnlCargando;
    private string leaderboard = GPGSIds.leaderboard_puntuacion;

    public void EmpezarJuego(int iEquipo)
    {
        pnlCargando.SetActive(true);
        GameScore.iControles = (int)GameScore.CONTROLES.TOUCH;
        GameScore.ResetGame(iEquipo);
        SceneManager.LoadScene("03_Game");
        /*if (GameObject.Find("Toggle").GetComponent<Toggle>().isOn)
            GameScore.iControles = (int)GameScore.CONTROLES.MOVIMIENTO;
        else if (GameObject.Find("cbArrastrando").GetComponent<Toggle>().isOn)
            GameScore.iControles = (int)GameScore.CONTROLES.ARRASTRAR;
        else if (GameObject.Find("cbToque").GetComponent<Toggle>().isOn)
            GameScore.iControles = (int)GameScore.CONTROLES.TOUCH;
        else
            GameScore.iControles = (int)GameScore.CONTROLES.ARRASTRAR;*/

    }

    public void Salir()
    {
        Application.Quit();
    }

    public void VolverMenu()
    {
        SceneManager.LoadScene("02_Menu");
    }

    public void VerRanking()
    {
        //SceneManager.LoadScene("04_Ranking");
        if (PlayGamesPlatform.Instance.localUser.authenticated)
        {
            PlayGamesPlatform.Instance.ShowLeaderboardUI(leaderboard);
        }
        else
        {
            Debug.Log("Cannot show leaderboard: not authenticated");
        }
    }

    public void SubirPuntuacion()
    {
        if (Social.localUser.authenticated)
        {
            Social.ReportScore(GameScore.GetScore(), leaderboard, (bool success) =>
            {
                if (success)
                {
                    Debug.Log("Update Score Success");
                    SceneManager.LoadScene(1);

                }
                else
                {
                    Debug.Log("Update Score Fail");
                }
            });
        }

        //GetComponent<GooglePlayHandler>().OnAddScoreToLeaderBorad(GameScore.GetScore());
        return;
        string nombre = GameObject.Find("inputNombre").GetComponent<InputField>().text;
        if (nombre != "")
        {
            GetComponent<GuardarJugador>().GuardarDatos(nombre, GameScore.GetScore(), GameScore.iEquipo);
            VerRanking();
        }
    }
}
