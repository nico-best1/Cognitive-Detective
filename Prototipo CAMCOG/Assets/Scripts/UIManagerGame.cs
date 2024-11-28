using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.IO;
using UnityEngine.SceneManagement;

public class UIManagerGame : MonoBehaviour
{

    [SerializeField] private GameObject _levelUI;
    [SerializeField] private GameObject _tutorialUI;
    [SerializeField] private List<GameObject> _roomsUI;
    [SerializeField] private List<GameObject> _rooms;
    [SerializeField] private List<GameObject> _excepcions;
    [SerializeField] private List<GameObject> _excepcions1;
    
    //private int auxNext = 0;
    //private int auxAct = 0;
    private string auxname;

    #region Reconocimiento
    public TMP_InputField inputField;
    private string userInput;
    private float startTime;
    private float writingStartTime; // Tiempo de inicio de escritura
    private bool isWriting = false; // Para controlar si ya ha empezado a escribir

    public static List<string> respuestas = new List<string>();
    public static List<float> tiempos = new List<float>();
    public static List<float> tiemposEscritura = new List<float>(); // Tiempo de escritura de cada respuesta
    private ImageLoader imageLoader;
    #endregion

    #region Memoria
    public static List<int> respuestasMem = new List<int>();
    public static List<float> tiemposMem = new List<float>();
    #endregion

    [Header("Animacion")]
    [SerializeField] private GameObject libro;
    [SerializeField] private Animator _animatorLibro;
    [SerializeField] private GameObject contenidoLibro;
    [SerializeField] private Button passPagina;
    [SerializeField] private Button cerrarLibro;
    int paginasLibro = 5;

    //[Header("Dialogos")]
    //[SerializeField] private GameObject texto1;
    //[SerializeField] private DialogueControl dialogue;
    //public void LevelActive()
    //{
    //    _tutorialUI.SetActive(false);
    //    _levelUI.SetActive(true);
    //}
    public static UIManagerGame Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    void Start()
    {
        if (inputField != null)
        {
            inputField.onEndEdit.AddListener(OnInputEnd);
            inputField.onValueChanged.AddListener(OnInputChanged); // Detectar cambios en el texto
        }

        imageLoader = FindObjectOfType<ImageLoader>();
        if (GameManager.Instance.isGame)
        {
            if (libro != null) { 
                _animatorLibro = libro.GetComponent<Animator>();
                passPagina = contenidoLibro.GetComponentInChildren<Button>();
            }
            
            //texto1.GetComponent<ActivateMessages>().ActivateText();
        }
    }
    public void setBackgrounds(int nNext, int nActual, string name)
    {
        startTime = Time.time;
        GameManager.Instance.roomAct = nActual;
        GameManager.Instance.roomNext = nNext;
        auxname = name;
        
        _roomsUI[nActual].SetActive(false);
        _roomsUI[nNext].SetActive(true);
        if (_rooms[nActual] != null)
            _rooms[nActual].SetActive(false);
        
        if (_rooms[nNext] != null)
            _rooms[nNext].SetActive(true);
        
        if(nNext == 2)
        {
            if (SceneManager.GetActiveScene().buildIndex == 0)
            {
                imageLoader.LoadNextImageReconocimiento(name);

            }
            else if (SceneManager.GetActiveScene().buildIndex == 1)
            {
                //imageLoader.roomAct = nActual;
                //imageLoader.roomNext = nNext;
                imageLoader.LoadNextImageMemoria();
                

            }

        }
        if (nNext == 3)
        {
            imageLoader.LoadNextImageLibroSospechosos(name);
        }
        if (GameManager.Instance.prueba2)
        {
            for (int i = 0; i < _excepcions.Count; i++) {
                _excepcions[i].SetActive(true);
            }
            GameManager.Instance.prueba2 = false;
        }
        if (GameManager.Instance.prueba3)
        {
            for (int i = 0; i < _excepcions1.Count; i++)
            {
                _excepcions1[i].SetActive(true);
            }
            GameManager.Instance.prueba3 = false;
        }
        
    }

    
    void OnInputChanged(string input)
    {
        if (!isWriting)
        {
            writingStartTime = Time.time; // Inicia el temporizador cuando el usuario empieza a escribir
            isWriting = true; // Indica que el usuario ha empezado a escribir
        }
    }
    void OnInputEnd(string input)
    {
        userInput = input;
        float responseTime = Time.time - startTime;
        float writingTime = Time.time - writingStartTime;

        respuestas.Add(userInput);
        tiempos.Add(responseTime);
        tiemposEscritura.Add(writingTime);

        ShowResults();
        SaveResultsToFile();

        if (imageLoader != null)
        {
            if(!GameManager.Instance.isGame)
                imageLoader.LoadNextImageReconocimiento("");
            else
                setBackgrounds(GameManager.Instance.roomAct, GameManager.Instance.roomNext, auxname);
        }
        else
        {
            Debug.LogError("No se encontró el script ImageLoader en la escena.");
        }

        inputField.text = "";
        isWriting = false;
    }

    public void QuitApp()
    {
        GameManager.Instance.CloseApp();
    }
    void ShowResults()
    {
        for (int i = 0; i < respuestas.Count; i++)
        {
            Debug.Log("Respuesta " + (i + 1) + ": " + respuestas[i] + ", Tiempo total: " + tiempos[i] + " segundos, Tiempo de escritura: " + tiemposEscritura[i] + " segundos.");
        }
    }

    void SaveResultsToFile()
    {
        string filePath = Application.dataPath + "/Resultados.txt";

        using (StreamWriter writer = new StreamWriter(filePath, false))
        {
            writer.WriteLine("Resultados:");
            for (int i = 0; i < respuestas.Count; i++)
            {
                writer.WriteLine("Respuesta " + (i + 1) + ": " + respuestas[i] + ", Tiempo total: " + tiempos[i] + " segundos, Tiempo de escritura: " + tiemposEscritura[i] + " segundos.");
            }
            writer.WriteLine();
        }
    }
    public void SaveResultsToFile(int r)
    {
        float responseTime = Time.time - startTime;

        tiemposMem.Add(responseTime);
        respuestasMem.Add(r);

        string filePath = Application.dataPath + "/ResultadosMemoria.txt";

        using (StreamWriter writer = new StreamWriter(filePath, false))
        {
            writer.WriteLine("Resultados:");
            for (int i = 0; i < respuestasMem.Count; i++)
            {
                writer.WriteLine("Respuesta " + (i + 1) + ": " + respuestasMem[i] + ", Tiempo total: " + tiemposMem[i] + " segundos.");
            }
            writer.WriteLine();
        }

        imageLoader.LoadNextImageMemoria();
    }

    public void PasarPagina()
    {
        if (paginasLibro > 1)
        {
            Debug.Log(_animatorLibro);
            contenidoLibro.SetActive(false);
            _animatorLibro.SetBool("pass", true);
            paginasLibro--;
        }
        if(paginasLibro == 1)
        {
            GameObject buttonGameObject = passPagina.gameObject;
            buttonGameObject.SetActive(false);
            GameObject buttonGameObject1 = cerrarLibro.gameObject;
            buttonGameObject1.SetActive(true);
        }

    }
    public void OnAnimationEnd()
    {
        _animatorLibro.SetBool("pass", false);
        contenidoLibro.SetActive(true);
        imageLoader.LoadNextImageLibroSospechosos("");
        Debug.Log(passPagina);
        passPagina.interactable = true;
        Debug.Log("La animación del libro terminó. Parámetro 'pass' establecido en false.");
    }
}
