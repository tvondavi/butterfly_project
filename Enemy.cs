using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Moving_Objects {

	public int playerDamage;

	private Animator animator;
	private Transform target;
	private bool Skip;

	protected override void Start () 
	{
		Game_Manager.instance.AddEnemyToList (this);
		animator = GetComponent<Animator> ();
		target = GameObject.FindGameObjectWithTag ("Player").transform;
		base.Start ();
	}

	protected override void AttemptMove <T> (int xDir, int yDir)
	{
		if (Skip) 
		{
			Skip = false;
			return;
		}

		base.AttemptMove <T> (xDir, yDir);

		Skip = true;
	}

	public void EnemyMove ()
	{
		int xDir = 0;
		int yDir = 0;

		if (Mathf.Abs (target.position.x - transform.position.x) < float.Epsilon)
			yDir = target.position.y > transform.position.y ? 1 : -1;
		else
			xDir = target.position.x > transform.position.x ? 1 : -1;

		AttemptMove <Player> (xDir, yDir);
	}

	protected override void CantMove <T> (T component)
	{
		Player hitPlayer = component as Player;
		hitPlayer.LoseHealth (playerDamage);
		animator.SetTrigger ("enemy_attac");
	}
}
