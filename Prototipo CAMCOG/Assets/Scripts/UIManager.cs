using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.IO;

public class UIManager : MonoBehaviour
{
    #region Reconocimiento
    public TMP_InputField inputField;
    private string userInput;
    private float startTime;
    private float writingStartTime; // Tiempo de inicio de escritura
    private bool isWriting = false; // Para controlar si ya ha empezado a escribir

    public static List<string> respuestas = new List<string>();
    public static List<float> tiempos = new List<float>();
    public static List<float> tiemposEscritura = new List<float>(); // Tiempo de escritura de cada respuesta
    #endregion

    #region Memoria

    #endregion

    private ImageLoader imageLoader;

    [SerializeField] private GameObject _levelUI;
    [SerializeField] private GameObject _tutorialUI;

    public void LevelActive()
    {
        _tutorialUI.SetActive(false);
        _levelUI.SetActive(true);
    }

    void Start()
    {
        startTime = Time.time;
        inputField.onEndEdit.AddListener(OnInputEnd);
        inputField.onValueChanged.AddListener(OnInputChanged); // Detectar cambios en el texto

        imageLoader = FindObjectOfType<ImageLoader>();
    }

    // Método que se ejecuta cada vez que el inputField cambia (cuando el usuario empieza a escribir)
    void OnInputChanged(string input)
    {
        if (!isWriting)
        {
            writingStartTime = Time.time; // Inicia el temporizador cuando el usuario empieza a escribir
            isWriting = true; // Indica que el usuario ha empezado a escribir
        }
    }

    // Método que se ejecuta cuando el usuario presiona "Enter"
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
            imageLoader.LoadNextImage();
        }
        else
        {
            Debug.LogError("No se encontró el script ImageLoader en la escena.");
        }

        inputField.text = "";
        isWriting = false; 
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
}
