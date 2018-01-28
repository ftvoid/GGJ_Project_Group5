using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Folder_Destroy : MonoBehaviour
{
    Ray ray;
    Ray mouseRay;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {

        GameObject obj = GetclickObj();
        if (obj != null)
        {
            GameObject.Destroy(this.gameObject);
        }

    }

    private GameObject GetclickObj()
    {
        GameObject result = null;
        // 左クリックされた場所のオブジェクトを取得
        if (InputManager.IsDown)
        {
            Vector2 tapPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D collider = Physics2D.OverlapPoint(tapPoint);
            if (collider)
            {
                result = collider.transform.gameObject;
            }
        }
        return result;
    }

}
