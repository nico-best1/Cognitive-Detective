using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Para trabajar con la UI
using System.IO;
using UnityEngine.SceneManagement;      // Para leer archivos

public class ImageLoader : MonoBehaviour
{
    #region Reconocimiento
    public Image imageUI; 
    private List<string> imagePaths;
    private int currentIndex = 0;
    private string carpetaDeImages;

    [System.Serializable]
    public class ImageData
    {
        public List<string> images;
    }
    #endregion

    #region Memoria
    private List<List<string>> imagePathsMemoria;
    public Button imageUI1;
    public Button imageUI2;
    public Button imageUI3;
    public int numSubcarpetas;
    #endregion

    void Start()
    {
        if (!GameManager.Instance.isGame)
        {
            Debug.Log(SceneManager.GetActiveScene().buildIndex);
            if (SceneManager.GetActiveScene().buildIndex == 0)
            {
                Debug.Log("entra");
                carpetaDeImages = "Images/Prueba1";
                ObtenerRutasImagenesReconocimiento(carpetaDeImages);
            }
            else if (SceneManager.GetActiveScene().buildIndex == 1)
            {
                carpetaDeImages = "Images/Prueba2";
                ObtenerRutasImagenesMemoria(carpetaDeImages);
            }
            LoadNextImageReconocimiento();
        }
        else {
            Debug.Log(SceneManager.GetActiveScene().buildIndex);
            if (SceneManager.GetActiveScene().buildIndex == 0)
            {
                carpetaDeImages = "Images/Prompt";
                ObtenerRutasImagenesReconocimiento(carpetaDeImages);
            }
            else if (SceneManager.GetActiveScene().buildIndex == 1)
            {
                carpetaDeImages = "Images/Prompt2";
                ObtenerRutasImagenesMemoria(carpetaDeImages);
            }

        }
        
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

    public void LoadNextImageReconocimiento()
    {
        Debug.Log(imagePaths.Count);
        if (currentIndex == imagePaths.Count)
        {
            if(!GameManager.Instance.isGame)
                GameManager.Instance.ChangeScene(1);
        }
        else { 
           
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

    public void LoadNextImageMemoria()
    {
        Debug.Log(currentIndex);
        if (currentIndex == imagePathsMemoria.Count)
        {
            Application.Quit();
        }
        else
        {
            Debug.Log("LoadNextImage");
            Debug.Log(imagePathsMemoria.Count);
            Debug.Log(imagePathsMemoria[currentIndex].Count);
            if (imagePathsMemoria != null && imagePathsMemoria.Count > 0)
            {
                Debug.Log("LoadNextImage2");
                string imagePath = imagePathsMemoria[currentIndex][0];
                string imagePath2 = imagePathsMemoria[currentIndex][1];
                string imagePath3 = imagePathsMemoria[currentIndex][2];
                Sprite sprite = Resources.Load<Sprite>(imagePath);
                Sprite sprite2 = Resources.Load<Sprite>(imagePath2);
                Sprite sprite3 = Resources.Load<Sprite>(imagePath3);
                Debug.Log("LoadNextImage3");
                if (sprite != null)
                {
                    imageUI1.image.sprite = sprite;
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
    
    void ObtenerRutasImagenesReconocimiento(string directorio)
    {
        imagePaths = new List<string>();

        // Cargar todas las imágenes dentro del directorio especificado en Resources
        Sprite[] imagenes = Resources.LoadAll<Sprite>(directorio);

        if (imagenes.Length > 0)
        {
            foreach (Sprite imagen in imagenes)
            {
                // Convertimos el nombre del archivo en una ruta relativa
                string rutaRelativa = directorio + "/" + imagen.name;
                imagePaths.Add(rutaRelativa);
            }
        }
        else
        {
            Debug.LogError("No se encontraron imágenes en el directorio especificado: " + directorio);
        }
    }

    
    void ObtenerRutasImagenesMemoria(string directorio)
    {
        imagePathsMemoria = new List<List<string>>();
        // Aquí asumimos que los subdirectorios están estructurados de manera conocida, 
        // como "Prueba2/SubCarpeta1", "Prueba2/SubCarpeta2", etc.
        for (int i = 0; i < numSubcarpetas; i++) // Por ejemplo, si esperas 3 subcarpetas
        {
            string subdirectorio = directorio + "/" + (i + 1);
            Sprite[] imagenes = Resources.LoadAll<Sprite>(subdirectorio);

            if (imagenes.Length > 0)
            {
                List<string> aux = new List<string>();
                foreach (Sprite imagen in imagenes)
                {
                    string rutaRelativa = subdirectorio + "/" + imagen.name;
                    aux.Add(rutaRelativa);
                }
                imagePathsMemoria.Add(aux);
            }
            else
            {
                Debug.LogError("No se encontraron imágenes en el subdirectorio: " + subdirectorio);
            }
        }
    }

}

