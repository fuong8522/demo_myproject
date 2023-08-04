using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioEnemy : MonoBehaviour
{

    private float count = 0;
    private AudioSource playerEffect;
    public List<AudioClip> clips = new List<AudioClip>();
    public List<AudioClip> clips2 = new List<AudioClip>();
    private AudioClip attack;
    public ParticleSystem effectblood;
    private EnemyFollow zombie;
    public AudioClip beaten;
    void Start()
    {
        zombie = GetComponent<EnemyFollow>();
        playerEffect = GetComponent<AudioSource>();
        count = 0;
    }


    private void Update()
    {
        count += Time.deltaTime;
    }
    //Âm thanh bước chân.
    void FootStep()
    {
        AudioClip step = clips[Random.Range(0, clips.Count)];
        playerEffect.PlayOneShot(step, 0.5f);
    }
    void SoundAttack()
    {
        AudioClip attack = clips2[Random.Range(0, clips2.Count)];
        playerEffect.PlayOneShot(attack, 0.5f);

    }
    void EffectBlood()
    {
        RestartParticleSystem();
    }
    void RestartParticleSystem()
    {
        effectblood.Play();  // Khởi động lại hiệu ứng
    }
    void CheckStartAnimation()
    {
        zombie.checkAnimationStart = true;
    }
    
    void CheckEndAnimation()
    {
        zombie.checkAnimationStart = false;
    }

    void SoundBeaten()
    {
        playerEffect.PlayOneShot(beaten, 0.7f);
    }


}
