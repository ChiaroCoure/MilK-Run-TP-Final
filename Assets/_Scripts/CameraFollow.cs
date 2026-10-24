using System.Collections;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform jugador; 
    public Vector3 offset = new Vector3(0f, 10f, -8f); 
    public float suavizado = 5f; 

    // Desplazamiento extra cuando tiembla la pantalla
    private Vector3 temblorOffset = Vector3.zero;

    void LateUpdate()
    {
        if (jugador != null)
        {
            Vector3 posicionDeseada = jugador.position + offset;
            Vector3 posicionSuavizada = Vector3.Lerp(transform.position, posicionDeseada, suavizado * Time.deltaTime);
            
            // Aplicamos la posición normal más el temblor (si lo hay)
            transform.position = posicionSuavizada + temblorOffset;
        }
    }

    public void SacudirCamara(float duracion = 0.15f, float fuerza = 0.2f)
    {
        StopAllCoroutines();
        StartCoroutine(RutinaTemblor(duracion, fuerza));
    }

    private IEnumerator RutinaTemblor(float duracion, float fuerza)
    {
        float tiempoTranscurrido = 0f;

        while (tiempoTranscurrido < duracion)
        {
            // Genera un movimiento aleatorio corto en X e Y
            temblorOffset = Random.insideUnitSphere * fuerza;
            tiempoTranscurrido += Time.unscaledDeltaTime;
            yield return null;
        }

        // Al terminar los 0.15s, el temblor vuelve a cero
        temblorOffset = Vector3.zero;
    }
}