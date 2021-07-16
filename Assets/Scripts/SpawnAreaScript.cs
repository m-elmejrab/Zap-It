using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnAreaScript : MonoBehaviour {



    // Use this for initialization
    void Awake () {

        
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnMouseDown()
    {  
        
        Ray raycast = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        RaycastHit raycastHit;
        if (Physics.Raycast(raycast, out raycastHit))
        {
            if (raycastHit.collider.name == gameObject.name)
            {

                GameManager.instance.CreatePlayer(raycastHit.point);
            }
        }
        
    }

}
