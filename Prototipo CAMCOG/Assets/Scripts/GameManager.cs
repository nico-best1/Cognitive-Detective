using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public enum GameStates { START, TUTORIAL, LEVEL } //estado pause por contemplar
    private static GameManager _instance;
    public UIManagerGame UI;
    public static GameManager Instance { get { return _instance; } }

    private GameStates _currentState;

    public GameStates getCurrentState() { return _currentState; }

    public bool isGame;

    private void Awake()
    {
        if (_instance == null) _instance = this;
        else Destroy(this);
    }
    // Start is called before the first frame update
    void Start()
    {
        _currentState = GameStates.TUTORIAL;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeState(GameStates new_state)
    {
        _currentState = new_state;
    }

    public void ChangeScene(int scene)
    {

        SceneManager.LoadScene(scene);
    }

    public void setBackground(int nNext, int nActual)
    {
        UI.setBackgrounds(nNext, nActual);
    }
}
