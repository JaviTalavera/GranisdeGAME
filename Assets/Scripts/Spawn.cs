using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour {

    public Item[] items;
    
    private int maxFrecuencia;

    public float frecuenciaEnemigos;            // Tiempo que tarda en aparecer un enemigo
    public float timeDec;                       // Tiempo que tarda en disminuir la frecuencia
    public float frecuenciaDecrementoTiempo;    // Tiempo que disminuye la frecuencia
    public float minFrecuenciaDivisor;          // Coeficiente entre el que se divide la frecuencia para calcular el mínimo
    private float minFrecuenciaEnemigos;
    private float originalFrecuenciaEnemigos;
    private int numEnemigosSimultaneos = 1;
    private int nItemsTotal = 0;

    private float timeToEnemy;
    
    public void RecalculFrecuencias ()
    {
        foreach (Item i in items)
            if (i.bFrecuencia)
                maxFrecuencia += i.frecuencia;
    }

    // Use this for initialization
    void Start ()
    {
        RecalculFrecuencias();
        timeToEnemy = frecuenciaEnemigos;
        originalFrecuenciaEnemigos = frecuenciaEnemigos;
        minFrecuenciaEnemigos = frecuenciaEnemigos / minFrecuenciaDivisor;
        StartCoroutine("TimeBetweenEnemiesDec");
        StartCoroutine("AumentaNumSimultaneos");
    }

    private void Update()
    {
        if (timeToEnemy <= 0)
        {
            SpawnEnemy();
            timeToEnemy = frecuenciaEnemigos;
        }
        timeToEnemy -= Time.deltaTime;

    }

    public void SpawnEnemy()
    {
        List<int> posiciones = new List<int>();
        for (int i = 0; i < numEnemigosSimultaneos; i++)
        {
            nItemsTotal++;
            // Elegimos por cual de los 5 fragmentos va a aparecer
            int r = 0;
            do
            {
                r = UnityEngine.Random.Range(-2, 3);
            } while (posiciones.Contains(r));
            posiciones.Add(r);
            // Generamos un número aleatorio entre 0 y la suma de las frecuencias
            int i_prefab = UnityEngine.Random.Range(0, maxFrecuencia);
            GameObject instance = null;

            int acumulado = 0;
            // Comprobamos qué enemigo aparece comprobando sus frecuencias. Como jugamos
            // con la variable acumulado nos da igual el orden del vector
            foreach (Item it in items)
            {
                if (!it.bFrecuencia && nItemsTotal%it.iFrecuenciaPropia == 0)
                {
                    instance = it.gameObject;
                    break;
                }
                acumulado += it.frecuencia;
                if (i_prefab < acumulado)
                {
                    instance = it.gameObject;
                    break;
                }
            }
            // Generamos la posición modo unity
            Vector3 pos = new Vector3(r, transform.position.y);
            // Instanciamos el objeto donde toca
            GameObject go = Instantiate(instance, pos, instance.transform.rotation);
            foreach (Item it in items)
            {
                if (!it.bFrecuencia && it.iFrecuenciaPropia > it.iMinFrecuenciaPropia && nItemsTotal % it.iItemsBetweenFrecuenciaDec == 0)
                {
                    it.iFrecuenciaPropia--;
                    break;
                }
            }
        }
    }

    public void UpdateFrecuencia(float multiplier, float time)
    {
        frecuenciaEnemigos = Mathf.Max(frecuenciaEnemigos / multiplier, minFrecuenciaEnemigos);
        StartCoroutine("SetOriginalFrecuency", time);
    }

    public IEnumerator SetOriginalFrecuency(float time)
    {
        yield return new WaitForSeconds(time);
        frecuenciaEnemigos = originalFrecuenciaEnemigos;
    }

    public IEnumerator AumentaNumSimultaneos()
    {
        int numMinutos = 5;
        while (numEnemigosSimultaneos < 3)
        {
            yield return new WaitForSeconds(numMinutos * 60);
            numEnemigosSimultaneos++;
            numMinutos += 3;
        }
    }

    public IEnumerator TimeBetweenEnemiesDec ()
    {
        while (frecuenciaEnemigos > minFrecuenciaEnemigos) {
            yield return new WaitForSeconds(frecuenciaDecrementoTiempo);
            frecuenciaEnemigos -= timeDec;
            originalFrecuenciaEnemigos = frecuenciaEnemigos;
        }
    }
}