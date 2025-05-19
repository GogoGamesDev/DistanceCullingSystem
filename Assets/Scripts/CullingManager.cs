using UnityEngine;
using System.Collections.Generic;

public class CullingManager : MonoBehaviour
{
    // Distancia máxima a la que los objetos se mantendrán visibles
    public float cullingDistance = 50f;
    // Referencia al jugador
    public Transform player;

    // Lista para almacenar los Renderers de todos los objetos en la escena
    public List<Renderer> renderers = new List<Renderer>();
    // Lista de excepciones que siempre estarán visibles
    public List<Renderer> exceptionRenderers = new List<Renderer>();

    void Start()
    {
        UpdateRenderer(); // Llamada inicial
    }

    // Método que se llamará cada vez que se active una nueva escena u objetos
    public void UpdateRenderer()
    {
        // Obtener todos los objetos que tienen un componente Renderer en la escena
        Renderer[] allRenderers = FindObjectsOfType<Renderer>();

        // Añadir cada Renderer a la lista, pero evitar duplicados
        foreach (Renderer rend in allRenderers)
        {
            if (!renderers.Contains(rend) && !exceptionRenderers.Contains(rend)) // Verificar si ya está en la lista
            {
                renderers.Add(rend);
            }
        }
        //Debug.Log("Renderer Updated Successfully!");
    }

    // Limpia la lista de renderers que hayan sido destruidos o desactivados
    private void CleanUpRenderers()
    {
        renderers.RemoveAll(rend => rend == null || !rend.gameObject.activeInHierarchy);
    }

    void Update()
    {
        // Llamar a la limpieza en cada frame para mantener la lista actualizada
        CleanUpRenderers();

        // Recorrer todos los objetos con Renderer en la escena
        foreach (Renderer rend in renderers)
        {
            if (rend != null)
            {
                // Si el Renderer es una excepción, continuar sin hacer nada
                if (exceptionRenderers.Contains(rend)) continue;

                // Comprobar la distancia entre el jugador y el objeto
                float distance = Vector3.Distance(player.position, rend.transform.position);

                // Si el objeto está fuera del rango de distancia, desactivar su Renderer
                if (distance > cullingDistance)
                {
                    if (rend.enabled) // Si el Renderer está activado, desactivarlo
                    {
                        rend.enabled = false;
                    }
                }
                else
                {
                    // Si el objeto está dentro del rango de distancia, reactiva el Renderer
                    if (!rend.enabled) // Si el Renderer está desactivado, activarlo
                    {
                        rend.enabled = true;
                    }
                }
            }
        }
    }
}
