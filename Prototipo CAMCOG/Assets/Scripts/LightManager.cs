using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightManager : MonoBehaviour
{
    public Light2D luz;
    public float intensidadMaxima = 1f;
    public float tiempoEntreParpadeos = 1.5f; 

    private float tiempoUltimoCambio;
    private bool luzEncendida = false;

    void Start()
    {
        if (luz == null)
        {
            Debug.LogError("No se ha asignado ninguna luz en el inspector.");
        }

        tiempoUltimoCambio = Time.time;
    }

    void Update()
    {
        if (luz != null && luz.enabled)
        {
            if (Time.time - tiempoUltimoCambio >= tiempoEntreParpadeos)
            {
                CambiarIntensidad();
                tiempoUltimoCambio = Time.time;
            }
        }
    }

    void CambiarIntensidad()
    {
        if (luzEncendida)
        {
            luz.intensity = 0f; 
        }
        else
        {
            luz.intensity = intensidadMaxima; 
        }

        luzEncendida = !luzEncendida;
    }
}
