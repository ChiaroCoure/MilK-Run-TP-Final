using System.Collections;
using UnityEngine;

public class GameUIManager : MonoBehaviour
{
    [Header("Pantallas Finales y Menús")]
    public GameObject pantallaGameOver;
    public GameObject pantallaVictoria;
    public GameObject pantallaOpciones;
    public GameObject botonEngranaje;

    [Header("Carteles de Aviso Previo")]
    public GameObject cartelPolicia;
    public GameObject cartelLlegaste;

    [Header("Configuración de Tiempos")]
    public int cantidadParpadeosPolicia = 3;
    public float velocidadParpadeo = 0.2f;
    public float duracionCartelLlegaste = 1.5f;

    public void MostrarPantallaDerrota()
    {
        if (botonEngranaje != null) botonEngranaje.SetActive(false);
        if (AudioManager.Instance != null) AudioManager.Instance.DetenerMusica();
        StartCoroutine(SecuenciaDerrota());
    }

    public void MostrarPantallaVictoria()
    {
        if (botonEngranaje != null) botonEngranaje.SetActive(false);
        if (AudioManager.Instance != null) AudioManager.Instance.DetenerMusica();
        StartCoroutine(SecuenciaVictoria());
    }

    private IEnumerator SecuenciaDerrota()
    {
        // ¡SONIDO DE CARTEL DE POLICÍA!
        if (AudioManager.Instance != null) AudioManager.Instance.SonarPolicia();

        if (cartelPolicia != null)
        {
            for (int i = 0; i < cantidadParpadeosPolicia; i++)
            {
                cartelPolicia.SetActive(true);
                yield return new WaitForSecondsRealtime(velocidadParpadeo);
                cartelPolicia.SetActive(false);
                yield return new WaitForSecondsRealtime(velocidadParpadeo);
            }
        }

        // ¡SONIDO DE PANTALLA DE DERROTA!
        if (AudioManager.Instance != null) AudioManager.Instance.SonarDerrota();
        if (pantallaGameOver != null) pantallaGameOver.SetActive(true);
    }

    private IEnumerator SecuenciaVictoria()
    {
        // ¡SONIDO DE CARTEL LLEGASTE!
        if (AudioManager.Instance != null) AudioManager.Instance.SonarLlegaste();

        if (cartelLlegaste != null)
        {
            cartelLlegaste.SetActive(true);
            yield return new WaitForSecondsRealtime(duracionCartelLlegaste);
            cartelLlegaste.SetActive(false);
        }

        // ¡SONIDO DE PANTALLA DE VICTORIA!
        if (AudioManager.Instance != null) AudioManager.Instance.SonarVictoria();
        if (pantallaVictoria != null) pantallaVictoria.SetActive(true);
    }

    public void AbrirOpcionesYPausar()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SonarBoton();
            AudioManager.Instance.PausarEventosLargos(true); // Pausa el sonido amarillo si estaba sonando
        }
        Time.timeScale = 0f;
        if (botonEngranaje != null) botonEngranaje.SetActive(false);
        if (pantallaOpciones != null) pantallaOpciones.SetActive(true);
    }

    public void CerrarOpcionesYReanudar()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SonarBoton();
            AudioManager.Instance.PausarEventosLargos(false); // Reanuda el sonido amarillo
        }
        Time.timeScale = 1f;
        if (botonEngranaje != null) botonEngranaje.SetActive(true);
        if (pantallaOpciones != null) pantallaOpciones.SetActive(false);
    }
}