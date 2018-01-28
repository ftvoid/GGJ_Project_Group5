using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Folder_Destroy : MonoBehaviour
{

    public float Damage = 50;

	// Use this for initialization
	void Start ()
    {
        GameManager.Instance.AttackDamage += Damage;
    }

    private void OnDestroy()
    {
        GameManager.Instance.AttackDamage -= Damage;
    }

    // Update is called once per frame
    void Update ()
    {

        //GameObject obj = GetclickObj();
        //if (obj != null)
        //{
        //    Destroy(obj);
        //}

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

}
