using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;


public class ConnectApi : MonoBehaviour
{
    public GameObject robots;
    public float timeStep = 1f;
    public GameObject[] agentes;

    public float velocidad = 5.0f; // Velocidad de movimiento en unidades por segundo
    public Vector3 direccion = Vector3.forward; // Dirección de movimiento (por defecto, hacia adelante)

    private int[] x;
    private int[] y;
    
    [System.Serializable]
    public class AgentData
    {
        public int id;
        public int[] pos;
        public string type;
    }

    [System.Serializable]
    public class AgentList
    {
        public List<AgentData> agentList;
    }

    private string baseURL = "http://127.0.0.1:5000";

    // Start is called before the first frame update
    void Start()
    {
        agentes = new GameObject[2];
        InitializeModel(24,32);
        StartCoroutine(Todo());


    }

    // Update is called once per frame
    void Update()
    {   
        
    }


    IEnumerator Todo(){
        while(true){
            yield return new WaitForSeconds(timeStep); // Espera 1 segundo
            StartCoroutine(Step(baseURL));
            StartCoroutine(GetAgentData(baseURL));
        }
        
    }
    

    public void InitializeModel(int M, int N)
    {
        string json = "{\"M\": " + M + ", \"N\": " + N + "}";
        StartCoroutine(PostRequest(baseURL + "/initialize", json));
        for (int i= 0; i<2; i++){
         agentes[i] = Instantiate(robots, transform.position, transform.rotation);
         agentes[i].transform.localScale = new Vector3(1,1,1);
        }
    }
                                                                                                                                                        
    IEnumerator Step(string baseURL)
    {
        
        string url = baseURL + "/step";
        UnityWebRequest request = UnityWebRequest.Get(url);
        
        yield return request.SendWebRequest();
        Debug.Log("Hola?");

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Received: " + request.downloadHandler.text);
            
        }
        else
        {
            Debug.LogError("Error: " + request.error);
            
        }

        
    }


IEnumerator GetAgentData(string baseURL)
{
    string url = baseURL + "/agent_data";
    UnityWebRequest request = UnityWebRequest.Get(url);
    
    yield return request.SendWebRequest();

    if (request.result == UnityWebRequest.Result.Success)
    {
        // Aquí encapsulamos la lista en una clase contenedora. //CAMBIAR ESTO 
        AgentList receivedData = JsonUtility.FromJson<AgentList>("{\"agentList\":" + request.downloadHandler.text + "}");

        // 
        int cont = 0;
        float tiempoDeAnimacion = 1.0f;
        foreach (AgentData agent in receivedData.agentList)
        {
            int x = agent.pos[0];
            int y = agent.pos[1];               //LOGIC EQUIVOCADA
            int id = agent.id; 
        
            Debug.Log("ID: " + id + ", X: " + x + ", Y: " + y);
    
            Vector3 nuevaPosicion = new Vector3(x, 0f, y);
            Vector3 posicionInicial = agentes[cont].transform.position;
            
            float tiempoPasado = 0f;

            while (tiempoPasado < tiempoDeAnimacion)
            {
                tiempoPasado += Time.deltaTime;
                float t = tiempoPasado / tiempoDeAnimacion;
                
                // Interpola suavemente la posición y la rotación
                agentes[cont].transform.position = Vector3.Lerp(posicionInicial, nuevaPosicion, t);
                agentes[cont].transform.rotation = Quaternion.Lerp(agentes[cont].transform.rotation, Quaternion.identity, t);

                yield return null;
            }
            
            // Asegúrate de que el objeto termine exactamente en la posición deseada
            agentes[cont].transform.position = nuevaPosicion;
            
            cont++;
        }
    }
    else
    {
        Debug.LogError("Error: " + request.error);
    }
}

    

    IEnumerator PostRequest(string url, string json)
    {
        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        Debug.Log("pene1rico");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Received: " + request.downloadHandler.text);
            
        }
        else
        {
            Debug.LogError("Error: " + request.error);
            
            
        }
    }

    IEnumerator GetRequest(string url)
    {
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Received: " + request.downloadHandler.text);
        }
        else
        {
            Debug.Log("Error: " + request.error);
        }
    }

}