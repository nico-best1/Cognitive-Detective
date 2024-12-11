using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class roomComponent : MonoBehaviour
{
    [SerializeField] private List<GameObject> luces;
    [SerializeField] private float timeToHelp;
    [SerializeField] private float auxTime;
    bool agregado = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (auxTime >= timeToHelp && !agregado)
        {
            for (int i = 0; i < luces.Count; i++)
            {
                luces[i].AddComponent<LightManager>();
                luces[i].GetComponent<LightManager>().luz = luces[i].GetComponent<Light2D>();
                luces[i].GetComponent<LightManager>().tiempoEntreParpadeos = 0.75f;
            }
            auxTime = 0;
            agregado = true;
        }
        else
        {
            auxTime += Time.deltaTime;
        }
    }
}
