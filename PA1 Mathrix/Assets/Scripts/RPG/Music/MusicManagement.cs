using UnityEngine;
using System.Collections;

public class MusicManagement : MonoBehaviour
{

    public AudioClip[] clips;
    public bool menu;
    private AudioSource musicaSource;
    private AudioSource winSource;

    private int count;
    float timer, MusicaTotal;
    GameObject player;
    //private CurrentPlayer jogadorActivo;
    [Range(0.0f,1.0f)]
    public float volumeInspector;
    
    

    // Use this for initialization
    void Start()
    {
        
        count = 0;

        musicaSource = GameObject.FindGameObjectWithTag("BackgroundMusic").GetComponent<AudioSource>();
        //winSource = GameObject.FindGameObjectWithTag("BackgroundMusic").GetComponent<AudioSource>();
        //winSource.enabled = false;
        //winSource.volume = 0.0f;
        timer = 5;
        musicaSource.enabled = true;
        musicaSource.volume = 0.0f;
        musicaSource.Play();
        musicaSource.loop = false;
        //timer = Time.deltaTime;
       // MusicaTotal = musicaSource.clip.length;
    }

    //// Update is called once per frame
    void Update(){
        if (musicaSource.volume <= volumeInspector)
                {
                    musicaSource.volume += 0.01f;
                }
            if (musicaSource.isPlaying == false && musicaSource != null)
            {
                if (timer <= 0) {
                musicaSource.clip = playMusic();
                musicaSource.Play();
                musicaSource.volume = volumeInspector;
                timer = 1;
                }
                else
                {
                    timer -= Time.deltaTime;
                }
            }
    }


    public void shutDown(bool desligar)
    {

    }

    private AudioClip GetRandomClip()
    {

        return clips[Random.Range(0, clips.Length)];

    }

    private AudioClip playMusic()
    {
        //print("Musica size " + clips.Length + "Count " + count);
        if (count == clips.Length-1)
        {
            count = 0;
        }
        else
        {
            count++;
        }
        return clips[count];
    }

    public AudioClip playWin()
    {
        return clips[count];
    }
}
