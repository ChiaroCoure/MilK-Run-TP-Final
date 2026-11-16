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

    [Header("Mecánica Ida y Vuelta")]
    public GameObject cartonLecheCabeza; 
    public bool requiereIdaYVuelta = false;
    public float segundosExtraAlAgarrarLeche = 25f;

    private bool tieneLeche = false;
    private bool juegoTerminado = false;

    void Awake()
    {
        // Se asegura de que SIEMPRE empiece sin el cartón de leche en la cabeza
        tieneLeche = false;
        if (cartonLecheCabeza != null)
        {
            // Desactiva cualquier Collider del cartón para que jamás tape el láser del abuelo
            Collider[] colisionadoresLeche = cartonLecheCabeza.GetComponentsInChildren<Collider>(true);
            foreach (Collider col in colisionadoresLeche)
            {
                col.enabled = false;
            }

            cartonLecheCabeza.SetActive(false);
        }
    }

    void Update()
    {
        if (juegoTerminado) return;
        DetectarSuelo();
    }

    void DetectarSuelo()
    {
        Vector3 puntoAEvaluar = (movimientoJugador != null) ? movimientoJugador.CasillaActual : transform.position;

        // Si está en un nivel de Ida y Vuelta y YA agarró la leche en la tienda, gana al volver a la fila 0
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
                if (!requiereIdaYVuelta)
                {
                    // Nivel 1: Solo ida -> Gana directamente al pisar la vereda blanca
                    FinalizarPartida(gano: true);
                }
                else if (!tieneLeche)
                {
                    // Niveles 2 y 3: Recién aquí aparece la leche en la cabeza y el cartel de volver a casa
                    RecogerLecheYEmpezarRegreso();
                }
                return;
            }

            // 2. ¿Pisó una baldosa roja activa?
            TileColorManager baldosaPisada = golpe.collider.GetComponent<TileColorManager>();
            if (baldosaPisada != null && baldosaPisada.esLetal)
            {
                FinalizarPartida(gano: false);
            }
        }
    }

    private void RecogerLecheYEmpezarRegreso()
    {
        tieneLeche = true;

        // 1. Aparece el cartón de leche sobre la cabeza del abuelo
        if (cartonLecheCabeza != null) cartonLecheCabeza.SetActive(true);

        // 2. Avisa a la grilla que ahora el camino de salvación debe ayudarlo a bajar hacia Z = 0
        if (gridManager != null) gridManager.jugadorRegresandoConLeche = true;

        // 3. Suma el tiempo extra configurado en el Inspector para ese nivel
        if (levelTimer != null) levelTimer.SumarTiempo(segundosExtraAlAgarrarLeche);

        // 4. Muestra el cartel "¡Llegaste! ¡Ahora de regreso a casa!" y suena la campana
        if (uiManager != null) uiManager.MostrarCartelRegresoCasa();
        if (AudioManager.Instance != null) AudioManager.Instance.SonarLlegaste();
    }

    public void ForzarDerrotaPorTiempo()
    {
        if (!juegoTerminado)
        {
            FinalizarPartida(gano: false);
        }
    }

    private void FinalizarPartida(bool gano)
    {
        juegoTerminado = true;
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
            if (CameraFollow.Instance != null) 
            {
                CameraFollow.Instance.SacudirCamara(0.35f, 0.1f, 30f);
            }
            if (uiManager != null) uiManager.MostrarPantallaDerrota();
        }
    }

    private System.Collections.IEnumerator PasarAlSiguienteNivel()
    {
        if (AudioManager.Instance != null) AudioManager.Instance.SonarLlegaste();
        
        if (uiManager != null)
        {
            // Espera a que termine de parpadear 3 veces el cartel de "¡LLEGASTE!"
            yield return StartCoroutine(uiManager.ParpadearCartelLlegaste());
        }
        else
        {
            yield return new WaitForSecondsRealtime(1.5f);
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}