using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ImageFileProcessor : MonoBehaviour
{
    // Directorio de las imágenes y archivo JSON
    public string carpetaDeImages = "Assets/Resources/Images";
    public string archivoJSON = "Assets/Resources/ConfigEntradaReconocimiento.json";

    // Clase para serializar la lista de imágenes
    [System.Serializable]
    public class Imagenes
    {
        public List<string> images;
    }

    void Start()
    {
        CrearJSONConRutas();
    }

    // Método para obtener todas las rutas de archivos PNG, JPG, y JPEG en la carpeta especificada
    List<string> ObtenerRutasImagenes(string directorio)
    {
        List<string> rutasImagenes = new List<string>();

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
                    // Convertimos la ruta en un formato que sea relativo a Resources y sin la extensión del archivo
                    string rutaRelativa = "Images/" + Path.GetFileNameWithoutExtension(archivo);
                    rutasImagenes.Add(rutaRelativa);
                }
            }
        }
        else
        {
            Debug.LogError("El directorio especificado no existe: " + directorio);
        }

        return rutasImagenes;
    }

    // Método para crear o actualizar el archivo JSON con las rutas de las imágenes
    void CrearJSONConRutas()
    {
        List<string> rutasImagenes = ObtenerRutasImagenes(carpetaDeImages);

        if (rutasImagenes.Count > 0)
        {
            // Creamos una instancia de la clase Imagenes y le asignamos las rutas
            Imagenes datos = new Imagenes();
            datos.images = rutasImagenes;

            // Convertimos el objeto a formato JSON
            string json = JsonUtility.ToJson(datos, true);

            // Guardamos el archivo JSON
            File.WriteAllText(archivoJSON, json);

            Debug.Log("Archivo JSON actualizado con las rutas de las imágenes.");
        }
        else
        {
            Debug.LogWarning("No se encontraron archivos PNG, JPG o JPEG en el directorio especificado.");
        }
    }
}
