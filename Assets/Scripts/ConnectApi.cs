using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;


public class ConnectApi : MonoBehaviour
{
    public GameObject robots;
    public GameObject cajas;
    public GameObject robots_recoger;
    public float timeStep = 1f;
    public GameObject[] agentes;
    public GameObject[] agentesRecoger;
    public GameObject[] agentesCaja;

    public TMP_InputField numAgents;
    public TMP_InputField ratePackages;

    public TMP_InputField numAgentsRecoger;

    public GameObject MostrarGrafica;
    private bool MostrarGraficaBool=false;
    public TMP_Text cajasActualesText;

    public TMP_Text cantidadAgentes;

    public TMP_Text Steps;
    private int stepsfinal = 0;

    public TMP_Text agenteRecogerText;
    public Button buttonToDisable;
    private Vector3[] posicionInicial;

    private Vector3[] nuevaPosicion;
    private Vector3[] posicionInicialRecoger;

    private Vector3[] nuevaPosicionRecoger;

    private Vector3[] posicionInicialCaja;

    private Vector3[] nuevaPosicionCaja;

    private bool[] enCarga;
    private bool[] enCargaRecoger;

    private bool validadorpos = false;

    public bool[] sucia;
    public int bateriaConsumida = 0;
    private int steps15 = 0;
    private List<int> agentesContados = new List<int>();
    private List<int> agentesContadosRecoger = new List<int>();

    private int[] paquetesEntregadosCont;

    private int paquetesEntregadosFinal;

    public int numAgents2;

    public int ratePackages2;

    public int numAgentsRecoger2;
    // public int numAgentes;

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
    public class AgentDataRecoger
    {
        public int id;
        public int[] pos;
        public string type;
        public bool enCarga;
        public int paquetesDespachados;
        
    }

    [System.Serializable]
    public class AgentDataCaja
    {
        public int id;
        public int[] pos;
        public string type;
        public bool sucia;
    }


    [System.Serializable]
    public class AgentList
    {
        public List<AgentData> agentList;
    }

    [System.Serializable]
    public class AgentListRecoger
    {
        public List<AgentDataRecoger> agentListRecoger;
    }

    [System.Serializable]
    public class AgentListCaja
    {
        public List<AgentDataCaja> agentListCaja;
    }

    

    private string baseURL = "http://127.0.0.1:5000";

    // // Start is called before the first frame update
    // void Start()
    // {


    // }

    public void ConvertInputFieldsToIntegers()
    {
        if (int.TryParse(numAgents.text, out numAgents2) &&
            int.TryParse(ratePackages.text, out ratePackages2) &&
            int.TryParse(numAgentsRecoger.text, out numAgentsRecoger2))
        {
            // Las conversiones fueron exitosas, los valores están en numAgents2, ratePackages2 y numAgentsRecoger2
            Debug.Log("numAgents: " + numAgents2);
            Debug.Log("ratePackages: " + ratePackages2);
            Debug.Log("numAgentesRecoger: " + numAgentsRecoger2);
            // Ahora puedes usar numAgents2, ratePackages2 y numAgentsRecoger2 en tu código
        }
        else
        {
            // Al menos una de las conversiones falló, maneja el error aquí
            Debug.LogError("Error: Los valores ingresados no son números válidos.");
        }
    }


    public void restart()
    {
        Debug.Log("entro al restart");

        ConvertInputFieldsToIntegers();

        agentes = new GameObject[numAgents2];
        agentesRecoger = new GameObject[numAgentsRecoger2];
        posicionInicial = new Vector3[numAgents2];
        nuevaPosicion = new Vector3[numAgents2];
        posicionInicialRecoger = new Vector3[numAgentsRecoger2];
        nuevaPosicionRecoger = new Vector3[numAgentsRecoger2];
        paquetesEntregadosCont = new int[numAgentsRecoger2];
        agentesCaja = new GameObject[200];
        enCarga = new bool[numAgents2];
        enCargaRecoger = new bool[numAgentsRecoger2];
        posicionInicialCaja = new Vector3[200];
        nuevaPosicionCaja = new Vector3[200];
        sucia = new bool[200];
        for (int i = 0; i < numAgents2; i++)
        {
            agentes[i] = Instantiate(robots, transform.position, transform.rotation);
            agentes[i].transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);


        }
        for (int i = 0; i < numAgentsRecoger2; i++)
        {
            agentesRecoger[i] = Instantiate(robots_recoger, transform.position, transform.rotation);
            agentesRecoger[i].transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        }

