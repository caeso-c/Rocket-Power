using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] float delayTime = 1f;

    [SerializeField] ParticleSystem successParticles;
    [SerializeField] ParticleSystem failureParticles;

    [SerializeField] AudioClip success;
    [SerializeField] AudioClip failure;

    AudioSource audioSource;

    bool isPlaying = false;
    bool isCollisionDisabled = false;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        StartDebugMode();
    }

    private void StartDebugMode()
    {
        if (Input.GetKey(KeyCode.L))
        {
            LoadNextLevel();
        }

        else if (Input.GetKey(KeyCode.C))
        {
            isCollisionDisabled = true; // toggle collision
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(isPlaying || isCollisionDisabled) // if isPlaying equals true (sequence playing) or if isCollisionDisabled is true, then return method as is.
        {
            return; // will not proceed to switch statement
        }
        
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                Debug.Log("This object is friendly.");
                break;
            case "Finish":
                StartFinishSequence();
                break;
            default:
                StartCrashSequence();
                break;
        }
    }

    private void StartFinishSequence()
    {
        isPlaying = true;
        audioSource.Stop(); // stops thrusting noise when pressing space
        audioSource.PlayOneShot(success);
        successParticles.Play();
        GetComponent<PlayerController>().enabled = false; // stops player from moving
        Invoke("LoadNextLevel", delayTime); // invokes method with a delay in seconds
    }

    private void StartCrashSequence()
    {
        isPlaying = true;
        audioSource.Stop(); // stops thrusting noise when pressing space
        audioSource.PlayOneShot(failure);
        failureParticles.Play();
        GetComponent<PlayerController>().enabled = false;
        Invoke("ReloadLevel", delayTime);
    }

    private void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings) // if current scene equals total number of scenes in build settings, then next scene will be the first scene
        {
            nextSceneIndex = 0;
        }
        
        SceneManager.LoadScene(nextSceneIndex);
    }

    private void ReloadLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex; // abstraction
        SceneManager.LoadScene(currentSceneIndex);
    }
}
