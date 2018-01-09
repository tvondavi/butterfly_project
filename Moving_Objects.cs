using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Moving_Objects : MonoBehaviour {

	public float moveTime = 0.1f;
	public LayerMask BlockingLayer;

	private BoxCollider2D boxCollider;
	private Rigidbody2D rb2d;
	private float inverseMoveTime;

	// Use this for initialization
	protected virtual void Start ()
	{
		boxCollider = GetComponent<BoxCollider2D> ();
		rb2d = GetComponent<Rigidbody2D> ();
		inverseMoveTime = 1f / moveTime;
	}

	protected bool Move (int xDir, int yDir, out RaycastHit2D hit)
	{
		Vector2 start = transform.position;
		Vector2 end = start + new Vector2 (xDir, yDir);

		boxCollider.enabled = false;
		hit = Physics2D.Linecast (start, end, BlockingLayer);
		boxCollider.enabled = true;

		if (hit.transform == null) 
		{
			StartCoroutine (Movement (end));
			return true;
		}

		return false;
	}

	protected IEnumerator Movement (Vector3 end)
	{
		float sqrRemainDistance = (transform.position - end).sqrMagnitude;

		while (sqrRemainDistance > float.Epsilon) 
		{
			Vector3 newPosition = Vector3.MoveTowards (rb2d.position, end, inverseMoveTime * Time.deltaTime);
			rb2d.MovePosition (newPosition);
			sqrRemainDistance = (transform.position - end).sqrMagnitude;
			yield return null;
		}
	}

	protected virtual void AttemptMove <T> (int xDir, int yDir)
		where T : Component
	{
		RaycastHit2D hit;
		bool canMove = Move (xDir, yDir, out hit);

		if (hit.transform == null)
			return;

		T hitComponent = hit.transform.GetComponent<T> ();

		if (!canMove && hitComponent != null)
		{
			CantMove(hitComponent);
		}
	}

	protected abstract void CantMove <T> (T component)
		where T : Component;
}
