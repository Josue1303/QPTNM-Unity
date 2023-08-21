using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstanciarBots : MonoBehaviour
{
    public GameObject Barril;
    public GameObject Dado;
    public int iterationCount = 10;
    public Vector3 rangoPosiciones = new Vector3(10f, 0.0f, 10f); // Rango de posiciones en el eje X, Y y Z


    // Start is called before the first frame update
    void Start()
    {
        GenerarInstancias(); // Initial        
        
    }

    void GenerarInstancias(){
        for(int i = 0; i < iterationCount; i++)
        {
            Vector3 posicionAleatoria = new Vector3(
                Random.Range(-rangoPosiciones.x, rangoPosiciones.x),
                rangoPosiciones.y,
                Random.Range(-rangoPosiciones.z, rangoPosiciones.z)
            );

            Vector3 posicionAleatoria2 = new Vector3(
                Random.Range(-rangoPosiciones.x, rangoPosiciones.x),
                rangoPosiciones.y,
                Random.Range(-rangoPosiciones.z, rangoPosiciones.z)
            );

             GameObject nuevoObjeto = Instantiate(Dado, posicionAleatoria, Quaternion.identity);
             GameObject nuevoObjeto2 = Instantiate(Barril, posicionAleatoria2, Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
