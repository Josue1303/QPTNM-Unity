using UnityEngine;



public class BarGraph : MonoBehaviour
{
    public static BarGraph Instance;

    public int paquetesAgenteMover = 0;  // Simula la cantidad de paquetes recogidos por el agente "Mover"
    public int paquetesAgenteRecoger = 0;  // Simula la cantidad de paquetes recogidos por el agente "Recoger"

    private int maxBarHeight = 100; // La altura máxima de la barra en pixels
    private int scaleFactor = 1;  // Factor para escalar la barra

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnGUI()
{
    // Dibuja etiquetas
    GUI.Label(new Rect(100, 10, 100, 20), "Agente Mover");  // Cambiado de (10, 10, 100, 20)
    GUI.Label(new Rect(210, 10, 100, 20), "Agente Recoger");  // Cambiado de (120, 10, 100, 20)

    // Dibuja las barras
    int moverHeight = Mathf.Min(paquetesAgenteMover * scaleFactor, maxBarHeight);
    int recogerHeight = Mathf.Min(paquetesAgenteRecoger * scaleFactor, maxBarHeight);

    GUI.Box(new Rect(100, 40, 20, moverHeight), "");  // Cambiado de (10, 40, 20, moverHeight)
    GUI.Box(new Rect(210, 40, 20, recogerHeight), "");  // Cambiado de (120, 40, 20, recogerHeight)

    // Mostrar números
    GUI.Label(new Rect(100, moverHeight + 50, 50, 20), paquetesAgenteMover.ToString());  // Cambiado de (10, moverHeight + 50, 50, 20)
    GUI.Label(new Rect(210, recogerHeight + 50, 50, 20), paquetesAgenteRecoger.ToString());  // Cambiado de (120, recogerHeight + 50, 50, 20)
}
}
