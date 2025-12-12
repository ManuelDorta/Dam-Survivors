using UnityEngine;

// [CreateAssetMenu]: Esta etiqueta es fundamental.
// Le dice a Unity que añada una opción en el menú "Create" (clic derecho en Project).
// Así podemos crear archivos .asset físicos para configurar cada enemigo (Zángano, Tanque...)
// sin tener que tocar código ni prefabs constantemente.
[CreateAssetMenu(fileName = "EnemyStats", menuName = "Stats/EnemyStats", order = 0)]
public class EnemyStats : ScriptableObject // IMPORTANTE: Hereda de ScriptableObject, no de MonoBehaviour
{
    [Header("Configuración Base")]
    // Definimos las variables públicas para que salgan en el Inspector del archivo.
    // Este script actúa como una "Hoja de Personaje" de rol.
    
    public int MaxHP;    // Vida Máxima con la que nace el enemigo
    public int Damage;   // Daño que inflige al tocar al jugador
    public int Defense;  // Puntos de defensa (restan al daño recibido)
    public float Speed;  // Velocidad de persecución
}