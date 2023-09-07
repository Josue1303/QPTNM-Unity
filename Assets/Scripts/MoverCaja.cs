using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Miss no pude, le falle @LuisperezEspantaminas
///

public class MoverCaja : MonoBehaviour
{
    public float velocidad = 1.0f;  // Velocidad de interpolación
    public Vector3 nuevaPosicion;   // La nueva posición a la que se moverá el objeto
    
    private bool mover = false;     // Indicador de si debemos mover el objeto o no

    // Update is called once per frame
    void Update()
    {
        // Si el indicador 'mover' es verdadero, interpola la posición del objeto
        if (mover)
        {
            transform.position = Vector3.Lerp(transform.position, nuevaPosicion, Time.deltaTime * velocidad);
        }
    }

    // Esta función se llama cuando otro objeto entra en el trigger
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Igualito a mi apa, y hasta los mismos gustos");
        // Verifica si el nombre del objeto con el que se interactuó es "x"
        if (other.gameObject.name == "Caja(Clone)" || other.gameObject.name == "Cube" || other.gameObject.name == "Caja")
        {
            // Cambia el indicador para comenzar a mover el objeto
            mover = true;
            Debug.Log("To earn the righ to work");

            // Aquí puedes asignar una nueva posición si lo deseas
            // nuevaPosicion = new Vector3(x, y, z);
        }
        else {
            Debug.LogError("TILIN NOS ABANDONO");
        }
    }

    // Esta función se llama cuando otro objeto sale del trigger
    // private void OnTriggerExit(Collider tilin)
    // {
    //     // Puedes hacer que el objeto deje de moverse cuando el objeto "x" ya no esté en contacto
    //     if (tilin.gameObject.name == "AgenteMover")
    //     {
    //         mover = false;
    //     }
    // }
}
