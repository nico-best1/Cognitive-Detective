using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.IO;

public class UIManagerGame : MonoBehaviour
{

    [SerializeField] private GameObject _levelUI;
    [SerializeField] private GameObject _tutorialUI;
    [SerializeField] private List<GameObject> _rooms;

    //public void LevelActive()
    //{
    //    _tutorialUI.SetActive(false);
    //    _levelUI.SetActive(true);
    //}

    public void setBackgrounds(int nNext, int nActual)
    {
        _rooms[nActual].SetActive(false);
        _rooms[nNext].SetActive(true);
    }

    void Start()
    {
        
    }



}
