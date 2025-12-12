using UnityEngine;
using System.Collections.Generic; // NECESARIO: Nos permite usar List<> en lugar de arrays fijos

// [CreateAssetMenu] permite crear archivos de este script haciendo:
// Clic derecho en Project > Create > Survivor > Oleada.
// Esto convierte el script en un creador de archivos de configuración.
[CreateAssetMenu(fileName = "NuevaOleada", menuName = "Survivor/Oleada")]
public class DataOleada : ScriptableObject // IMPORTANTE: No hereda de MonoBehaviour
{
    // CLASE ANIDADA: Define qué es un "Grupo" de enemigos.
    // Usamos [System.Serializable] para que esta clase "rara" se pueda ver y editar
    // dentro del Inspector de Unity como si fuera una variable normal.
    [System.Serializable]
    public class GrupoEnemigos
    {
        public string nombre;            // Etiqueta organizativa (ej: "Grupo Zánganos")
        public GameObject prefabEnemigo; // El "molde" del enemigo que vamos a generar
        public int cantidadTotal;        // Cuántos enemigos de este tipo saldrán
        public float cadencia;           // Velocidad de aparición (Tiempo entre uno y otro)
    }

    [Header("Configuración de la Oleada")]
    // LISTA DE GRUPOS: La clave del sistema.
    // Al usar una Lista, podemos tener en una misma oleada:
    // - 1 Grupo de Zánganos
    // - Y ADEMÁS 1 Grupo de Tanques
    // Esto cumple el requisito de las "Oleadas Mixtas" (Ej: Oleada 4).
    public List<GrupoEnemigos> gruposDeEnemigos; 

    public float tiempoAntesDeSiguienteOleada = 5f; // Tiempo de descanso al acabar esta oleada
}