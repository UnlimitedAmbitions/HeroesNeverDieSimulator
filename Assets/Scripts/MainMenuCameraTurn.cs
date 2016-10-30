using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenuCameraTurn : MonoBehaviour {
	public Animator camAnim;
	public AudioSource magic;

    public void StartGame()
    {
        SceneManager.LoadScene("game");
        magic.Play();
    }
	
	public void ToStats(){
		camAnim.SetTrigger("MainToStats");
        magic.Play();
	}

	public void ToMainMenu(){
		camAnim.SetTrigger("StatsToMain");
        magic.Play();
	}
}
