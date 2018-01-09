using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour {

	private int health;

	void Awake ()
	{
		health = Game_Manager.instance.playerHealth;
	}

	public void DamagePlayer(int loss)
	{
		health -= loss;
	}
}