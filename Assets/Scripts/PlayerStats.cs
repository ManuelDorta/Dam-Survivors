using UnityEngine;
using UnityEngine.SceneManagement; // ⚠️ VITAL: Sin esto no podemos cargar el Menú al morir

public class PlayerStats : MonoBehaviour
{
    [Header("Configuración de Vida")]
    // 'currentHealth' es privada porque solo este script debe modificarla para evitar trampas/bugs.
    private int currentHealth;
    
    // 'maxHealth' es pública para poder equilibrar la dificultad desde el Inspector de Unity.
    public int maxHealth = 100; 
    
    // Estadísticas defensivas (se podrían ampliar con ScriptableObjects en el futuro)
    private int defensa = 0;
    
    // Variable de ESTADO (Semáforo):
    // Nos sirve para saber si el jugador sigue jugando o si ya ha perdido.
    // Evita que recibamos daño o nos movamos cuando ya estamos muertos.
    private bool estaVivo;

    // Usamos Awake en vez de Start para inicializar variables críticas.
    // Awake se ejecuta antes que cualquier Start, asegurando que la vida esté lista
    // antes de que ningún enemigo intente atacarnos.
    private void Awake() 
    {
        currentHealth = maxHealth;
        estaVivo = true; // ¡Importante! Siempre nacemos vivos.
    }

    //////////////////////////////// LÓGICA DE DAÑO /////////////////////////
    
    public void RecibirDmg(int dmg)
    {
        // 1. CLÁUSULA DE GUARDA (Guard Clause)
        // Si ya estamos muertos, salimos de la función inmediatamente.
        // Esto evita bugs raros, como morir dos veces seguidas o recibir daño en la pantalla de Game Over.
        if (!estaVivo) return;

        // 2. MATEMÁTICAS SEGURAS (Mathf.Max) 
        // Calculamos el daño real restando la defensa.
        // Usamos Mathf.Max(0, ...) para asegurar que el resultado NUNCA sea negativo.
        // Si el daño fuera negativo (ej: Ataque 5 - Defensa 10 = -5), ¡el golpe nos curaría!
        // Con esto evitamos ese error lógico.
        int danioFinal = Mathf.Max(0, dmg - defensa);

        currentHealth -= danioFinal;
        
        // Feedback para depuración (console logging)
        Debug.Log($"💔 Jugador recibe daño. Vida restante: {currentHealth}");

        // 3. CONDICIÓN DE DERROTA
        if (currentHealth <= 0)
        {
            estaVivo = false; // Cambiamos el estado a "Muerto"
            Morir();
        }
    }

    private void Morir()
    {
        Debug.Log("💀 HAS MUERTO. Volviendo al Menú Principal...");
        
        // Carga de Escena:
        // Usamos el SceneManager para reiniciar el ciclo de juego enviando al usuario al menú.
        SceneManager.LoadScene("MainMenu");
    }
}