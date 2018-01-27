using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Increase : MonoBehaviour
{
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
	}
	
	// Update is called once per frame
	void Update ()
    {
        time += Time.deltaTime;
        if (time >= timeout)
        {
            Instantiate(increase, new Vector3(Random.Range(-7.5f,7.5f),Random.Range(-4.0f,4.0f),0) , transform.rotation);
            time = 0.0f;
        }
    }
}
