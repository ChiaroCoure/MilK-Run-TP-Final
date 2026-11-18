using UnityEngine;

public class SafeAreaPanel : MonoBehaviour
{
    private RectTransform rectTransform;
    private Rect ultimaSafeArea = new Rect(0, 0, 0, 0);

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        AplicarSafeArea();
    }

    void Update()
    {
        if (ultimaSafeArea != Screen.safeArea)
        {
            AplicarSafeArea();
        }
    }

    private void AplicarSafeArea()
    {
        Rect safeArea = Screen.safeArea;
        ultimaSafeArea = safeArea;

        Vector2 anchorMin = safeArea.position;
        Vector2 anchorMax = safeArea.position + safeArea.size;

        anchorMin.x /= Screen.width;
        anchorMin.y /= Screen.height;
        anchorMax.x /= Screen.width;
        anchorMax.y /= Screen.height;

        rectTransform.anchorMin = anchorMin;
        rectTransform.anchorMax = anchorMax;
    }
}