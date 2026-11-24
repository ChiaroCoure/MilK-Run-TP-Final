using System.Collections;
using UnityEngine;
using TMPro;

[System.Serializable]
public class ConfiguracionNivel
{
    public string textoHUD = "NIVEL 1/3";
    public string cartelAnuncioInicial = "¡NIVEL 1: LLEGA A LA TIENDA!";
    
    [Header("Modalidad y Largo de Calle")]
    public bool esIdaYVuelta = false;
    public int largoFilas = 8;

    [Header("Configuración del Cronómetro")]
    public float tiempoInicialReloj = 50f;
    public float segundosExtraAlRegresar = 25f;

    [Header("Velocidad de las Baldosas")]
    public float tiempoAmarilloNormal = 1.2f;
    public float tiempoAmarilloPanico = 0.85f;
    public float tiempoRojoLetal = 1.2f;
    [Range(0.1f, 0.95f)] public float probabilidadTrampa = 0.70f;
}

public class LevelProgressionManager : MonoBehaviour
{
    public static int NivelActual = 1;

    [Header("Referencias del Juego (Se autoconectan si quedan vacías)")]
    public GridManager gridManager;
    public PlayerGridMovement movimientoJugador;
    public PlayerSensor playerSensor;
    public LevelTimer levelTimer;

    [Header("Textos de Nivel en Pantalla (TMP)")]
    public TMP_Text textoEtapaHUD;
    public TMP_Text cartelAnuncioInicio;
    public float duracionAnuncioInicio = 2.0f;

    [Header("Objetos de la Meta (Solo plataforma y tienda_final)")]
    public Transform[] objetosTienda;
    public int largoBaseEscena = 10;

    [Header("EDITA AQUÍ LOS TIEMPOS Y VELOCIDADES DE CADA NIVEL")]
    public ConfiguracionNivel[] niveles = new ConfiguracionNivel[3]
    {
        new ConfiguracionNivel {
            textoHUD = "NIVEL 1/3",
            cartelAnuncioInicial = "¡NIVEL 1: LLEGA A LA TIENDA!",
            esIdaYVuelta = false,
            largoFilas = 8,
            tiempoInicialReloj = 50f,
            segundosExtraAlRegresar = 0f,
            tiempoAmarilloNormal = 1.2f,
            tiempoAmarilloPanico = 0.85f,
            tiempoRojoLetal = 1.2f,
            probabilidadTrampa = 0.70f
        },
        new ConfiguracionNivel {
            textoHUD = "NIVEL 2/3",
            cartelAnuncioInicial = "¡NIVEL 2: COMPRA Y VUELVE A CASA!",
            esIdaYVuelta = true,
            largoFilas = 10,
            tiempoInicialReloj = 45f,
            segundosExtraAlRegresar = 25f,
            tiempoAmarilloNormal = 1.0f,
            tiempoAmarilloPanico = 0.70f,
            tiempoRojoLetal = 1.2f,
            probabilidadTrampa = 0.75f
        },
        new ConfiguracionNivel {
            textoHUD = "NIVEL 3/3",
            cartelAnuncioInicial = "¡NIVEL 3: HORA PICO EXTREMA!",
            esIdaYVuelta = true,
            largoFilas = 12,
            tiempoInicialReloj = 40f,
            segundosExtraAlRegresar = 25f,
            tiempoAmarilloNormal = 0.85f,
            tiempoAmarilloPanico = 0.65f,
            tiempoRojoLetal = 1.0f,
            probabilidadTrampa = 0.85f
        }
    };

    void Awake()
    {
        Application.targetFrameRate = 60;

        // Autoconexión de seguridad por si faltó arrastrar alguno en el Inspector
        if (gridManager == null) gridManager = FindFirstObjectByType<GridManager>();
        if (movimientoJugador == null) movimientoJugador = FindFirstObjectByType<PlayerGridMovement>();
        if (playerSensor == null) playerSensor = FindFirstObjectByType<PlayerSensor>();
        if (levelTimer == null) levelTimer = FindFirstObjectByType<LevelTimer>();

        if (niveles == null || niveles.Length == 0) return;

        int indice = Mathf.Clamp(NivelActual - 1, 0, niveles.Length - 1);
        AplicarConfiguracion(niveles[indice]);
    }

    private void AplicarConfiguracion(ConfiguracionNivel config)
    {
        if (textoEtapaHUD != null) textoEtapaHUD.text = config.textoHUD;

        if (gridManager != null)
        {
            gridManager.largo = config.largoFilas;
            gridManager.tiempoAdvertencia = config.tiempoAmarilloNormal;
            gridManager.tiempoLetalRojo = config.tiempoRojoLetal;
            gridManager.probabilidadTrampa = config.probabilidadTrampa;
        }

        if (movimientoJugador != null)
        {
            movimientoJugador.filasLargo = config.largoFilas;
        }

        if (playerSensor != null)
        {
            playerSensor.requiereIdaYVuelta = config.esIdaYVuelta;
            playerSensor.segundosExtraAlAgarrarLeche = config.segundosExtraAlRegresar;
        }

        if (levelTimer != null)
        {
            levelTimer.ConfigurarRelojNivel(
                config.tiempoInicialReloj,
                config.tiempoAmarilloNormal,
                config.tiempoAmarilloPanico
            );
        }

        AjustarPosicionTienda(config.largoFilas);

        if (cartelAnuncioInicio != null)
        {
            StartCoroutine(MostrarAnuncioDeNivel(config.cartelAnuncioInicial));
        }
    }

    private IEnumerator MostrarAnuncioDeNivel(string mensaje)
    {
        cartelAnuncioInicio.text = mensaje;
        cartelAnuncioInicio.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(duracionAnuncioInicio);
        cartelAnuncioInicio.gameObject.SetActive(false);
    }

    private void AjustarPosicionTienda(int nuevoLargo)
    {
        if (gridManager == null || objetosTienda == null) return;

        float diferenciaFilas = nuevoLargo - largoBaseEscena;
        float desplazamientoZ = diferenciaFilas * gridManager.tamañoBaldosa;

        foreach (Transform obj in objetosTienda)
        {
            if (obj != null)
            {
                obj.position += new Vector3(0f, 0f, desplazamientoZ);
            }
        }
    }
}