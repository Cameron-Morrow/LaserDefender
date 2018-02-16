﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormationController : MonoBehaviour {
	public GameObject enemyPrefab4;
	
	public float width;
	public float height;
	public float speed = 3.0f;
	public float spawnDelay = 0.5f;
	
	float Xpadding;
	float xmin;
	float xmax;
	float ymin;
	float ymax;
	float leftEdge;
	float rightEdge;
	
	string direction = "right";

	// Use this for initialization
	void Start () {
		Xpadding =  width / 2;
		
		float distance = transform.position.z - Camera.main.transform.position.z;
		Vector3 cameraMin = Camera.main.ViewportToWorldPoint(new Vector3(0,0,distance));
		Vector3 cameraMax = Camera.main.ViewportToWorldPoint(new Vector3(1,1,distance));
		xmin = cameraMin.x;
		xmax = cameraMax.x;
		//ymin = cameraMin.y + padding;
		//ymax = cameraMax.y - padding;
		leftEdge = -Xpadding;
		rightEdge = Xpadding;
		SpawnUntilFull();
	}
	
	// Update is called once per frame
	void Update () {
		Xpadding =  width / 2;
		/*float checker = -1000000;
		foreach ( Transform child in transform ) {
			if(child.transform.position.x >= checker){
				checker = child.transform.position.x;
			}
		}
		rightEdge = (checker) + transform.position.x;
		//Debug.Log(checker);
		checker = 10000000;
		foreach ( Transform child in transform ) {
			if(child.transform.position.x <= checker){
				checker = child.transform.position.x;
			}
		}
		leftEdge = (checker) - transform.position.x;
		//Debug.Log(checker);*/
		leftEdge = transform.position.x - Xpadding;
		rightEdge = transform.position.x + Xpadding;
		
		if(direction == "right"){
			transform.Translate(Vector2.right * Time.deltaTime * speed);
		}
		if(direction == "left"){
			transform.Translate(Vector2.left * Time.deltaTime * speed);
		}
		
		float newX = Mathf.Clamp(transform.position.x, xmin, xmax);
		//float newY = Mathf.Clamp(transform.position.y, ymin, ymax);
		transform.position = new Vector2 (newX, transform.position.y);
		
		if(leftEdge <= xmin){
			direction = "right";
		}
		if(rightEdge >= xmax){
			direction = "left";
		}
		
		if(AllDead()){
			SpawnUntilFull();
		}
	}
	
	void OnDrawGizmos () {
		Gizmos.DrawWireCube(transform.position, new Vector3(width, height, 0));
	}
	bool AllDead () {
		foreach ( Transform childPositionGameObject in transform ) {
			if (childPositionGameObject.childCount > 0){
				return false;
			}
		}
		return true;
	}
	
	bool Spawn () {
		foreach ( Transform child in transform ) {
			if (child.childCount <= 0){
				GameObject enemy = Instantiate(enemyPrefab4, child.transform.position, Quaternion.identity) as GameObject;
				enemy.transform.parent = child;
			}
		}
		if(false){//error checking in the future
			return false;
		}else{
			return true;
		}
		
	}
	
	void SpawnUntilFull () {
		Transform freePosition = NextFreePosition();
		if (freePosition != null){
			GameObject enemy = Instantiate(enemyPrefab4, freePosition.position, Quaternion.identity) as GameObject;
			enemy.transform.parent = freePosition;
		}
		if (NextFreePosition()) {
			Invoke("SpawnUntilFull", spawnDelay);
		}
	}
	
	Transform NextFreePosition () {
		foreach ( Transform childPositionGameObject in transform ) {
			if (childPositionGameObject.childCount <= 0){
				return childPositionGameObject;
			}
		}
		return null;
	}
}