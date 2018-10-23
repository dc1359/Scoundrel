using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInput : MonoBehaviour {
	[SerializeField] private float speed;
	public GameObject Player;
	public GameObject Menu;
	public GameObject Options;
	public Button Inventory;
	public Vector3 playerPosition;
	public bool isPaused;

	// Use this for initialization
	void Start () {
		Player = GameObject.Find("Player");
		speed = 4;
		isPaused = false;
	}

	// Update is called once per frame
	void Update () {
		Rigidbody rb = this.GetComponent<Rigidbody>();
		playerPosition = Player.transform.position;

		// If the game is unpaused, pause it and vice versa
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (isPaused == true) 
			{
				if (Options.activeSelf) 
				{
					Options.SetActive (false);
					Menu.SetActive (true);
				} 
				else 
				{
					Menu.SetActive(false);
					isPaused = false;
					Time.timeScale = 1;
				}
			} 
			else 
			{
				isPaused = true;
				Time.timeScale = 0;
				Menu.SetActive(true);
				Inventory.Select();
			}
		}

		if (Input.GetKeyDown(KeyCode.LeftShift))
		{
			speed = 8;
		}

		if (Input.GetKeyUp(KeyCode.LeftShift))
		{
			speed = 4;
		}

		float xMove = Input.GetAxis("Horizontal") * speed;
		float yMove = Input.GetAxis("Vertical") * speed;

		//rb.AddForce(Vector3.right * xMove + Vector3.forward * yMove);

		rb.velocity = Vector3.right * xMove + Vector3.forward * yMove;
		//rb.angularVelocity = Vector3.right * xMove + Vector3.forward * yMove;

		if ((Input.GetAxis("Horizontal") == 0) && (Input.GetAxis("Vertical") == 0))
		{
			rb.velocity = Vector3.zero;
			rb.angularVelocity = Vector3.zero;
			rb.Sleep();
		}
	}

	/**
     * -Incomplete- Script to shrink component for reducing hitbox related to sight
     */
	void Press (Vector3 direction) {
		Rigidbody rb = this.GetComponent<Rigidbody>();

	}

	void OnCollisionEnter (Collision o) { 
		if (o.gameObject.tag == "Wall")
		{

		}
	}

	void OnGUI()
	{
		// placeholder for GUI stuff
	}
}