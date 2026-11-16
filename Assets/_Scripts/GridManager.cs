using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [Header("Configuración de la Grilla")]
    public GameObject baldosaPrefab; 
    public int ancho = 5;
    public int largo = 10;
    public float tamañoBaldosa = 1.1f;
    
    [Header("Referencias")]
    public Transform jugador; 
    public CameraFollow camaraJuego;

    [Header("Tiempos y Dificultad")]
    public float tiempoAdvertencia = 1.0f; 
    public float tiempoLetalRojo = 1.2f;
    [Range(0.1f, 0.95f)] public float probabilidadTrampa = 0.75f;

    [HideInInspector] public bool jugadorRegresandoConLeche = false;

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
            
            int jX = Mathf.Clamp(Mathf.RoundToInt(jugador.position.x / tamañoBaldosa), 0, ancho - 1);
            int jZ = Mathf.RoundToInt(jugador.position.z / tamañoBaldosa);

            // 1. FASE CAÓTICA
            for (int x = 0; x < ancho; x++)
            {
                for (int z = 0; z < largo; z++)
                {
                    // La casilla inicial es refugio seguro a la ida, y meta segura a la vuelta
                    if (x == inicioX && z == inicioZ)
                    {
                        vanASerRojas[x, z] = false;
                        matrizDeBaldosas[x, z].PonerVerde();
                        continue; 
                    }

                    if (Random.value < probabilidadTrampa) 
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
            if (jZ >= 0 && jZ < largo)
            {
                if (!jugadorRegresandoConLeche && jX == inicioX && jZ == inicioZ)
                {
                    vanASerRojas[jX, jZ] = false;
                    matrizDeBaldosas[jX, jZ].PonerVerde();
                }
                else
                {
                    vanASerRojas[jX, jZ] = true;
                    matrizDeBaldosas[jX, jZ].PonerAmarillo();
                }

                int escape = 0;
                float suerte = Random.value; 

                // Si ya tiene la leche, invertimos la prioridad para ayudarlo a volver hacia Z = 0
                if (!jugadorRegresandoConLeche)
                {
                    if (suerte < 0.15f) escape = 0;      // Adelante
                    else if (suerte < 0.40f) escape = 1; // Atrás
                    else if (suerte < 0.70f) escape = 2; // Derecha
                    else escape = 3;                     // Izquierda
                }
                else
                {
                    if (suerte < 0.25f) escape = 1;      // Hacia la salida (Z - 1)
                    else if (suerte < 0.40f) escape = 0; // Hacia la tienda (Z + 1)
                    else if (suerte < 0.70f) escape = 2; // Derecha
                    else escape = 3;                     // Izquierda
                }
                
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
            else if (jZ >= largo)
            {
                // Si el jugador está parado en la vereda blanca con la leche, abrimos paso en la fila superior para que pueda reingresar
                vanASerRojas[jX, largo - 1] = false;
                matrizDeBaldosas[jX, largo - 1].PonerVerde();
            }

            if (AudioManager.Instance != null) AudioManager.Instance.SonarAmarillo();

            // 2.5 TRANSICIÓN CON PARPADEO + VIBRACIÓN DE ACCESIBILIDAD EN AMARILLO
            yield return new WaitForSeconds(tiempoAdvertencia * 0.65f);

            // ¡NUEVO! Vibración preventiva suave durante el último tramo del amarillo
            float tiempoParpadeoTotal = tiempoAdvertencia * 0.35f;
            if (CameraFollow.Instance != null)
            {
                CameraFollow.Instance.SacudirCamara(tiempoParpadeoTotal, 0.045f, 45f);
            }

            float intervaloParpadeo = tiempoParpadeoTotal / 4f;
            for (int p = 0; p < 2; p++)
            {
                CambiarColorTrampas(vanASerRojas, destelloRojo: true);
                yield return new WaitForSeconds(intervaloParpadeo);
                CambiarColorTrampas(vanASerRojas, destelloRojo: false);
                yield return new WaitForSeconds(intervaloParpadeo);
            }

            if (AudioManager.Instance != null) AudioManager.Instance.DetenerAmarillo();

            // 3. FASE LETAL
            for (int x = 0; x < ancho; x++)
            {
                for (int z = 0; z < largo; z++)
                {
                    if (vanASerRojas[x, z]) matrizDeBaldosas[x, z].PonerRojo();
                }
            }

            if (AudioManager.Instance != null) AudioManager.Instance.SonarRojo();

            // Pulso muy sutil al activarse el rojo
            if (CameraFollow.Instance != null) CameraFollow.Instance.SacudirCamara(0.1f, 0.03f, 25f);

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