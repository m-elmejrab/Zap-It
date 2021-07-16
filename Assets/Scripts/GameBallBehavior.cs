using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBallBehavior : MonoBehaviour {

    Rigidbody rb;

	// Use this for initialization
	void Awake () {

        rb = gameObject.GetComponent<Rigidbody>();
        rb.AddForce(new Vector3(-30f, 0f), ForceMode.Acceleration);
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    void OnCollisionEnter(Collision colInfo)
    {
        if (colInfo.transform.tag == "Zap")
        {
            Destroy(gameObject);
            GameManager.instance.CreateEnemy();
        }
        else if (colInfo.transform.tag == "Edges")
        {
            GameManager.instance.GameOver();
        }

    }

}
