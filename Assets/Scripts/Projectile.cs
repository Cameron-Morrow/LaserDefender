using UnityEngine;

public class Projectile : MonoBehaviour {
	
	public int ProjectileType;
	public float Damage = 10;
	public float ProjectileSpeed = 25f;
	public Vector2 BirthDirection;

	private void Update () {
		transform.Translate(BirthDirection * Time.deltaTime * ProjectileSpeed);
		Destroy(gameObject, 2.0f);
	}
	
	public void Hit (int type) {
		//eventually be able to determine if projectile is destroyed when it collides based on what it hit
		//for now projectile is destroyed no matter what if it receives Hit()
		Debug.Log(string.Format("Type of object hit: {0}, determine if projectile should be destroyed", type));
		Destroy(gameObject);
	}
	
	public float GetDamage (int type)
	{
		//in the future I will be able to calculate damage based on the type of object that the projectile hit.
		Debug.Log(string.Format("Type of object hit: {0}", type));
		return Damage;
	}
}
