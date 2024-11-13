using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClickManager : MonoBehaviour
{
    
    public void ClickReaction(ObjectData obj)
    {
        Debug.Log("entra");
        GameManager.Instance.setBackground(obj.nNext, obj.nActual);
    }
    public void GoToAlmacenPruebas()
    {
        GameManager.Instance.ChangeScene(1);
    }
    public void destroyObject(GameObject obj)
    {
        Destroy(obj);
    }
    public void destroyLight(GameObject obj)
    {
        Destroy(obj);
    }
}
