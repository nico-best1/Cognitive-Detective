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
    #endregion

    void Start()
    {
        if (!GameManager.Instance.isGame)
        {
            Debug.Log(SceneManager.GetActiveScene().buildIndex);
            if (SceneManager.GetActiveScene().buildIndex == 0)
            {
                Debug.Log("entra");
                carpetaDeImages = "Assets/Resources/Images/Prueba1";
                ObtenerRutasImagenesReconocimiento(carpetaDeImages);
            }
            if (SceneManager.GetActiveScene().buildIndex == 1)
            {
                carpetaDeImages = "Assets/Resources/Images/Prueba2";
                ObtenerRutasImagenesMemoria(carpetaDeImages);
            }
            LoadNextImageReconocimiento();
        }
        else {
            if (SceneManager.GetActiveScene().buildIndex == 0)
            {
                carpetaDeImages = "Assets/Resources/Images/Prompt";
                ObtenerRutasImagenesReconocimiento(carpetaDeImages);
            }
            if (SceneManager.GetActiveScene().buildIndex == 1)
            {
                carpetaDeImages = "Assets/Resources/Images/Prompt2";
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
            if (imagePaths != null && imagePaths.Count > 0)
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
    void ObtenerRutasImagenesMemoria(string directorio)
    {
        imagePathsMemoria = new List<List<string>>();

        if (Directory.Exists(directorio))
        {
            // Extensiones de archivo que queremos buscar
            string[] extensiones = new[] { "*.png", "*.jpg", "*.jpeg" };
            string[] directorios = Directory.GetDirectories(directorio);

            foreach (string direc in directorios)
            {
                List<string> aux = new List<string>();
                foreach (string extension in extensiones)
                {
                    // Obtener los archivos para cada tipo de extensión
                    string[] archivos = Directory.GetFiles(direc, extension, SearchOption.AllDirectories);
                    foreach (string archivo in archivos)
                    {
                        string rutaRelativa = "";
                        if (!GameManager.Instance.isGame)
                        {
                            // Convertimos la ruta en un formato que sea relativo a Resources y sin la extensión del archivo
                            rutaRelativa = "Images/Prueba2/" + Path.GetRelativePath(directorio, direc) + "/" + Path.GetFileNameWithoutExtension(archivo);
                        }
                        else
                        {
                            rutaRelativa = "Images/Prompt2/" + Path.GetRelativePath(directorio, direc) + "/" + Path.GetFileNameWithoutExtension(archivo);
                        }
                        // Convertimos la ruta en un formato que sea relativo a Resources y sin la extensión del archivo
                        aux.Add(rutaRelativa);
                    }
                }
                imagePathsMemoria.Add(aux);
            }

        }
        else
        {
            Debug.LogError("El directorio especificado no existe: " + directorio);
        }
    }
}

