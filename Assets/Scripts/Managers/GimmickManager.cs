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
        
        //GameObject[] obj = GameObject.FindGameObjectsWithTag("Virus");
        //if(obj.Length > 1)
        //{
        //    Destroy(gameObject);
        //}
        //else
        //{
        //    DontDestroyOnLoad(gameObject);
        //}

        for(int i = 0; i < SEsources.Length; i++)
        {
            SEsources[i] = gameObject.AddComponent<AudioSource>();
        }
        AudioTuning();

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
        SEsources[number].Play();
            return;
    }

    public void SoundLopeStart(int number)
    {
        if (0 > number || SE.Length < number)
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

    /// <summary>
    /// 死亡
    /// </summary>
    public void Dath()
    {
        Destroy(gameObject);
    }

    private void AudioTuning()
    {
        SEsources[0].volume = 0.355f;
        SEsources[1].volume = 1;
        SEsources[2].volume = 1;
        SEsources[3].volume = 1;
        SEsources[4].volume = 1;
        SEsources[5].volume = 0.1f;
        SEsources[6].volume = 1;
        SEsources[7].volume = 1;
        SEsources[8].volume = 1;
        SEsources[9].volume = 0.5f;
        SEsources[10].volume = 1;
        SEsources[11].volume = 0.8f;
        SEsources[12].volume = 0.5f;
        SEsources[13].volume = 0.5f;
        SEsources[14].volume = 0.15f;
    }
}
