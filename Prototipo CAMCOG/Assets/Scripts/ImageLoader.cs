using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Para trabajar con la UI
using System.IO;
using UnityEngine.SceneManagement;
using System.Numerics;  

public class ImageLoader : MonoBehaviour
{
    #region Reconocimiento
    public Image imageUI; 
    private List<ImagesLeer> imagePaths;
    private int currentIndex = 0;
    private string carpetaDeImages;
    private int numImages = 0;
    
    struct ImagesLeer
    {
        public string ruta, name;
    }
    private List<bool> mostrados;
    
    #endregion

    #region Memoria
    private List<List<string>> imagePathsMemoria;
    public Button imageUI1;
    public Button imageUI2;
    public Button imageUI3;
    public int numSubcarpetas;
    #endregion

    #region LibroSospechosos
    private List<ImagesLeer> imagePathsLibroSospechosos;
    public Image imageUILibroSospechsos;
    private int currentIndexLibroSospechsos = 0;
    #endregion

    #region PruebaMemoriaSospechosos
    private List<string> imagePathsLPruebaSospechosos;
    public Button sospechoso1;
    public Button sospechoso2;
    public Button sospechoso3;
    public Button sospechoso4;
    public Button sospechoso5;
    public Button sospechoso6;
    public Button sospechoso7;
    public Button sospechoso8;
    public Button sospechoso9;
    public Button sospechoso10;
    private int currentIndexPruebaSospechsos = 0;
    private int numimagesSospechosos = 0;
    #endregion

    //public int roomAct;
    //public int roomNext;

    void Start()
    {
        mostrados = new List<bool>();
        if (!GameManager.Instance.isGame)
        {
            Debug.Log(SceneManager.GetActiveScene().buildIndex);
            if (SceneManager.GetActiveScene().buildIndex == 1)
            {
                
                Debug.Log("entra");
                carpetaDeImages = "Images/Prueba1";
                ObtenerRutasImagenesReconocimiento(carpetaDeImages);
            }
            else if (SceneManager.GetActiveScene().buildIndex == 2)
            {
                carpetaDeImages = "Images/Prueba2";
                ObtenerRutasImagenesMemoria(carpetaDeImages);
            }
            LoadNextImageReconocimiento("");
        }
        else {
            Debug.Log(SceneManager.GetActiveScene().buildIndex);
            if (SceneManager.GetActiveScene().buildIndex == 1)
            {
                carpetaDeImages = "Images/Prompt";
                ObtenerRutasImagenesReconocimiento(carpetaDeImages);
            }
            else if (SceneManager.GetActiveScene().buildIndex == 2)
            {
                carpetaDeImages = "Images/Prompt2";
                ObtenerRutasImagenesMemoria(carpetaDeImages);
                ObtenerRutasImagenesLibroSospechosos("Images/Prompt3Libro");
                ObtenerRutasImagenesPruebaSospechosos("Images/Prompt3Prueba");
            }

        }
        
    }

    //void LoadJson()
    //{
    //    TextAsset jsonFile = Resources.Load<TextAsset>("ConfigEntradaReconocimiento");

    //    if (jsonFile != null)
    //    {
    //        ImageData jsonData = JsonUtility.FromJson<ImageData>(jsonFile.text);
    //        imagePaths = jsonData.images;
    //    }
    //    else
    //    {
    //        Debug.LogError("No se pudo encontrar el archivo JSON.");
    //    }
    //}

    int getIndexImagesPaths(string imageName)
    {
        int i = 0;
        while(i < imagePaths.Count)
        {
            if(imagePaths[i].name == imageName)
            {
                return i;
            }
            i++;
        }

        return -1;
    }
    int getIndexImagesPathsLibroSospechsos(string imageName)
    {
        int i = 0;
        while (i < imagePathsLibroSospechosos.Count)
        {
            if (imagePathsLibroSospechosos[i].name == imageName)
            {
                return i;
            }
            i++;
        }

        return -1;
    }
    int getRandomIndex()
    {
        int rand = Random.Range(0, mostrados.Count);
        if (!mostrados[rand])
        {
            mostrados[rand] = true;
            return rand;
        }
        else
        {
           return getRandomIndex();
        }
    }
    public void LoadNextImageReconocimiento(string name)
    {
        if (GameManager.Instance.pruebasConseguidas == imagePaths.Count)
        {
            if(!GameManager.Instance.isGame)
                GameManager.Instance.ChangeScene(2);
        }
        else { 
           
            if (imagePaths != null && imagePaths.Count > 0)
            {
                if (GameManager.Instance.isGame)
                    currentIndex = getIndexImagesPaths(name);
                else
                {
                    currentIndex = getRandomIndex();   
                }
                if (currentIndex > -1)
                {
                    string imagePath = imagePaths[currentIndex].ruta;
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
                    GameManager.Instance.pruebasConseguidas++;
                }
            }
        }
    }

