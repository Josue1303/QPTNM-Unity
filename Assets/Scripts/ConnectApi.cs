using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;


public class ConnectApi : MonoBehaviour
{
    public GameObject robots;
    public GameObject cajas;
    public float timeStep = 1f;
    public GameObject[] agentes;
    public GameObject[] agentesCaja;

    public int numAgents = 2;


    private Vector3[] posicionInicial;

    private Vector3[] nuevaPosicion;

    private Vector3[] posicionInicialCaja;

    private Vector3[] nuevaPosicionCaja;

    private bool[] enCarga;

    private int cajasActuales;

    [System.Serializable]
    public class AgentData
    {
        public int id;
        public int[] pos;
        public string type;
        public bool enCarga;
    }

    [System.Serializable]
    public class AgentDataCaja
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

    [System.Serializable]
    public class AgentListCaja
    {
        public List<AgentDataCaja> agentListCaja;
    }

    private string baseURL = "http://127.0.0.1:5000";

    // Start is called before the first frame update
    void Start()
    {
        agentes = new GameObject[numAgents];
        posicionInicial = new Vector3[numAgents];
        nuevaPosicion = new Vector3[numAgents];
        agentesCaja = new GameObject[1000];
        enCarga = new bool[numAgents];
        posicionInicialCaja = new Vector3[1000];
        nuevaPosicionCaja = new Vector3[1000];
        InitializeModel(32, 24);
        StartCoroutine(Todo());


    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            // Coloca aquí la acción que deseas ejecutar cuando se presiona "R"
            InitializeModel(32,24);
        }
    }


    IEnumerator Todo()
    {
        while (true)
        {
            StartCoroutine(GetAgentData(baseURL));
            yield return new WaitForSeconds(timeStep); // Espera 1 segundo
            StartCoroutine(Step(baseURL));
        }

    }


    public void InitializeModel(int M, int N)
    {
        string json = "{\"M\": " + M + ", \"N\": " + N + "}";
        StartCoroutine(PostRequest(baseURL + "/initialize", json));
        for (int i = 0; i < numAgents; i++)
        {
            agentes[i] = Instantiate(robots, transform.position, transform.rotation);
            agentes[i].transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        }
        for (int i = 0; i < 1000; i++)
        {
            agentesCaja[i] = Instantiate(cajas, transform.position, transform.rotation);
            agentesCaja[i].transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        }
    }

    IEnumerator Step(string baseURL)
    {

        string url = baseURL + "/step";
        UnityWebRequest request = UnityWebRequest.Get(url);

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
            float tiempoDeAnimacion = timeStep - 0.2f;
            float tiempoPasado = 0f;

            foreach (AgentData agent in receivedData.agentList)
            {
                int x = agent.pos[0];
                int y = agent.pos[1];               //LOGIC EQUIVOCADA
                int id = agent.id;
                enCarga[cont] = agent.enCarga;
                Debug.Log("ID: " + id + ", X: " + x + ", Y: " + y + ", enCarga: " + enCarga[cont]);

                // aquí ando pero voy a comer -LG
                nuevaPosicion[cont] = new Vector3(x, 0f, y);
                posicionInicial[cont] = agentes[cont].transform.position;

                cont++;
            }

            while (tiempoPasado < tiempoDeAnimacion)
            {

                tiempoPasado += Time.deltaTime;
                float t = tiempoPasado / tiempoDeAnimacion;

                // Interpola suavemente la posición y la rotación

                for (int i = 0; i < numAgents; i++)
                {
                    agentes[i].transform.position = Vector3.Lerp(posicionInicial[i], nuevaPosicion[i], t);
                    agentes[i].transform.rotation = Quaternion.Lerp(agentes[i].transform.rotation, Quaternion.identity, t);
                    GameObject hijoADesactivar = agentes[i].transform.GetChild(1).gameObject;
                    if (enCarga[i] == true)
                    {
                        // Desactiva el GameObject hijo
                        hijoADesactivar.SetActive(true);
                    }
                    else
                    {
                        hijoADesactivar.SetActive(false);
                    }
                }

                yield return null;
            }

            // Asegúrate de que el objeto termine exactamente en la posición deseada
            // agentes[cont].transform.position = nuevaPosicion;
        }
        else
        {
            Debug.LogError("Error: " + request.error);
        }

        StartCoroutine(GetAgentDataCaja(baseURL));
    }


    IEnumerator GetAgentDataCaja(string baseURL)
    {
        string url = baseURL + "/caja_data";
        UnityWebRequest request = UnityWebRequest.Get(url);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            // Aquí encapsulamos la lista en una clase contenedora. //CAMBIAR ESTO 
            AgentListCaja receivedData = JsonUtility.FromJson<AgentListCaja>("{\"agentListCaja\":" + request.downloadHandler.text + "}");
            // 
            int cont = 0;
            float tiempoDeAnimacion = timeStep - 0.2f;
            float tiempoPasado = 0f;

            foreach (AgentDataCaja agent in receivedData.agentListCaja)
            {
                int x = agent.pos[0];
                int y = agent.pos[1];               //LOGIC EQUIVOCADA
                int id = agent.id;
                nuevaPosicionCaja[cont] = new Vector3(x, 0.4f, y);
                posicionInicialCaja[cont] = agentesCaja[cont].transform.position;

                cont++;
            }



            for (int i = 0; i < cont; i++)
            {
                agentesCaja[i].transform.position = nuevaPosicionCaja[i];
            }


            // Asegúrate de que el objeto termine exactamente en la posición deseada
            // agentes[cont].transform.position = nuevaPosicion;
        }
        else
        {
            Debug.LogError("Error: " + request.error);
        }
    }



    IEnumerator PostRequest(string url, string jsonData)
    {


        // Configura la solicitud POST
        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        // Envía la solicitud
        yield return request.SendWebRequest();

        // Maneja la respuesta
        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error en la solicitud: " + request.error);
        }
        else
        {
            Debug.Log("Solicitud exitosa. Respuesta: " + request.downloadHandler.text);
            // Puedes procesar la respuesta aquí.
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