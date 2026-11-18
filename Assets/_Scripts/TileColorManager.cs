using UnityEngine;

public class TileColorManager : MonoBehaviour
{
    public Material mat_baldosa_verde;
    public Material mat_baldosa_amarillo;
    public Material mat_baldosa_roja;

    private MeshRenderer meshRenderer;
    
    public bool esLetal = false;
    public bool esAdvertencia = false; // Indica si la baldosa está amarilla

    void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    public void PonerVerde() 
    { 
        meshRenderer.material = mat_baldosa_verde; 
        esLetal = false; 
        esAdvertencia = false;
    }
    
    public void PonerAmarillo() 
    { 
        meshRenderer.material = mat_baldosa_amarillo; 
        esLetal = false; 
        esAdvertencia = true;
    }

    public void PonerDestelloAdvertencia()
    {
        meshRenderer.material = mat_baldosa_roja;
        esLetal = false;
        esAdvertencia = true;
    }
    
    public void PonerRojo() 
    { 
        meshRenderer.material = mat_baldosa_roja; 
        esLetal = true; 
        esAdvertencia = false;
    }
}