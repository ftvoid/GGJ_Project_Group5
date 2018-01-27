using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

/// <summary>
/// タッチ入力管理
/// </summary>
public class InputManager : SingletonMonoBehaviour<InputManager> {
    private Subject<Vector2> _onPress = new Subject<Vector2>();
    private Subject<Vector2> _onRelease = new Subject<Vector2>();

    public static IObservable<Vector2> OnPress => Instance._onPress;
    public static IObservable<Vector2> OnRelease => Instance._onRelease;

    public static bool IsPress => Input.GetMouseButton(0);
    public static bool IsDown => Input.GetMouseButtonDown(0);
    public static bool IsUp => Input.GetMouseButtonUp(0);
    public static Vector2 Position => Input.mousePosition;

    protected override void Awake() {
        base.Awake();

        this.ObserveEveryValueChanged(_ => IsPress)
            .Subscribe(x => {
                if ( x ) {
                    _onPress.OnNext(Position);
                } else {
                    _onRelease.OnNext(Position);
                }
            });
    }
}
