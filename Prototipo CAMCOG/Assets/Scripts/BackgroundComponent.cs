using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundComponent : MonoBehaviour
{
    private Animator Animator;
    [SerializeField] int nextBack;
    [SerializeField] int actBack;
    [SerializeField] int nextScene;
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
                case 3:
                    GameManager.Instance.ChangeScene(nextScene);
                    break;
            }

        }
        else
        {
            Debug.LogError("No se encontró una instancia de UIManagerGame.");
        }
    }
    private void Start()
    {
        Animator = GetComponent<Animator>();
    }
    public void ActiveIdle()
    {
        Animator.SetBool("active",true);
    }
    public void activeDesaparicion()
    {
        if(GameManager.Instance.numPruebas == GameManager.Instance.pruebasConseguidas)
            Animator.SetBool("desaparecer", true);
        else
        {
            GameManager.Instance.activeAdvertencia();
        }
    }
    public void activeFinal()
    {
        if (GameManager.Instance.prueba4)
        {
            Animator.SetBool("desaparecer", true);
        }
    }
    
}
