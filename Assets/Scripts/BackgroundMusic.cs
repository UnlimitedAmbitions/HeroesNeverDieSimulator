using UnityEngine;
using System.Collections;

public class BackgroundMusic : MonoBehaviour {
	public AudioSource aS;
	public static BackgroundMusic bg;

	// Use this for initialization
	void Start () {
		if(bg != null) Destroy(this.gameObject);
		bg = this;
		DontDestroyOnLoad(this.gameObject);
		aS.volume = 0.5f;
	}
}
