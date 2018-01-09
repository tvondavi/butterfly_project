using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Game_Manager : MonoBehaviour {

	public float gameStartDelay = 1.5f;
	public float turnDelay = 0.1f;
	public static Game_Manager instance = null;                                               
	public int playerLevel = 0;
	public int playerHealth = 30;
	[HideInInspector] public bool playersTurn = true;

	private Text levelText;
	private GameObject levelScreen;
	private Board_Manager boardScript;                       
	private int level = 1;
	private List<Enemy> enemies;
	private bool enemiesMoving;
	private bool settingUp;

	void Awake()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy(gameObject);    
		
		DontDestroyOnLoad(gameObject);
		enemies = new List<Enemy> ();
		boardScript = GetComponent<Board_Manager>(); 
		InitGame();
	}

	void OnLevelWasLoaded (int index)
	{
		level++;
		InitGame ();
	}

	void InitGame()
	{
		settingUp = true;

		levelScreen = GameObject.Find ("LevelScreen");
		levelText = GameObject.Find ("LevelText").GetComponent<Text> ();
		levelText.text = "Level: " + level;
		levelScreen.SetActive (true);
		Invoke ("HideLevelScreen", gameStartDelay);

		enemies.Clear ();
	    boardScript.SetupLevel(level);
	}

	private void HideLevelScreen ()
	{
		levelScreen.SetActive (false);
		settingUp = false;
	}

	public void GameOver ()
	{
		levelText.text = "Game Over";
		levelScreen.SetActive (true);
		enabled = false;
	}

	void Update ()
	{
		if (playersTurn || enemiesMoving || settingUp)
			return;
		StartCoroutine (MoveEnemies ());
	}

	public void AddEnemyToList (Enemy script)
	{
		enemies.Add (script);

	}

	IEnumerator MoveEnemies ()
	{
		enemiesMoving = true;

		yield return new WaitForSeconds (turnDelay);

		if (enemies.Count == 0) 
		{
			yield return new WaitForSeconds (turnDelay);
		}

		for (int i = 0; i < enemies.Count; i++) 
		{
			enemies[i].EnemyMove ();
			yield return new WaitForSeconds (enemies[i].moveTime);
		}

		playersTurn = true;

		enemiesMoving = false;
	}
}