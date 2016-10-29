using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	public int hp;
	public int decreaseHPThreshold;
	public int[] damageShots;

	// Use this for initialization
	void Start () {
		resetHP();
	}
	/*
        scalingRate = allScalingRates[Mathf.FloorToInt(Random.Range(0, allScalingRates.Length))];
        */
	
	// Update is called once per frame
	void FixedUpdate () {
		// Randomly deduct hp
		if(GameManager.instance.gameStarted && Mathf.FloorToInt(Random.Range(0, 100)) > decreaseHPThreshold){
			hp -= damageShots[Mathf.FloorToInt(Random.Range(0, damageShots.Length))];

			// If dead, alert GameManager
			if(hp <= 0) GameManager.instance.ImDead();
		}
	}

	public void resetHP(){ hp = 200; }
}
