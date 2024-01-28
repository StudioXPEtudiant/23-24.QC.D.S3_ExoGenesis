using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalAction : MonoBehaviour
{
    [SerializeField] private bool isPortalComplete = false;

    private void OnTriggerEnter(Collider other) 
    {
        if(other.tag == "Player" && isPortalComplete)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    public void SetIsPortalComplete(bool _trueOrFalse)
    {
        isPortalComplete = _trueOrFalse;
    }
}


