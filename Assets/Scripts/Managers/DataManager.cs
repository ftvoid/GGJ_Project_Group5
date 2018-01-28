using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public class DataManager : SingletonMonoBehaviour<DataManager> {
    [SerializeField]
    private FloatReactiveProperty _remainTime = new FloatReactiveProperty(0);
    [SerializeField]
    private FloatReactiveProperty _hp = new FloatReactiveProperty(0);

    public IObservable<float> RemainTimeReactiveProperty => _remainTime;
    public IObservable<float> HPReactiveProperty => _hp;

    /// <summary>
    /// 残り時間(s)
    /// </summary>
    public float RemainTime {
        get { return _remainTime.Value; }
        set { _remainTime.Value = value; }
    }

    /// <summary>
    /// HP
    /// </summary>
    public float HP {
        get { return _hp.Value; }
        set { _hp.Value = value; }
    }
}
