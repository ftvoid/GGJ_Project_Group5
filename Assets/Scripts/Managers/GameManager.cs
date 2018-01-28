using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
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
    /// TrashのLayer
    /// </summary>
    public LayerMask TrashMask;

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

    public IObservable<float> VirusMemoryDamage => VirusMemoryAttackDamage;

    public float AttackDamage
    {
        get { return VirusMemoryAttackDamage.Value; }
        set { VirusMemoryAttackDamage.Value = value; }
    }

    /// <summary>
    /// 悪いメールのウィルスブーストの時間
    /// </summary>
    public float BadMailRemainTimer;

    /// <summary>
    /// 悪いメールのウィルス状態
    /// </summary>
    public bool BadMailNow;

    /// <summary>
    /// 体力の最大値
    /// </summary>
    public float MaxHP;

    /// <summary>
    /// メールの待機時間
    /// </summary>
    public float MailCanTapTime;

    /// <summary>
    /// メールの表示状態
    /// </summary>
    public bool MailComeFlag;

    /// <summary>
    /// BackGroundの色
    /// </summary>
    public Image BackGround;

    /// <summary>
    /// メールのLayer
    ///</summary>>
    public LayerMask GoodMailLayerMask;
    public LayerMask BadMailLayerMask;

    public Animator GoodMailAnim;

    public Animator BadMailAnim;

    GlitchFx GF;
    void Start()
    {

        GameOverScene.SetActive(false);
        MailComeFlag = false;
        RemainTime = DataManager.Instance.RemainTime;
        HP = DataManager.Instance.HP;
        MaxHP = DataManager.Instance.HP;
        /*
        InputManager.OnBeginSwipe
            .Take(5)
            .Subscribe(x => {
            Debug.Log("pos = " + x);
        }).AddTo(this);
        */
        VirusRemainTimer = UnityEngine.Random.Range(8, 12);

        EmailRemainTime = UnityEngine.Random.Range(30, 40);

        BadMailNow = false;
        GF = GetComponent<GlitchFx>();
        GF.intensity = 0;

        InputManager.OnPress.Subscribe(_ => GimmickManager.Instance.SoundStart(13)).AddTo(this);

        PressHitDecision();

        GoodMailAnim.SetBool("MailAinmBool", true);
        BadMailAnim.SetBool("MailAinmBool", true);
    }

    void Update()
    {


        if (HP >= 0)
        {
            //PressHitDecision();
            Pinch();

            TimeDegrease();

            VirusInstiateTimer();

            BadMailTimer();

            EmailAppend();

            if (MailComeFlag == true)
            {
                MailAppearTimer();
            }
            VirusAttackToPC();
            //VirusMemoryAttackDamage.Value = 0;

            /*
            if ()
            {
            81
            }
            */
        }

    }

    public void VirusInstantiate()
    {
        int i = UnityEngine.Random.Range(0, Virus.Length);
        Instantiate(Virus[i], VirusTransform[i].position, Virus[i].transform.rotation);
        if (BadMailNow == true) VirusRemainTimer = UnityEngine.Random.Range(4, 6);
        else
        {
            VirusRemainTimer = UnityEngine.Random.Range(8, 12);
        }
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
            HP = MaxHP - VirusMemoryAttackDamage.Value;
            GF.intensity = 1 - HP / MaxHP;
            float ColorGain = (HP / MaxHP); 
            //BackGround.color = new Color(255, ColorGain, ColorGain);
            Camera.main.backgroundColor = new Color(1, ColorGain, ColorGain);
            DataManager.Instance.HP = HP;
            if (HP <= 0.0f)
            {
                HPBecomeZero();
            }
        }
    }

    public void HPBecomeZero()
    {
        GF.intensity = 0;
        GameObject[] VirusNum;
        VirusNum = GameObject.FindGameObjectsWithTag("Virus");
        for (int i = 0; i < VirusNum.Length; i++)
        {
            Destroy(VirusNum[i]);
        }
        GameOverScene.SetActive(true);

    }

    public void GameClearScene()
    {
        GameObject[] VirusNum;
        VirusNum = GameObject.FindGameObjectsWithTag("Virus");
        for (int i = 0; i < VirusNum.Length; i++)
        {
            Destroy(VirusNum[i]);
        }
        ClearScene.SetActive(true);
        
    }

    public void TimeDegrease()
    {
        RemainTime -= Time.deltaTime;
        DataManager.Instance.RemainTime = RemainTime;
        if (RemainTime <= 0.0f)
        {
            GameClearScene();
        }
    }

    public void VirusInstiateTimer()
    {
        VirusRemainTimer -= Time.deltaTime;        
        if (VirusRemainTimer <= 0)
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

                Vector3 PressPointLaser = new Vector3(PressPointPos.x, PressPointPos.y, 89);
                Debug.DrawLine(PressPointPos, PressPointLaser, Color.red);

                Collider2D col = Physics2D.OverlapPoint(PressPointPos, VirusMask);
                if ( col != null ) {
                    //Virus消すスクリプト
                    VirusOccurenceDicision = false;

                    // 爆弾判定
                    var sc = col.GetComponent<Scaling>();
                    if ( sc != null ) {
                        sc.StateChangePinching(0);
                    }

                    var ball = col.GetComponent<Bound_ball>();
                    if ( ball != null ) {
                        Destroy(col.gameObject);
                        return;
                    }

                    var folder = col.GetComponent<Folder_Destroy>();
                    if ( folder != null ) {
                        Destroy(col.gameObject);
                        return;
                    }

                    var increase = col.GetComponent<Increase>();
                    if ( increase != null ) {
                        Destroy(col.gameObject);
                        return;
                    }
                }

                col = Physics2D.OverlapPoint(PressPointPos, TrashMask);
                if ( col != null ) {
                    var trash = col.GetComponent<Trash>();
                    if ( trash != null ) {
                        Destroy(col.gameObject);
                        return;
                    }
                }

                Debug.Log("InputManager.OnPress");
            }).AddTo(this);
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
        float i = 0;
        InputManager.OnPress
            .Subscribe(Scrollbar =>
           {
               PressPointPos = Camera.main.ScreenToWorldPoint(Scrollbar);
               PressPointPos.z = -10;
               //i = Scrollbar;
           }
            ).AddTo(this);

        
        //Vector3 PressPointLaser = new Vector3(PressPointPos.x, PressPointPos.y, 89);
        //Debug.DrawLine(PressPointPos, PressPointLaser, Color.red);
        //RaycastHit hit;
        //Scaling SC = null;
        //if (Physics.Raycast(PressPointPos, Vector3.Normalize(PressPointLaser - PressPointPos), out hit, Vector3.Distance(PressPointPos, PressPointLaser), VirusMask))
        //{
        //    SC = hit.collider.gameObject.GetComponent<Scaling>();
        //}
        //    InputManager.OnPinching
        //    .Subscribe(Scroller =>
        //    {
        //        i = Scroller;                
        //        SC.StateChangePinching(i);
        //    }).AddTo(this);
         
        /*
        InputManager.OnPress
            .Subscribe(x => {
                PressPointPos = Camera.main.ScreenToWorldPoint(x);
                PressPointPos.z = -10;
            }).AddTo(this);
        Vector3 PressPointLaser = new Vector3(PressPointPos.x, PressPointPos.y, 89);
        Debug.DrawLine(PressPointPos, PressPointLaser, Color.red);
        RaycastHit hit;
        if (Physics.Raycast(PressPointPos, Vector3.Normalize(PressPointLaser - PressPointPos), out hit, Vector3.Distance(PressPointPos, PressPointLaser), VirusMask))
        {
            Debug.Log("b");
            Scaling SC = hit.collider.gameObject.GetComponent<Scaling>();
            SC.StateChangePinching();
            
            InputManager.OnPinching
                .Subscribe(scroll =>{
                   
                    //
            }).AddTo(this);
            
        }*/


    }

    public void EmailAppend()
    {
        EmailRemainTime -= Time.deltaTime;
        if (EmailRemainTime <= 0)
        {
            EmailLottery();
            MailComeFlag = true;
            EmailRemainTime = UnityEngine.Random.Range(30, 40);
        }
    }

    public void EmailLottery()
    {
        int i = UnityEngine.Random.Range(0, 100);
        if (i >= 80)
        {
            BadMailAnim.SetBool("MailAinmBool", false);
            MailCanTapTime = 5.0f;
        }
        else if (i < 80)
        {
            GoodMailAnim.SetBool("MailAinmBool", false);
            MailCanTapTime = 5.0f;
        }

    }

    public void MailAppearTimer()
    {
        BadMailAppear();
        GoodMailAppear();

        MailCanTapTime -= Time.deltaTime;
        if (MailCanTapTime <= 0)
        {
            if(MailComeFlag == true)GoodMailAnim.SetBool("MailAinmBool", true);
            MailComeFlag = false;
            
            MailCanTapTime = 5.0f;
        }
    }

    public void GoodMailAppear()
    {
        InputManager.OnPress
            .Subscribe(x => {
                PressPointPos = Camera.main.ScreenToWorldPoint(x);
                PressPointPos.z = -10;
            }).AddTo(this);
        Vector3 PressPointLaser = new Vector3(PressPointPos.x, PressPointPos.y, 0);
        Debug.DrawLine(PressPointPos, PressPointLaser, Color.yellow);
        if (Physics.Raycast(PressPointPos, Vector3.Normalize(PressPointLaser - PressPointPos), Vector3.Distance(PressPointPos, PressPointLaser), GoodMailLayerMask))
        {
            GoodMailAnim.SetBool("MailAinmBool", true);
            GameObject[] VirusNum;
            VirusNum = GameObject.FindGameObjectsWithTag("Virus");
            for (int i = 0; i < VirusNum.Length; i++)
            {
                Destroy(VirusNum[i]);
            }
            VirusRemainTimer = UnityEngine.Random.Range(20, 25);
        }
    }

    public void BadMailAppear()
    {
        InputManager.OnPress
            .Subscribe(x => {
                PressPointPos = Camera.main.ScreenToWorldPoint(x);
                PressPointPos.z = -10;
            }).AddTo(this);
        Vector3 PressPointLaser = new Vector3(PressPointPos.x, PressPointPos.y, 0);
        Debug.DrawLine(PressPointPos, PressPointLaser, Color.yellow);
        if (Physics.Raycast(PressPointPos, Vector3.Normalize(PressPointLaser - PressPointPos), Vector3.Distance(PressPointPos, PressPointLaser), BadMailLayerMask))
        {
            if (BadMailNow == false)
            {
                VirusInstantiate();
                VirusRemainTimer = UnityEngine.Random.Range(4, 6);
                BadMailRemainTimer = 5.0f;
                BadMailNow = true;
                Debug.Log("BagMail");
                BadMailAnim.SetBool("MailAinmBool", true);
            }



        }
    }

    public void BadMailTimer()
    {
        if (BadMailNow == true)
        {
            BadMailRemainTimer -= Time.deltaTime;
            if (BadMailRemainTimer <= 0)
            {
                BadMailNow = false;
            }
        }
        else
        {
            BadMailRemainTimer = 5.0f; ;
        }
    }


}

