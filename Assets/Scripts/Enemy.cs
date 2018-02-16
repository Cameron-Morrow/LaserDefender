using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
	
	public GameObject projectile;
	public float life = 100f;
	public float type = 4.0f;
	public float shotsPerSecond = 0.5f;
	
	void Update () {
		float probability = Time.deltaTime * shotsPerSecond;
		if(Random.value < probability){
			LaunchProjectile();
		}
	}

	void OnTriggerEnter2D (Collider2D col){
		if(col.GetComponent<Projectile>() != null && col.GetComponent<Projectile>().ProjectileType != 4.0f){
			col.GetComponent<Projectile>().Hit(type);
			life = life - col.GetComponent<Projectile>().GetDamage(type);
		}
		
		if (life <= 0) {
			Destroy(gameObject);
		}
	}
	
	void LaunchProjectile() {
		GameObject laser = Instantiate(projectile, transform.position, Quaternion.identity) as GameObject;
		laser.GetComponent<Projectile>().birthDirection = new Vector2(0, -1);
	}
}
