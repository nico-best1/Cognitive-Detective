using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Para trabajar con la UI
using System.IO;      // Para leer archivos

public class ImageLoader : MonoBehaviour
{
    public Image imageUI; 
    private List<string> imagePaths;
    private int currentIndex = 0; 

    [System.Serializable]
    public class ImageData
    {
        public List<string> images;
    }

    void Start()
    {
        LoadJson(); 
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
        if (imagePaths != null && imagePaths.Count > 0)
        {
            string imagePath = imagePaths[currentIndex];
            Debug.Log("Cargando imagen: " + imagePath); // Verificar la ruta
            Sprite sprite = Resources.Load<Sprite>(imagePath);

            if (sprite != null)
            {
                imageUI.sprite = sprite;
            }
            else
            {
                Debug.LogError("No se pudo cargar la imagen: " + imagePath);
            }

            currentIndex = (currentIndex + 1) % imagePaths.Count;
        }
    }

}
