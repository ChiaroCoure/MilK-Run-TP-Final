using System.Collections;
using UnityEngine;

public class GameUIManager : MonoBehaviour
{
    [Header("Pantallas Finales y Menús")]
    public GameObject pantallaGameOver;
    public GameObject pantallaVictoria;
    public GameObject pantallaOpciones;
    public GameObject botonEngranaje;

    [Header("Carteles de Texto (TMP)")]
    public GameObject cartelPolicia;
    public GameObject cartelLlegaste;
    public GameObject cartelRegresoCasa;

    [Header("Configuración de Parpadeo")]
    public int cantidadParpadeos = 3;
    public float velocidadParpadeo = 0.22f;

    public void MostrarCartelRegresoCasa()
    {
        if (cartelRegresoCasa != null)
        {
            StopAllCoroutines();
            StartCoroutine(ParpadearCartel(cartelRegresoCasa));
        }
    }

    public IEnumerator ParpadearCartelLlegaste()
    {
        yield return StartCoroutine(ParpadearCartel(cartelLlegaste));
    }

    public void MostrarPantallaDerrota()
    {
        if (botonEngranaje != null) botonEngranaje.SetActive(false);
        if (cartelRegresoCasa != null) cartelRegresoCasa.SetActive(false);
        if (AudioManager.Instance != null) AudioManager.Instance.DetenerMusica();
        StopAllCoroutines();
        StartCoroutine(SecuenciaDerrota());
    }

    public void MostrarPantallaVictoria()
    {
        if (botonEngranaje != null) botonEngranaje.SetActive(false);
        if (cartelRegresoCasa != null) cartelRegresoCasa.SetActive(false);
        if (AudioManager.Instance != null) AudioManager.Instance.DetenerMusica();
        StopAllCoroutines();
        StartCoroutine(SecuenciaVictoria());
    }

    private IEnumerator SecuenciaDerrota()
    {
        if (AudioManager.Instance != null) AudioManager.Instance.SonarPolicia();

        // Parpadea el cartel de "¡OH NO! ¡VIENE LA POLICÍA!"
        yield return StartCoroutine(ParpadearCartel(cartelPolicia));

        if (AudioManager.Instance != null) AudioManager.Instance.SonarDerrota();
        if (pantallaGameOver != null) pantallaGameOver.SetActive(true);
    }

    private IEnumerator SecuenciaVictoria()
    {
        if (AudioManager.Instance != null) AudioManager.Instance.SonarLlegaste();

        // Parpadea el cartel de "¡LLEGASTE!" antes de abrir la pantalla final
        yield return StartCoroutine(ParpadearCartel(cartelLlegaste));

        if (AudioManager.Instance != null) AudioManager.Instance.SonarVictoria();
        if (pantallaVictoria != null) pantallaVictoria.SetActive(true);
    }

    private IEnumerator ParpadearCartel(GameObject cartel)
    {
        if (cartel == null) yield break;

        for (int i = 0; i < cantidadParpadeos; i++)
        {
            cartel.SetActive(true);
            yield return new WaitForSecondsRealtime(velocidadParpadeo);
            cartel.SetActive(false);
            yield return new WaitForSecondsRealtime(velocidadParpadeo);
        }
    }

    public void AbrirOpcionesYPausar()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SonarBoton();
            AudioManager.Instance.PausarEventosLargos(true);
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
            AudioManager.Instance.PausarEventosLargos(false);
        }
        Time.timeScale = 1f;
        if (botonEngranaje != null) botonEngranaje.SetActive(true);
        if (pantallaOpciones != null) pantallaOpciones.SetActive(false);
    }
}