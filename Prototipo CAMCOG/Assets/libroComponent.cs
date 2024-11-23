using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class libroComponent : MonoBehaviour
{
    public void OnAnimationEnd()
    {
        if (UIManagerGame.Instance != null)
        {
            UIManagerGame.Instance.OnAnimationEnd();
        }
        else
        {
            Debug.LogError("No se encontró una instancia de UIManagerGame.");
        }
    }
}
