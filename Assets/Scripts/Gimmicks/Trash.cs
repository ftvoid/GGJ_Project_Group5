using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trash : MonoBehaviour {
    public float Damage = 50;

    private void Start() {
        GameManager.Instance.AttackDamage += Damage;
    }

    private void OnDestroy() {
        GimmickManager.Instance.SoundStart(17);
        GameManager.Instance.AttackDamage -= Damage;
    }
}
