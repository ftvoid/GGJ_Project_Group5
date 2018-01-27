using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bound_ball : MonoBehaviour
{
    Rigidbody2D _rg;
    Transform rote;
    float x;
    float y;

	void Start ()
    {
        _rg = GetComponent<Rigidbody2D>();
        transform.Rotate(new Vector3(0,0,Random.Range(180, 0)));
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetMouseButton(0))
        {
            _rg.velocity = transform.up * 20;
            //_rg.AddForce(new Vector3(2,2,0), ForceMode2D.Impulse);
        }
	}

    
}
