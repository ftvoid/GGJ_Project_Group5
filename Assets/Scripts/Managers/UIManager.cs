using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ゲーム中のUI管理
/// </summary>
public class UIManager : SingletonMonoBehaviour<UIManager> {
    [SerializeField]
    private Text _remainTime;

    [SerializeField]
    private Text _hp;

    public float RemainTime {
        set {
            if ( _remainTime == null ) {
                return;
            }
            int intTime = (int)value;
            if ( intTime < 0 ) {
                intTime = 0;
            }
            _remainTime.text = string.Format("{0:00}:{1:00}", intTime / 60, intTime % 60);
        }
    }

    public float HP {
        set {
            if ( _hp == null ) {
                return;
            }
            _hp.text = string.Format("HP : {0}", value);
        }
    }
}
