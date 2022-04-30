using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class KnightBehaviour : MonoBehaviour {

	private Animator animator;
	private GameObject planet;

	private bool attackWindowActive;
	private bool pullWindowActive;
	private string[] typesOfHerbs;
	private string[] typesOfCrystals;
	private int specialHatchetLives;
	private float health = 10000;
	public string timePlayed;
	public int herbCount;
	public bool hasCrystal;
	public int greenCrystalCount;
	public bool isGameOver;
	public HUD hud;
	private bool hasKit;
	private int hatchetLifes;
	// Use this for initialization
	void Start() {
		isGameOver = false;
		greenCrystalCount = 0;
		typesOfHerbs = new string[] { "Herb1", "Herb2", "Herb3" };
		typesOfCrystals = new string[] { "GreenCrystal", "GreenCrystal1" };
		Time.timeScale = 1;
		herbCount = 0;
		animator = GetComponent<Animator>();
		planet = GameObject.Find("Planet");

		// constantly decrementing health with a given interval
		if (!isGameOver)
		{
			InvokeRepeating("DecrementHealth", 0, 0.8f);
		}
	}
	void PauseGame()
	{
		Time.timeScale = 0;
	}
	public void DecrementHealth() {
		health--;
		if (health <= 0) {
			// gameover
			timePlayed = "" + Time.timeSinceLevelLoad;
			isGameOver = true;
			PauseGame();
			var music = GameObject.Find("Music").GetComponent<AudioSource>();
			var youDiedSfx = Resources.Load<AudioClip>(@"Music/youDied");
			if (music.clip != youDiedSfx)
			{
				music.clip = youDiedSfx;
				music.Play();
				music.loop = false;
			}
			hud.showGameOverDialog();
		}
	}

	// Update is called once per frame
	void Update() {
		if (!isGameOver)
		{
			ChangeSpeedOfFasterEnemies();
			UpdateInterface();
			PlayerControls();
		}
	}
	public void ChangeSpeedOfFasterEnemies()
    {
		var fasterEnemies = GameObject.FindGameObjectsWithTag("FasterEnemy");
        foreach (var fasterEnemy in fasterEnemies)
        {
			fasterEnemy.GetComponent<EnemyBehaviour>().enemySpeed = 0.0065f;
        }
    }

	// synchronize all script properties and flags with UI elements
	void UpdateInterface() {
		GameObject.Find("Slider").GetComponent<Slider>().value = health;
		GameObject.Find("Fill").GetComponent<Image>().color =
			Color.Lerp(Color.red, Color.green, health / 100f);
		foreach (var typeOfHerb in typesOfHerbs)
		{
			foreach (GameObject herb in GameObject.FindGameObjectsWithTag(typeOfHerb))
			{
				herb.GetComponent<Image>().color = Color.black;
			}
		}
		if (herbCount > 0)
		{
			foreach (GameObject herb in GameObject.FindGameObjectsWithTag("Herb1"))
			{
				herb.GetComponent<Image>().color = Color.white;
			}
			if (herbCount > 1)
			{
				foreach (GameObject herb in GameObject.FindGameObjectsWithTag("Herb2"))
				{
					herb.GetComponent<Image>().color = Color.white;
				}
			}
			if (herbCount > 2)
			{
				foreach (GameObject herb in GameObject.FindGameObjectsWithTag("Herb3"))
				{
					herb.GetComponent<Image>().color = Color.white;
				}
			}
		}
		if (herbCount > 2)
		{
			GameObject.FindGameObjectWithTag("Spell").GetComponent<Image>().color = Color.white;
		}
		else
		{
			GameObject.FindGameObjectWithTag("Spell").GetComponent<Image>().color = Color.black;
		}
		if (greenCrystalCount > 1 && herbCount > 0)
		{
			GameObject.FindGameObjectWithTag("SpecialHatchet").GetComponent<Image>().color = Color.yellow;
		}
		else
		{
			GameObject.FindGameObjectWithTag("SpecialHatchet").GetComponent<Image>().color = Color.black;
		}
		if (herbCount > 2)
		{
			GameObject.FindGameObjectWithTag("Spell").GetComponent<Image>().color = Color.white;
		}
        else
        {
			GameObject.FindGameObjectWithTag("Spell").GetComponent<Image>().color = Color.black;
		}
		if (greenCrystalCount > 0)
		{
			GameObject.FindGameObjectWithTag("GreenCrystal").GetComponent<Image>().color = Color.cyan;
		}
		else
		{
			GameObject.FindGameObjectWithTag("GreenCrystal").GetComponent<Image>().color = Color.black;

		}
		if (greenCrystalCount > 1)
		{
			GameObject.FindGameObjectWithTag("GreenCrystal1").GetComponent<Image>().color = Color.cyan;
		}
		else
		{
			GameObject.FindGameObjectWithTag("GreenCrystal1").GetComponent<Image>().color = Color.black;

		}
		foreach (GameObject crystal in GameObject.FindGameObjectsWithTag("Crystal"))
		{
			crystal.GetComponent<Image>().color = hasCrystal ? Color.white : Color.black;
		}
		GameObject.Find("Hatchet").GetComponent<Image>().color =
			hatchetLifes > 0 ? Color.white : Color.black;
		GameObject.Find("Kit").GetComponent<Image>().color =
			hasKit ? Color.white : Color.black;
	}

	// key presses and actions
	void PlayerControls() {
		if (Input.GetKey(KeyCode.A)) {
			Move(-1);
		}
		if (Input.GetKey(KeyCode.D)) {
			Move(1);
		}
		// setting animator properties to cause animation transitions!!
		if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D)) {
			animator.SetBool("IsRunning", false);
		}
		if (Input.GetKeyDown(KeyCode.F)) {
			animator.SetTrigger("Attack");
		}

		// crafting logic
		if (Input.GetKeyDown(KeyCode.E)) {
			if (hatchetLifes > 0) {
				UseHatchet();
			} else {
				TryCraftHatchet();
			}
		}
		if (Input.GetKeyDown(KeyCode.G))
		{
			if (specialHatchetLives > 0)
			{
				UseSpecialHatchet();
			}
			else
			{
				TryCraftSpecialHatchet();
			}
		}
		if (Input.GetKeyDown(KeyCode.Q)) {
			if (hasKit) {
				UseKit();
			} else {
				TryCraftKit();
			}
		}
		if (Input.GetKeyDown(KeyCode.R)) {
			animator.SetTrigger("Pull");
		}
		if (Input.GetKeyDown(KeyCode.S))
		{
			CastSpell();
		}


	}

	public void UseKit() {
		health += 20;
		hasKit = false;
	}

	public void TryCraftKit() {
		if (herbCount > 0) {
			hasKit = true;
			herbCount--;
		}
	}
	public void TryCraftSpecialHatchet()
    {
        if (greenCrystalCount > 1 && herbCount > 0)
        {
			specialHatchetLives = 2;
			greenCrystalCount -= 2;
		}
		herbCount--;
    }
	public void CastSpell()
    {
        if (CanCastSpell())
        {
			herbCount -= 3;
			var enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (var enemy in enemies)
            {
				enemy.GetComponent<EnemyBehaviour>().enemySpeed -= 0.0020f;
            }
        }
    }
	public void UseHatchet(){
		animator.SetTrigger("Smash");
		if (hatchetLifes >= 0) hatchetLifes--;
	}
	public void UseSpecialHatchet()
    {
		animator.SetTrigger("Smash");
		if (specialHatchetLives > 0) specialHatchetLives--;
        
		var enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var enemy in enemies)
        {
			Destroy(enemy.gameObject);
        }
	}
	public void TryCraftHatchet(){
		if (hasCrystal && herbCount > 0) {
			hatchetLifes = 200;
			hasCrystal = false;
			herbCount--;
		}
	}
	public bool CanCastSpell()
    {
        if (herbCount > 2)
        {
			return true;
        }
		return false;
    }
	// rotates the player and launches running animation, localScale.x mirrors the sprite in the right direction
	void Move(int leftRight){
			animator.SetBool("IsRunning", true);
			gameObject.transform.parent.localScale = new Vector3(leftRight * 1, 1, 1);
			planet.transform.Rotate(new Vector3(0, 0, leftRight * 1.2f));
	}

	// animation windows / animtion marks, used from animation events to tell when the actual ATTACK/PULL frames are playing
	private void SetAttackWindowValue(int value){
		attackWindowActive = value == 1;
	}

	private void SetPullWindowValue(int value){
		pullWindowActive = value == 1;
	}

	// used from animation event, the effect of the ground shake -- shake all opposide side enemies out of the planet
	private void ShakeGround(){
		planet.GetComponent<Animator>().Play("GroundShake", 0, 0);
		foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy")){
			Vector3 outOfPlanetDirection = (enemy.transform.position - planet.transform.position).normalized;
			if (outOfPlanetDirection.y <= -0.35f){
				enemy.transform.position += outOfPlanetDirection * 1.0f;
			}
		}
		foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("FasterEnemy"))
		{
			Vector3 outOfPlanetDirection = (enemy.transform.position - planet.transform.position).normalized;
			if (outOfPlanetDirection.y <= -0.35f)
			{
				enemy.transform.position += outOfPlanetDirection * 1.5f;
			}
		}

	}


	// unity sends this signal when COLLIDERS ARE OVERLAPPING!!!
	// we catch it and apply logic depending on the incoming object
	private void OnTriggerStay2D(Collider2D other) {
		if (other.tag == "Enemy" && pullWindowActive) {
			other.GetComponent<EnemyBehaviour>().PullOut();
		}
		if (other.tag == "HerbRes" && pullWindowActive) {
			herbCount++;
			Destroy(other.gameObject);
		}
		if (other.tag == "CrystalRes" && attackWindowActive) {
			Animator otherAnimator = other.GetComponent<Animator>();
			otherAnimator.Play("Blink");
			var crystal = other.GetComponent<Crystal>();
			if (crystal.hitsToDestroy <= 0){
                if (crystal.color.Equals("pink"))
                {
					hasCrystal = true;
                }
                else
                {
					greenCrystalCount++;
                }
				Destroy(other.gameObject);
			}
		}
	}
}
