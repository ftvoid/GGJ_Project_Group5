using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bound_ball : MonoBehaviour
{
    Rigidbody2D _rg;
    Transform rote;
    private float time = 0.0f;
    public float Damage = 50.0f;

    [SerializeField]
    [Header("分裂時間")]
    private float timeout = 3.0f;

    [SerializeField]
    [Header("速度")]
    private float speed = 10.0f;

	void Start ()
    {
        _rg = GetComponent<Rigidbody2D>();
        transform.Rotate(new Vector3(0,0,Random.Range(180, 0)));
        _rg.velocity = transform.up * speed;
	}
	
	// Update is called once per frame
	void Update ()
    {

        time += Time.deltaTime;
        if(time >= timeout)
        {

            Instantiate(gameObject, transform.position, transform.transform.rotation);
            time = 0.0f;
        }

	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            GimmickManager.Instance.SoundStart(0);
        }
    }

    public GameObject CloneBound()
    {
        //transform.Rotate(new Vector3(0, 0, Random.Range(180, 0)));
        GameObject bound_Clone = Instantiate(gameObject, transform.position, transform.transform.rotation);
        return bound_Clone;
    }


}
