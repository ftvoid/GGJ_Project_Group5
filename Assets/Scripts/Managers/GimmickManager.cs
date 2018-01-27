using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GimmickManager : SingletonMonoBehaviour<GimmickManager>
{
    private AudioSource[] SEsources = new AudioSource[20];
    public AudioClip[] SE;

    protected override void Awake()
    {
        base.Awake();

        GameObject[] obj = GameObject.FindGameObjectsWithTag("GimmickManager");
        if(obj.Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }

        for(int i = 0; i < SEsources.Length; i++)
        {
            SEsources[i] = gameObject.AddComponent<AudioSource>();
        }

	}

    /// <summary>
    /// animeation を始めるタイミング
    /// </summary>
    public void AnimetionStart()
    {

    }

    /// <summary>
    /// animetionを止めるタイミング
    /// </summary>
    public void AnimetionStop()
    {

    }

    /// <summary>
    /// sound を開始する
    /// </summary>
    public void SoundStart(int number)
    {

        if(0 > number || SE.Length < number)
        {
            return;
        }

        SEsources[number].clip = SE[number];
        SEsources[number].loop = true;
        SEsources[number].Play();
            return;
    }

    /// <summary>
    /// soundを止める
    /// </summary>
    public void SoundStop()
    {
        foreach(AudioSource source in SEsources)
        {
            source.Stop();
            source.clip = null;
        }
    }

    public void Dath()
    {
        Destroy(gameObject);
    }

}
