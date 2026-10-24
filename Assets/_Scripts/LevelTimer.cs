using UnityEngine;
using UnityEngine.UI;
using TMPro; // Librería necesaria para controlar TextMeshPro

public class LevelTimer : MonoBehaviour
{
    [Header("Configuración de Tiempo")]
    public float tiempoRestante = 50f; // 50 segundos como en tu imagen
    public float segundoDePanico = 5f; // Cuándo se activa la alerta roja

    [Header("Elementos Visuales (UI)")]
    public TMP_Text textoReloj;
    public Image imagenReloj;
    public Sprite relojAzul; // reloj.png
    public Sprite relojRojo; // reloj3.png
    public Color colorBordePanico = new Color(0.8f, 0.35f, 0.35f, 1f); // Rojo del reloj3.png

    [Header("Conexiones del Juego")]
    public GridManager gridManager;
    public PlayerSensor playerSensor;

    private bool cronometroActivo = true;
    private bool modoPanicoActivado = false;

    void Start()
    {
        if (imagenReloj != null && relojAzul != null)
        {
            imagenReloj.sprite = relojAzul;
        }
        ActualizarTextoUI();
    }

    void Update()
    {
        if (!cronometroActivo) return;

        // Restamos el tiempo segundo a segundo
        tiempoRestante -= Time.deltaTime;

        // ¿Entramos en los últimos 5 segundos?
        if (tiempoRestante <= segundoDePanico && !modoPanicoActivado)
        {
            ActivarModoPanico();
        }

        // ¿Se acabó el tiempo?
        if (tiempoRestante <= 0f)
        {
            tiempoRestante = 0f;
            cronometroActivo = false;
            ActualizarTextoUI();
            
            // Forzamos la derrota porque cerró la tienda
            if (playerSensor != null) playerSensor.ForzarDerrotaPorTiempo();
            return;
        }

        ActualizarTextoUI();
    }

    private void ActivarModoPanico()
    {
        modoPanicoActivado = true;

        // 1. Cambiamos el icono del reloj azul al rojo
        if (imagenReloj != null && relojRojo != null)
        {
            imagenReloj.sprite = relojRojo;
        }

        // 2. Cambiamos el delineado (Outline) del texto a rojo
        if (textoReloj != null)
        {
            // Modificamos el color del borde en el material del texto
            textoReloj.fontMaterial.SetColor(ShaderUtilities.ID_OutlineColor, colorBordePanico);
        }

        // 3. Aceleramos las baldosas amarillas de 1.0s a 0.7s
        if (gridManager != null)
        {
            gridManager.tiempoAdvertencia = 0.7f;
        }
    }

    private void ActualizarTextoUI()
    {
        if (textoReloj == null) return;

        int minutos = Mathf.FloorToInt(tiempoRestante / 60f);
        int segundos = Mathf.FloorToInt(tiempoRestante % 60f);

        // Formato 0:50 o 0:05
        textoReloj.text = string.Format("{0}:{1:00}", minutos, segundos);
    }

    public void DetenerCronometro()
    {
        cronometroActivo = false;
    }
}