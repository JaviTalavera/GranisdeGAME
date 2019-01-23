using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour {

    public Text txtComenzar;
    private bool bTocar = false;
	// Use this for initialization
	void Start () {
        StartCoroutine("CanTouch");
	}
	
    IEnumerator CanTouch ()
    {
        yield return new WaitForSeconds(1);
        bTocar = true;
        Camera.main.GetComponent<AudioSource>().Play();
        txtComenzar.enabled = true;
    }
	// Update is called once per frame
	void Update () {
        if (bTocar && Input.touchCount > 0 || Input.GetKeyDown(KeyCode.Return))
            SceneManager.LoadScene("02_Menu");
	}
}
