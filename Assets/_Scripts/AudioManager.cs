using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Canales de Reproducción (AudioSources)")]
    public AudioSource fuenteMusica;
    public AudioSource fuenteSFX;          // Intocable: Botones, Páginas, Pasos, Rojo
    public AudioSource fuenteSFX_Amarillo; // Interrumpible: Amarillo, Policía, Derrota, Llegaste, Victoria

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
        // SOLO cortamos el canal de eventos largos (Victoria, Derrota, Amarillo).
        // fuenteSFX queda libre para que los botones y páginas suenen completos al cambiar de escena.
        if (fuenteSFX_Amarillo != null) fuenteSFX_Amarillo.Stop();

        if (escena.name == nombreEscenaMenu || escena.name == nombreEscenaIntro)
        {
            ReproducirMusica(musicaMenuEIntro);
        }
        else
        {
            ReproducirMusica(musicaGameplay);
        }
    }

    public void ReproducirMusica(AudioClip nuevaMusica)
    {
        if (fuenteMusica == null || nuevaMusica == null) return;
        if (fuenteMusica.clip == nuevaMusica && fuenteMusica.isPlaying) return;

        fuenteMusica.clip = nuevaMusica;
        fuenteMusica.loop = true;
        fuenteMusica.Play();
    }

    public void DetenerMusica()
    {
        if (fuenteMusica != null) fuenteMusica.Stop();
    }

    // --- CANAL 1 DE EFECTOS: CORTOS E INTOCABLES (PlayOneShot) ---
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

    // --- CANAL 2 DE EFECTOS: LARGOS E INTERRUMPIBLES ---
    public void SonarAmarillo() { ReproducirEventoLargo(sfxBaldosaAmarilla); }
    public void SonarPolicia() { ReproducirEventoLargo(sfxCartelPolicia); }
    public void SonarDerrota() { ReproducirEventoLargo(sfxPantallaDerrota); }
    public void SonarLlegaste() { ReproducirEventoLargo(sfxCartelLlegaste); }
    public void SonarVictoria() { ReproducirEventoLargo(sfxPantallaVictoria); }

    private void ReproducirEventoLargo(AudioClip clip)
    {
        if (fuenteSFX_Amarillo != null && clip != null)
        {
            fuenteSFX_Amarillo.Stop(); // Corta el evento anterior (ej. calla el amarillo al sonar la policía)
            fuenteSFX_Amarillo.clip = clip;
            fuenteSFX_Amarillo.Play();
        }
    }

    public void DetenerAmarillo()
    {
        if (fuenteSFX_Amarillo != null) fuenteSFX_Amarillo.Stop();
    }

    // Evita que el cronómetro amarillo siga sonando mientras estás en el menú de pausa
    public void PausarEventosLargos(bool pausar)
    {
        if (fuenteSFX_Amarillo == null) return;
        
        if (pausar) fuenteSFX_Amarillo.Pause();
        else fuenteSFX_Amarillo.UnPause();
    }
}