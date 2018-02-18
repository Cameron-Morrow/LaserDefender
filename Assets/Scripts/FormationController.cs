using UnityEngine;

public class FormationController : MonoBehaviour {
	public GameObject EnemyPrefab4;
	
	public float Width;
	public float Height;
	public float Speed = 3.0f;
	public float SpawnDelay = 0.5f;
	
	private float _xpadding;
	private float _xmin;
	private float _xmax;
	private float _ymin;
	private float _ymax;
	private float _leftEdge;
	private float _rightEdge;
	
	private string _direction = "right";

	private void Start () {
		_xpadding =  Width / 2;
		
		var distance = transform.position.z - Camera.main.transform.position.z;
		var cameraMin = Camera.main.ViewportToWorldPoint(new Vector3(0,0,distance));
		var cameraMax = Camera.main.ViewportToWorldPoint(new Vector3(1,1,distance));
		_xmin = cameraMin.x;
		_xmax = cameraMax.x;
		//_ymin = cameraMin.y + padding;
		//_ymax = cameraMax.y - padding;
		_leftEdge = -_xpadding;
		_rightEdge = _xpadding;
		SpawnUntilFull();
	}
	
	private void Update () {
		_xpadding =  Width / 2;
		/*float checker = -1000000;
		foreach ( Transform child in transform ) {
			if(child.transform.position.x >= checker){
				checker = child.transform.position.x;
			}
		}
		_rightEdge = (checker) + transform.position.x;
		//Debug.Log(checker);
		checker = 10000000;
		foreach ( Transform child in transform ) {
			if(child.transform.position.x <= checker){
				checker = child.transform.position.x;
			}
		}
		_leftEdge = (checker) - transform.position.x;
		//Debug.Log(checker);*/
		_leftEdge = transform.position.x - _xpadding;
		_rightEdge = transform.position.x + _xpadding;
		
		switch (_direction)
		{
			case "right":
				transform.Translate(Vector2.right * Time.deltaTime * Speed);
				break;
			case "left":
				transform.Translate(Vector2.left * Time.deltaTime * Speed);
				break;
			default:
				break;
		}
		
		var newX = Mathf.Clamp(transform.position.x, _xmin, _xmax);
		//float newY = Mathf.Clamp(transform.position.y, _ymin, _ymax);
		transform.position = new Vector2 (newX, transform.position.y);
		
		if(_leftEdge <= _xmin){
			_direction = "right";
		}
		if(_rightEdge >= _xmax){
			_direction = "left";
		}
		
		if(AllDead()){
			SpawnUntilFull();
		}
	}
	
	private void OnDrawGizmos () {
		Gizmos.DrawWireCube(transform.position, new Vector3(Width, Height, 0));
	}
	private bool AllDead () {
		foreach ( Transform childPositionGameObject in transform ) {
			if (childPositionGameObject.childCount > 0){
				return false;
			}
		}
		return true;
	}
	
	private bool Spawn () {
		foreach ( Transform child in transform ) {
			if (child.childCount <= 0){
				var enemy = Instantiate(EnemyPrefab4, child.transform.position, Quaternion.identity);
				enemy.transform.parent = child;
			}
		}
		if(false){//error checking in the future
			return false;
		}else{
			return true;
		}
		
	}
	
	private void SpawnUntilFull () {
		var freePosition = NextFreePosition();
		if (freePosition != null){
			var enemy = Instantiate(EnemyPrefab4, freePosition.position, Quaternion.identity);
			enemy.transform.parent = freePosition;
		}
		if (NextFreePosition()) {
			Invoke("SpawnUntilFull", SpawnDelay);
		}
	}
	
	private Transform NextFreePosition () {
		foreach ( Transform childPositionGameObject in transform ) {
			if (childPositionGameObject.childCount <= 0){
				return childPositionGameObject;
			}
		}
		return null;
	}
}
