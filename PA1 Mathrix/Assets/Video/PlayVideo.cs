using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


[RequireComponent (typeof(AudioSource))]

public class PlayVideo : MonoBehaviour
{

    public MovieTexture movie;
    AudioSource audio;
    float timeLeft = 1.0f;
    private bool once = false;
    private bool isLoading = false;

    void Start()
    {
        GetComponent<RawImage>().texture = movie as MovieTexture;
        audio = GetComponent<AudioSource>();
        audio.clip = movie.audioClip;

    }

    void Update()
    {
        if (once == false) { 
         timeLeft -= Time.deltaTime;
        }

        if (timeLeft < 0 && once == false)
         {
                movie.Play();
                audio.Play();
             once = true;
         }
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return)) && movie.isPlaying && isLoading == false)
         {
             movie.Stop();

             SceneManager.LoadSceneAsync("Rpg");
             isLoading = true;
         }

        if (movie.isPlaying == false && once == true)
        {
            SceneManager.LoadSceneAsync("Rpg");

        }

    }

}

