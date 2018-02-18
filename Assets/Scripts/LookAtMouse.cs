using UnityEngine;

public class LookAtMouse : MonoBehaviour {

    /*********************************************/
    /*               LOOK AT MOUSE               */
    /*                                           */
    /*This script can be given to any object with*/
    /*a Rigidbody2D component and the object will*/
    /*point itself at the mouse position on the  */
    /*screen.                                    */
    /*                                           */
    /*********************************************/
    
    private Camera _camera;
    private Transform _transform;
    private Rigidbody2D _rigidbody2D;
    
    private void Awake () {
        _camera = Camera.main;
        _transform = GetComponent <Transform> ();
        _rigidbody2D = GetComponent <Rigidbody2D> ();
    }
    
    private void Update () {
        // Distance from camera to object.
        var cameraDistance = _camera.transform.position.y - _transform.position.y;
        
        // Get the mouse position in world space. Using cameraDistance for the Z axis.
        var mousePosition = _camera.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, cameraDistance));
        
        var angleInRadians = Mathf.Atan2 (mousePosition.y - _transform.position.y, mousePosition.x - _transform.position.x);
        var angleInDegrees = ((180 / Mathf.PI) * angleInRadians) - 90;
        
        _rigidbody2D.rotation = angleInDegrees;
    }
}