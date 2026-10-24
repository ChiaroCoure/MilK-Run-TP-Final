using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneAndAudioController : MonoBehaviour
{
    public void IniciarJuego(string nombreEscena)
    {
        if (AudioManager.Instance != null) AudioManager.Instance.SonarBoton();
        Time.timeScale = 1f;
        SceneManager.LoadScene(nombreEscena);
    }

    public void ReiniciarNivelActual()
    {
        if (AudioManager.Instance != null) AudioManager.Instance.SonarBoton();
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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