using System.Collections;
using UnityEngine;

public class LanzadorArma : MonoBehaviour
{
    [Header("Configuración del Arma")]
    public GameObject armaPrefab;
    
    [Tooltip("Nivel actual del arma. Súbelo para mejorar stats.")]
    public int level = 1; 

    [Header("Estadísticas Base (Nivel 1)")]
    public float cooldownBase = 1f; // Tiempo entre disparos original

    void Start()
    {
        StartCoroutine(dispararArma());
    }

    public IEnumerator dispararArma()
    {
        while (true)
        {
            // 1. CALCULAR VELOCIDAD DE DISPARO SEGÚN NIVEL
            // Fórmula: Cada nivel reduce el tiempo de espera un 10% (0.9)
            // Nivel 1 = 100% tiempo, Nivel 2 = 90%, Nivel 3 = 81%...
            float cooldownActual = cooldownBase * Mathf.Pow(0.9f, level - 1);

            // 2. CREAR EL PROYECTIL
            GameObject nuevaArma = Instantiate(armaPrefab, transform.position, transform.rotation);

            // 3. APLICAR DAÑO SEGÚN NIVEL
            AplicarMejoras(nuevaArma);

            // 4. ESPERAR EL TIEMPO REDUCIDO
            yield return new WaitForSeconds(cooldownActual);
        }
    }

    void AplicarMejoras(GameObject armaObj)
    {
        // Calculamos el multiplicador de daño: +20% de daño por nivel extra
        float multiplicadorDano = 1f + ((level - 1) * 0.2f);

        // BUSCAMOS QUÉ TIPO DE ARMA ES PARA SUBIRLE EL DAÑO
        
        // A) Caso Hacha
        Hacha scriptHacha = armaObj.GetComponent<Hacha>();
        if (scriptHacha != null)
        {
            scriptHacha.damage = Mathf.RoundToInt(scriptHacha.damage * multiplicadorDano);
        }

        // B) Caso Bumeran
        Bumeran scriptBumeran = armaObj.GetComponent<Bumeran>();
        if (scriptBumeran != null)
        {
            scriptBumeran.damage = Mathf.RoundToInt(scriptBumeran.damage * multiplicadorDano);
        }
    }
}