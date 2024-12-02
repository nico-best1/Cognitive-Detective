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

    public int numPruebas = 0;
    public int pruebasConseguidas = 0;
    public bool prueba2 = false;
    public bool prueba3 = false;
    public int roomAct;
    public int roomNext;
    public int opcionMenu;
    private AudioSource _audioSource;
    private void Awake()
    {
        if (_instance == null) _instance = this;
        else Destroy(this);
    }
    // Start is called before the first frame update
    void Start()
    {
        _currentState = GameStates.TUTORIAL;
        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void prueba3start()
    {
        prueba3 = true;
    }
    public void setOpcionMenu(int newopcionMenu)
    {
        opcionMenu = newopcionMenu;
    }
    public void ChangeState(GameStates new_state)
    {
        _currentState = new_state;
    }

    public void ChangeScene(int scene)
    {
        
        if(pruebasConseguidas == numPruebas)
            SceneManager.LoadScene(scene);
        else
        {
            Debug.Log("No tienes las pruebas suficientes");
        }
    }
    public void changeSceneMenu(int scene)
    {
        SceneManager.LoadScene(scene);
    }

    public void setBackground(int nNext, int nActual, string name)
    {
        UI.setBackgrounds(nNext, nActual, name);
    }

    public void PlaySound(AudioClip clip)
    {
        if (clip != null && !_audioSource.isPlaying)
        {
            _audioSource.PlayOneShot(clip);
        }
    }

    public void CloseApp()
    {
        Application.Quit();
    }
}
