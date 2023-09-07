using UnityEngine;



public class BarGraph : MonoBehaviour
{
    public static BarGraph Instance;
    public int bateriaAgentes2;

    public int[] paquetesEntregados;

    private int divisor = 80;
    private int divisor2 = 80;

    public int reduxGraph = 1;
    public int reduxGraph2 = 1;
    public int[] steps;
    public int[] bateriaAgentes;  // Simula la cantidad de paquetes recogidos por el agente "Mover"
    public int paquetesAgenteRecoger = 0;  // Simula la cantidad de paquetes recogidos por el agente "Recoger"

    private int maxBarHeight = 120; // La altura máxima de la barra en pixels
    private int scaleFactor = 1;  // Factor para escalar la barra
    private int scaleFactor2 = 1;  // Factor para escalar la barra

    private void Awake()
    {
        bateriaAgentes = new int[15];
        paquetesEntregados = new int[15];
        steps = new int[15];
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
    GUI.Label(new Rect(100, 10, 500, 20), "Bateria consumida, en cada step");  // Cambiado de (10, 10, 100, 20)
    // GUI.Label(new Rect(210, 10, 100, 20), "Agente Recoger");  // Cambiado de (120, 10, 100, 20)

    GUI.Label(new Rect(100+400, 10, 500, 20), "Paquetes entregados");  // Cambiado de (10, 10, 100, 20)
    // GUI.Label(new Rect(210, 10, 100, 20), "Agente Recoger");  // Cambiado de (120, 10, 100, 20)

    // // Dibuja las barras
    // int bateriaHeight = Mathf.Min(bateriaAgentes * scaleFactor, maxBarHeight);

    for (int i = 0; i<15; i++){
        // Dibuja las barras
        GUI.Label(new Rect(100+(i*25), 30, 20, 20),steps[i].ToString() );
        if(bateriaAgentes[i] / divisor > reduxGraph){
            scaleFactor = 1+reduxGraph;
            reduxGraph += 1;
        }
        int bateriaHeight = Mathf.Min(bateriaAgentes[i] / scaleFactor, maxBarHeight);
        GUI.Box(new Rect(100+(i*25), 60, 20, bateriaHeight), "");    
        GUI.Label(new Rect(100+(i*25), bateriaHeight + 70, 50, 20), bateriaAgentes[i].ToString());


        GUI.Label(new Rect(100+400+(i*25), 30, 20, 20),steps[i].ToString() );
        if(paquetesEntregados[i] / divisor2 > reduxGraph2){
            scaleFactor2 = 1+reduxGraph2;
            reduxGraph2 += 1;
        }
        int paquetesHeight = Mathf.Min(paquetesEntregados[i] / scaleFactor2, maxBarHeight);
        GUI.Box(new Rect(100+400+(i*25), 60, 20, paquetesHeight), "");    
        GUI.Label(new Rect(100+400+(i*25), paquetesHeight + 70, 50, 20), paquetesEntregados[i].ToString());
    }

    // // int recogerHeight = Mathf.Min(paquetesAgenteRecoger * scaleFactor, maxBarHeight);

    // GUI.Box(new Rect(100, 40, 20, bateriaHeight), "");
    // GUI.Box(new Rect(125, 40, 20, bateriaHeight), "");
    //   // Cambiado de (10, 40, 20, moverHeight)
    // // GUI.Box(new Rect(210, 40, 20, recogerHeight), "");  // Cambiado de (120, 40, 20, recogerHeight)

    // // Mostrar números
    // GUI.Label(new Rect(100, bateriaHeight + 50, 50, 20), bateriaAgentes.ToString());
    // GUI.Label(new Rect(125, bateriaHeight + 50, 50, 20), bateriaAgentes.ToString());  // Cambiado de (10, moverHeight + 50, 50, 20)
    // // GUI.Label(new Rect(210, recogerHeight + 50, 50, 20), paquetesAgenteRecoger.ToString());  // Cambiado de (120, recogerHeight + 50, 50, 20)
}
}
