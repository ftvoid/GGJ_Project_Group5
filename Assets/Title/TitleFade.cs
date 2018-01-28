using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class TitleFade : MonoBehaviour {

    private AsyncOperation async;
    public GameObject LoadingUi;
   // public Slider Slider;

    public Animator anim;

    public string SceneName;
    

     void Start()
    {
        
    }

    public void LoadNextScene()
    {
        LoadingUi.SetActive(true);
        anim.enabled =true;
    }

    public void AnimEnd()
    {
        StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene()
    {
        async = SceneManager.LoadSceneAsync(SceneName);

        //while (!async.isDone)
        //{
        //    Slider.value = async.progress;
           yield return null;
        //}
    }
}
