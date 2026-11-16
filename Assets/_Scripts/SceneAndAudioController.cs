using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneAndAudioController : MonoBehaviour
{
    [Header("Nombre de la escena del Menú")]
    public string nombreEscenaMenu = "MenuPrincipal";

    public void IniciarJuego(string nombreEscena)
    {
        if (AudioManager.Instance != null) AudioManager.Instance.SonarBoton();
        
        // Al darle a Jugar desde el menú, siempre empieza desde el Nivel 1
        LevelProgressionManager.NivelActual = 1;
        Time.timeScale = 1f;
        SceneManager.LoadScene(nombreEscena);
    }

    public void ReiniciarNivelActual()
    {
        if (AudioManager.Instance != null) AudioManager.Instance.SonarBoton();
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // ¡NUEVA FUNCIÓN PARA EL BOTÓN DE IR AL MENÚ PRINCIPAL!
    public void VolverAlMenuPrincipal()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SonarBoton();
            AudioManager.Instance.DetenerAmarillo();
        }

        // Reiniciamos el contador al Nivel 1 y quitamos la pausa
        LevelProgressionManager.NivelActual = 1;
        Time.timeScale = 1f;
        SceneManager.LoadScene(nombreEscenaMenu);
    }

    public void AlternarAudio()
    {
        AudioListener.pause = !AudioListener.pause;
        if (!AudioListener.pause && AudioManager.Instance != null)
        {
            AudioManager.Instance.SonarBoton();
        }
    }

    public void SalirDelJuego()
    {
        if (AudioManager.Instance != null) AudioManager.Instance.SonarBoton();
        Time.timeScale = 1f;
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }
}