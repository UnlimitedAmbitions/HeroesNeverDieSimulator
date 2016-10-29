using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    public GameObject targetPrefab;
    public int minNbTargets, maxNbTargets;

    public Player playerScript;

    [Header("Background")]
    public GameObject backgroundPlane;
    public Material[] allBackgrounds;

    [Tooltip("corners of screen")]
    public Transform lowerLeft, upperRight;

    public float startDelay;

    [Header("UI")]
    //UI
    public Button replayBtn, menuBtn;
    public GameObject Dmg, HP, Reaction, Kills;
    public Text dmgCount, HPCount, reactionCount, killCount;
    [Header("Audio")]

    public AudioSource itsHighNoonSource, gunshotSource;
    public AudioClip itsHighNoon, gunshot;

    [Header("EndGame")]

    //end game
    public float totalAnimationTime;
    public float betweenShotsTime;
    public Animator gunAnimator;

    private List<GameObject> targets;
    private bool fired;
    public bool gameStarted;

    private int killed;
    private int remainingHp;
    private float damageDone;
    private float timeWaited;

    private int nbTargets;

    private int destroyTargetCount;

	// Use this for initialization
	void Start () {
        Debug.Log("game manager start");
        if(instance != null) {
            Destroy(this.gameObject);
        }

        replayBtn.gameObject.SetActive(false);
        menuBtn.gameObject.SetActive(false);
        Dmg.gameObject.SetActive(false);
        //HP.gameObject.SetActive(false);
        Reaction.gameObject.SetActive(false);
        Kills.gameObject.SetActive(false);

        instance = this;
        gameStarted = false;
        fired = false;
        killed = 0;
        remainingHp = 0;
        destroyTargetCount = 0;
        damageDone = 0f;
        timeWaited = 0f;

	    targets = new List<GameObject>();

        backgroundPlane.GetComponent<MeshRenderer>().material = allBackgrounds[Mathf.FloorToInt(Random.Range(0, allBackgrounds.Length))];

        // start game after delay
        Invoke("StartGame", startDelay);      
	}
	
	// Update is called once per frame
	void Update () {
        // timer
        if(gameStarted) {
            timeWaited += Time.deltaTime;
            HPCount.text = "" + playerScript.hp;
        }

        // logic for firing
        if(gameStarted && Input.GetButtonDown("Fire1") && !fired){
            fired = true;
            Debug.Log("BANG");
            CheckTargets();
            EndGame();
        }
	}

    private void StartGame() {
        Debug.Log("start game");
        RandomizeTargets();
        itsHighNoonSource.clip = itsHighNoon;
        itsHighNoonSource.Play();
        gameStarted = true;
    }

    private void EndGame() {
        Debug.Log("end game");
        gameStarted = false;
        
        StatAssessment();
        
    }

    // Creates an animation that animates the shooting of kills
    private void killTarget(){
        
        GameObject o = targets[destroyTargetCount];
        o.GetComponent<Target>().HideSkull();
        gunshotSource.clip = gunshot;
        gunshotSource.Play();

        if(o.GetComponent<Target>().IsDead()){
            //play screaming sound of death
        }

        destroyTargetCount++;
        if(destroyTargetCount < nbTargets){
            Invoke("killTarget", betweenShotsTime);
        }
        else {
            ActivateEndUI();
        }

    }

    public void ImDead(){
        HPCount.text = "0";
        foreach(GameObject o in targets) {
            o.GetComponent<Target>().HideSkull();
        }
        EndGame();
        ActivateEndUI();
    }

    public void RestartGame(){
        SceneManager.LoadScene("game");
    }

    public void ReturnToMenu(){
        SceneManager.LoadScene("MainMenu");
    }

    private void RandomizeTargets()
    {
        //random nb of targets
       nbTargets = Mathf.FloorToInt(Random.Range(minNbTargets, maxNbTargets));

       for(int i = 0; i < nbTargets; ++i) {
        
        //random position
        float x = Random.Range(lowerLeft.position.x, upperRight.position.x);
        float y = Random.Range(lowerLeft.position.y, upperRight.position.y);
        Vector3 pos = new Vector3(x, y, 0f);

        targets.Add(Instantiate(targetPrefab, pos, Quaternion.identity) as GameObject);
       }
    }

    private void CheckTargets(){
        Debug.Log("check target");
        // update nb of kill and dmg for each target
        foreach(GameObject target in targets) {
            float curr = target.GetComponent<Target>().GetScale();
            float HP = target.GetComponent<Target>().HP;
            float total = target.GetComponent<Target>().startingScale;
            bool isDead = target.GetComponent<Target>().IsDead();

            if(isDead) {
                damageDone  += HP;
                killed += 1;
            }
            else {
                damageDone += HP * (total - (curr/total));
            }
        }
        // slowly kill everyone
        if(nbTargets == 1) gunAnimator.SetTrigger("shot1");
        else gunAnimator.SetTrigger("shot2");
        Invoke("killTarget", 0.25f);
    }

    private void StatAssessment() {
        // get old value
        //int prevTotalKilled = PlayerPrefs.GetInt("TotalKilled");
        //int prevRemainingHp = PlayerPrefs.GetInt("RemainingHp");
        float prevDamageDone = PlayerPrefs.GetFloat("DamageDone");
        float prevTimeWaited = PlayerPrefs.GetFloat("TimeWaited"+killed);
        Debug.Log("killed: "+killed);
        Debug.Log("dmg done: "+damageDone);
        Debug.Log("reaction time: "+timeWaited);

        // update stats
        //if(prevRemainingHp < remainingHp) PlayerPrefs.SetInt("TotalDeath", remainingHp);
        if(prevDamageDone < damageDone) PlayerPrefs.SetFloat("DamageDone", damageDone);
        if(prevTimeWaited < timeWaited) PlayerPrefs.SetFloat("TimeWaited"+killed, timeWaited);
        PlayerPrefs.SetInt("GamesPlayed", PlayerPrefs.GetInt("GamesPlayed") + 1);
        PlayerPrefs.SetInt("TotalKilled", PlayerPrefs.GetInt("TotalKilled") + 1);
        if(remainingHp <= 0) PlayerPrefs.SetInt("TotalDeath", PlayerPrefs.GetInt("TotalDeath") + 1);

    }

    private void ActivateEndUI(){
        dmgCount.text = "" + damageDone.ToString("F0");
        reactionCount.text = "" + timeWaited.ToString("F2");
        killCount.text = "" + killed;

        replayBtn.gameObject.SetActive(true);
        menuBtn.gameObject.SetActive(true);
        Dmg.gameObject.SetActive(true);
        HP.gameObject.SetActive(true);
        Reaction.gameObject.SetActive(true);
        Kills.gameObject.SetActive(true);

    }
}
