using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterScript : MonoBehaviour
{
    public GameObject dialog;
   
    void OnTriggerEnter(Collider other)
    {
        dialog.SetActive(true);
    }

    void OnTriggerExit(Collider other)
    {
        dialog.SetActive(false);
    }

    public void KillSanta()
    {
        Destroy(gameObject);
        Destroy(dialog);
    }

}
