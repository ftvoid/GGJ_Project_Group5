using System.Collections;
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

    private void Start() {
        Damage();
    }

    /// <summary>
    /// ゴミ箱がダメージを受ける
    /// </summary>
    public void Damage() {
        _cover.DOShakePosition(_shakeDuration, 0.2f, 10, 90, false, false);
        _cover.DOShakeRotation(_shakeDuration, 15, 10, 90, false);
    }
}
