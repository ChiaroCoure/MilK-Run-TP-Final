using System.Collections;
using UnityEngine;

public class PlayerGridMovement : MonoBehaviour
{
    [Header("Movimiento")]
    public float tamañoBaldosa = 1.1f; 
    public float velocidadMovimiento = 5f;
    
    [Header("Límites del Mapa (En cantidad de baldosas)")]
    public int columnasAncho = 5;
    public int filasLargo = 10;

    [HideInInspector] public bool puedeMoverse = true;
    
    private Animator animator;
    private bool seEstaMoviendo = false;
    private Vector3 posicionObjetivo;

    // ¡NUEVO! Permite que el sensor lea la casilla lógica actual (o de destino)
    // ignorando el borde de la baldosa que el abuelo está dejando atrás.
    public Vector3 CasillaActual => posicionObjetivo;

    void Awake()
    {
        // Lo inicializamos en Awake para que esté listo antes de que empiece el sensor
        posicionObjetivo = transform.position;
    }

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!puedeMoverse || Time.timeScale == 0f) return; 
        if (seEstaMoviendo) return;

        float inputX = Input.GetAxisRaw("Horizontal");
        float inputZ = Input.GetAxisRaw("Vertical");

        if (inputX != 0) inputZ = 0;

        if (inputX != 0 || inputZ != 0)
        {
            Vector3 direccionMovimiento = new Vector3(inputX, 0, inputZ);
            Vector3 futuroDestino = transform.position + (direccionMovimiento * tamañoBaldosa);

            if (EsPosicionValida(futuroDestino))
            {
                posicionObjetivo = futuroDestino;
                StartCoroutine(MoverA(posicionObjetivo, direccionMovimiento));
            }
        }
    }

    private bool EsPosicionValida(Vector3 destino)
    {
        int casillaX = Mathf.RoundToInt(destino.x / tamañoBaldosa);
        int casillaZ = Mathf.RoundToInt(destino.z / tamañoBaldosa);

        bool dentroDeAncho = casillaX >= 0 && casillaX < columnasAncho;
        bool dentroDeLargo = casillaZ >= 0 && casillaZ <= filasLargo;

        return dentroDeAncho && dentroDeLargo;
    }

    IEnumerator MoverA(Vector3 destino, Vector3 direccion)
    {
        seEstaMoviendo = true;
        if (animator != null) animator.SetBool("isWalking", true);

        if (AudioManager.Instance != null) AudioManager.Instance.SonarPaso();

        if (direccion != Vector3.zero) transform.rotation = Quaternion.LookRotation(direccion);

        while (Vector3.Distance(transform.position, destino) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, destino, velocidadMovimiento * Time.deltaTime);
            yield return null; 
        }

        transform.position = destino;
        if (animator != null) animator.SetBool("isWalking", false);
        seEstaMoviendo = false;
    }
}