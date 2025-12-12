using UnityEngine;

public class EnemigoEnjambre : MonoBehaviour
{
    [Header("Configuración del Enjambre")]
    // Referencia al prefab del "hijo" que vamos a invocar (Enemigo 1 / Zángano).
    // Es vital que sea público para asignarlo desde el Inspector.
    public GameObject prefabZangano; 
    
    // Número de enemigos que soltará de golpe.
    public int cantidadZanganos = 10;

    // Start se ejecuta UNA sola vez, justo en el primer frame en que este enemigo nace.
    // Es el momento perfecto para realizar la invocación inicial.
    void Start()
    {
        // BUCLE FOR: Repetimos la acción de crear un hijo tantas veces como diga la variable.
        // Es más limpio que escribir 10 veces la misma línea.
        for (int i = 0; i < cantidadZanganos; i++)
        {
            GenerarHijo();
        }
    }

    void GenerarHijo()
    {
        // Null Check: Seguridad por si se nos olvidó arrastrar el prefab en el Inspector.
        if (prefabZangano != null)
        {
            // CÁLCULO DE POSICIÓN ALEATORIA (Offset):
            // Si todos nacieran en (0,0,0) relativo al padre, estarían superpuestos y 
            // las físicas de Unity explotarían (saldrían disparados).
            // Añadimos un pequeño rango aleatorio entre -2 y 2 metros en X y Z.
            Vector3 offset = new Vector3(Random.Range(-2f, 2f), 0, Random.Range(-2f, 2f));
            
            // INSTANTIATE: El comando que crea la copia real en el juego.
            // 1. Qué creamos: prefabZangano.
            // 2. Dónde: Posición del padre + el desplazamiento aleatorio.
            // 3. Rotación: Identity (rotación por defecto, sin girar).
            Instantiate(prefabZangano, transform.position + offset, Quaternion.identity);
        }
    }
}