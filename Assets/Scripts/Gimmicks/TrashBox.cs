﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TrashBox : MonoBehaviour {
    [SerializeField]
    [Header("ゴミ箱の蓋")]
    private Transform _cover;

    [SerializeField]
    [Header("ゴミ箱の本体")]
    private Transform _body;

    [SerializeField]
    [Header("ふたが震える時間(s)")]
    private float _shakeDuration = 5;

    private Tweener _shakePos;
    private Tweener _shakeRot;

    public bool _isDamage;
    [SerializeField]
    [Header("ゴミの出現頻度(s)")]
    private float _coolTime = 1.0f;

    private void Start() {
        Damage();
        //StopShake();
        StartCoroutine("InstanceGarbage");
    }

    private void Update()
    {
        
    }

    /// <summary>
    /// ゴミ箱がダメージを受ける
    /// </summary>
    public void Damage() {
        _shakePos = _cover.DOShakePosition(_shakeDuration, 0.2f, 10, 90, false, false);
        _shakeRot = _cover.DOShakeRotation(_shakeDuration, 15, 10, 90, false);
    }

    /// <summary>
    /// ゴミをインスタンスするためのコルーチン
    /// </summary>
    /// <returns></returns>
    private IEnumerator InstanceGarbage() 
    {
        for (int i = 0; i < 5; i++)
        {
            yield return new WaitForSeconds(_coolTime);
            Debug.Log("Instance");
            

        }
    }
    /// <summary>
    /// ゴミ箱の振動を止める
    /// </summary>
    public void StopShake() {
        if ( _shakePos != null ) {
            _shakePos.Complete();
            _shakePos = null;
        }
        if ( _shakeRot != null ) {
            _shakeRot.Complete();
            _shakeRot = null;
        }
    }
}
