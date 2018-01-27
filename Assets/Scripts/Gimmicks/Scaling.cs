using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scaling : MonoBehaviour
{

    enum State
    {
        ScaleUp,
        ScaleDown,
        Pinching,
        Stop
    }

    private State _state = State.Stop;

    private Transform myscale;
    private float pinch_size = 0.0f;

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

	void Start ()
    {
        myscale = GetComponent<Transform>();

	}
	
	void Update ()
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
                if(0 != pinch_size)
                {
                    _state = State.ScaleDown;
                }
                break;
            case State.ScaleDown:
                myscale.transform.localScale -= new Vector3(1, 1, 0) * Time.deltaTime * speed;
                if(myscale.transform.localScale.x <= pinch_size)
                {
                    _state = State.Stop;
                }
                break;

        }
    }

    public void Dmage()
    {
        _state = State.ScaleUp;
    }

}
