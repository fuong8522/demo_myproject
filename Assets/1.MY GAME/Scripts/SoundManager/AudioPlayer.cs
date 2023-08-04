using System.Collections;
using System.Collections.Generic;
using System.Xml.XPath;
using UnityEngine;
using UnityEngine.Audio;

public class AudioPlayer : MonoBehaviour
{
    private AudioSource playerEffect1;
    public AudioSource playerEffect2;
    public AudioClip basesound;
    public AudioClip head_hit;
    public AudioClip sound_Coin;
    public AudioClip coinCollect;
    public AudioClip ButtonSound;
    public List<AudioClip> clips = new List<AudioClip>();
    void Start()
    {
        playerEffect1= GetComponent<AudioSource>();
    }

    //Âm thanh bước chân.
    void OnFootStep()
    {
        AudioClip step = clips[Random.Range(0,clips.Count)];
        playerEffect2.PlayOneShot(step, 1f);
    }
    //Âm thanh vũ khí 1.
    void SoundBaseBall()
    {
        playerEffect1.PlayOneShot(basesound, 0.5f);
    }
    //Âm thanh coin collect.
    void SoundCollect()
    {
        playerEffect1.PlayOneShot(coinCollect, 1);

    }
    //Âm thanh button.
    public void SoundButton()
    {
        playerEffect1.PlayOneShot(ButtonSound, 1);

    }
    int xPrevious = 1;
    private void Update()
    {
        if(xPrevious < SpawnManager.instance.x)
        {
            playerEffect1.PlayOneShot(sound_Coin, 2f);
        }
            xPrevious = SpawnManager.instance.x;

        if(MovementPlayer.instance.checkCollect)
        {
            SoundCollect();
            MovementPlayer.instance.checkCollect = false;
        }
    }

}
