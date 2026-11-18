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

    [Header("Ajustes Móviles")]
    [Tooltip("Arrastra aquí el objeto 3D 'cartel' (WASD) para que se oculte automáticamente en Android")]
    public GameObject cartelWASD3D;

    [HideInInspector] public bool puedeMoverse = true;
    
    private Animator animator;
    private bool seEstaMoviendo = false;
    private Vector3 posicionObjetivo;

    public Vector3 CasillaActual => posicionObjetivo;

    void Awake()
    {
        posicionObjetivo = transform.position;

        // Oculta el cartel 3D de madera WASD cuando corre en un teléfono
        if (Application.isMobilePlatform && cartelWASD3D != null)
        {
            cartelWASD3D.SetActive(false);
        }
    }

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!puedeMoverse || Time.timeScale == 0f) return; 
        if (seEstaMoviendo) return;

        // Entrada de Teclado (Para versión base de PC y pruebas en Editor)
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputZ = Input.GetAxisRaw("Vertical");

        if (inputX != 0) inputZ = 0;

        if (inputX != 0 || inputZ != 0)
        {
            IntentarMover(new Vector3(inputX, 0, inputZ));
        }
    }

    // Funciones Públicas para los 4 Botones de Dirección en Pantalla (Conectar con Event Trigger -> Pointer Down)
    public void MoverArriba() { IntentarMover(Vector3.forward); }
    public void MoverAbajo() { IntentarMover(Vector3.back); }
    public void MoverDerecha() { IntentarMover(Vector3.right); }
    public void MoverIzquierda() { IntentarMover(Vector3.left); }

    public void IntentarMover(Vector3 direccionMovimiento)
    {
        if (!puedeMoverse || Time.timeScale == 0f || seEstaMoviendo) return;

        Vector3 futuroDestino = transform.position + (direccionMovimiento * tamañoBaldosa);

        if (EsPosicionValida(futuroDestino))
        {
            posicionObjetivo = futuroDestino;
            StartCoroutine(MoverA(posicionObjetivo, direccionMovimiento));
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