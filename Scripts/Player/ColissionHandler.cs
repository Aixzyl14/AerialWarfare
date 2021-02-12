using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; //ok as long as this is the only script that loads scenes
public class ColissionHandler : MonoBehaviour
{
    [Tooltip("In seconds")][SerializeField] float LvlLoadDelay = 1f;
    [Tooltip("Fx Prefab on Player")][SerializeField] GameObject DeathFx;
    void Start()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        StartDeathSequence();
    }

    private void StartDeathSequence()
    {
        SendMessage("OnPlayerDeath");
        DeathFx.SetActive(true);
        Invoke("ReloadLevel", LvlLoadDelay);

    }

    private void ReloadLevel() //String Referenced
    {
        SceneManager.LoadScene(1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
