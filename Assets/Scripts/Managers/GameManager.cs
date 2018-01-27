using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UniRx;
using System;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
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
    /// クリアUI
    /// </summary>
    public GameObject ClearScene;

    /// <summary>
    /// ゲームオーバーUI
    /// </summary>
    public GameObject GameOverScene;

    /// <summary>
    /// VirusのLayer
    /// </summary>
    public LayerMask VirusMask;

    /// <summary>
    /// マウスの箇所取得
    /// </summary>
    /// 
    private Vector3 PressPointPos = new Vector3(100, 100, 100);

    /// <summary>
    /// E-mailの送信までのタイム
    /// </summary>
    [SerializeField]
    private float EmailRemainTime;

    /// <summary>
    /// E-mailの種類
    /// </summary>
    [SerializeField]
    private bool GoodOrBad;

    [SerializeField]
    private ReactiveProperty<float> VirusMemoryAttackDamage = new ReactiveProperty<float>(0);

    public IObservable<float> VirusMemoryDamage=> VirusMemoryAttackDamage;

    public float AttackDamage
    {
        get { return VirusMemoryAttackDamage.Value; }
        set { VirusMemoryAttackDamage.Value = value; }
    }

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

        EmailRemainTime = UnityEngine.Random.Range(30, 40);
    }

    void Update()
    {
        VirusAttackToPC();
        VirusMemoryAttackDamage.Value = 0;
        PressHitDecision();
        Pinch();
        
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
            HP -= VirusMemoryAttackDamage.Value;
            DataManager.Instance.HP = HP;
            if (HP <=0.0f)
            {
                HPBecomeZero();
            }
        }
    }

    public void HPBecomeZero()
    {
        GameOverScene.SetActive(true);
    }

    public void GameClearScene()
    {
        ClearScene.SetActive(true);
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
        DataManager.Instance.RemainTime = VirusRemainTimer;
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

    public void EmailAppend()
    {
        EmailRemainTime -= Time.deltaTime;
        if (EmailRemainTime <=0)
        {
            EmailRemainTime = UnityEngine.Random.Range(30, 40);
        }
    }

    public void EmailLottery()
    {
        int i = UnityEngine.Random.Range(0, 100);
        if (i >= 80)
        {
            BadMailAppear();
        }
        else if (i < 80)
        {
            GoodMailAppear();
        }

    }

    public void GoodMailAppear()
    {

    }

    public void BadMailAppear()
    {

    }


}