    public void LoadNextImageMemoria(int numBotones, ref List<GameObject> botones)
    {
        
        if (numImages == imagePathsMemoria.Count)
        {
            //Debug.Log("Salir");
            //Application.Quit();
            GameManager.Instance.prueba2 = true;
            GameManager.Instance.setBackground(GameManager.Instance.roomAct, GameManager.Instance.roomNext, "");
            
        }
        else
        {
            currentIndex = getRandomIndex();
            Debug.Log("LoadNextImage");
            Debug.Log(imagePathsMemoria.Count);
            Debug.Log(imagePathsMemoria[currentIndex].Count);
            if (imagePathsMemoria != null && imagePathsMemoria.Count > 0)
            {
                // Cargar el grupo de tres im?genes

                List<Sprite> sprites = new List<Sprite>();
                for (int i = 0; i < numBotones; i++)
                {
                    sprites.Add(Resources.Load<Sprite>(imagePathsMemoria[currentIndex][i]));
                }

                // Verificar que los sprites se hayan cargado correctamente
                for (int i = 0; i < numBotones; i++)
                {
                    if (sprites[i] == null)
                    {
                        Debug.LogError("No se pudo cargar una o m?s im?genes en el grupo.");
                        return;
                    }
                }

                // Crear una lista de los botones

                // Asignar los sprites a los botones de forma aleatoria
                List<int> indices = new List<int>();

                for (int i = 0; i < numBotones; i++)
                {
                    indices.Add(i);
                }

                for (int i = 0; i < numBotones; i++)
                {
                    
                    int randomIndex = Random.Range(0, indices.Count);
                    botones[i].GetComponent<Button>().image.sprite = sprites[indices[randomIndex]];
                    //Debug.Log(botones[i].GetComponent<Button>().image.sprite);
                    indices.RemoveAt(randomIndex);  // Remover el ?ndice para no repetirlo
                }

                // Avanzar al siguiente grupo de im?genes
                numImages++;
            }
        }

    }

    public void LoadNextImageMemoriaSospechosos()
    {

        if (numimagesSospechosos == imagePathsLPruebaSospechosos.Count)
        {
            GameManager.Instance.prueba2 = true;
            GameManager.Instance.setBackground(GameManager.Instance.roomAct, GameManager.Instance.roomNext, "");
        }
        else
        {

            if (imagePathsLPruebaSospechosos != null && imagePathsLPruebaSospechosos.Count > 0)
            {
                string imagePath = imagePathsLPruebaSospechosos[0];
                string imagePath2 = imagePathsLPruebaSospechosos[1];
                string imagePath3 = imagePathsLPruebaSospechosos[2];
                string imagePath4 = imagePathsLPruebaSospechosos[3];
                string imagePath5 = imagePathsLPruebaSospechosos[4];
                string imagePath6 = imagePathsLPruebaSospechosos[5];
                string imagePath7 = imagePathsLPruebaSospechosos[6];
                string imagePath8 = imagePathsLPruebaSospechosos[7];
                string imagePath9 = imagePathsLPruebaSospechosos[8];
                string imagePath10 = imagePathsLPruebaSospechosos[9];
                Sprite sprite = Resources.Load<Sprite>(imagePath);
                Sprite sprite2 = Resources.Load<Sprite>(imagePath2);
                Sprite sprite3 = Resources.Load<Sprite>(imagePath3);
                Sprite sprite4 = Resources.Load<Sprite>(imagePath4);
                Sprite sprite5 = Resources.Load<Sprite>(imagePath5);
                Sprite sprite6 = Resources.Load<Sprite>(imagePath6);
                Sprite sprite7 = Resources.Load<Sprite>(imagePath7);
                Sprite sprite8 = Resources.Load<Sprite>(imagePath8);
                Sprite sprite9 = Resources.Load<Sprite>(imagePath9);
                Sprite sprite10 = Resources.Load<Sprite>(imagePath10);
                Debug.Log("LoadNextImage3");
                if (sprite != null)
                {
                    sospechoso1.image.sprite = sprite;
                    Debug.Log("image1");
                }
                else
                {
                    Debug.LogError("No se pudo cargar la imagen: " + imagePath);
                }
                if (sprite2 != null)
                {
                    sospechoso2.image.sprite = sprite2;
                    Debug.Log("image2");
                }
                else
                {
                    Debug.LogError("No se pudo cargar la imagen: " + imagePath2);
                }
                if (sprite3 != null)
                {
                    sospechoso3.image.sprite = sprite3;
                    Debug.Log("image3");
                }
                else
                {
                    Debug.LogError("No se pudo cargar la imagen: " + imagePath3);
                }
                if (sprite4 != null)
                {
                    sospechoso4.image.sprite = sprite4;
                    Debug.Log("image4");
                }
                else
                {
                    Debug.LogError("No se pudo cargar la imagen: " + imagePath4);
                }
                if (sprite5 != null)
                {
                    sospechoso5.image.sprite = sprite5;
                    Debug.Log("image5");
                }
                else
                {
                    Debug.LogError("No se pudo cargar la imagen: " + imagePath5);
                }
                if (sprite6 != null)
                {
                    sospechoso6.image.sprite = sprite6;
                    Debug.Log("image6");
                }
                else
                {
                    Debug.LogError("No se pudo cargar la imagen: " + imagePath6);
                }
                if (sprite7 != null)
                {
                    sospechoso7.image.sprite = sprite7;
                    Debug.Log("image7");
                }
                else
                {
                    Debug.LogError("No se pudo cargar la imagen: " + imagePath7);
                }
                if (sprite8 != null)
                {
                    sospechoso8.image.sprite = sprite8;
                    Debug.Log("image8");
                }
                else
                {
                    Debug.LogError("No se pudo cargar la imagen: " + imagePath8);
                }
                if (sprite9 != null)
                {
                    sospechoso9.image.sprite = sprite9;
                    Debug.Log("image9");
                }
                else
                {
                    Debug.LogError("No se pudo cargar la imagen: " + imagePath9);
                }
                if (sprite10 != null)
                {
                    sospechoso10.image.sprite = sprite10;
                    Debug.Log("image10");
                }
                else
                {
                    Debug.LogError("No se pudo cargar la imagen: " + imagePath10);
                }
                numimagesSospechosos++;
            }
        }

    }

