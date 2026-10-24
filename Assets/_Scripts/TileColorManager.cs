using UnityEngine;

public class TileColorManager : MonoBehaviour
{
    public Material mat_baldosa_verde;
    public Material mat_baldosa_amarillo;
    public Material mat_baldosa_roja;

    private MeshRenderer meshRenderer;
    public bool esLetal = false; 

    void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    public void PonerVerde() 
    { 
        meshRenderer.material = mat_baldosa_verde; 
        esLetal = false; 
    }
    
    public void PonerAmarillo() 
    { 
        meshRenderer.material = mat_baldosa_amarillo; 
        esLetal = false; 
    }

    // Muestra un destello rojo de advertencia, pero AÚN NO MATA (esLetal sigue false)
    public void PonerDestelloAdvertencia()
    {
        meshRenderer.material = mat_baldosa_roja;
        esLetal = false;
    }
    
    public void PonerRojo() 
    { 
        meshRenderer.material = mat_baldosa_roja; 
        esLetal = true; // Aquí sí mata al jugador
    }
}