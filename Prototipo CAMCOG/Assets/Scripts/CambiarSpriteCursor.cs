using UnityEngine;
using UnityEngine.UI;

public class CambiarSpriteCursor : MonoBehaviour
{
    public Image cursorImage;
    public Sprite nuevoSprite;
    public float tiempoParaCambio;

    private Sprite spriteOriginal;

    void Start()
    {
        if (cursorImage == null)
        {
            Debug.LogError("El cursorImage no está asignado.");
            return;
        }

        spriteOriginal = cursorImage.sprite;
        Invoke("CambiarSprite", tiempoParaCambio);
    }

    void CambiarSprite()
    {
        cursorImage.sprite = nuevoSprite;
    }
}
