using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.IO;

public class UIManager : MonoBehaviour
{
    public TMP_InputField inputField; 
    private string userInput; 
    private float startTime; 

    public static List<string> respuestas = new List<string>();
    public static List<float> tiempos = new List<float>();

    private ImageLoader imageLoader;

    void Start()
    {
        startTime = Time.time;
        inputField.onEndEdit.AddListener(OnInputEnd);

        imageLoader = FindObjectOfType<ImageLoader>();
    }

    void OnInputEnd(string input)
    {
        userInput = input;
        float responseTime = Time.time - startTime;

        respuestas.Add(userInput);
        tiempos.Add(responseTime);

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
    }

    void ShowResults()
    {
        for (int i = 0; i < respuestas.Count; i++)
        {
            Debug.Log("Respuesta " + (i + 1) + ": " + respuestas[i] + ", Tiempo: " + tiempos[i] + " segundos.");
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
                writer.WriteLine("Respuesta " + (i + 1) + ": " + respuestas[i] + ", Tiempo: " + tiempos[i] + " segundos.");
            }
            writer.WriteLine();
        }
    }
}
