using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour
{
    [Header("Pantallas Finales y Menús")]
    public GameObject pantallaGameOver;
    public GameObject pantallaVictoria;
    public GameObject pantallaOpciones;
    public GameObject botonEngranaje;

    [Header("Pop-ups dentro de Opciones")]
    public GameObject popupSonido;
    public GameObject popupConfirmarSalida;

    [Header("Sliders de Sonido")]
    public Slider sliderMusica;
    public Slider sliderEfectos;

    [Header("Carteles de Texto (TMP)")]
    public GameObject cartelPolicia;
    public GameObject cartelLlegaste;
    public GameObject cartelRegresoCasa;

    [Header("Configuración de Parpadeo")]
    public int cantidadParpadeos = 3;
    public float velocidadParpadeo = 0.22f;

    void Start()
    {
        // Sincroniza automáticamente las barritas de los Sliders con el AudioManager
        if (AudioManager.Instance != null)
        {
            if (sliderMusica != null)
            {
                sliderMusica.value = AudioManager.Instance.volumenMusica;
                sliderMusica.onValueChanged.AddListener(AudioManager.Instance.CambiarVolumenMusica);
            }

            if (sliderEfectos != null)
            {
                sliderEfectos.value = AudioManager.Instance.volumenEfectos;
                sliderEfectos.onValueChanged.AddListener(AudioManager.Instance.CambiarVolumenEfectos);
            }
        }
    }

    // --- FUNCIONES PARA EL POP-UP DE SONIDO (MENÚ Y GAMEPLAY) ---
    public void AbrirPopupSonido()
    {
        if (AudioManager.Instance != null) AudioManager.Instance.SonarBoton();
        if (popupSonido != null) popupSonido.SetActive(true);
    }

    public void CerrarPopupSonido()
    {
        if (AudioManager.Instance != null) AudioManager.Instance.SonarBoton();
        if (popupSonido != null) popupSonido.SetActive(false);
    }

    // --- FUNCIONES PARA EL POP-UP DE CONFIRMAR SALIDA (GAMEPLAY) ---
    public void AbrirPopupConfirmarSalida()
    {
        if (AudioManager.Instance != null) AudioManager.Instance.SonarBoton();
        if (popupConfirmarSalida != null) popupConfirmarSalida.SetActive(true);
    }

    public void CerrarPopupConfirmarSalida()
    {
        if (AudioManager.Instance != null) AudioManager.Instance.SonarBoton();
        if (popupConfirmarSalida != null) popupConfirmarSalida.SetActive(false);
    }

    // --- CARTELES Y PANTALLAS DE GAMEPLAY ---
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
        yield return StartCoroutine(ParpadearCartel(cartelPolicia));

        if (AudioManager.Instance != null) AudioManager.Instance.SonarDerrota();
        if (pantallaGameOver != null) pantallaGameOver.SetActive(true);
    }

    private IEnumerator SecuenciaVictoria()
    {
        if (AudioManager.Instance != null) AudioManager.Instance.SonarLlegaste();
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
        // Por seguridad, si había algún pop-up abierto adentro de Opciones, lo cierra
        if (popupSonido != null) popupSonido.SetActive(false);
        if (popupConfirmarSalida != null) popupConfirmarSalida.SetActive(false);

        Time.timeScale = 1f;
        if (botonEngranaje != null) botonEngranaje.SetActive(true);
        if (pantallaOpciones != null) pantallaOpciones.SetActive(false);
    }
}