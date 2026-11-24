using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelTimer : MonoBehaviour
{
    [Header("Configuración de Pánico")]
    [Tooltip("A cuántos segundos del final se pone el reloj rojo y se aceleran las baldosas")]
    public float segundoDePanico = 5f;

    // Se ocultan en el Inspector para evitar confusiones, ya que los controla LevelProgressionManager
    [HideInInspector] public float tiempoRestante = 50f;
    [HideInInspector] public float tiempoAdvertenciaNormal = 1.0f;
    [HideInInspector] public float tiempoAdvertenciaPanico = 0.7f;

    [Header("Elementos Visuales (UI)")]
    public TMP_Text textoReloj;
    public Image imagenReloj;
    public Sprite relojAzul;
    public Sprite relojRojo;
    public Color colorBordeNormal = new Color(0.45f, 0.55f, 0.85f, 1f);
    public Color colorBordePanico = new Color(0.8f, 0.35f, 0.35f, 1f);

    [Header("Conexiones del Juego (Se autoconectan si quedan vacías)")]
    public GridManager gridManager;
    public PlayerSensor playerSensor;

    private bool cronometroActivo = true;
    private bool modoPanicoActivado = false;

    void Awake()
    {
        if (gridManager == null) gridManager = FindFirstObjectByType<GridManager>();
        if (playerSensor == null) playerSensor = FindFirstObjectByType<PlayerSensor>();
    }

    void Start()
    {
        if (imagenReloj != null && relojAzul != null)
        {
            imagenReloj.sprite = relojAzul;
        }
        if (textoReloj != null)
        {
            colorBordeNormal = textoReloj.fontMaterial.GetColor(ShaderUtilities.ID_OutlineColor);
        }
        ActualizarTextoUI();
    }

    public void ConfigurarRelojNivel(float tiempoInicial, float advNormal, float advPanico)
    {
        tiempoRestante = tiempoInicial;
        tiempoAdvertenciaNormal = advNormal;
        tiempoAdvertenciaPanico = advPanico;
        modoPanicoActivado = false;
        ActualizarTextoUI();
    }

    void Update()
    {
        if (!cronometroActivo) return;

        tiempoRestante -= Time.deltaTime;

        if (tiempoRestante <= segundoDePanico && !modoPanicoActivado)
        {
            ActivarModoPanico();
        }

        if (tiempoRestante <= 0f)
        {
            tiempoRestante = 0f;
            cronometroActivo = false;
            ActualizarTextoUI();
            
            if (playerSensor != null) playerSensor.ForzarDerrotaPorTiempo();
            return;
        }

        ActualizarTextoUI();
    }

    public void SumarTiempo(float segundosExtra)
    {
        tiempoRestante += segundosExtra;

        if (modoPanicoActivado && tiempoRestante > segundoDePanico)
        {
            modoPanicoActivado = false;
            if (imagenReloj != null && relojAzul != null) imagenReloj.sprite = relojAzul;
            if (textoReloj != null) textoReloj.fontMaterial.SetColor(ShaderUtilities.ID_OutlineColor, colorBordeNormal);
            if (gridManager != null) gridManager.tiempoAdvertencia = tiempoAdvertenciaNormal;
        }

        ActualizarTextoUI();
    }

    private void ActivarModoPanico()
    {
        modoPanicoActivado = true;

        if (imagenReloj != null && relojRojo != null)
        {
            imagenReloj.sprite = relojRojo;
        }

        if (textoReloj != null)
        {
            textoReloj.fontMaterial.SetColor(ShaderUtilities.ID_OutlineColor, colorBordePanico);
        }

        if (gridManager != null)
        {
            gridManager.tiempoAdvertencia = tiempoAdvertenciaPanico;
        }
    }

    private void ActualizarTextoUI()
    {
        if (textoReloj == null) return;

        int minutos = Mathf.FloorToInt(tiempoRestante / 60f);
        int segundos = Mathf.FloorToInt(tiempoRestante % 60f);

        textoReloj.text = string.Format("{0}:{1:00}", minutos, segundos);
    }

    public void DetenerCronometro()
    {
        cronometroActivo = false;
    }
}