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

    [System.Serializable]
    public class ImageData
    {
        public List<string> images;
    }
    private List<Textos> imagePaths;
    public void MessageActive(Textos ObjectText)
    {
        //animacion cartel
        Debug.Log("Entro");
        _animator.SetBool("Activado", true);
        _text = ObjectText;

    }
    public Textos selectText(int i)
    {
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
    void LoadJson()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("ConfigEntradaReconocimiento");
        imagePaths = new List<Textos>();
        if (jsonFile != null)
        {
            ImageData jsonData = JsonUtility.FromJson<ImageData>(jsonFile.text);
            Textos text = new Textos();
            text._arrayText = jsonData.images;
            imagePaths.Add(text);
            for (int i = 0; i < imagePaths.Count; i++)
            {
                Debug.Log(imagePaths[i] + " " + i); 
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
        //_player = GameManager.Instance.SetPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
