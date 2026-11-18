using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSensor : MonoBehaviour
{
    [Header("Referencias")]
    public GameUIManager uiManager;
    public PlayerGridMovement movimientoJugador;
    public GridManager gridManager;
    public LevelTimer levelTimer;
    public CameraFollow camaraJuego;

    [Header("Signo de Peligro en la Cabeza")]
    [Tooltip("Arrastra aquí el objeto SignoPeligro ('!') que está dentro del player")]
    public GameObject indicadorPeligroCabeza;

    [Header("Mecánica Ida y Vuelta")]
    public GameObject cartonLecheCabeza; 
    public bool requiereIdaYVuelta = false;
    public float segundosExtraAlAgarrarLeche = 25f;

    private bool tieneLeche = false;
    private bool juegoTerminado = false;

    void Awake()
    {
        tieneLeche = false;
        if (indicadorPeligroCabeza != null) indicadorPeligroCabeza.SetActive(false);

        if (cartonLecheCabeza != null)
        {
            Collider[] colisionadoresLeche = cartonLecheCabeza.GetComponentsInChildren<Collider>(true);
            foreach (Collider col in colisionadoresLeche) col.enabled = false;
            cartonLecheCabeza.SetActive(false);
        }
    }

    void Update()
    {
        if (juegoTerminado) return;
        DetectarSuelo();
    }

    void LateUpdate()
    {
        // Mantiene el signo '!' mirando siempre de frente a la cámara aunque el abuelo gire de costado
        if (indicadorPeligroCabeza != null && indicadorPeligroCabeza.activeSelf && Camera.main != null)
        {
            indicadorPeligroCabeza.transform.rotation = Camera.main.transform.rotation;
        }
    }

    void DetectarSuelo()
    {
        Vector3 puntoAEvaluar = (movimientoJugador != null) ? movimientoJugador.CasillaActual : transform.position;

        if (requiereIdaYVuelta && tieneLeche && movimientoJugador != null)
        {
            int casillaZ = Mathf.RoundToInt(puntoAEvaluar.z / movimientoJugador.tamañoBaldosa);
            if (casillaZ <= 0)
            {
                FinalizarPartida(gano: true);
                return;
            }
        }

        if (Physics.Raycast(puntoAEvaluar + Vector3.up * 1.5f, Vector3.down, out RaycastHit golpe, 3f))
        {
            // 1. ¿Pisó la vereda blanca ("Meta")?
            if (golpe.collider.CompareTag("Meta"))
            {
                MostrarSignoPeligro(false);

                if (!requiereIdaYVuelta) FinalizarPartida(gano: true);
                else if (!tieneLeche) RecogerLecheYEmpezarRegreso();
                return;
            }

            // 2. ¿Pisó una baldosa?
            TileColorManager baldosaPisada = golpe.collider.GetComponent<TileColorManager>();
            if (baldosaPisada != null)
            {
                if (baldosaPisada.esLetal)
                {
                    MostrarSignoPeligro(false);
                    FinalizarPartida(gano: false);
                }
                else
                {
                    // Prende el '!' si la baldosa es amarilla, y lo apaga apenas pisa una verde
                    MostrarSignoPeligro(baldosaPisada.esAdvertencia);
                }
            }
        }
    }

    private void MostrarSignoPeligro(bool mostrar)
    {
        if (indicadorPeligroCabeza != null && indicadorPeligroCabeza.activeSelf != mostrar)
        {
            indicadorPeligroCabeza.SetActive(mostrar);
        }
    }

    private void RecogerLecheYEmpezarRegreso()
    {
        tieneLeche = true;
        if (cartonLecheCabeza != null) cartonLecheCabeza.SetActive(true);
        if (gridManager != null) gridManager.jugadorRegresandoConLeche = true;
        if (levelTimer != null) levelTimer.SumarTiempo(segundosExtraAlAgarrarLeche);
        if (uiManager != null) uiManager.MostrarCartelRegresoCasa();
        if (AudioManager.Instance != null) AudioManager.Instance.SonarLlegaste();
    }

    public void ForzarDerrotaPorTiempo()
    {
        if (!juegoTerminado) FinalizarPartida(gano: false);
    }

    private void FinalizarPartida(bool gano)
    {
        juegoTerminado = true;
        MostrarSignoPeligro(false);

        if (movimientoJugador != null) movimientoJugador.puedeMoverse = false;
        if (gridManager != null) gridManager.StopAllCoroutines();
        if (levelTimer != null) levelTimer.DetenerCronometro();

        if (gano)
        {
            if (LevelProgressionManager.NivelActual < 3)
            {
                LevelProgressionManager.NivelActual++;
                StartCoroutine(PasarAlSiguienteNivel());
            }
            else
            {
                LevelProgressionManager.NivelActual = 1;
                if (uiManager != null) uiManager.MostrarPantallaVictoria();
            }
        }
        else
        {
            if (CameraFollow.Instance != null) CameraFollow.Instance.SacudirCamara(0.35f, 0.1f, 30f);
            if (uiManager != null) uiManager.MostrarPantallaDerrota();
        }
    }

    private System.Collections.IEnumerator PasarAlSiguienteNivel()
    {
        if (AudioManager.Instance != null) AudioManager.Instance.SonarLlegaste();
        if (uiManager != null) yield return StartCoroutine(uiManager.ParpadearCartelLlegaste());
        else yield return new WaitForSecondsRealtime(1.5f);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}