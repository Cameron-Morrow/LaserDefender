using UnityEngine;

public class Enemy : MonoBehaviour {
	
	public GameObject Projectile;
	public float Life = 100f;
	public float Type = 4.0f;
	public float ShotsPerSecond = 0.5f;
	
	private void Update () {
		var probability = Time.deltaTime * ShotsPerSecond;
		if(Random.value < probability){
			LaunchProjectile();
		}
	}

	private void OnTriggerEnter2D (Collider2D col){
		if(col.GetComponent<Projectile>() != null && col.GetComponent<Projectile>().ProjectileType != Type){
			col.GetComponent<Projectile>().Hit(Type);
			Life = Life - col.GetComponent<Projectile>().GetDamage(Type);
		}
		
		if (Life <= 0) {
			Destroy(gameObject);
		}
	}
	
	private void LaunchProjectile() {
		var laser = Instantiate(Projectile, transform.position, Quaternion.identity);
		laser.GetComponent<Projectile>().birthDirection = new Vector2(0, -1);
	}
}
