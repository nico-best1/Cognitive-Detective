using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class botonComponent : MonoBehaviour
{
    [SerializeField] int nextBack;
    [SerializeField] int actBack;
    public void OnAnimationEnd()
    {
        if (GameManager.Instance != null)
        {
            switch (GameManager.Instance.opcionMenu)
            {
                case 0:
                    GameManager.Instance.setBackground(nextBack, actBack, "");
                    break;
                case 1:
                    GameManager.Instance.changeSceneMenu(1);
                    break;
                case 2:
                    Debug.Log("Salir Juego");
                    GameManager.Instance.CloseApp();
                    break;
            }
            
        }
        else
        {
            Debug.LogError("No se encontró una instancia de UIManagerGame.");
        }
    }
}
