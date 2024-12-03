using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundComponent : MonoBehaviour
{
    private Animator Animator;
    private void Start()
    {
        Animator = GetComponent<Animator>();
    }
    public void ActiveIdle()
    {
        Animator.SetBool("active",true);
    }
}
