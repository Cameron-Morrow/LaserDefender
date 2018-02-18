using UnityEngine;

public class Position : MonoBehaviour {

	private void OnDrawGizmos () {
		Gizmos.DrawWireSphere(transform.position, 0.5f); 
	}
	
}
