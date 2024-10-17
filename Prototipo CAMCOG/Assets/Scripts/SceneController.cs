using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

public class SceneController : MonoBehaviour
{
    public InputField inputField;
    public string nextSceneName = "2";
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
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
