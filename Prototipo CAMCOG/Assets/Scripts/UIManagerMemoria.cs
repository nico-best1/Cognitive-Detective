using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.IO;

public class UIManagerMemoria : MonoBehaviour
{
    public Button imageUI;
    public Button imageUI2;
    public Button imageUI3;
    private List<List<string>> imagePaths;
    private int currentIndex = 0;

    private float startTime;
    public static List<int> respuestas = new List<int>();
    public static List<float> tiempos = new List<float>();

    [System.Serializable]
    public class ImageData
    {
        public List<string> basculas;
        public List<string> termometros;
        public List<string> zapatos;
        public List<string> maletas;
        public List<string> lamparas;
        public List<string> portatiles;
    }

    void Start()
    {
        startTime = Time.time;
        LoadJson();
        LoadNextImage();
        
    }

    void LoadJson()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("ConfigEntradaMemoria");
        imagePaths = new List<List<string>>();
        if (jsonFile != null)
        {
            ImageData jsonData = JsonUtility.FromJson<ImageData>(jsonFile.text);
            imagePaths.Add(jsonData.basculas);
            imagePaths.Add(jsonData.termometros);
            imagePaths.Add(jsonData.zapatos);
            imagePaths.Add(jsonData.maletas);
            imagePaths.Add(jsonData.lamparas);
            imagePaths.Add(jsonData.portatiles);
            Debug.Log("LoadJson");
        }
        else
        {
            Debug.LogError("No se pudo encontrar el archivo JSON.");
        }
    }

    void LoadNextImage()
    {
        Debug.Log("LoadNextImage");
        Debug.Log(imagePaths.Count);
        if (imagePaths != null && imagePaths.Count > 0)
        {
            Debug.Log("LoadNextImage2");
            string imagePath = imagePaths[currentIndex][0];
            string imagePath2 = imagePaths[currentIndex][1];
            string imagePath3 = imagePaths[currentIndex][2];
            Sprite sprite = Resources.Load<Sprite>(imagePath);
            Sprite sprite2 = Resources.Load<Sprite>(imagePath2);
            Sprite sprite3 = Resources.Load<Sprite>(imagePath3);
            Debug.Log("LoadNextImage3");
            if (sprite != null)
            {
                imageUI.image.sprite = sprite;
                Debug.Log("image1");
            }
            else
            {
                Debug.LogError("No se pudo cargar la imagen: " + imagePath);
            }
            if (sprite2 != null)
            {
                imageUI2.image.sprite = sprite2;
                Debug.Log("image2");
            }
            else
            {
                Debug.LogError("No se pudo cargar la imagen: " + imagePath2);
            }
            if (sprite3 != null)
            {
                imageUI3.image.sprite = sprite3;
                Debug.Log("image3");
            }
            else
            {
                Debug.LogError("No se pudo cargar la imagen: " + imagePath3);
            }

            currentIndex = (currentIndex + 1) % imagePaths.Count;
        }
    }

    public void SaveResultsToFile(int r)
    {
        float responseTime = Time.time - startTime;

        tiempos.Add(responseTime);
        respuestas.Add(r);

        string filePath = Application.dataPath + "/ResultadosMemoria.txt";

        using (StreamWriter writer = new StreamWriter(filePath, false))
        {
            writer.WriteLine("Resultados:");
            for (int i = 0; i < respuestas.Count; i++)
            {
                writer.WriteLine("Respuesta " + (i + 1) + ": " + respuestas[i] + ", Tiempo total: " + tiempos[i] + " segundos.");
            }
            writer.WriteLine();
        }

        LoadNextImage();
    }
    // Start is called before the first frame update
}
