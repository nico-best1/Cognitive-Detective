using TMPro;
using UnityEngine;
using System.Collections;

public class TypewriterEffectTMP : MonoBehaviour
{
    public TMP_InputField tmpText; 
    public string message; 
    public float typingSpeed; 

    private Coroutine typingCoroutine; 

    // M�todo para iniciar la animaci�n
    public void StartTyping()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine); 
        }
        typingCoroutine = StartCoroutine(TypeText());
    }

    // Corrutina para simular el efecto de escritura
    private IEnumerator TypeText()
    {
        tmpText.text = ""; 
        foreach (char letter in message.ToCharArray())
        {
            Debug.Log("Escribiendo");
            tmpText.text += letter; 
            yield return new WaitForSeconds(typingSpeed);
        }
        typingCoroutine = null;
        tmpText.text = "";
    }
}
