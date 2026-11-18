using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class IntroSlideManager : MonoBehaviour
{
    [Header("Elementos Visuales")]
    public Image visorDeImagen;
    public Sprite[] paginasHistoria;

    [Header("Configuración de Escena")]
    public string nombreEscenaJuego = "GameplayScene";

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
        // Funciona con teclado en PC y con toque en pantalla (o clic) en celular
        bool avanzoTeclado = Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow);
        bool avanzoToque = Input.GetMouseButtonDown(0);

        if (avanzoTeclado || avanzoToque)
        {
            AvanzarPagina();
        }
    }

    public void AvanzarPagina()
    {
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