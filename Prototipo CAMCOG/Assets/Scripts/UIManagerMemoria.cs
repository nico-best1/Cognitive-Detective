using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.IO;

public class UIManagerMemoria : MonoBehaviour
{
    public Image imageUI;
    public Image imageUI2;
    public Image imageUI3;
    private List<List<string>> imagePaths;
    private int currentIndex = 0;

    [System.Serializable]
    public class ImageData
    {
        public List<string> basculas;
        public List<string> termometros;
        public List<string> zapatos;
        public List<string> maletas;
        public List<string> lamparas;
        public List<string> portatiles;
    }

    void Start()
    {
        LoadJson();
        LoadNextImage();
    }

    void LoadJson()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("ConfigEntradaMemoria");
        imagePaths = new List<List<string>>();
        if (jsonFile != null)
        {
            ImageData jsonData = JsonUtility.FromJson<ImageData>(jsonFile.text);
            imagePaths.Add(jsonData.basculas);
            imagePaths.Add(jsonData.termometros);
            imagePaths.Add(jsonData.zapatos);
            imagePaths.Add(jsonData.maletas);
            imagePaths.Add(jsonData.lamparas);
            imagePaths.Add(jsonData.portatiles);
            Debug.Log("LoadJson");
        }
        else
        {
            Debug.LogError("No se pudo encontrar el archivo JSON.");
        }
    }

    public void LoadNextImage()
    {
        Debug.Log("LoadNextImage");
        Debug.Log(imagePaths.Count);
        if (imagePaths != null && imagePaths.Count > 0)
        {
            Debug.Log("LoadNextImage2");
            string imagePath = imagePaths[currentIndex][0];
            string imagePath2 = imagePaths[currentIndex][1];
            string imagePath3 = imagePaths[currentIndex][2];
            Sprite sprite = Resources.Load<Sprite>(imagePath);
            Sprite sprite2 = Resources.Load<Sprite>(imagePath2);
            Sprite sprite3 = Resources.Load<Sprite>(imagePath3);
            Debug.Log("LoadNextImage3");
            if (sprite != null)
            {
                imageUI.sprite = sprite;
                Debug.Log("image1");
            }
            else
            {
                Debug.LogError("No se pudo cargar la imagen: " + imagePath);
            }
            if (sprite2 != null)
            {
                imageUI2.sprite = sprite2;
                Debug.Log("image2");
            }
            else
            {
                Debug.LogError("No se pudo cargar la imagen: " + imagePath2);
            }
            if (sprite3 != null)
            {
                imageUI3.sprite = sprite3;
                Debug.Log("image3");
            }
            else
            {
                Debug.LogError("No se pudo cargar la imagen: " + imagePath3);
            }

            currentIndex = (currentIndex + 1) % imagePaths.Count;
        }
    }
    // Start is called before the first frame update
}
