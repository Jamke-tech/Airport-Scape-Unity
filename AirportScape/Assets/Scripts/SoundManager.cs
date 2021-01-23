using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource effectSource;
    public AudioSource backSource;

    public static SoundManager instance = null;
    public AudioClip clip;



    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    public void PlaySingleSound ()
    {
        effectSource.clip = clip;
        effectSource.Play();
    }




}
