using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class AudioManager : MonoBehaviour
{
    private AudioSource AudioSource;
    private static GameObject manager;
    public AudioClip stageMusic;
    public AudioClip stageMusicPrelude;
    public AudioClip currentClip;
    
    void Awake()
    {
        if (manager == gameObject)
        {
            return;
        }
        else if (manager == null)
        {
            manager = gameObject;
        }
        else
        {
            manager.GetComponent<AudioManager>().stageMusic = stageMusic;
            manager.GetComponent<AudioManager>().stageMusicPrelude = stageMusicPrelude;
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoad;
        OnSceneLoad(SceneManager.GetActiveScene(), LoadSceneMode.Single);      
    }

    void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded += OnSceneLoad;
        AudioSource = GetComponent<AudioSource>();
        if (AudioSource.clip == stageMusic && AudioSource.isPlaying)
        {
            return;
        }
        if (stageMusic != null)
        {
            AudioSource.clip = stageMusic;
            AudioSource.loop = true;
            if (stageMusicPrelude)
            {
                AudioSource.PlayOneShot(stageMusicPrelude);
                AudioSource.PlayDelayed(stageMusicPrelude.length);
            }
            else {
                AudioSource.Play();
            }
            
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}
