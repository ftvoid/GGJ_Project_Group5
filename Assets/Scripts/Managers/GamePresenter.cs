using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class GamePresenter : MonoBehaviour {
    private void Start() {
        DataManager.Instance.RemainTimeReactiveProperty.Subscribe(x => UIManager.Instance.RemainTime = x).AddTo(this);
        DataManager.Instance.HPReactiveProperty.Subscribe(x => UIManager.Instance.HP = x).AddTo(this);
    }
}
