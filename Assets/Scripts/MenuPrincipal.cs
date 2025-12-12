using UnityEngine;
using UnityEngine.SceneManagement; // IMPORTANTE: Sin esta librería, Unity no sabe cambiar de escena.

public class MenuPrincipal : MonoBehaviour
{
    // Esta función se activará al pulsar el botón "JUGAR"
    public void Jugar()
    {
        // SceneManager es la clase encargada de gestionar los niveles.
        // LoadScene carga la escena por su nombre.
        // Es vital que el nombre escrito aquí ("Game") coincida EXACTAMENTE con el archivo en la carpeta Scenes.
        // Si la escena se llamara "Nivel1", aquí tendríamos que poner "Nivel1".
        SceneManager.LoadScene("Game"); 
    }

    // Esta función se activará al pulsar el botón "SALIR"
    public void Salir()
    {
        // Debug.Log es útil porque Application.Quit() NO funciona dentro del editor de Unity.
        // Así sabemos que el botón funciona aunque la ventana no se cierre.
        Debug.Log("👋 Saliendo del juego...");
        
        // Cierra la aplicación por completo.
        // Esto solo se nota cuando generas el archivo ejecutable final (.exe / .apk).
        Application.Quit();
    }
}