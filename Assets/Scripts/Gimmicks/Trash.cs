using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trash : MonoBehaviour {
    public float Damage = 50;

    private void Start() {
        GameManager.Instance.AttackDamage += Damage;
    }

    private void OnDestroy() {
        GameManager.Instance.AttackDamage -= Damage;
    }
}
