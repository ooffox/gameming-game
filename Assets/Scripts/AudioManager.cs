using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class AudioManager : MonoBehaviour
{
    private AudioSource AudioSource;
    public static GameObject manager;
    private Coroutine currentPlayer;
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
            SceneManager.sceneLoaded += PlayMusic;
        }
        else
        {
            AudioManager audioManager = manager.GetComponent<AudioManager>();
            audioManager.stageMusic = stageMusic;
            audioManager.stageMusicPrelude = stageMusicPrelude;
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    void PlayMusic(Scene scene, LoadSceneMode mode)
    {
        AudioSource = GetComponent<AudioSource>();
        
        if ((AudioSource.clip == stageMusic && AudioSource.isPlaying) || stageMusic == null)
        {
            return;
        }
        if (currentPlayer != null) {
            StopStageMusic();
            currentPlayer = null;
            }
        if (stageMusicPrelude)
        {
            PlayPreludeMusic();
            currentPlayer = StartCoroutine(PlayStageMusicDelayed(stageMusicPrelude.length));
        }
        else {
            StartCoroutine(PlayStageMusicDelayed());
        }
            

    }


    void PlayPreludeMusic()
    {
        AudioSource.loop = false;
        AudioSource.clip = stageMusicPrelude;
        AudioSource.Play();
    }

    IEnumerator PlayStageMusicDelayed(float _waitTime = 0.0f)
    {
        yield return new WaitForSeconds(_waitTime);
        AudioSource.loop = true;
        AudioSource.clip = stageMusic;
        AudioSource.Play();
    }

    public void StopStageMusic()
    {
        StopCoroutine(currentPlayer);
    }
}
