using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Canales de Reproducción (AudioSources)")]
    public AudioSource fuenteMusica;
    public AudioSource fuenteSFX;          // Cortos: Botones, Páginas, Pasos, Rojo
    public AudioSource fuenteSFX_Amarillo; // Largos: Amarillo, Policía, Derrota, Llegaste, Victoria

    [Header("Volúmenes Actuales (0 a 1)")]
    [Range(0f, 1f)] public float volumenMusica = 0.5f;
    [Range(0f, 1f)] public float volumenEfectos = 1.0f;

    [Header("1. Músicas de Fondo")]
    public AudioClip musicaMenuEIntro;
    public AudioClip musicaGameplay;

    [Header("2. Efectos del Gameplay")]
    public AudioClip sfxPasoJugador;
    public AudioClip sfxBaldosaAmarilla;
    public AudioClip sfxBaldosaRoja;

    [Header("3. Efectos de Derrota y Victoria")]
    public AudioClip sfxCartelPolicia;
    public AudioClip sfxPantallaDerrota;
    public AudioClip sfxCartelLlegaste;
    public AudioClip sfxPantallaVictoria;

    [Header("4. Efectos de Interfaz e Intro")]
    public AudioClip sfxBotonClick;
    public AudioClip sfxPasarPagina;

    [Header("Nombres de tus Escenas")]
    public string nombreEscenaMenu = "MenuPrincipal";
    public string nombreEscenaIntro = "EscenaIntro";

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            AplicarVolumenes();
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void OnEnable() { SceneManager.sceneLoaded += AlCargarEscena; }
    void OnDisable() { SceneManager.sceneLoaded -= AlCargarEscena; }

    private void AlCargarEscena(Scene escena, LoadSceneMode modo)
    {
        if (fuenteSFX_Amarillo != null) fuenteSFX_Amarillo.Stop();
        AplicarVolumenes();

        if (escena.name == nombreEscenaMenu || escena.name == nombreEscenaIntro)
        {
            ReproducirMusica(musicaMenuEIntro);
        }
        else
        {
            ReproducirMusica(musicaGameplay);
        }
    }

    // --- CONTROL DE VOLUMEN PARA LOS SLIDERS ---
    public void CambiarVolumenMusica(float nuevoVolumen)
    {
        volumenMusica = nuevoVolumen;
        if (fuenteMusica != null) fuenteMusica.volume = volumenMusica;
    }

    public void CambiarVolumenEfectos(float nuevoVolumen)
    {
        volumenEfectos = nuevoVolumen;
        if (fuenteSFX != null) fuenteSFX.volume = volumenEfectos;
        if (fuenteSFX_Amarillo != null) fuenteSFX_Amarillo.volume = volumenEfectos;
    }

    private void AplicarVolumenes()
    {
        if (fuenteMusica != null) fuenteMusica.volume = volumenMusica;
        if (fuenteSFX != null) fuenteSFX.volume = volumenEfectos;
        if (fuenteSFX_Amarillo != null) fuenteSFX_Amarillo.volume = volumenEfectos;
    }

    public void ReproducirMusica(AudioClip nuevaMusica)
    {
        if (fuenteMusica == null || nuevaMusica == null) return;
        if (fuenteMusica.clip == nuevaMusica && fuenteMusica.isPlaying) return;

        fuenteMusica.clip = nuevaMusica;
        fuenteMusica.loop = true;
        fuenteMusica.volume = volumenMusica;
        fuenteMusica.Play();
    }

    public void DetenerMusica()
    {
        if (fuenteMusica != null) fuenteMusica.Stop();
    }

    // --- CANAL 1: CORTOS E INTOCABLES ---
    public void SonarPaso() { ReproducirSFX(sfxPasoJugador); }
    public void SonarRojo() { ReproducirSFX(sfxBaldosaRoja); }
    public void SonarBoton() { ReproducirSFX(sfxBotonClick); }
    public void SonarPagina() { ReproducirSFX(sfxPasarPagina); }

    private void ReproducirSFX(AudioClip clip)
    {
        if (fuenteSFX != null && clip != null)
        {
            fuenteSFX.PlayOneShot(clip);
        }
    }

    // --- CANAL 2: LARGOS E INTERRUMPIBLES ---
    public void SonarAmarillo() { ReproducirEventoLargo(sfxBaldosaAmarilla); }
    public void SonarPolicia() { ReproducirEventoLargo(sfxCartelPolicia); }
    public void SonarDerrota() { ReproducirEventoLargo(sfxPantallaDerrota); }
    public void SonarLlegaste() { ReproducirEventoLargo(sfxCartelLlegaste); }
    public void SonarVictoria() { ReproducirEventoLargo(sfxPantallaVictoria); }

    private void ReproducirEventoLargo(AudioClip clip)
    {
        if (fuenteSFX_Amarillo != null && clip != null)
        {
            fuenteSFX_Amarillo.Stop();
            fuenteSFX_Amarillo.clip = clip;
            fuenteSFX_Amarillo.volume = volumenEfectos;
            fuenteSFX_Amarillo.Play();
        }
    }

    public void DetenerAmarillo()
    {
        if (fuenteSFX_Amarillo != null) fuenteSFX_Amarillo.Stop();
    }

    public void PausarEventosLargos(bool pausar)
    {
        if (fuenteSFX_Amarillo == null) return;
        
        if (pausar) fuenteSFX_Amarillo.Pause();
        else fuenteSFX_Amarillo.UnPause();
    }
}