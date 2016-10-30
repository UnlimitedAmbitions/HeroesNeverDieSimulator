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
    public GameObject HP, Reaction, Revives, UltSymbol;
    public Text HPCount, reactionCount, reviveCount;

    [Header("Audio")]
    public AudioSource quoteSource, rezSource;
    public AudioClip heroesNeverDie, rezEffect;

    [Header("EndGame")]

    //end game
    public float totalAnimationTime;
    public float betweenShotsTime;
    public Animator mercyHandAnimator;
    public Animator mercyStaffAnimator;

    private List<GameObject> targets;
    private bool fired;
    public bool gameStarted;
    public float probabilityStartDead = 0.5f;

    //private int killed;
    private int remainingHp;
    //private float damageDone;
    private float timeWaited;

    private int revived;

    private int nbTargets;

    //private int destroyTargetCount;

	// Use this for initialization
	void Start () {
        Debug.Log("game manager start");
        if(instance != null) {
            Destroy(this.gameObject);
        }

        replayBtn.gameObject.SetActive(false);
        menuBtn.gameObject.SetActive(false);
        //Dmg.gameObject.SetActive(false);
        //HP.gameObject.SetActive(false);
        Reaction.gameObject.SetActive(false);
        Revives.gameObject.SetActive(false);
        UltSymbol.gameObject.SetActive(true);
        instance = this;
        gameStarted = false;
        fired = false;
        //killed = 0;
        remainingHp = 0;
        //destroyTargetCount = 0;
        //damageDone = 0f;
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
            Debug.Log("REZ");
            UltSymbol.gameObject.SetActive(false);
            mercyHandAnimator.SetTrigger("Rez");
            mercyStaffAnimator.SetTrigger("HideStaff");
            rezSource.clip = rezEffect;
            rezSource.Play();
            quoteSource.clip = heroesNeverDie;
            quoteSource.PlayDelayed(0.0f);
            CheckTargets();
            EndGame();
        }
	}

    private void StartGame() {
        Debug.Log("start game");
        RandomizeTargets();
        //itsHighNoonSource.clip = itsHighNoon;
        //itsHighNoonSource.Play();
        gameStarted = true;
    }

    private void EndGame() {
        Debug.Log("end game");
        gameStarted = false;
        
        StatAssessment();
        
    }

    // Creates an animation that animates the shooting of kills
    private void reviveTarget(GameObject target){
        
        //reveive animation
        target.GetComponent<Target>().RemoveSoul();
        target.GetComponent<Target>().Revive();

        ActivateEndUI();
    }

    public void ImDead(){
        HPCount.text = "0";
        //foreach(GameObject o in targets) {
            //o.GetComponent<Target>().HideSkull();
       //}
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

        GameObject newTarget = Instantiate(targetPrefab, pos, Quaternion.identity) as GameObject;
        targets.Add(newTarget);

        float isDeadProb = Random.Range(0f, 1f);
        if(isDeadProb > probabilityStartDead) {
            newTarget.GetComponent<Target>().isDead = true;
        }

       }
    }

    private void CheckTargets(){
        Debug.Log("check target");
        // update nb of kill and dmg for each target
        foreach(GameObject target in targets) {
            bool isDead = target.GetComponent<Target>().IsDead();
            bool isRespawned = target.GetComponent<Target>().isRespawned;

            if(isDead && !isRespawned) {
                revived += 1;
                reviveTarget(target);
            }
        }
        // slowly kill everyone
        //if(nbTargets == 1) gunAnimator.SetTrigger("shot1");
        //else gunAnimator.SetTrigger("shot2");
    }

    private void StatAssessment() {
        // get old value
        //int prevTotalKilled = PlayerPrefs.GetInt("TotalKilled");
        //int prevRemainingHp = PlayerPrefs.GetInt("RemainingHp");
        //float prevDamageDone = PlayerPrefs.GetFloat("DamageDone");
        float prevTimeWaited = PlayerPrefs.GetFloat("TimeWaited"+revived);
        Debug.Log("revived: "+revived);
        Debug.Log("reaction time: "+timeWaited);

        // update stats
        //if(prevRemainingHp < remainingHp) PlayerPrefs.SetInt("TotalDeath", remainingHp);
       // if(prevDamageDone < damageDone) PlayerPrefs.SetFloat("DamageDone", damageDone);
        if(prevTimeWaited < timeWaited) PlayerPrefs.SetFloat("TimeWaited"+revived, timeWaited);
        PlayerPrefs.SetInt("GamesPlayed", PlayerPrefs.GetInt("GamesPlayed") + 1);
        PlayerPrefs.SetInt("TotalRevived", PlayerPrefs.GetInt("TotalRevived") + 1);
        if(remainingHp <= 0) PlayerPrefs.SetInt("TotalDeath", PlayerPrefs.GetInt("TotalDeath") + 1);

    }

    private void ActivateEndUI(){
        //dmgCount.text = "" + damageDone.ToString("F0");
        reactionCount.text = "" + timeWaited.ToString("F2");
        reviveCount.text = "" + revived;

        replayBtn.gameObject.SetActive(true);
        menuBtn.gameObject.SetActive(true);
        //Dmg.gameObject.SetActive(true);
        HP.gameObject.SetActive(true);
        Reaction.gameObject.SetActive(true);
        Revives.gameObject.SetActive(true);

    }
}
