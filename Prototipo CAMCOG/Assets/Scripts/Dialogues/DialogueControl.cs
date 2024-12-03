using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueControl : MonoBehaviour
{
    private Queue <string> _colaDialogos;  
    Textos _text;
    [SerializeField] TextMeshProUGUI _textMeshPro;
    [SerializeField] private float _timeCaracter;
    private Animator _animator;
    private GameObject _player;
    [SerializeField] private Button _button;
    [SerializeField] private GameObject texto1;
    [System.Serializable]
    public class ImageData
    {
        public List<string> images;
    }
    private List<Textos> imagePaths;
    [System.Serializable]
    public class GenericJsonData
    {
        public List<JsonEntry> entries;
    }

    [System.Serializable]
    public class JsonEntry
    {
        public string key;
        public List<string> value;
    }

    public void MessageActive(Textos ObjectText)
    {
        //animacion cartel
        Debug.Log("Entro");
        _animator.SetBool("Activado", true);
        _text = ObjectText;

    }
    public Textos selectText(int i)
    {
        Debug.Log(imagePaths);
        return imagePaths[i];
    }
    public void TextActive()
    {
        _colaDialogos.Clear();
        foreach(string _saveText in _text._arrayText)
        {
            _colaDialogos.Enqueue(_saveText);
        }
        NextPhrase();
    }
    public void NextPhrase()
    {
        if(_colaDialogos.Count == 0)
        {
            CloseMessage();
            _textMeshPro.text = "";
            return;
        }
        string _targetPhrase = _colaDialogos.Dequeue();
        _textMeshPro.text = _targetPhrase;
        StartCoroutine(ShowCharacters(_targetPhrase));

    }
    IEnumerator ShowCharacters(string showText)
    {
        _textMeshPro.text = "";
        foreach(char caracter in showText.ToCharArray())
        {
            _button.interactable = false;
            _textMeshPro.text += caracter;
            yield return new WaitForSeconds(_timeCaracter);
        }
        _button.interactable = true;

    }

    public void CloseMessage()
    {
        //_player.GetComponent<InputComponent>().enabled= true;
        _animator.SetBool("Activado", false);
        _colaDialogos.Clear();
        
    }
    public void ResetAnim()
    {
        _animator.SetTrigger("Empezar");
    }
    public void PassScene()
    {
        GameManager.Instance.ChangeScene(1);
    }
    //public void LoadJson()
    //{
    //    TextAsset jsonFile = Resources.Load<TextAsset>("ConfigEntradaReconocimiento");
    //    imagePaths = new List<Textos>();
    //    Debug.Log(jsonFile);
    //    if (jsonFile != null)
    //    {
    //        //for (int i = 0; i < jsonFile.text.Length; i++)
    //        //{
    //            ImageData jsonData = JsonUtility.FromJson<ImageData>(jsonFile.text);
    //            Textos text = new Textos();
    //            text._arrayText = jsonData.images;
    //            for (int i = 0; i < text._arrayText.Count; i++)
    //            {
    //                Debug.Log(text._arrayText[i]);
    //            }
    //            imagePaths.Add(text);

    //        //}
    //    }
    //    else
    //    {
    //        Debug.LogError("No se pudo encontrar el archivo JSON.");
    //    }
    //}
    public void LoadJson()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("ConfigEntradaReconocimiento");
        imagePaths = new List<Textos>();
        if (jsonFile != null)
        {
            GenericJsonData jsonData = JsonUtility.FromJson<GenericJsonData>(jsonFile.text);

            foreach (var entry in jsonData.entries)
            {
                Debug.Log($"Key: {entry.key}, Values: {string.Join(", ", entry.value)}");

                // Agregar a imagePaths
                Textos text = new Textos { _arrayText = entry.value };
                imagePaths.Add(text);
            }
        }
        else
        {
            Debug.LogError("No se pudo encontrar el archivo JSON.");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        LoadJson();
        _animator = GetComponent<Animator>();
        _colaDialogos = new Queue<string>();
        texto1.GetComponent<ActivateMessages>().ActivateText();
    
    //_player = GameManager.Instance.SetPlayer();
}

    // Update is called once per frame
    void Update()
    {
        
    }
}
