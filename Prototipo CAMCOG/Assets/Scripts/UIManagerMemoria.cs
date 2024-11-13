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

    private float startTime;
    public static List<int> respuestas = new List<int>();
    public static List<float> tiempos = new List<float>();

    private string carpetaDeImages = "Assets/Resources/Images/Prueba2";

    void Start()
    {
        startTime = Time.time;
        ObtenerRutasImagenes(carpetaDeImages);
        LoadNextImage();

        // Asegurarnos de que el texto "Fin Test" esté inicialmente oculto
        finTestText.gameObject.SetActive(false);
    }

    public void QuitApp()
    {
        GameManager.Instance.CloseApp();
    }
    void LoadNextImage()
    {
        // Si estamos en el último grupo de imágenes, mostrar el mensaje "Fin Test"
        if (currentIndex >= imagePaths.Count)
        {
            ShowEndMessage();
            return;
        }

        if (imagePaths != null && imagePaths.Count > 0)
        {
            // Cargar el grupo de tres imágenes
            string imagePath1 = imagePaths[currentIndex][0];
            string imagePath2 = imagePaths[currentIndex][1];
            string imagePath3 = imagePaths[currentIndex][2];

            Sprite sprite1 = Resources.Load<Sprite>(imagePath1);
            Sprite sprite2 = Resources.Load<Sprite>(imagePath2);
            Sprite sprite3 = Resources.Load<Sprite>(imagePath3);

            // Verificar que los sprites se hayan cargado correctamente
            if (sprite1 == null || sprite2 == null || sprite3 == null)
            {
                Debug.LogError("No se pudo cargar una o más imágenes en el grupo.");
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
                indices.RemoveAt(randomIndex);  // Remover el índice para no repetirlo
            }

            // Avanzar al siguiente grupo de imágenes
            currentIndex++;
        }
        else
        {
            Debug.LogError("No se encontraron rutas de imágenes en la lista.");
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

        if (Directory.Exists(directorio))
        {
            string[] extensiones = new[] { "*.png", "*.jpg", "*.jpeg" };
            string[] directorios = Directory.GetDirectories(directorio);

            foreach (string direc in directorios)
            {
                List<string> aux = new List<string>();
                foreach (string extension in extensiones)
                {
                    string[] archivos = Directory.GetFiles(direc, extension, SearchOption.AllDirectories);
                    foreach (string archivo in archivos)
                    {
                        string rutaRelativa = "Images/Prueba2/" + Path.GetRelativePath(directorio, direc) + "/" + Path.GetFileNameWithoutExtension(archivo);
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
