using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UpdateStats : MonoBehaviour {

    public Text time1, time2, time3, time4, time5, time6;
    public Text kill, death, games;

	// Use this for initialization
	void Start () {
        time1.text = ""+PlayerPrefs.GetFloat("TimeWaited1").ToString("F2");
        time2.text = ""+PlayerPrefs.GetFloat("TimeWaited2").ToString("F2");
        time3.text = ""+PlayerPrefs.GetFloat("TimeWaited3").ToString("F2");
        time4.text = ""+PlayerPrefs.GetFloat("TimeWaited4").ToString("F2");
        time5.text = ""+PlayerPrefs.GetFloat("TimeWaited5").ToString("F2");
        time6.text = ""+PlayerPrefs.GetFloat("TimeWaited6").ToString("F2");
        kill.text = ""+PlayerPrefs.GetInt("TotalKilled");
        death.text = ""+PlayerPrefs.GetInt("TotalDeath");
        games.text = ""+PlayerPrefs.GetInt("GamesPlayed");
	
	}
	
}
