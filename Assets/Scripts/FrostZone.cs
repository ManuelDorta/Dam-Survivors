using UnityEngine;
using System.Collections.Generic;

public class FrostZone : MonoBehaviour
{
    [Header("Configuración Base")]
    public float damageBase = 5f;
    public float ralentizacionBase = 0.2f; // 20% de freno inicial

    // ESTRUCTURA DE DATOS AVANZADA: Diccionario
    // Usamos un Dictionary en vez de una List porque necesitamos guardar PAREJAS de datos:
    // Clave (Key): El enemigo que ha entrado.
    // Valor (Value): La velocidad original que tenía ANTES de entrar (para devolvérsela luego).
    private Dictionary<EnemyController, float> enemigosEnZona = new Dictionary<EnemyController, float>();
    
    private float timerDanio = 0f;        // Cronómetro para el daño por segundo (DPS)
    private LanzadorArma lanzadorJugador; // Referencia para leer el nivel

    void Start()
    {
        // Conexión con el padre (Player) para leer el nivel del arma
        lanzadorJugador = GetComponentInParent<LanzadorArma>();
    }

    void Update()
    {
        // Seguridad: Mantenemos la zona pegada al jugador manualmente
        // (Aunque al ser hijo debería seguirlo, esto evita lags visuales en algunos casos)
        if (transform.parent != null)
        {
            transform.position = transform.parent.position;
        }

        // --- CÁLCULO DE DAÑO DINÁMICO (Nivel) ---
        int nivelActual = 1;
        if (lanzadorJugador != null) nivelActual = lanzadorJugador.level;

        // Fórmula: Daño base + 1 punto extra por cada nivel.
        float danioActual = damageBase + (nivelActual * 1f);
        
        // --- SISTEMA DE DAÑO PERIÓDICO (Tick de Daño) ---
        // No hacemos daño en cada frame (sería demasiado).
        // Usamos un temporizador para aplicar el daño solo 1 vez por segundo.
        timerDanio += Time.deltaTime;
        if (timerDanio >= 1f)
        {
            AplicarDanioArea((int)danioActual);
            timerDanio = 0f; // Reiniciamos el reloj
        }
    }

    void AplicarDanioArea(int danio)
    {
        // Lista auxiliar para apuntar a los que mueran durante el proceso
        List<EnemyController> enemigosMuertos = new List<EnemyController>();

        // Recorremos el diccionario (todos los que están dentro del hielo)
        foreach (var par in enemigosEnZona)
        {
            EnemyController enemigo = par.Key; // La "Key" es el script del enemigo
            
            if (enemigo != null)
            {
                enemigo.Recibirdano(danio);
            }
            else
            {
                // Si es null (ha muerto o se ha destruido), lo apuntamos para borrarlo
                enemigosMuertos.Add(enemigo);
            }
        }

        // Limpieza: Quitamos del diccionario a los que ya no existen para evitar errores
        foreach (var muerto in enemigosMuertos) enemigosEnZona.Remove(muerto);
    }

    // --- ENTRADA EN LA ZONA (Aplicar Freno) ---
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyController enemy = other.GetComponent<EnemyController>();
            
            // Solo lo procesamos si NO está ya en la lista (evitar duplicados)
            if (enemy != null && !enemigosEnZona.ContainsKey(enemy))
            {
                // 1. CALCULAR POTENCIA DEL FRENO (Según Nivel)
                int nivel = (lanzadorJugador != null) ? lanzadorJugador.level : 1;
                
                // Fórmula: 20% base + 5% extra por nivel.
                // Ej: Nivel 1 = 0.25 (25%). Nivel 10 = 0.70 (70%).
                float ralentizacionTotal = ralentizacionBase + (nivel * 0.05f);
                
                // Tope de seguridad (90%) para que no se queden congelados del todo (bug visual)
                if (ralentizacionTotal > 0.9f) ralentizacionTotal = 0.9f;

                // 2. GUARDAR ESTADO ORIGINAL (Vital)
                // Guardamos su velocidad actual en el diccionario antes de modificarla
                float velocidadOriginal = enemy.speed;
                enemigosEnZona.Add(enemy, velocidadOriginal);

                // 3. APLICAR EL FRENO
                // Velocidad = Velocidad * (1 - 0.25) -> Se queda al 75%
                enemy.speed = velocidadOriginal * (1f - ralentizacionTotal);
            }
        }
    }

    // --- SALIDA DE LA ZONA (Restaurar Velocidad) ---
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyController enemy = other.GetComponent<EnemyController>();
            
            // Si el enemigo sale y lo tenemos registrado...
            if (enemy != null && enemigosEnZona.ContainsKey(enemy))
            {
                // 1. LE DEVOLVEMOS SU VELOCIDAD
                // Leemos del diccionario la velocidad que guardamos al entrar
                enemy.speed = enemigosEnZona[enemy]; 
                
                // 2. LO BORRAMOS DE LA LISTA
                enemigosEnZona.Remove(enemy);
            }
        }
    }
}