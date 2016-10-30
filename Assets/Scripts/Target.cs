using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Target : MonoBehaviour {

    public GameObject target, point;
    public Image circle;

    public bool healerMode;

    public float HPrate;
    public float[] allHPrates;
    public float startingHP;

    public float respawnHPrate;
    public float startingRespawnHP;
    public int[] totalHP;

    public bool isDead;
    public bool isRespawned;
    private float HP;
    private float respawnHP;

	// Use this for initialization
	void Start () {
        target.gameObject.SetActive(false);
        point.gameObject.SetActive(true);
        //HP = startingHP;
        respawnHP = startingRespawnHP;
        isDead = false;
        isRespawned = false;
        //HPrate = allHPrates[Mathf.FloorToInt(Random.Range(0, allHPrates.Length-1))];
        HP = totalHP[Mathf.FloorToInt(Random.Range(0, totalHP.Length-1))];
 
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if(!GameManager.instance.gameStarted) return;

        if (!isDead)
        {
            if(Random.Range(-10, 100) < 0)
            {
                removeHP();
            }
            if (HP <= 0f) {isDead = true; Debug.Log("target died");}
        }

        if (isDead && !isRespawned) {
            respawnHP -= respawnHPrate * Time.deltaTime;
            if (respawnHP <= 0f) {isRespawned = true; Debug.Log("target expired");}
        }
    }

    private void removeHP()
    {
        float minDamage = 0;

        if (healerMode) minDamage = (startingHP / 6);

        float damageDone;
        damageDone = Random.Range(minDamage, startingHP);
        HP -= damageDone;
    }

    public float GetScale() {
        return HP;
    }

    public bool IsDead() {
        return isDead;
    }

    public void HideSkull() {
        this.gameObject.SetActive(false);
    }

    private void setSkull() {
        // set skull
        target.gameObject.SetActive(true);
        point.gameObject.SetActive(false);
        Debug.Log("target set skull");
    }
}
