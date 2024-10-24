using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PNGFileProcessor : MonoBehaviour
{
    public string carpetaDeTexturas = "Assets/Texturas";
    public string archivoJSON = "Assets/Config/rutasTexturas.json";

    [System.Serializable]
    public class RutasPNG
    {
        public List<string> rutas;
    }

    void Start()
    {
        CrearJSONConRutas();
    }

    // Método para obtener todas las rutas de los archivos PNG en la carpeta especificada
    List<string> ObtenerRutasPNG(string directorio)
    {
        List<string> rutasPNG = new List<string>();

        if (Directory.Exists(directorio))
        {
            string[] archivosPNG = Directory.GetFiles(directorio, "*.png", SearchOption.AllDirectories);

            foreach (string archivo in archivosPNG)
            {
                string rutaRelativa = "Assets" + archivo.Replace(Application.dataPath, "").Replace("\\", "/");
                rutasPNG.Add(rutaRelativa);
            }
        }
        else
        {
            Debug.LogError("El directorio especificado no existe: " + directorio);
        }

        return rutasPNG;
    }

    // Método para crear o actualizar el archivo JSON con las rutas de los PNG
    void CrearJSONConRutas()
    {
        List<string> rutasPNG = ObtenerRutasPNG(carpetaDeTexturas);

        if (rutasPNG.Count > 0)
        {
            // Creamos una instancia de la clase RutasPNG y le asignamos las rutas
            RutasPNG datos = new RutasPNG();
            datos.rutas = rutasPNG;

            // Convertimos el objeto a formato JSON
            string json = JsonUtility.ToJson(datos, true); // true para que esté bien formateado y sea legible

            // Guardamos el archivo JSON
            File.WriteAllText(archivoJSON, json);

            Debug.Log("Archivo JSON actualizado con las rutas de los PNG.");
        }
        else
        {
            Debug.LogWarning("No se encontraron archivos PNG en el directorio especificado.");
        }
    }
}
