using UnityEngine;
using System.Collections.Generic;

public class FrostZone : MonoBehaviour
{
    [Header("Configuración")]
    public float damagePorSegundo = 5f;
    public float porcentajeRalentizacion = 0.3f; // 30% más lento

    // Diccionario para recordar qué enemigo entró y qué velocidad tenía antes
    private Dictionary<EnemyController, float> enemigosEnZona = new Dictionary<EnemyController, float>();
    private float timerDanio = 0f;

    void Update()
    {
        // Forzamos que la zona siga al jugador (por si acaso)
        if (transform.parent != null)
        {
            transform.position = transform.parent.position;
        }

        // Causar daño cada 1 segundo
        timerDanio += Time.deltaTime;
        if (timerDanio >= 1f)
        {
            AplicarDanioArea();
            timerDanio = 0f;
        }
    }

    void AplicarDanioArea()
    {
        // Lista auxiliar para limpiar enemigos que hayan muerto
        List<EnemyController> enemigosMuertos = new List<EnemyController>();

        foreach (var par in enemigosEnZona)
        {
            EnemyController enemigo = par.Key;
            
            if (enemigo != null)
            {
                enemigo.Recibirdano((int)damagePorSegundo);
            }
            else
            {
                enemigosMuertos.Add(enemigo);
            }
        }

        // Limpiamos la lista
        foreach (var muerto in enemigosMuertos)
        {
            enemigosEnZona.Remove(muerto);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyController enemy = other.GetComponent<EnemyController>();
            
            // Si es un enemigo nuevo, guardamos su velocidad y lo frenamos
            if (enemy != null && !enemigosEnZona.ContainsKey(enemy))
            {
                float velocidadOriginal = enemy.speed;
                enemigosEnZona.Add(enemy, velocidadOriginal);

                // Aplicamos la ralentización
                enemy.speed = velocidadOriginal * (1f - porcentajeRalentizacion);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyController enemy = other.GetComponent<EnemyController>();

            // Si sale de la zona, le devolvemos su velocidad normal
            if (enemy != null && enemigosEnZona.ContainsKey(enemy))
            {
                enemy.speed = enemigosEnZona[enemy]; // Restaurar velocidad
                enemigosEnZona.Remove(enemy);        // Olvidarlo
            }
        }
    }
}