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
    public Image sospechoso;
    public Button SI;
    public Button NO;
    private int currentIndexPruebaSospechsos = 0;
    private int numimagesSospechosos = 0;
    private List<bool> mostradosSospechocos;
    #endregion

    #region Puntos
    private List<string> spritesBuenos = new List<string>();
    private string spriteCorrecto;
    private List<string> carasBuenas = new List<string>();
    private string caraActual;
    #endregion
    //public int roomAct;
    //public int roomNext;

    void Start()
    {
        mostrados = new List<bool>();
        mostradosSospechocos = new List<bool>();
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

    int getIndexImagesPaths(string imageName )
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
    int getRandomIndex(List<bool> mostrados)
    {
        int rand = Random.Range(0, mostrados.Count);
        if (!mostrados[rand])
        {
            mostrados[rand] = true;
            return rand;
        }
        else
        {
           return getRandomIndex(mostrados);
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
                    currentIndex = getRandomIndex(mostrados);   
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
            currentIndex = getRandomIndex(mostrados);
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
                    if(i == 0)
                    {
                        spriteCorrecto = sprites[0].name;
                    }
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
                    spritesBuenos.Add(sprites[indices[randomIndex]].name);
                    Debug.Log(randomIndex + "ListaNombreSprites: "+sprites[indices[randomIndex]].name);
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
            GameManager.Instance.prueba4 = true;
        }
        else
        {
            currentIndex = getRandomIndex(mostradosSospechocos);
            if (imagePathsLPruebaSospechosos != null && imagePathsLPruebaSospechosos.Count > 0)
            {
                string imagePath = imagePathsLPruebaSospechosos[currentIndex];
                Sprite sprite = Resources.Load<Sprite>(imagePath);

                if (sprite != null)
                {
                    sospechoso.sprite = sprite;
                    caraActual = sprite.name;
                    Debug.Log("imageSospechoso");
                }
                else
                {
                    Debug.LogError("No se pudo cargar la imagen: " + imagePath);
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
                        carasBuenas.Add(sprite.name);
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
                mostradosSospechocos.Add(false);
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
        GameManager.Instance.puntosLimite = GameManager.Instance.puntosLimite + (numSubcarpetas / 2);
        Debug.Log("Puntos Limite: " + GameManager.Instance.puntosLimite);
    }
    public void acierto(int boton)
    {
        for (int i = 0;i < spritesBuenos.Count; i++)
        {
            Debug.Log(i + spritesBuenos[i]);
        }
        Debug.Log("Boton: " + (boton -1) );
        Debug.Log(spriteCorrecto);
        if (spritesBuenos[boton -1] == spriteCorrecto)
        {
            GameManager.Instance.puntos++;
        }
        spritesBuenos.Clear();
    }
    public void aciertoCara()
    {
        for (int i = 0; i < spritesBuenos.Count; i++)
        {
            Debug.Log(i + carasBuenas[i]);
        }
        Debug.Log(caraActual);
        for (int j = 0;j < carasBuenas.Count; j++) {
            if (carasBuenas[j] == caraActual)
            {
                GameManager.Instance.puntos++;
                break;
            }
        }
    }
}

