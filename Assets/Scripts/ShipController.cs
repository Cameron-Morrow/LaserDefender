using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ShipController : MonoBehaviour {
	
	public GameObject projectile;
	public AudioClip fireLaser;
	public float type = 0.0f;
	public float life = 1000;
	
	int boost;
	int speed = 5;
	float padding = 0.4f;
	float xmin;
	float xmax;
	float ymin;
	float ymax;
	
	void Start () {
		float distance = transform.position.z - Camera.main.transform.position.z;
		Vector3 cameraMin = Camera.main.ViewportToWorldPoint(new Vector3(0,0,distance));
		Vector3 cameraMax = Camera.main.ViewportToWorldPoint(new Vector3(1,1,distance));
		xmin = cameraMin.x + padding;
		xmax = cameraMax.x - padding;
		ymin = cameraMin.y + padding;
		ymax = cameraMax.y - padding;
	}

	void Update () {
		//determine if boost
		if (Input.GetKey(KeyCode.LeftShift)) {
			boost = 2;
		} else {
			boost = 1;
		}
		
		//control power of ship
		if (Input.GetKey(KeyCode.UpArrow)) { //up
			transform.Translate(Vector2.up * Time.deltaTime * boost * speed);
		}
		if (Input.GetKey(KeyCode.DownArrow)) { //down
			transform.Translate(Vector2.down * Time.deltaTime * boost * speed);
		}
		if (Input.GetKey(KeyCode.LeftArrow)) { //left
			transform.Translate(Vector2.left * Time.deltaTime * boost * speed);
		}
		if (Input.GetKey(KeyCode.RightArrow)) { //right
			transform.Translate(Vector2.right * Time.deltaTime * boost * speed);
		}

		float newX = Mathf.Clamp(transform.position.x, xmin, xmax);
		float newY = Mathf.Clamp(transform.position.y, ymin, ymax);
		transform.position = new Vector2 (newX, newY);
		
		
		if (Input.GetKeyDown(KeyCode.Space)) { //up
			InvokeRepeating("LaunchProjectile", 0.0f, 0.25f);
		}
		if (Input.GetKeyUp(KeyCode.Space)) { //up
			CancelInvoke("LaunchProjectile");
		}
	}
	
	void LaunchProjectile() {
		GameObject laser1 = Instantiate(projectile, transform.position, Quaternion.identity) as GameObject;
		GameObject laser2 = Instantiate(projectile, transform.position, Quaternion.identity) as GameObject;
		GameObject laser3 = Instantiate(projectile, transform.position, Quaternion.identity) as GameObject;
		laser1.GetComponent<Projectile>().birthDirection = new Vector2(0, 1);
		laser2.GetComponent<Projectile>().birthDirection = new Vector2(0, 1);
		laser3.GetComponent<Projectile>().birthDirection = new Vector2(0, 1);
		AudioSource.PlayClipAtPoint(fireLaser, transform.position);
	}
	
	void OnTriggerEnter2D (Collider2D col){
		if(col.GetComponent<Projectile>() != null && col.GetComponent<Projectile>().ProjectileType != 0.0f){
			col.GetComponent<Projectile>().Hit(type);
			life = life - col.GetComponent<Projectile>().GetDamage(type);
		}
		
		if (life <= 0) {
			Destroy(gameObject);
		}
	}
}
