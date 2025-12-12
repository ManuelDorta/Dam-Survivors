using System.Collections;
using UnityEngine;

public class LanzadorArma : MonoBehaviour
{
    [Header("Configuración del Arma")]
    public GameObject armaPrefab; // El proyectil que vamos a disparar (Hacha, Bumerán...)
    
    [Tooltip("Sistema de Progresión: Nivel actual que determina daño y cadencia.")]
    public int level = 1; 

    [Header("Estadísticas Base")]
    public float cooldownBase = 1f; // Cadencia de disparo inicial (segundos)

    void Start()
    {
        // Iniciamos el ciclo de disparo automático.
        // Usamos una Corrutina para manejar los tiempos de espera de forma eficiente
        // sin bloquear el hilo principal del juego.
        StartCoroutine(dispararArma());
    }

    public IEnumerator dispararArma()
    {
        // Bucle infinito controlado (While True): El jugador dispara siempre mientras viva.
        while (true)
        {
            // 1. CÁLCULO MATEMÁTICO DE CADENCIA (Escalado Exponencial)
            // Usamos una función de potencia: Cada nivel multiplica el tiempo por 0.9.
            // Esto reduce el tiempo de espera un 10% compuesto por cada nivel.
            // Nivel 1 = 1s. Nivel 2 = 0.9s. Nivel 10 = 0.38s.
            float cooldownActual = cooldownBase * Mathf.Pow(0.9f, level - 1);

            // 2. INSTANCIACIÓN (GENERACIÓN DE OBJETOS)
            if (armaPrefab != null) 
            {
                // Creamos el objeto físico en la escena copiando la posición y rotación del lanzador
                GameObject nuevaArma = Instantiate(armaPrefab, transform.position, transform.rotation);
                
                // 3. INYECCIÓN DE DATOS (Configuración dinámica)
                // Antes de que el proyectil se mueva, le pasamos sus estadísticas calculadas.
                AplicarMejoras(nuevaArma);
            }

            // 4. GESTIÓN DE TIEMPO
            // Pausamos la ejecución de esta función hasta el siguiente disparo.
            yield return new WaitForSeconds(cooldownActual);
        }
    }

    void AplicarMejoras(GameObject armaObj)
    {
        // Fórmula Lineal de Daño: Aumentamos un 20% el daño base por cada nivel extra.
        // Nivel 1 = 100%. Nivel 2 = 120%. Nivel 5 = 180%.
        float multiplicadorDano = 1f + ((level - 1) * 0.2f);

        // --- SISTEMA DE IDENTIFICACIÓN DE ARMAS (Polimorfismo Básico) ---
        // Identificamos qué tipo de arma hemos creado para configurar sus mecánicas únicas.
        
        // A) Caso Bumerán: Necesita saber el nivel para calcular sus REBOTES.
        Bumeran scriptBumeran = armaObj.GetComponent<Bumeran>();
        if (scriptBumeran != null)
        {
            // Comunicación entre scripts: Pasamos el dato del nivel al hijo.
            scriptBumeran.nivelArma = level; 
            
            // Aplicamos el multiplicador de daño redondeando a entero (Mathf.RoundToInt)
            scriptBumeran.damage = Mathf.RoundToInt(scriptBumeran.damage * multiplicadorDano);
        }

        // B) Caso Hacha: Solo necesita escalar su daño.
        Hacha scriptHacha = armaObj.GetComponent<Hacha>();
        if (scriptHacha != null)
        {
            scriptHacha.damage = Mathf.RoundToInt(scriptHacha.damage * multiplicadorDano);
        }
    }
}