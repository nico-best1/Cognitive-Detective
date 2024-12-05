using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueControl : MonoBehaviour
{
    private Queue<string> _colaDialogos;
    private Textos _text;
    [SerializeField] private List<TextMeshProUGUI> _textFields; // Lista de TextMeshProUGUI
    [SerializeField] private float _timeCaracter;
    private Animator _animator;
    private GameObject _player;
    [SerializeField] private Button _button;
    [SerializeField] private GameObject texto1;
    private int _currentTextFieldIndex = 0; // Índice del TextMeshPro actual

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
        // Animación de cartel
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
        foreach (string _saveText in _text._arrayText)
        {
            _colaDialogos.Enqueue(_saveText);
        }
        _currentTextFieldIndex = 0; // Reinicia al primer campo
        NextPhrase();
    }

    public void NextPhrase()
    {
        if (_colaDialogos.Count == 0)
        {
            CloseMessage();
            ClearAllTextFields();
            return;
        }

        string _targetPhrase = _colaDialogos.Dequeue();
        StartCoroutine(ShowCharacters(_targetPhrase));
    }

    IEnumerator ShowCharacters(string showText)
    {
        _textFields[_currentTextFieldIndex].text = "";
        Debug.Log(showText);
        int char_index = 0;
        string auxText = showText;
        // Asegurarse de que haya un campo disponible
        while (_currentTextFieldIndex < _textFields.Count && char_index < auxText.Length)
        {
            var currentField = _textFields[_currentTextFieldIndex];
            _button.interactable = false;

            foreach (char caracter in showText.ToCharArray())
            {

                if (IsTextFieldFull(currentField))
                {
                    _currentTextFieldIndex++;
                    if(char_index < showText.Length)
                        showText = showText.Substring(char_index);
                    break; // Pasa al siguiente TextMeshProUGUI
                }
                currentField.text += caracter;
                char_index++;
                yield return new WaitForSeconds(_timeCaracter);
            }
            _button.interactable = true;
        }

        if (_currentTextFieldIndex >= _textFields.Count)
        {
            Debug.LogWarning("No hay más campos disponibles para mostrar texto.");
            _button.interactable = true;
        }
    }

    private bool IsTextFieldFull(TextMeshProUGUI textField)
    {
        // Verificar si la altura requerida por el texto excede la altura del campo
        return textField.preferredHeight > textField.rectTransform.rect.height;
    }

    private void ClearAllTextFields()
    {
        foreach (var field in _textFields)
        {
            field.text = "";
        }
    }

    public void CloseMessage()
    {
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

    void Start()
    {
        LoadJson();
        _animator = GetComponent<Animator>();
        _colaDialogos = new Queue<string>();
        texto1.GetComponent<ActivateMessages>().ActivateText();
    }

    void Update()
    {
    }
}
