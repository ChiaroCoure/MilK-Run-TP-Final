using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [Header("Configuración de la Grilla")]
    public GameObject baldosaPrefab; 
    public int ancho = 5;
    public int largo = 10;
    public float tamañoBaldosa = 1.1f; // Asegúrate de que coincida con el Tamaño Baldosa del player en el Inspector
    
    [Header("Referencias")]
    public Transform jugador; 
    public CameraFollow camaraJuego;

    [Header("Tiempos de Oleada")]
    public float tiempoAdvertencia = 1.0f; // LevelTimer lo baja a 0.7f automáticamente en los últimos 5 segundos
    public float tiempoLetalRojo = 1.2f;

    private TileColorManager[,] matrizDeBaldosas; 
    private int inicioX;
    private int inicioZ;

    void Start()
    {
        matrizDeBaldosas = new TileColorManager[ancho, largo];

        for (int x = 0; x < ancho; x++)
        {
            for (int z = 0; z < largo; z++)
            {
                Vector3 posicion = new Vector3(x * tamañoBaldosa, 0, z * tamañoBaldosa);
                GameObject nueva = Instantiate(baldosaPrefab, posicion, baldosaPrefab.transform.rotation);
                
                matrizDeBaldosas[x, z] = nueva.GetComponent<TileColorManager>();
                nueva.transform.parent = this.transform;
            }
        }
        
        // Guardamos la baldosa inicial como Zona Segura permanente
        inicioX = Mathf.RoundToInt(jugador.position.x / tamañoBaldosa);
        inicioZ = Mathf.RoundToInt(jugador.position.z / tamañoBaldosa);

        StartCoroutine(SistemaDeOleadasCaoticas());
    }

    IEnumerator SistemaDeOleadasCaoticas()
    {
        yield return new WaitForSeconds(0.5f);

        while (true)
        {
            bool[,] vanASerRojas = new bool[ancho, largo];
            
            int jX = Mathf.RoundToInt(jugador.position.x / tamañoBaldosa);
            int jZ = Mathf.RoundToInt(jugador.position.z / tamañoBaldosa);

            // 1. FASE CAÓTICA
            for (int x = 0; x < ancho; x++)
            {
                for (int z = 0; z < largo; z++)
                {
                    // La primera baldosa siempre es verde
                    if (x == inicioX && z == inicioZ)
                    {
                        vanASerRojas[x, z] = false;
                        matrizDeBaldosas[x, z].PonerVerde();
                        continue; 
                    }

                    if (Random.value < 0.75f) 
                    {
                        vanASerRojas[x, z] = true;
                        matrizDeBaldosas[x, z].PonerAmarillo();
                    }
                    else
                    {
                        vanASerRojas[x, z] = false;
                        matrizDeBaldosas[x, z].PonerVerde();
                    }
                }
            }

            // 2. LA REGLA DE SALVACIÓN
            if (jX >= 0 && jX < ancho && jZ >= 0 && jZ < largo)
            {
                if (jX == inicioX && jZ == inicioZ)
                {
                    vanASerRojas[jX, jZ] = false;
                    matrizDeBaldosas[jX, jZ].PonerVerde();
                }
                else
                {
                    // La baldosa que pisa actualmente se vuelve trampa para obligarlo a moverse
                    vanASerRojas[jX, jZ] = true;
                    matrizDeBaldosas[jX, jZ].PonerAmarillo();
                }

                int escape = 0;
                float suerte = Random.value; 

                if (suerte < 0.15f) escape = 0;      // 15% Adelante
                else if (suerte < 0.40f) escape = 1; // 25% Atrás
                else if (suerte < 0.70f) escape = 2; // 30% Derecha
                else escape = 3;                     // 30% Izquierda
                
                bool escapeExitoso = false;

                if (escape == 0 && jZ + 1 < largo) { vanASerRojas[jX, jZ + 1] = false; matrizDeBaldosas[jX, jZ + 1].PonerVerde(); escapeExitoso = true; }
                else if (escape == 1 && jZ - 1 >= 0) { vanASerRojas[jX, jZ - 1] = false; matrizDeBaldosas[jX, jZ - 1].PonerVerde(); escapeExitoso = true; }
                else if (escape == 2 && jX + 1 < ancho) { vanASerRojas[jX + 1, jZ] = false; matrizDeBaldosas[jX + 1, jZ].PonerVerde(); escapeExitoso = true; }
                else if (escape == 3 && jX - 1 >= 0) { vanASerRojas[jX - 1, jZ] = false; matrizDeBaldosas[jX - 1, jZ].PonerVerde(); escapeExitoso = true; }
                
                if (!escapeExitoso) 
                {
                    if (jX + 1 < ancho) { vanASerRojas[jX + 1, jZ] = false; matrizDeBaldosas[jX + 1, jZ].PonerVerde(); }
                    else if (jX - 1 >= 0) { vanASerRojas[jX - 1, jZ] = false; matrizDeBaldosas[jX - 1, jZ].PonerVerde(); }
                    else if (jZ - 1 >= 0) { vanASerRojas[jX, jZ - 1] = false; matrizDeBaldosas[jX, jZ - 1].PonerVerde(); }
                    else if (jZ + 1 < largo) { vanASerRojas[jX, jZ + 1] = false; matrizDeBaldosas[jX, jZ + 1].PonerVerde(); }
                }
            }

            // ¡SONIDO DE BALDOSA AMARILLA (Empieza una sola vez al inicio de la fase amarilla)!
            if (AudioManager.Instance != null) AudioManager.Instance.SonarAmarillo();

            // 2.5 TRANSICIÓN CON PARPADEO (70% amarillo fijo, 30% parpadeo rápido de alerta)
            yield return new WaitForSeconds(tiempoAdvertencia * 0.7f);

            float intervaloParpadeo = (tiempoAdvertencia * 0.3f) / 4f;
            for (int p = 0; p < 2; p++)
            {
                CambiarColorTrampas(vanASerRojas, destelloRojo: true);
                yield return new WaitForSeconds(intervaloParpadeo);
                CambiarColorTrampas(vanASerRojas, destelloRojo: false);
                yield return new WaitForSeconds(intervaloParpadeo);
            }

            // ¡CORTA EL SONIDO AMARILLO JUSTO AL TERMINAR EL TIEMPO DE ADVERTENCIA!
            if (AudioManager.Instance != null) AudioManager.Instance.DetenerAmarillo();

            // 3. FASE LETAL
            for (int x = 0; x < ancho; x++)
            {
                for (int z = 0; z < largo; z++)
                {
                    if (vanASerRojas[x, z]) matrizDeBaldosas[x, z].PonerRojo();
                }
            }

            // ¡SONIDO DE BALDOSA ROJA LETAL!
            if (AudioManager.Instance != null) AudioManager.Instance.SonarRojo();

            // Sacudida sutil de cámara cuando las trampas se activan
            if (camaraJuego != null) camaraJuego.SacudirCamara(0.12f, 0.08f);

            yield return new WaitForSeconds(tiempoLetalRojo); 
        }
    }

    private void CambiarColorTrampas(bool[,] trampas, bool destelloRojo)
    {
        for (int x = 0; x < ancho; x++)
        {
            for (int z = 0; z < largo; z++)
            {
                if (trampas[x, z])
                {
                    if (destelloRojo) matrizDeBaldosas[x, z].PonerDestelloAdvertencia();
                    else matrizDeBaldosas[x, z].PonerAmarillo();
                }
            }
        }
    }
}