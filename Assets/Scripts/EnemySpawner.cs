using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement; // VITAL: Sin esto no podemos cambiar de nivel

public class EnemySpawner : MonoBehaviour
{
    [Header("Configuración")]
    // Lista de archivos ScriptableObject. Cada uno contiene los datos de una oleada.
    // Al ser una Lista, podemos hacer niveles infinitos simplemente arrastrando archivos.
    public List<DataOleada> oleadas;

    [Header("Referencias")]
    public Transform jugador;      // Referencia para saber dónde nacen los enemigos (a su alrededor)
    public float radioSpawn = 10f; // Distancia (radio) del círculo de aparición

    // Variables de Control
    private int indiceOleadaActual = 0; // ¿Por cuál vamos? (0, 1, 2...)
    private bool oleadaEnCurso = false; // Semáforo para no lanzar dos oleadas a la vez

    void Start()
    {
        // Validación de seguridad para evitar errores si se nos olvida configurar el Inspector
        if (oleadas.Count > 0 && jugador != null)
        {
            // Iniciamos la rutina de tiempo. No usamos Update() porque necesitamos
            // esperar tiempos concretos (segundos) entre enemigos.
            StartCoroutine(IniciarOleada(indiceOleadaActual));
        }
        else
        {
            Debug.LogWarning("⚠️ Faltan referencias en el Spawner.");
        }
    }

    // --- LÓGICA DE TIEMPO (CORRUTINAS) ---
    // Usamos IEnumerator para poder usar 'yield return new WaitForSeconds'
    IEnumerator IniciarOleada(int indice)
    {
        oleadaEnCurso = true;
        DataOleada datos = oleadas[indice]; // Cargamos los datos del archivo correspondiente
        Debug.Log($"🌊 INICIANDO OLEADA {indice + 1}");

        // 1. LANZAMIENTO SIMULTÁNEO
        // Recorremos la lista de grupos (ej: Zánganos y Corredores)
        // y lanzamos una sub-rutina independiente para cada uno.
        // Así pueden salir tipos diferentes a ritmos diferentes A LA VEZ.
        foreach (var grupo in datos.gruposDeEnemigos)
        {
            StartCoroutine(SpawnGrupo(grupo));
        }

        // 2. CÁLCULO DE DURACIÓN
        // Necesitamos saber cuánto esperar antes de la siguiente oleada.
        // Calculamos cuál es el grupo que más tarda en terminar de salir.
        float tiempoMaximo = 0f;
        foreach (var grupo in datos.gruposDeEnemigos)
        {
            float tiempoEsteGrupo = grupo.cantidadTotal * grupo.cadencia;
            if (tiempoEsteGrupo > tiempoMaximo) tiempoMaximo = tiempoEsteGrupo;
        }

        // 3. ESPERA INTELIGENTE
        // Esperamos el tiempo que tardan en salir + el tiempo de descanso configurado en el archivo
        yield return new WaitForSeconds(tiempoMaximo + datos.tiempoAntesDeSiguienteOleada);

        // Cuando termina la espera, pasamos a la siguiente
        SiguienteOleada();
    }

    // Sub-rutina que genera los enemigos uno a uno
    IEnumerator SpawnGrupo(DataOleada.GrupoEnemigos grupo)
    {
        for (int i = 0; i < grupo.cantidadTotal; i++)
        {
            if (jugador != null) // Check por si el jugador muere mientras salen
            {
                SpawnEnemigo(grupo.prefabEnemigo);
            }
            // Esperamos X segundos antes de sacar al siguiente del mismo grupo
            yield return new WaitForSeconds(grupo.cadencia);
        }
    }

    // --- MATEMÁTICAS DE POSICIONAMIENTO ---
    void SpawnEnemigo(GameObject prefab)
    {
        // 1. Generamos un punto aleatorio dentro de un círculo 2D (X, Y)
        // .normalized hace que el punto esté siempre en el borde del círculo (perímetro)
        Vector2 puntoAleatorio = Random.insideUnitCircle.normalized * radioSpawn;
        
        // 2. Convertimos ese punto 2D al mundo 3D
        // La Y del círculo (altura 2D) pasa a ser la Z del mundo (profundidad 3D).
        // La altura del mundo (Y) la dejamos en 0 (o a la altura del jugador).
        Vector3 posicionSpawn = new Vector3(puntoAleatorio.x, 0, puntoAleatorio.y) + jugador.position;

        // 3. Creamos el enemigo
        Instantiate(prefab, posicionSpawn, Quaternion.identity);
    }

    // --- GESTIÓN DE NIVELES ---
    void SiguienteOleada()
    {
        oleadaEnCurso = false;
        indiceOleadaActual++;

        // Si quedan oleadas en la lista, seguimos jugando
        if (indiceOleadaActual < oleadas.Count)
        {
            StartCoroutine(IniciarOleada(indiceOleadaActual));
        }
        else
        {
            // SI NO QUEDAN OLEADAS -> HEMOS GANADO EL NIVEL
            Debug.Log("🏆 NIVEL COMPLETADO. Verificando siguiente escena...");
            
            Scene sceneActual = SceneManager.GetActiveScene();
            
            // Comprobamos el nombre de la escena actual para saber a dónde ir
            if (sceneActual.name == "Game" || sceneActual.name == "0 (1)") 
            {
                // Del Nivel 1 pasamos al Nivel 2
                SceneManager.LoadScene("Nivel2");
            }
            else if (sceneActual.name == "Nivel2")
            {
                // Del Nivel 2 volvemos al Menú (Victoria total)
                SceneManager.LoadScene("MainMenu");
            }
        }
    }
}