using UnityEngine;

public class GameData : MonoBehaviour
{
    private static GameData _instance;
    public static GameData Instance { get { return _instance; } }

    public int puntos;
    public int puntosLimite;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("GameData inicializado correctamente.");
        }
        else
        {
            Debug.LogWarning("Se intentó crear un duplicado de GameData. Este será destruido.");
            Destroy(gameObject);
        }
    }

}
