using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateMessages : MonoBehaviour
{
    
    
    [SerializeField] private DialogueControl _dialogueControl;
    [SerializeField] private GameObject _message;
    [SerializeField] private int numText;

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if ((bool)collision.gameObject.GetComponent<MovementComponent>())
    //    {
    //        _player.GetComponent<InputComponent>().enabled = false;
    //        _player.GetComponent<MovementComponent>().Move(0);
    //        _dialogueControl.MessageActive(text);

    //    }
    //}
    public void ActivateText()
    {
        _dialogueControl.MessageActive(_dialogueControl.selectText(numText));
    }
    public void DesactivateText()
    {
        _message.SetActive(false);
    }
    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if ((bool)collision.gameObject.GetComponent<MovementComponent>())
    //    {
    //        _message.SetActive(false);

    //    }
    //}

    // Start is called before the first frame update
    void Start()
    {
        //_player = GameManager.Instance.SetPlayer();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
