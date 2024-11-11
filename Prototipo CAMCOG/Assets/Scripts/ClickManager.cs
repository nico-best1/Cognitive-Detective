using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickManager : MonoBehaviour
{
    [SerializeField] private ObjectData puerta;
    public void ClickReaction()
    {
        GameManager.Instance.setBackground(puerta.nNext, puerta.nActual);
    }
}
