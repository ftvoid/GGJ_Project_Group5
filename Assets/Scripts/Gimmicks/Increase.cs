using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Increase : MonoBehaviour
{
    enum State
    {
        Start,
        Touch,
        Stop
    }

    private State _state = State.Start;

    private float time = 0.0f;
    public float Damage;
    [SerializeField]
    [Header("増殖時間")]
    private float timeout = 3.0f;
    [SerializeField]
    [Header("増殖物")]
    private GameObject increase;

	void Start ()
    {
        //GameManager.Instance.AttackDamage += Damage;
    }
	
	// Update is called once per frame
	void Update ()
    {
        time += Time.deltaTime;

        switch (_state)
        {
            case State.Stop:
                break;
            case State.Start:
                if (time >= timeout)
                {
                    //Instantiate(increase, new Vector3(Random.Range(-7.5f,7.5f),Random.Range(-4.0f,4.0f),0) , transform.rotation);
                    CloneBound();
                    time = 0.0f;
                }
                break;
            case State.Touch:
                TouchStateChange();
                break;


        }
    }

    public void TouchStateChange()
    {
        _state = State.Stop;
    }

    //private GameObject GetclickObj()
    //{
    //    GameObject result = null;
    //    // 左クリックされた場所のオブジェクトを取得
    //    if (InputManager.IsDown)
    //    {
    //        Vector2 tapPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //        Collider2D collider = Physics2D.OverlapPoint(tapPoint);
    //        if (collider)
    //        {
    //            result = collider.transform.gameObject;
    //        }
    //    }
    //    return result;
    //}

    private GameObject CloneBound()
    {
        GameObject clonebound = Instantiate(increase, new Vector3(Random.Range(-7.5f, 7.5f), Random.Range(-4.0f, 4.0f), 0), transform.rotation);
        return clonebound;
    }
}
