using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;
using TMPro;

public class UIManagerMemoria : MonoBehaviour
{
    public Button imageUI;
    public Button imageUI2;
    public Button imageUI3;
    public TextMeshProUGUI finTestText;
    public Button quitButton;

    private List<List<string>> imagePaths;
    private int currentIndex = 0;
    private int numImages = 0;

    private float startTime;
    public static List<int> respuestas = new List<int>();
    public static List<float> tiempos = new List<float>();

    private string carpetaDeImages = "Images/Prueba2";
    public int numSubcarpetas;
    private List<bool> mostrados;
    void Start()
    {
        mostrados = new List<bool>();
        startTime = Time.time;
        ObtenerRutasImagenes(carpetaDeImages);
        LoadNextImage();

        // Asegurarnos de que el texto "Fin Test" est� inicialmente oculto
        finTestText.gameObject.SetActive(false);
    }

    public void QuitApp()
    {
        GameManager.Instance.CloseApp();
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
    void LoadNextImage()
    {
        // Si estamos en el �ltimo grupo de im�genes, mostrar el mensaje "Fin Test"
        if (numImages >= imagePaths.Count)
        {
            ShowEndMessage();
            return;
        }
        currentIndex = getRandomIndex();
        if (imagePaths != null && imagePaths.Count > 0)
        {
            // Cargar el grupo de tres im�genes
            string imagePath1 = imagePaths[currentIndex][0];
            string imagePath2 = imagePaths[currentIndex][1];
            string imagePath3 = imagePaths[currentIndex][2];

            Sprite sprite1 = Resources.Load<Sprite>(imagePath1);
            Sprite sprite2 = Resources.Load<Sprite>(imagePath2);
            Sprite sprite3 = Resources.Load<Sprite>(imagePath3);

            // Verificar que los sprites se hayan cargado correctamente
            if (sprite1 == null || sprite2 == null || sprite3 == null)
            {
                Debug.LogError("No se pudo cargar una o m�s im�genes en el grupo.");
                return;
            }

            // Crear una lista de los botones
            Button[] buttons = { imageUI, imageUI2, imageUI3 };
            // Crear una lista de los sprites cargados
            Sprite[] sprites = { sprite1, sprite2, sprite3 };

            // Asignar los sprites a los botones de forma aleatoria
            List<int> indices = new List<int> { 0, 1, 2 };
            for (int i = 0; i < buttons.Length; i++)
            {
                int randomIndex = Random.Range(0, indices.Count);
                buttons[i].image.sprite = sprites[indices[randomIndex]];
                indices.RemoveAt(randomIndex);  // Remover el �ndice para no repetirlo
            }

            // Avanzar al siguiente grupo de im�genes
            numImages++;
        }
        else
        {
            Debug.LogError("No se encontraron rutas de im�genes en la lista.");
        }
    }

    void ShowEndMessage()
    {
        // Ocultar los botones
        imageUI.gameObject.SetActive(false);
        imageUI2.gameObject.SetActive(false);
        imageUI3.gameObject.SetActive(false);

        // Mostrar el texto "Fin Test"
        finTestText.gameObject.SetActive(true);
        quitButton.gameObject.SetActive(true);
    }

    void ObtenerRutasImagenes(string directorio)
    {
        imagePaths = new List<List<string>>();
        
        // Aqu� asumimos que los subdirectorios est�n estructurados de manera conocida, 
        // como "Prueba2/SubCarpeta1", "Prueba2/SubCarpeta2", etc.
        for (int i = 0; i < numSubcarpetas; i++) // Por ejemplo, si esperas 3 subcarpetas
        {
            string subdirectorio = directorio + "/" + (i + 1);
            Sprite[] imagenes = Resources.LoadAll<Sprite>(subdirectorio);
            Debug.Log(imagenes.Length);
            if (imagenes.Length > 0)
            {
                List<string> aux = new List<string>();
                foreach (Sprite imagen in imagenes)
                {
                    string rutaRelativa = subdirectorio + "/" + imagen.name;
                    aux.Add(rutaRelativa);
                }
                mostrados.Add(false);
                imagePaths.Add(aux);
            }
            else
            {
                Debug.LogError("No se encontraron im�genes en el subdirectorio: " + subdirectorio);
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
}
