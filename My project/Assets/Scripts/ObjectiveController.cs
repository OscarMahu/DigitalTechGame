using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveController : MonoBehaviour
{
    // This function is called when a collisions is detected between a Objective in collision2D.
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // This checks if the Object colliding with the object is the player, if it is then it calls the CaptureObjective function in the GameController file, then after that it destroys the gameObject that had the collision.
        if(collision.gameObject.tag == "Player")
        {
            GameController.instance.CaptureObjective();
            Destroy(gameObject);
        }
    }
}
