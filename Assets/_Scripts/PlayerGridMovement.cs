using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerGridMovement : MonoBehaviour
{
    [Header("Movimiento")]
    public float tamañoBaldosa = 1.1f; 
    public float velocidadMovimiento = 5f;
    
    [Header("Límites del Mapa (En cantidad de baldosas)")]
    public int columnasAncho = 5;
    public int filasLargo = 10;

    [Header("Controles Táctiles (Móvil)")]
    [Tooltip("Distancia mínima en píxeles para detectar un deslizamiento del dedo")]
    public float distanciaMinimaSwipe = 40f;
    [Tooltip("Arrastra aquí el objeto 3D 'cartel' (WASD) si quieres que se oculte en Android")]
    public GameObject cartelWASD3D;

    [HideInInspector] public bool puedeMoverse = true;
    
    private Animator animator;
    private bool seEstaMoviendo = false;
    private Vector3 posicionObjetivo;

    private Vector2 puntoInicioToque;
    private bool detectandoSwipe = false;

    public Vector3 CasillaActual => posicionObjetivo;

    void Awake()
    {
        posicionObjetivo = transform.position;

        // Si corre en un teléfono Android o iOS, oculta el cartel de madera WASD
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

        // 1. Entrada de Teclado (PC)
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputZ = Input.GetAxisRaw("Vertical");

        if (inputX != 0) inputZ = 0;

        if (inputX != 0 || inputZ != 0)
        {
            IntentarMover(new Vector3(inputX, 0, inputZ));
            return;
        }

        // 2. Entrada Táctil por Gestos Swipe (Móvil)
        DetectarSwipe();
    }

    private void DetectarSwipe()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Ignora el toque si el jugador está tocando el botón de pausa o un menú
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject()) return;

            puntoInicioToque = Input.mousePosition;
            detectandoSwipe = true;
        }
        else if (Input.GetMouseButtonUp(0) && detectandoSwipe)
        {
            detectandoSwipe = false;
            Vector2 delta = (Vector2)Input.mousePosition - puntoInicioToque;

            if (delta.magnitude >= distanciaMinimaSwipe)
            {
                if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
                {
                    IntentarMover(new Vector3(Mathf.Sign(delta.x), 0, 0));
                }
                else
                {
                    IntentarMover(new Vector3(0, 0, Mathf.Sign(delta.y)));
                }
            }
        }
    }

    // 3. Funciones Públicas para Botones de Cruceta Virtual (D-Pad en Canvas)
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