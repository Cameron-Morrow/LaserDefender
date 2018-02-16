using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
	
	public float ProjectileType;
	public float damage = 10;
	public float projectileSpeed = 25f;
	public Vector2 birthDirection;

	// Use this for initialization
	void Start () {
		//birthDirection = new Vector2(0 , 1);
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate(birthDirection * Time.deltaTime * projectileSpeed);
		Destroy(gameObject, 2.0f);
	}
	
	public float Hit (float type) {
		Destroy(gameObject);
		if(gameObject == null){
			return 1;
		}else{
			return 0;
		}
	}
	
	public float GetDamage (float type) {
		return damage;
	}
}
