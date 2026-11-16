using System.Collections;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public static CameraFollow Instance;

    public Transform jugador; 
    public Vector3 offset = new Vector3(0f, 4f, -3f); 
    public float suavizado = 5f; 

    private Vector3 posicionBase;
    private Vector3 temblorOffset = Vector3.zero;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (jugador != null)
        {
            posicionBase = jugador.position + offset;
            transform.position = posicionBase;
        }
        else
        {
            posicionBase = transform.position;
        }
    }

    void LateUpdate()
    {
        if (jugador != null)
        {
            Vector3 posicionDeseada = jugador.position + offset;
            posicionBase = Vector3.Lerp(posicionBase, posicionDeseada, suavizado * Time.deltaTime);
        }

        transform.position = posicionBase + temblorOffset;
    }

    public void SacudirCamara(float duracion, float fuerza, float frecuencia = 35f)
    {
        StopAllCoroutines();
        StartCoroutine(RutinaTemblorSuave(duracion, fuerza, frecuencia));
    }

    private IEnumerator RutinaTemblorSuave(float duracion, float fuerza, float frecuencia)
    {
        float tiempoTranscurrido = 0f;

        while (tiempoTranscurrido < duracion)
        {
            // Se va apagando suavemente hacia el final de la duración (de 1 a 0)
            float amortiguacion = 1f - (tiempoTranscurrido / duracion);

            // Onda suave en X e Y (sin saltos bruscos en Z)
            float ondaX = Mathf.Sin(Time.unscaledTime * frecuencia) * fuerza * amortiguacion;
            float ondaY = Mathf.Cos(Time.unscaledTime * (frecuencia * 0.8f)) * (fuerza * 0.6f) * amortiguacion;

            temblorOffset = new Vector3(ondaX, ondaY, 0f);

            tiempoTranscurrido += Time.unscaledDeltaTime;
            yield return null;
        }

        temblorOffset = Vector3.zero;
    }
}