using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour {
    [SerializeField] private float speed;

    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        Rigidbody rb = this.GetComponent<Rigidbody>();

        float xMove = Input.GetAxis("Horizontal") * speed;
        float yMove = Input.GetAxis("Vertical") * speed;


        rb.AddForce(Vector3.right * xMove + Vector3.forward * yMove);
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
}
