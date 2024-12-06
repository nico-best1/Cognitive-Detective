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
    private Animator _animatorParent;
    private GameObject _player;
    [SerializeField] private GameObject background;

    [SerializeField] private Button _button;
    [SerializeField] private GameObject texto1;
    private int _currentTextFieldIndex = 0; // Índice del TextMeshPro actual
    [SerializeField] bool canPassScene = false;

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
        // Inicializamos el campo actual
        _textFields[_currentTextFieldIndex].text = "";
        Debug.Log(showText);

        string[] words = showText.Split(' '); // Dividir el texto en palabras
        string currentLine = "";

        foreach (string word in words)
        {
            _button.interactable = false;
            // Intentamos añadir la palabra actual a la línea
            string testLine = currentLine + (currentLine == "" ? "" : " ") + word;
            // Letra por letra en la palabra actual
            foreach (char letter in word)
            {
                _textFields[_currentTextFieldIndex].text += letter;
                yield return new WaitForSeconds(_timeCaracter); // Efecto tecleado
            }
            // Probamos si el campo actual puede contener la nueva línea
            _textFields[_currentTextFieldIndex].text = testLine;
            _textFields[_currentTextFieldIndex].ForceMeshUpdate();

            if (IsTextFieldFullHeight(_textFields[_currentTextFieldIndex]))
            {
                // Si no cabe, confirmamos la línea anterior como el texto final del campo actual
                _textFields[_currentTextFieldIndex].text = currentLine;

                // Avanzamos al siguiente campo
                _currentTextFieldIndex++;
                if (_currentTextFieldIndex >= _textFields.Count)
                {
                    Debug.LogWarning("No hay más campos disponibles para mostrar texto.");
                    yield break; // Salimos si no hay más campos
                }

                // Reiniciamos el campo nuevo y asignamos la palabra actual como nueva línea
                _textFields[_currentTextFieldIndex].text = "";
                currentLine = word; // Solo la palabra actual se transfiere
            }
            else
            {
                // Si cabe, actualizamos la línea actual
                currentLine = testLine;
            }

            

            // Añadimos el espacio después de la palabra
            _textFields[_currentTextFieldIndex].text += " ";
            yield return new WaitForSeconds(_timeCaracter);
        }

        // Al final, aseguramos que el último campo tenga la línea restante
        if (_currentTextFieldIndex < _textFields.Count)
        {
            _textFields[_currentTextFieldIndex].text = currentLine.TrimEnd();
        }

        // Habilitamos el botón al finalizar
        _button.interactable = true;
    }




    private bool IsTextFieldFullHeight(TextMeshProUGUI textField)
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
        if (canPassScene)
            
            _animatorParent.Play("desaparicion");
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
        if(background != null)
            _animatorParent = background.GetComponent<Animator>();
        _colaDialogos = new Queue<string>();
        texto1.GetComponent<ActivateMessages>().ActivateText();
    }

    void Update()
    {
    }
}
