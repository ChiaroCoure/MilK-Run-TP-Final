using UnityEngine;

public class SafeAreaPanel : MonoBehaviour
{
    private RectTransform rectTransform;
    private Rect ultimaSafeArea = new Rect(0, 0, 0, 0);
    private Vector2Int ultimaResolucion = Vector2Int.zero;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        AplicarSafeArea();
    }

    void Start()
    {
        // Refuerzo al iniciar el primer fotograma del Canvas
        AplicarSafeArea();
    }

    // Se dispara automáticamente apenas el Canvas o el Simulador cambian de tamaño
    void OnRectTransformDimensionsChange()
    {
        AplicarSafeArea();
    }

    void Update()
    {
        if (ultimaSafeArea != Screen.safeArea || ultimaResolucion.x != Screen.width || ultimaResolucion.y != Screen.height)
        {
            AplicarSafeArea();
        }
    }

    private void AplicarSafeArea()
    {
        if (rectTransform == null) rectTransform = GetComponent<RectTransform>();
        if (Screen.width <= 0 || Screen.height <= 0) return;

        Rect safeArea = Screen.safeArea;
        ultimaSafeArea = safeArea;
        ultimaResolucion = new Vector2Int(Screen.width, Screen.height);

        Vector2 anchorMin = safeArea.position;
        Vector2 anchorMax = safeArea.position + safeArea.size;

        anchorMin.x /= Screen.width;
        anchorMin.y /= Screen.height;
        anchorMax.x /= Screen.width;
        anchorMax.y /= Screen.height;

        rectTransform.anchorMin = anchorMin;
        rectTransform.anchorMax = anchorMax;

        // Fuerza a 0 los bordes Left, Right, Top y Bottom para que se aplique de inmediato
        rectTransform.offsetMin = Vector2.zero;
        rectTransform.offsetMax = Vector2.zero;
    }
}