    public void LoadNextImageLibroSospechosos(string name)
    {
        Debug.Log("Ha entrado en loadnextimagelibrosospechosos");
        if (imagePathsLibroSospechosos != null && imagePathsLibroSospechosos.Count > 0)
        {
            //currentIndexLibroSospechsos = getIndexImagesPathsLibroSospechsos(name);
            Debug.Log(currentIndexLibroSospechsos);
            if (currentIndexLibroSospechsos > -1)
            {
                Debug.Log("Ha entrado en  if");
                string imagePath = imagePathsLibroSospechosos[currentIndexLibroSospechsos].ruta;
                    Debug.Log("Cargando imagen: " + imagePath); 
                    Sprite sprite = Resources.Load<Sprite>(imagePath);
                    Debug.Log(sprite);
                    if (sprite != null)
                    {
                        imageUILibroSospechsos.sprite = sprite;
                    }
                    else
                    {
                        Debug.LogError("No se pudo cargar la imagen: " + imagePath);
                    }
                    currentIndexLibroSospechsos++;
                }
            }
    }

    void ObtenerRutasImagenesReconocimiento(string directorio)
    {
        imagePaths = new List<ImagesLeer>();

        // Cargar todas las imágenes dentro del directorio especificado en Resources
        Sprite[] imagenes = Resources.LoadAll<Sprite>(directorio);

        if (imagenes.Length > 0)
        {
            foreach (Sprite imagen in imagenes)
            {
                // Convertimos el nombre del archivo en una ruta relativa
                ImagesLeer images = new ImagesLeer();
                images.ruta = directorio + "/" + imagen.name;
                images.name = imagen.name;
                Debug.Log(images.name);
                mostrados.Add(false);
                imagePaths.Add(images);
            }
        }
        else
        {
            Debug.LogError("No se encontraron imágenes en el directorio especificado: " + directorio);
        }
        GameManager.Instance.numPruebas = imagePaths.Count;
        Debug.Log("Pruebas: " + GameManager.Instance.numPruebas);
    }


    void ObtenerRutasImagenesLibroSospechosos(string directorio)
    {
        imagePathsLibroSospechosos = new List<ImagesLeer>();

        // Cargar todas las imágenes dentro del directorio especificado en Resources
        Sprite[] imagenes = Resources.LoadAll<Sprite>(directorio);

        if (imagenes.Length > 0)
        {
            foreach (Sprite imagen in imagenes)
            {
                // Convertimos el nombre del archivo en una ruta relativa
                ImagesLeer images = new ImagesLeer();
                 images.ruta = directorio + "/" + imagen.name;
                 images.name = imagen.name;
                Debug.Log(images.name);
                imagePathsLibroSospechosos.Add(images);
            }
        }
        else
        {
            Debug.LogError("No se encontraron imágenes en el directorio especificado: " + directorio);
        }
    }

    void ObtenerRutasImagenesPruebaSospechosos(string directorio)
    {
        imagePathsLPruebaSospechosos = new List<string>();

        // Cargar todas las imágenes dentro del directorio especificado en Resources
        Sprite[] imagenes = Resources.LoadAll<Sprite>(directorio);

        if (imagenes.Length > 0)
        {
            foreach (Sprite imagen in imagenes)
            {
                // Convertimos el nombre del archivo en una ruta relativa
                string images;
                images = directorio + "/" + imagen.name;
                imagePathsLPruebaSospechosos.Add(images);
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
                mostrados.Add(false);
                imagePathsMemoria.Add(aux);
            }
            else
            {
                Debug.LogError("No se encontraron imágenes en el subdirectorio: " + subdirectorio);
            }
        }
    }

}

