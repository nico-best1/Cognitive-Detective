using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickManager : MonoBehaviour
{
    
    public void ClickReaction(ObjectData obj)
    {
        Debug.Log("entra");
        GameManager.Instance.setBackground(obj.nNext, obj.nActual);
    }
}
