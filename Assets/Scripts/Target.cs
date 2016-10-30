using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Target : MonoBehaviour {

    public GameObject soul, respawnable;
    public Sprite[] characters;
    public Image currentChar, respawnableProgressBar;
    public ParticleSystem lightParticle;
   // public Image circle;

    public bool healerMode;

    public float HPrate;
    public float[] allHPrates;
    public float startingHP;

    public float respawnHPrate;
    public float startingRespawnHP;
    public int[] totalHP;

    public bool isDead = false;
    public bool isRespawned;
    private float HP;
    private float respawnHP;

	// Use this for initialization
	void Start () {
        soul.gameObject.SetActive(false);
        respawnable.gameObject.SetActive(false);
        //HP = startingHP;
        respawnHP = startingRespawnHP;
        isRespawned = false;
        //HPrate = allHPrates[Mathf.FloorToInt(Random.Range(0, allHPrates.Length-1))];
        HP = totalHP[Mathf.FloorToInt(Random.Range(0, totalHP.Length-1))];
        int index = Mathf.FloorToInt(Random.Range(0, characters.Length));
        currentChar.sprite = characters[index];
 
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if(!GameManager.instance.gameStarted) return;

        if (!isDead)
        {
            if(Random.Range(-50, 100) < 0)
            {
                removeHP();
            }
            if (HP <= 0f) {
                isDead = true;     
            }
        }

        if (isDead && !isRespawned) {
            soul.SetActive(true);
            respawnable.SetActive(true);
            currentChar.gameObject.SetActive(false);
            respawnHP -= respawnHPrate * Time.deltaTime;
            respawnableProgressBar.fillAmount = Mathf.Min(1f - respawnHP/startingRespawnHP, 1f);
            if (respawnHP <= 0f) {isRespawned = true; soul.SetActive(false); respawnable.SetActive(false);}
        }
    }

    private void removeHP()
    {
        float minDamage = 0;

        //if (healerMode) minDamage = (startingHP / 6);

        float damageDone;
        damageDone = Random.Range(minDamage, startingHP/33f);
        HP -= damageDone;
    }

    public float GetScale() {
        return HP;
    }

    public bool IsDead() {
        return isDead;
    }

    public void RemoveSoul()
    {
        soul.gameObject.SetActive(false);
        respawnable.gameObject.SetActive(false);
    }

    public void Revive() {
        currentChar.gameObject.SetActive(true);
        lightParticle.Play();
    }

    
}
