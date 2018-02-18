using UnityEngine;

public class ShipController : MonoBehaviour {
	
	public GameObject Projectile;
	public AudioClip FireLaser;
	public int Type;
	public float Life = 1000;
	public int Speed = 5;
	
	private int _boost;
	private const float Padding = 0.4f;
	private float _xmin;
	private float _xmax;
	private float _ymin;
	private float _ymax;
	
	private void Start () {
		var distance = transform.position.z - Camera.main.transform.position.z;
		var cameraMin = Camera.main.ViewportToWorldPoint(new Vector3(0,0,distance));
		var cameraMax = Camera.main.ViewportToWorldPoint(new Vector3(1,1,distance));
		_xmin = cameraMin.x + Padding;
		_xmax = cameraMax.x - Padding;
		_ymin = cameraMin.y + Padding;
		_ymax = cameraMax.y - Padding;
	}

	private void Update () {
		//determine if _boost
		_boost = (Input.GetKey(KeyCode.LeftShift)) ? 2 : 1;
		
		//control power of ship
		if (Input.GetKey(KeyCode.UpArrow)) { //up
			transform.Translate(Vector2.up * Time.deltaTime * _boost * Speed);
		}
		if (Input.GetKey(KeyCode.DownArrow)) { //down
			transform.Translate(Vector2.down * Time.deltaTime * _boost * Speed);
		}
		if (Input.GetKey(KeyCode.LeftArrow)) { //left
			transform.Translate(Vector2.left * Time.deltaTime * _boost * Speed);
		}
		if (Input.GetKey(KeyCode.RightArrow)) { //right
			transform.Translate(Vector2.right * Time.deltaTime * _boost * Speed);
		}

		var newX = Mathf.Clamp(transform.position.x, _xmin, _xmax);
		var newY = Mathf.Clamp(transform.position.y, _ymin, _ymax);
		transform.position = new Vector2 (newX, newY);
		
		
		if (Input.GetKeyDown(KeyCode.Space)) {
			InvokeRepeating("LaunchProjectile", 0.0f, 0.25f);
		}
		if (Input.GetKeyUp(KeyCode.Space)) {
			CancelInvoke("LaunchProjectile");
		}
	}
	
	private void LaunchProjectile() {
		var laser = Instantiate(Projectile, transform.position, Quaternion.identity);
		laser.GetComponent<Projectile>().BirthDirection = new Vector2(0, 1);
		AudioSource.PlayClipAtPoint(FireLaser, transform.position);
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
}