        for (int i = 0; i < 200; i++)
        {
            agentesCaja[i] = Instantiate(cajas, transform.position, transform.rotation);
            agentesCaja[i].transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        }

        Debug.Log("numagents input" + numAgents2 + "rate" + ratePackages2);
        InitializeModel(32, 24, numAgents2, ratePackages2, numAgentsRecoger2);
        StartCoroutine(Todo());

        buttonToDisable.interactable = false;


    }

    // Update is called once per frame
    void Update()
    {
        // Boton de restart

        if (Input.GetKeyDown(KeyCode.R))
        {
            InitializeModel(32, 24, numAgents2, ratePackages2, numAgentsRecoger2);
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            if(MostrarGraficaBool){
                MostrarGrafica.SetActive(false);    
            }

            else{
                MostrarGrafica.SetActive(true);
            }
            

        }
    }


    IEnumerator Todo()
    {
        BarGraph barGraph = GameObject.Find("grafica").GetComponent<BarGraph>();
        while (true)
        {
            yield return new WaitForSeconds(timeStep); // Espera 1 segundo
            stepsfinal += 1;
            
            steps15 += 1;
            if (steps15 == 15){
                steps15 = 0;
            }
            
            Steps.text= ("Steps: " + stepsfinal); //
            barGraph.steps[steps15] = stepsfinal;
            barGraph.bateriaAgentes[steps15]=barGraph.bateriaAgentes2;
            barGraph.paquetesEntregados[steps15] = paquetesEntregadosFinal; //
            StartCoroutine(GetAgentRecoger(baseURL));
            StartCoroutine(GetAgentData(baseURL));
            StartCoroutine(Step(baseURL));
            StartCoroutine(GetAgentDataCaja(baseURL));

        }

    }


    public void InitializeModel(int M, int N, int numAgents2, int ratePackages2, int numAgentsRecoger2)
    {

        string json = "{\"M\": " + M + ", \"N\": " + N + ", \"num_agentes\": " + numAgents2 + ", \"rate_packages\": " + ratePackages2 + ", \"num_agentesRecoger\": " + numAgentsRecoger2 + "}";
        StartCoroutine(PostRequest(baseURL + "/initialize", json));

    }

    IEnumerator Step(string baseURL)
    {

        string url = baseURL + "/step";
        UnityWebRequest request = UnityWebRequest.Get(url);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            //Debug.Log("Received: " + request.downloadHandler.text);

        }
        else
        {
            //Debug.LogError("Error: " + request.error);

        }


    }

    IEnumerator GetAgentDataCaja(string baseURL)
    {
        ConvertInputFieldsToIntegers();
        string url = baseURL + "/caja_data";
        UnityWebRequest request = UnityWebRequest.Get(url);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            // Aquí encapsulamos la lista en una clase contenedora. //CAMBIAR ESTO 
            AgentListCaja receivedData = JsonUtility.FromJson<AgentListCaja>("{\"agentListCaja\":" + request.downloadHandler.text + "}");
            // 

            if (cajasActualesText == null)
            {
                Debug.LogError("cajasActualesText no ha sido asignado");
            }

            if (receivedData == null)
            {
                Debug.LogError("receivedData es null");
            }
            else if (receivedData.agentListCaja == null)
            {
                Debug.LogError("agentListCaja es null");
            }
            else
            {
                cajasActualesText.text = "Cajas Actuales: " + receivedData.agentListCaja.Count.ToString();
            }



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
                sucia[cont] = agent.sucia;

                cont++;
            }


            while (tiempoPasado < tiempoDeAnimacion)
            {

                tiempoPasado += Time.deltaTime;
                float t = tiempoPasado / tiempoDeAnimacion;

                // Interpola suavemente la posición y la rotación

                

                for (int i = 0; i < cont; i++)
                {
                    agentesCaja[i].transform.position = Vector3.Lerp(posicionInicialCaja[i], nuevaPosicionCaja[i], t);
                    agentesCaja[i].transform.rotation = Quaternion.Lerp(agentesCaja[i].transform.rotation, Quaternion.identity, t);
                    if (!nuevaPosicionCaja[i].Equals(new Vector3(23f, 0.4f, 9f)) && !nuevaPosicionCaja[i].Equals(new Vector3(23f, 0.4f, 14f)) && !nuevaPosicionCaja[i].Equals(new Vector3(18f, 0.4f, 5f)) && !nuevaPosicionCaja[i].Equals(new Vector3(17f, 0.4f, 5f)) && !nuevaPosicionCaja[i].Equals(new Vector3(16f, 0.4f, 5f)) && !nuevaPosicionCaja[i].Equals(new Vector3(15f, 0.4f, 5f)) && !nuevaPosicionCaja[i].Equals(new Vector3(14f, 0.4f, 5f)) && !nuevaPosicionCaja[i].Equals(new Vector3(13f, 0.4f, 5f)) && !nuevaPosicionCaja[i].Equals(new Vector3(12f, 0.4f, 5f)) && !nuevaPosicionCaja[i].Equals(new Vector3(11f, 0.4f, 5f)) && !nuevaPosicionCaja[i].Equals(new Vector3(10f, 0.4f, 5f)) && !nuevaPosicionCaja[i].Equals(new Vector3(9f, 0.4f, 5f)) && !nuevaPosicionCaja[i].Equals(new Vector3(8f, 0.4f, 5f))
                    && !nuevaPosicionCaja[i].Equals(new Vector3(18f, 0.4f, 8f)) && !nuevaPosicionCaja[i].Equals(new Vector3(17f, 0.4f, 8f)) && !nuevaPosicionCaja[i].Equals(new Vector3(16f, 0.4f, 8f)) && !nuevaPosicionCaja[i].Equals(new Vector3(15f, 0.4f, 8f)) && !nuevaPosicionCaja[i].Equals(new Vector3(14f, 0.4f, 8f)) && !nuevaPosicionCaja[i].Equals(new Vector3(13f, 0.4f, 8f)) && !nuevaPosicionCaja[i].Equals(new Vector3(12f, 0.4f, 8f)) && !nuevaPosicionCaja[i].Equals(new Vector3(11f, 0.4f, 8f)) && !nuevaPosicionCaja[i].Equals(new Vector3(10f, 0.4f, 8f)) && !nuevaPosicionCaja[i].Equals(new Vector3(9f, 0.4f, 8f)) && !nuevaPosicionCaja[i].Equals(new Vector3(8f, 0.4f, 8f))
                    && !nuevaPosicionCaja[i].Equals(new Vector3(18f, 0.4f, 15f)) && !nuevaPosicionCaja[i].Equals(new Vector3(17f, 0.4f, 15f)) && !nuevaPosicionCaja[i].Equals(new Vector3(16f, 0.4f, 15f)) && !nuevaPosicionCaja[i].Equals(new Vector3(15f, 0.4f, 15f)) && !nuevaPosicionCaja[i].Equals(new Vector3(14f, 0.4f, 15f)) && !nuevaPosicionCaja[i].Equals(new Vector3(13f, 0.4f, 15f)) && !nuevaPosicionCaja[i].Equals(new Vector3(12f, 0.4f, 15f)) && !nuevaPosicionCaja[i].Equals(new Vector3(11f, 0.4f, 15f)) && !nuevaPosicionCaja[i].Equals(new Vector3(10f, 0.4f, 15f)) && !nuevaPosicionCaja[i].Equals(new Vector3(9f, 0.4f, 15f)) && !nuevaPosicionCaja[i].Equals(new Vector3(8f, 0.4f, 15f)) && !nuevaPosicionCaja[i].Equals(new Vector3(18f, 0.4f, 18f)) && !nuevaPosicionCaja[i].Equals(new Vector3(17f, 0.4f, 18f))
                    && !nuevaPosicionCaja[i].Equals(new Vector3(16f, 0.4f, 18f)) && !nuevaPosicionCaja[i].Equals(new Vector3(15f, 0.4f, 18f)) && !nuevaPosicionCaja[i].Equals(new Vector3(14f, 0.4f, 18f)) && !nuevaPosicionCaja[i].Equals(new Vector3(13f, 0.4f, 18f)) && !nuevaPosicionCaja[i].Equals(new Vector3(12f, 0.4f, 18f)) && !nuevaPosicionCaja[i].Equals(new Vector3(11f, 0.4f, 18f)) && !nuevaPosicionCaja[i].Equals(new Vector3(10f, 0.4f, 18f)) && !nuevaPosicionCaja[i].Equals(new Vector3(9f, 0.4f, 18f)) && !nuevaPosicionCaja[i].Equals(new Vector3(8f, 0.4f, 18f)) && !nuevaPosicionCaja[i].Equals(new Vector3(3f, 0.4f, 9f)) && !nuevaPosicionCaja[i].Equals(new Vector3(3f, 0.4f, 14f)) && !nuevaPosicionCaja[i].Equals(new Vector3(24f, 0.4f, 9f)) && !nuevaPosicionCaja[i].Equals(new Vector3(25f, 0.4f, 9f)) && !nuevaPosicionCaja[i].Equals(new Vector3(26f, 0.4f, 9f))&& !nuevaPosicionCaja[i].Equals(new Vector3(27f, 0.4f, 9f))&& !nuevaPosicionCaja[i].Equals(new Vector3(28f, 0.4f, 9f))&& !nuevaPosicionCaja[i].Equals(new Vector3(29f, 0.4f, 9f))
                    && !nuevaPosicionCaja[i].Equals(new Vector3(24f, 0.4f, 14f)) && !nuevaPosicionCaja[i].Equals(new Vector3(25f, 0.4f, 14f)) && !nuevaPosicionCaja[i].Equals(new Vector3(26f, 0.4f, 14f))&& !nuevaPosicionCaja[i].Equals(new Vector3(27f, 0.4f, 14f))&& !nuevaPosicionCaja[i].Equals(new Vector3(28f, 0.4f, 14f))&& !nuevaPosicionCaja[i].Equals(new Vector3(29f, 0.4f, 14f)) && !nuevaPosicionCaja[i].Equals(new Vector3(0f, 0.4f, 9f)) && !nuevaPosicionCaja[i].Equals(new Vector3(1f, 0.4f, 9f)) && !nuevaPosicionCaja[i].Equals(new Vector3(2f, 0.4f, 9f)) && !nuevaPosicionCaja[i].Equals(new Vector3(0f, 0.4f, 14f)) && !nuevaPosicionCaja[i].Equals(new Vector3(1f, 0.4f, 14f)) && !nuevaPosicionCaja[i].Equals(new Vector3(2f, 0.4f, 14f)))
                    {
                        agentesCaja[i].SetActive(false);
                    }
                    else
                    {
                        agentesCaja[i].SetActive(true);
                        
                    }
                }


                // Asegúrate de que el objeto termine exactamente en la posición deseada
                // agentes[cont].transform.position = nuevaPosicion;
            }
        }
        else
        {
            //Debug.LogError("Error: " + request.error);
        }
    }
    IEnumerator GetAgentData(string baseURL)
    {
        ConvertInputFieldsToIntegers();
        BarGraph barGraph = GameObject.Find("grafica").GetComponent<BarGraph>();

        string url = baseURL + "/agent_data";
        UnityWebRequest request = UnityWebRequest.Get(url);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            // Aquí encapsulamos la lista en una clase contenedora. //CAMBIAR ESTO 
            AgentList receivedData = JsonUtility.FromJson<AgentList>("{\"agentList\":" + request.downloadHandler.text + "}");

            if (cantidadAgentes == null)
            {
                Debug.LogError("cajasActualesText no ha sido asignado");
            }
            if (receivedData == null)
            {
                Debug.LogError("receivedData es null");
            }
            else if (receivedData.agentList == null)
            {
                Debug.LogError("agentListCaja es null");
            }
            else
            {
                cantidadAgentes.text = "Agentes mover: " + receivedData.agentList.Count.ToString();
            }

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
                barGraph.bateriaAgentes2++;
                //Debug.Log("ID: " + id + ", X: " + x + ", Y: " + y + ", enCarga: " + enCarga[cont]);

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

                for (int i = 0; i < numAgents2; i++)
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
            //Debug.LogError("Error: " + request.error);
        }

    }





    IEnumerator GetAgentRecoger(string baseURL)
    {
        ConvertInputFieldsToIntegers();
        BarGraph barGraph = GameObject.Find("grafica").GetComponent<BarGraph>();

        string url = baseURL + "/agent_recoger";
        UnityWebRequest request = UnityWebRequest.Get(url);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            // Aquí encapsulamos la lista en una clase contenedora. //CAMBIAR ESTO 
            AgentListRecoger receivedData = JsonUtility.FromJson<AgentListRecoger>("{\"agentListRecoger\":" + request.downloadHandler.text + "}");
            // 

            if (agenteRecogerText == null)
            {
                Debug.LogError("cajasActualesText no ha sido asignado");
            }

            if (receivedData == null)
            {
                Debug.LogError("receivedData es null");
            }
            else if (receivedData.agentListRecoger == null)
            {
                Debug.LogError("agentListCaja es null");
            }
            else
            {
                agenteRecogerText.text = "Agentes Recoger: " + receivedData.agentListRecoger.Count.ToString();
            }

            int cont = 0;
            float tiempoDeAnimacion = timeStep - 0.2f;
            float tiempoPasado = 0f;

            foreach (AgentDataRecoger agent in receivedData.agentListRecoger)
            {
                int x = agent.pos[0];
                int y = agent.pos[1];               //LOGIC EQUIVOCADA
                int id = agent.id; //
                enCargaRecoger[cont] = agent.enCarga;
                barGraph.bateriaAgentes2++;
                paquetesEntregadosCont[cont] = agent.paquetesDespachados;
                
                //Debug.Log("AGENTE RECOGER AAAA ID: " + id + ", X: " + x + ", Y: " + y + ", enCarga: " + enCargaRecoger[cont]);

                // aquí ando pero voy a comer -LG
                nuevaPosicionRecoger[cont] = new Vector3(x, 0f, y);
                posicionInicialRecoger[cont] = agentesRecoger[cont].transform.position;

                cont++;
            }

            while (tiempoPasado < tiempoDeAnimacion)
            {

                tiempoPasado += Time.deltaTime;
                float t = tiempoPasado / tiempoDeAnimacion;

                // Interpola suavemente la posición y la rotación

                paquetesEntregadosFinal = 0;
                for (int i = 0; i < numAgentsRecoger2; i++)
                {
                    paquetesEntregadosFinal += paquetesEntregadosCont[i];
                    agentesRecoger[i].transform.position = Vector3.Lerp(posicionInicialRecoger[i], nuevaPosicionRecoger[i], t);
                    agentesRecoger[i].transform.rotation = Quaternion.Lerp(agentesRecoger[i].transform.rotation, Quaternion.identity, t);
                    GameObject hijoADesactivar = agentesRecoger[i].transform.GetChild(1).gameObject;
                    if (enCargaRecoger[i] == true)
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
            //Debug.LogError("Error: " + request.error);
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
            //Debug.LogError("Error en la solicitud: " + request.error);
        }
        else
        {
            //Debug.Log("Solicitud exitosa. Respuesta: " + request.downloadHandler.text);
            // Puedes procesar la respuesta aquí.
        }
    }

    IEnumerator GetRequest(string url)
    {
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            //Debug.Log("Received: " + request.downloadHandler.text);
        }
        else
        {
            //Debug.Log("Error: " + request.error);
        }
    }
}

