using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : Moving_Objects {

	public float restartLevelDelay = 1f;
	public int wallDamage = 1;
	public Text healthText;

	private Animator animator;
	private int flowers;
	private int health;

	// Use this for initialization
	protected override void Start ()
	{
		animator = GetComponent<Animator> ();

		flowers = Game_Manager.instance.playerLevel;
		health = Game_Manager.instance.playerHealth;

		healthText.text = "Health: " + health;

		base.Start ();
	}

	private void OnDisable ()
	{
		Game_Manager.instance.playerLevel = flowers;
		Game_Manager.instance.playerHealth = health;
	}

	void Update ()
	{
		if (!Game_Manager.instance.playersTurn)
			return;

		int horizontal = 0;
		int vertical = 0;

		horizontal = (int) (Input.GetAxisRaw ("Horizontal"));
		vertical = (int) (Input.GetAxisRaw ("Vertical"));

		if (horizontal != 0)
			vertical = 0;

		if (horizontal != 0 || vertical != 0)
			AttemptMove<Wall> (horizontal, vertical);
	}

	protected override void AttemptMove <T> (int xDir, int yDir)
	{
		base.AttemptMove <T> (xDir, yDir);

		RaycastHit2D hit;

		CheckIfGameOver ();

		Game_Manager.instance.playersTurn = false;
	}

	private void OnTriggerEnter2D (Collider2D other)
	{
		if (other.tag == "Portal") {
			Invoke ("Restart", restartLevelDelay);
			enabled = false;
		} else if (other.tag == "Flower1") {
			animator.SetTrigger ("Level2");
			other.gameObject.SetActive (false);
		} else if (other.tag == "Flower2") {
			animator.SetTrigger ("Level3");
			other.gameObject.SetActive (false);
		} else if (other.tag == "Flower3") {
			animator.SetTrigger ("Level4");
			other.gameObject.SetActive (false);
		} else if (other.tag == "Flower4") {
			animator.SetTrigger ("FinalLevel");
			other.gameObject.SetActive (false);
		}
	}

	protected override void CantMove <T> (T component)
	{
		Wall hitWall = component as Wall;
		hitWall.DamagePlayer (wallDamage);
		animator.SetTrigger ("playerHurt");
		healthText.text ="Health: " + health;
		CheckIfGameOver ();
	}

	private void Restart ()
	{
		SceneManager.LoadScene (0);
	}

	public void LoseHealth (int loss)
	{
		animator.SetTrigger ("playerHurt");
		health -= loss;
		healthText.text = "- " + loss + "Health: " + health;
		CheckIfGameOver ();
	}

	private void CheckIfGameOver ()
	{
		if (health <= 0)
			Game_Manager.instance.GameOver ();
	}
}
