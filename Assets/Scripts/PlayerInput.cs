﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInput : MonoBehaviour
{
    [SerializeField]
    private float speed;
    Rigidbody rb;
    Animator animator;
	public GameObject Player;
	public GameObject Menu;
	public GameObject Options;
	public Button Inventory;
	public Vector3 playerPosition;
	public bool isPaused;

    // Use this for initialization
    void Start()
    {
        speed = 4;
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
		isPaused = false;
    }

    // Update is called once per frame
    void Update()
    {
		// If the game is unpaused, pause it and vice versa
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (isPaused == true) 
			{
				if (Options.activeSelf) 
				{
					Options.SetActive(false);
					Menu.SetActive(true);
				} 
				else 
				{
					Menu.SetActive(false);
					Options.SetActive(false);
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
			animator.SetBool("isRunning", true);
            speed = 8;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
			animator.SetBool("isRunning", false);
            speed = 4;
        }

        float xMove = Input.GetAxis("Horizontal") * speed;
        float yMove = Input.GetAxis("Vertical") * speed;

        Vector3 heading = new Vector3(xMove, 0, yMove).normalized;
        float angle = Vector3.Angle(Vector3.forward, heading);
        if (xMove < 0) {
            angle = -angle;
        }

        /*
        if (Input.GetAxisRaw("Horizontal") > 0) {
            rb.transform.Rotate(Vector3.up, Utility.DesireSmoothAngle(rb.transform.rotation.eulerAngles.y, 90));
        } else if (Input.GetAxisRaw("Horizontal") < 0) {
            rb.transform.Rotate(Vector3.up, Utility.DesireSmoothAngle(rb.transform.rotation.eulerAngles.y, -90));
        } else if (Input.GetAxisRaw("Vertical") > 0) {
            rb.transform.Rotate(Vector3.up, Utility.DesireSmoothAngle(rb.transform.rotation.eulerAngles.y, 0));
        } else if (Input.GetAxisRaw("Vertical") < 0) {
            rb.transform.Rotate(Vector3.up, Utility.DesireSmoothAngle(rb.transform.rotation.eulerAngles.y, 180));
        }
        */

        //rb.AddForce(Vector3.right * xMove + Vector3.forward * yMove);

        if (xMove != 0 || yMove != 0) {
            rb.transform.Rotate(Vector3.up, Utility.DesireSmoothAngle(rb.transform.rotation.eulerAngles.y, angle, 12));
        }

        /*
        if (Mathf.Abs((rb.transform.rotation.eulerAngles.y + 360) % 360 - (angle + 360) % 360) < 30) {
            rb.velocity = Vector3.right * xMove + Vector3.forward * yMove;
        }
        */
        rb.velocity = Vector3.right * xMove + Vector3.forward * yMove;

        if (xMove == 0 && yMove == 0)
        {
            animator.SetBool("isWalking", false);
        } else animator.SetBool("isWalking", true);

		if (Input.GetKeyDown(KeyCode.Q))
		{
			animator.SetBool("isAttacking", true);
		}else animator.SetBool("isAttacking", false);
        
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
    //void Press(Vector3 direction)
    //{
    //    Rigidbody rb = this.GetComponent<Rigidbody>();

    //}

    void OnCollisionEnter(Collision o)
    {
        if (o.gameObject.tag == "Wall")
        {

        }
    }

	void OnGUI()
	{
		// placeholder for GUI stuff
	}
}