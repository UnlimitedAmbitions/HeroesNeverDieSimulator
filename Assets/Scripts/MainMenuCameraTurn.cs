using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenuCameraTurn : MonoBehaviour {
	public Animator camAnim;
	public AudioSource gunshot;

    public void StartGame()
    {
        SceneManager.LoadScene("game");
        gunshot.Play();
    }
	
	public void ToStats(){
		camAnim.SetTrigger("MainToStats");
        gunshot.Play();
	}

	public void ToMainMenu(){
		camAnim.SetTrigger("StatsToMain");
        gunshot.Play();
	}
}
