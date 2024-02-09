using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathScreenButton : MonoBehaviour
    {
    public void ReturnToHub()
        {
        SceneManager.LoadScene("HubWorld");
        }
    }