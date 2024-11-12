using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Para trabajar con la UI
using System.IO;
using UnityEngine.SceneManagement;      // Para leer archivos

public class ImageLoader : MonoBehaviour
{
    public Image imageUI; 
    private List<string> imagePaths;
    private int currentIndex = 0;
    private string carpetaDeImages;

    [System.Serializable]
    public class ImageData
    {
        public List<string> images;
    }

    void Start()
    {
        if (!GameManager.Instance.isGame)
        {
            carpetaDeImages = "Assets/Resources/Images/Prueba1";
        }
        else {
            carpetaDeImages = "Assets/Resources/Images/Prompt";
        }
        ObtenerRutasImagenes(carpetaDeImages);
        if (!GameManager.Instance.isGame)
            LoadNextImage(); 
    }

    void LoadJson()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("ConfigEntradaReconocimiento");

        if (jsonFile != null)
        {
            ImageData jsonData = JsonUtility.FromJson<ImageData>(jsonFile.text);
            imagePaths = jsonData.images;
        }
        else
        {
            Debug.LogError("No se pudo encontrar el archivo JSON.");
        }
    }

    public void LoadNextImage()
    {
        if (currentIndex == imagePaths.Count)
        {
            if(!GameManager.Instance.isGame)
                GameManager.Instance.ChangeScene(1);
        }
        else { 
            Debug.Log(imagePaths.Count);
            if (imagePaths != null && imagePaths.Count > 0)
            {
                string imagePath = imagePaths[currentIndex];
                Debug.Log("Cargando imagen: " + imagePath); // Verificar la ruta
                Sprite sprite = Resources.Load<Sprite>(imagePath);
                Debug.Log(sprite);
                if (sprite != null)
                {
                    imageUI.sprite = sprite;
                }
                else
                {
                    Debug.LogError("No se pudo cargar la imagen: " + imagePath);
                }

                currentIndex = (currentIndex + 1);
            }
        }
    }

    void ObtenerRutasImagenes(string directorio)
    {
        imagePaths = new List<string>();

        if (Directory.Exists(directorio))
        {
            // Extensiones de archivo que queremos buscar
            string[] extensiones = new[] { "*.png", "*.jpg", "*.jpeg" };

            foreach (string extension in extensiones)
            {
                // Obtener los archivos para cada tipo de extensión
                string[] archivos = Directory.GetFiles(directorio, extension, SearchOption.AllDirectories);
                foreach (string archivo in archivos)
                {
                    string rutaRelativa = "";
                    if (!GameManager.Instance.isGame)
                    {
                        // Convertimos la ruta en un formato que sea relativo a Resources y sin la extensión del archivo
                        rutaRelativa = "Images/Prueba1/" + Path.GetFileNameWithoutExtension(archivo);
                        imagePaths.Add(rutaRelativa);
                    }
                    else
                    {
                        rutaRelativa = "Images/Prompt/" + Path.GetFileNameWithoutExtension(archivo);
                    }
                    imagePaths.Add(rutaRelativa);
                }
            }
        }
        else
        {
            Debug.LogError("El directorio especificado no existe: " + directorio);
        }
    }

}
