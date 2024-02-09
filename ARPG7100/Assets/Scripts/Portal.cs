using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : Interactable
    {
    bool inHubWorld = false;
    [SerializeField]
    string destination;
    void Start()
        {
        Scene currentScene = SceneManager.GetActiveScene();

        if (currentScene.name == "HubWorld") inHubWorld = true;
        }
    public override void Interact()
        {
        if (inHubWorld) SceneManager.LoadScene(destination);
        else SceneManager.LoadScene("HubWorld");
        }
    public static void SpawnPortal(string scene)
        {
        //destroy any existing portal
        Destroy(GameObject.Find("PortalPrefab"));

        //spawn a new portal
        GameObject obj = Instantiate(Resources.Load("PortalPrefab"), GameObject.FindGameObjectWithTag("Player").transform.position, Quaternion.identity) as GameObject;
        obj.GetComponentInChildren<Portal>().destination = scene;
        }
    }
