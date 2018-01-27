using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UniRx;
using System;

public class GameManager : MonoBehaviour {
    [SerializeField]
    private float RemainTime;

    [SerializeField]
    private float HP;

    /// <summary>
    /// ウィルス置き場
    /// </summary>
    public GameObject[] Virus;

    /// <summary>
    /// ウィルス発生場所
    /// </summary>
    public Transform[] VirusTransform;

    /// <summary>
    /// ウィルスが発生しているかどうか
    /// </summary>
    public bool VirusOccurenceDicision;

    /// <summary>
    /// ウィルス発生タイマー
    /// </summary>
    public float VirusRemainTimer;

    /// <summary>
    /// クリアシーン名
    /// </summary>
    public string ClearScene;

    /// <summary>
    /// ゲームオーバーシーン名
    /// </summary>
    public string GameOverScene;

    /// <summary>
    /// VirusのLayer
    /// </summary>
    public LayerMask VirusMask;

    /// <summary>
    /// マウスの箇所取得
    /// </summary>
    private Vector3 PressPointPos = new Vector3(100,100,100);
    void Start ()
    {
        RemainTime = DataManager.Instance.RemainTime;
        HP = DataManager.Instance.HP;
        /*
        InputManager.OnBeginSwipe
            .Take(5)
            .Subscribe(x => {
            Debug.Log("pos = " + x);
        }).AddTo(this);
        */
        VirusRemainTimer = UnityEngine.Random.Range(8, 12);
    }

    void Update()
    {
        PressHitDecision();
        Pinch();
        VirusAttackToPC();
        TimeDegrease();
        
        VirusInstiateTimer();

        /*
        if ()
        {
        81
        }
        */
    }

    public void VirusInstantiate()
    {
        int i = UnityEngine.Random.Range(0, Virus.Length);
        Instantiate(Virus[i], VirusTransform[i].position, Virus[i].transform.rotation);
        VirusRemainTimer = UnityEngine.Random.Range(8, 12);
        VirusOccurenceDicision = true;
        //ウィルスのアニメや音の再生関数
    }

    public void VirusActionEnd()

        {//ウィルスの動きを止める関数呼びだし        
        GameObject[] VirusNum;
        VirusNum = GameObject.FindGameObjectsWithTag("Virus");
        if (VirusNum.Length == 0)
        {
            VirusOccurenceDicision = false;
        }
        
    }

    public void VirusAttackToPC()
    {
        if (VirusOccurenceDicision == true) 
        {
            GameObject[] VirusNum;
            VirusNum = GameObject.FindGameObjectsWithTag("Virus");
            HP -= VirusNum.Length;
            if (HP <=0.0f)
            {
                HPBecomeZero();
            }
        }
    }

    public void HPBecomeZero()
    {
        SceneManager.LoadScene(GameOverScene);
    }

    public void GameClearScene()
    {
        SceneManager.LoadScene(ClearScene);
    }

    public void TimeDegrease()
    {
        RemainTime -= Time.deltaTime;
        if (RemainTime <= 0.0f)
        {
            GameClearScene();
        }
    }

    public void VirusInstiateTimer()
    {
        VirusRemainTimer -= Time.deltaTime;
        if (VirusRemainTimer <=0)
        {
            VirusInstantiate();
        }
    }

    public void PressHitDecision()
    {
        
        InputManager.OnPress
            .Subscribe(x => {
                PressPointPos = Camera.main.ScreenToWorldPoint(x);
                PressPointPos.z = -10;
            }).AddTo(this);        
        Vector3 PressPointLaser = new Vector3(PressPointPos.x, PressPointPos.y, 89);
        Debug.DrawLine(PressPointPos,PressPointLaser,Color.red);
        if (Physics.Raycast(PressPointPos,Vector3.Normalize(PressPointLaser - PressPointPos),Vector3.Distance(PressPointPos,PressPointLaser),VirusMask))
        {
            //Virus消すスクリプト
            VirusOccurenceDicision = false;
        }
    }

    public void ReleaseHitDecision()
    {
        InputManager.OnRelease
            .Subscribe(x => {
                PressPointPos = Camera.main.ScreenToWorldPoint(x);
                PressPointPos.z = -10;
            }).AddTo(this);
        Vector3 PressPointLaser = new Vector3(PressPointPos.x, PressPointPos.y, 89);
        Debug.DrawLine(PressPointPos, PressPointLaser, Color.red);
        if (Physics.Raycast(PressPointPos, Vector3.Normalize(PressPointLaser - PressPointPos), Vector3.Distance(PressPointPos, PressPointLaser), VirusMask))
        {
            //Virus消すスクリプト
            VirusOccurenceDicision = false;
        }
    }

    public void Pinch()
    {
        InputManager.OnPress
            .Subscribe(x => {
                PressPointPos = Camera.main.ScreenToWorldPoint(x);
                PressPointPos.z = -10;
            }).AddTo(this);
        Vector3 PressPointLaser = new Vector3(PressPointPos.x, PressPointPos.y, 89);
        Debug.DrawLine(PressPointPos, PressPointLaser, Color.red);
        RaycastHit hit;
        if (Physics.Raycast(PressPointPos, Vector3.Normalize(PressPointLaser - PressPointPos),out hit, Vector3.Distance(PressPointPos, PressPointLaser), VirusMask))
        {
            Debug.Log("b");
            Scaling SC = hit.collider.gameObject.GetComponent<Scaling>();
            SC.StateChangePinching();
            /*
            InputManager.OnPinching
                .Subscribe(scroll =>{
                   
                    //
            }).AddTo(this);
            */
        }
    }


}
