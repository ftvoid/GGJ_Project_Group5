using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scaling : MonoBehaviour
{
    [SerializeField]
    enum State
    {
        ScaleUp,
        ScaleDown,
        Pinching,
        Stop
    }

    [SerializeField]
    private State _state = State.Stop;

    private Transform myscale;
    private float pinch_size = 1.0f;

    [SerializeField]
    [Header("拡大速度(s)")]
    private int speed = 1;
    [SerializeField]
    [Header("最大size")]
    private int size = 5;

    /// <summary>
    /// ピンチの移動量
    /// </summary>
    public float Size { set { pinch_size = value; } }

    public float Damage;
    void Start()
    {
        myscale = GetComponent<Transform>();
        Dmage();

        GameManager.Instance.AttackDamage += Damage;

    }

    void Update()
    {
        switch (_state)
        {

            case State.Stop:
                
                break;
            case State.ScaleUp:
                myscale.transform.localScale += new Vector3(1, 1, 0) * Time.deltaTime * speed;
                if (myscale.transform.localScale.x >= size)
                {
                    _state = State.Stop;
                }
                break;
            case State.Pinching:
                if (0 != pinch_size)
                {
                    _state = State.ScaleDown;
                }
                break;
            case State.ScaleDown:
                myscale.transform.localScale -= new Vector3(1, 1, 0) * Time.deltaTime * speed;
                if (myscale.transform.localScale.x <= pinch_size)
                {
                    _state = State.Stop;
                    GimmickManager.Instance.SoundStart(16);
                    GameManager.Instance.AttackDamage -= Damage;
                    Destroy(gameObject);
                }
                break;

        }
    }

    public void Dmage()
    {
        _state = State.ScaleUp;
    }

    public void StateChangePinching(float sp)
    {
        myscale.transform.localScale -= new Vector3(1, 1, 0) * sp;
        _state = State.ScaleDown;
    }

}
