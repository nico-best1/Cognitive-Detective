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
    //private GameObject imageUI4;
    //public Image menu;
    //public GameObject buttonPrefab;
    private List<List<string>> imagePaths;
    private int currentIndex = 0;

    private float startTime;
    public static List<int> respuestas = new List<int>();
    public static List<float> tiempos = new List<float>();

    private string carpetaDeImages = "Assets/Resources/Images/Prueba2";

    //[System.Serializable]
    //public class ImageData
    //{
    //    public List<string> basculas;
    //    public List<string> termometros;
    //    public List<string> zapatos;
    //    public List<string> maletas;
    //    public List<string> lamparas;
    //    public List<string> portatiles;
    //}

    void Start()
    {
        startTime = Time.time;

        //CreateButtons();
        ObtenerRutasImagenes(carpetaDeImages);
        LoadNextImage();
        
    }

    //void CreateButtons()
    //{
    //    imageUI4 = Instantiate(buttonPrefab, menu.transform);

    //    imageUI4.GetComponent<Button>().onClick.AddListener(() => SaveResultsToFile(4));

    //    imageUI4.GetComponent<Transform>().SetLocalPositionAndRotation(new Vector3(500,100,0), Quaternion.identity);
    //}

    void LoadNextImage()
    {
        Debug.Log(currentIndex);
        if(currentIndex == imagePaths.Count) {
            Application.Quit();
        }
        else
        {
            Debug.Log("LoadNextImage");
            Debug.Log(imagePaths.Count);
            Debug.Log(imagePaths[currentIndex].Count);
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

                currentIndex = currentIndex + 1;
            }
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

    void ObtenerRutasImagenes(string directorio)
    {
        imagePaths = new List<List<string>>();

        if (Directory.Exists(directorio))
        {
            // Extensiones de archivo que queremos buscar
            string[] extensiones = new[] { "*.png", "*.jpg", "*.jpeg" };
            string[] directorios = Directory.GetDirectories(directorio);

            foreach(string direc in directorios)
            {
                List<string> aux = new List<string>();
                foreach (string extension in extensiones)
                {
                    // Obtener los archivos para cada tipo de extensión
                    string[] archivos = Directory.GetFiles(direc, extension, SearchOption.AllDirectories);
                    foreach (string archivo in archivos)
                    {
                        // Convertimos la ruta en un formato que sea relativo a Resources y sin la extensión del archivo
                        string rutaRelativa = "Images/Prueba2/" + Path.GetRelativePath(directorio,direc) + "/" + Path.GetFileNameWithoutExtension(archivo);
                        aux.Add(rutaRelativa);
                    }
                }
                imagePaths.Add(aux);
            }
            
        }
        else
        {
            Debug.LogError("El directorio especificado no existe: " + directorio);
        }
    }
}
