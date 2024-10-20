using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using System.IO;  

public class SceneController : MonoBehaviour
{
    public TMP_InputField inputField;
    private string nextSceneName;
    public int escena;
    private float startTime;
    private string userInput;

    public static List<string> respuestas = new List<string>();
    public static List<float> tiempos = new List<float>();

    void Start()
    {
        startTime = Time.time;
        inputField.onEndEdit.AddListener(OnInputEnd);
    }

    void OnInputEnd(string input)
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            userInput = input;
            float responseTime = Time.time - startTime;
            respuestas.Add(userInput);
            tiempos.Add(responseTime);
            ShowResults();
            SaveResultsToFile();  
            nextSceneName = escena.ToString();
            escena++;
            SceneManager.LoadScene(nextSceneName);
        }
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
