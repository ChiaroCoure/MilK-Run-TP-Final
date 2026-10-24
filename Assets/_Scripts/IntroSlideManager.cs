using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class IntroSlideManager : MonoBehaviour
{
    [Header("Elementos Visuales")]
    public Image visorDeImagen;
    public Sprite[] paginasHistoria;

    [Header("Configuración de Escena")]
    public string nombreEscenaJuego = "SampleScene";

    private int indicePaginaActual = 0;

    void Start()
    {
        Time.timeScale = 1f;
        
        if (paginasHistoria.Length > 0 && visorDeImagen != null)
        {
            visorDeImagen.sprite = paginasHistoria[0];
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            AvanzarPagina();
        }
    }

    public void AvanzarPagina()
    {
        // ¡SONIDO DE PASAR PÁGINA!
        if (AudioManager.Instance != null) AudioManager.Instance.SonarPagina();

        indicePaginaActual++;

        if (indicePaginaActual < paginasHistoria.Length)
        {
            visorDeImagen.sprite = paginasHistoria[indicePaginaActual];
        }
        else
        {
            SceneManager.LoadScene(nombreEscenaJuego);
        }
    }
}