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
    public static int NivelActual = 1; // 1, 2 o 3

    [Header("Referencias del Juego")]
    public GridManager gridManager;
    public PlayerGridMovement movimientoJugador;
    public PlayerSensor playerSensor;
    public LevelTimer levelTimer;

    [Header("Textos de Nivel en Pantalla (TMP)")]
    [Tooltip("Texto fijo arriba en la pantalla que dice NIVEL 1/3")]
    public TMP_Text textoEtapaHUD;
    [Tooltip("Cartel central que parpadea al empezar el nivel")]
    public TMP_Text cartelAnuncioInicio;

    [Header("Objetos de la Meta (Solo plataforma y tienda_final)")]
    public Transform[] objetosTienda;
    public int largoBaseEscena = 10;

    [Header("Parámetros por Nivel (Editables en Inspector)")]
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
        if (niveles == null || niveles.Length == 0) return;

        int indice = Mathf.Clamp(NivelActual - 1, 0, niveles.Length - 1);
        AplicarConfiguracion(niveles[indice]);
    }

    private void AplicarConfiguracion(ConfiguracionNivel config)
    {
        // 1. Actualiza el texto fijo del HUD ("NIVEL 1/3")
        if (textoEtapaHUD != null)
        {
            textoEtapaHUD.text = config.textoHUD;
        }

        // 2. Configura largo y velocidades en GridManager
        if (gridManager != null)
        {
            gridManager.largo = config.largoFilas;
            gridManager.tiempoAdvertencia = config.tiempoAmarilloNormal;
            gridManager.tiempoLetalRojo = config.tiempoRojoLetal;
            gridManager.probabilidadTrampa = config.probabilidadTrampa;
        }

        // 3. Configura el límite de pasos hacia adelante del abuelo
        if (movimientoJugador != null)
        {
            movimientoJugador.filasLargo = config.largoFilas;
        }

        // 4. Configura si hay que volver con la leche y cuánto tiempo suma
        if (playerSensor != null)
        {
            playerSensor.requiereIdaYVuelta = config.esIdaYVuelta;
            playerSensor.segundosExtraAlAgarrarLeche = config.segundosExtraAlRegresar;
        }

        // 5. Configura el cronómetro del nivel
        if (levelTimer != null)
        {
            levelTimer.ConfigurarRelojNivel(
                config.tiempoInicialReloj,
                config.tiempoAmarilloNormal,
                config.tiempoAmarilloPanico
            );
        }

        // 6. Acomoda la tienda y la vereda blanca al final de las baldosas
        AjustarPosicionTienda(config.largoFilas);

        // 7. Muestra el cartel anunciando la misión del nivel al empezar
        if (cartelAnuncioInicio != null)
        {
            StartCoroutine(MostrarAnuncioDeNivel(config.cartelAnuncioInicial));
        }
    }

    private IEnumerator MostrarAnuncioDeNivel(string mensaje)
    {
        cartelAnuncioInicio.text = mensaje;

        for (int i = 0; i < 3; i++)
        {
            cartelAnuncioInicio.gameObject.SetActive(true);
            yield return new WaitForSecondsRealtime(0.22f);
            cartelAnuncioInicio.gameObject.SetActive(false);
            yield return new WaitForSecondsRealtime(0.18f);
        }
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