using UnityEngine;

public class PlayerSensor : MonoBehaviour
{
    [Header("Referencias")]
    public GameUIManager uiManager;
    public PlayerGridMovement movimientoJugador;
    public GridManager gridManager;
    public LevelTimer levelTimer;
    public CameraFollow camaraJuego;

    private bool juegoTerminado = false;

    void Update()
    {
        if (juegoTerminado) return;
        DetectarSuelo();
    }

    void DetectarSuelo()
    {
        // ¡LA SOLUCIÓN! Evaluamos el centro de la casilla de destino (o donde está parado),
        // nunca la baldosa vieja que está abandonando a mitad del paso.
        Vector3 puntoAEvaluar = (movimientoJugador != null) ? movimientoJugador.CasillaActual : transform.position;

        if (Physics.Raycast(puntoAEvaluar + Vector3.up * 1.5f, Vector3.down, out RaycastHit golpe, 3f))
        {
            // 1. ¿Pisó el área blanca con el Tag "Meta"?
            if (golpe.collider.CompareTag("Meta"))
            {
                FinalizarPartida(gano: true);
                return;
            }

            // 2. ¿La casilla en la que está (o a la que acaba de saltar) es roja?
            TileColorManager baldosaPisada = golpe.collider.GetComponent<TileColorManager>();
            if (baldosaPisada != null && baldosaPisada.esLetal)
            {
                FinalizarPartida(gano: false);
            }
        }
    }

    public void ForzarDerrotaPorTiempo()
    {
        if (!juegoTerminado)
        {
            FinalizarPartida(gano: false);
        }
    }

    private void FinalizarPartida(bool gano)
    {
        juegoTerminado = true;
        if (movimientoJugador != null) movimientoJugador.puedeMoverse = false;
        if (gridManager != null) gridManager.StopAllCoroutines();
        if (levelTimer != null) levelTimer.DetenerCronometro();

        if (gano)
        {
            if (uiManager != null) uiManager.MostrarPantallaVictoria();
        }
        else
        {
            if (camaraJuego != null) camaraJuego.SacudirCamara(0.15f, 0.25f);
            if (uiManager != null) uiManager.MostrarPantallaDerrota();
        }
    }
}