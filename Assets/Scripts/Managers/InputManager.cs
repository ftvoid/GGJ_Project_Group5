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
    [SerializeField]
    private float _swipeThreshold = 0;

    private Subject<Vector2> _onPress = new Subject<Vector2>();
    private Subject<Vector2> _onRelease = new Subject<Vector2>();
    private Subject<Vector2> _onBeginSwipe = new Subject<Vector2>();
    private Subject<Vector2> _onSwiping = new Subject<Vector2>();
    private Subject<Vector2> _onEndSwipe = new Subject<Vector2>();
    private Subject<Vector2> _onBeginPinch = new Subject<Vector2>();
    private Subject<float> _onPinching = new Subject<float>();
    private Subject<Vector2> _onEndPinch = new Subject<Vector2>();

    public static IObservable<Vector2> OnPress => Instance._onPress;
    public static IObservable<Vector2> OnRelease => Instance._onRelease;
    public static IObservable<Vector2> OnBeginSwipe => Instance._onBeginSwipe;
    public static IObservable<Vector2> OnSwiping => Instance._onSwiping;
    public static IObservable<Vector2> OnEndSwipe => Instance._onEndSwipe;
    public static IObservable<Vector2> OnBeginPinch => Instance._onBeginPinch;
    public static IObservable<float> OnPinching => Instance._onPinching;
    public static IObservable<Vector2> OnEndPinch => Instance._onEndPinch;

    public static bool IsPress => Input.GetMouseButton(0);
    public static bool IsDown => Input.GetMouseButtonDown(0);
    public static bool IsUp => Input.GetMouseButtonUp(0);
    public static Vector2 Position => Input.mousePosition;

    protected override void Awake() {
        base.Awake();

        // タップ判定
        this.ObserveEveryValueChanged(_ => IsPress)
            .Subscribe(x => {
                if ( x ) {
                    _onPress.OnNext(Position);
                } else {
                    _onRelease.OnNext(Position);
                }
            });

        // スワイプ判定
        bool isSwiping = false;
        this.ObserveEveryValueChanged(_ => Position)
            .Where(_ => IsPress)
            .Subscribe(_ => {
                if ( isSwiping ) {
                    _onSwiping.OnNext(Position);
                } else {
                    _onBeginSwipe.OnNext(Position);
                    isSwiping = true;
                }
            });
        this.ObserveEveryValueChanged(_ => IsPress)
            .Where(x => !x)
            .Subscribe(_ => {
                if ( isSwiping ) {
                    _onEndSwipe.OnNext(Position);
                    isSwiping = false;
                }
            });

        // ピンチ判定
        // TODO : 2本指判定はあとで実装、今はマウスホイールで疑似的に再現
        bool isPinching = false;

#if UNITY_EDITOR
        float pinchDelta = 0;
        const float pinchSpeed = 1;
        this.UpdateAsObservable()
            .Subscribe(_ => {
                if ( Input.GetMouseButtonDown(2) ) {
                    isPinching = true;
                    pinchDelta = 0;
                    _onBeginPinch.OnNext(Position);
                } else if ( Input.GetMouseButtonUp(2) ) {
                    isPinching = false;
                    _onEndPinch.OnNext(Position);
                }

                if ( isPinching ) {
                    var scroll = Input.GetAxis("Mouse ScrollWheel");
                    if ( scroll > 0 ) {
                        pinchDelta += pinchSpeed;
                        _onPinching.OnNext(pinchDelta);
                    } else if ( scroll < 0 ) {
                        pinchDelta -= pinchSpeed;
                        _onPinching.OnNext(pinchDelta);
                    }
                }
            });
#else
        float beginDist = 0;
        float sensitivity = 0.01f;
        this.UpdateAsObservable()
            .Subscribe(_ => {
                if ( Input.touchCount >= 2 ) {
                    var p1 = Input.touches[0].position;
                    var p2 = Input.touches[1].position;
                    var dist = Vector2.Distance(p1, p2);

                    if ( !isPinching ) {
                        isPinching = true;
                        _onBeginPinch.OnNext(p1);
                        beginDist = dist;
                    } else {
                        _onPinching.OnNext((dist - beginDist) * sensitivity);
                    }
                } else {
                    if ( isPinching ) {
                        isPinching = false;
                        _onEndPinch.OnNext(Input.touches[0].position);
                    }
                }
            });
#endif
    }
}
