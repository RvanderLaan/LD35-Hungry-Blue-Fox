using UnityEngine;
using System.Collections;

public class NextLevel : MonoBehaviour {

    GameObject character;

    float collideTime;
    bool collided = false;

    private Vector3 endPos;

	// Use this for initialization
	void Start () {
        character = GameObject.Find("CharContainer");
        collideTime = float.MaxValue;
    }
	
	// Update is called once per frame
	void Update () {
	    if (collided) {
            Camera.main.transform.position = Vector3.Slerp(Camera.main.transform.position, endPos, Time.deltaTime * .3f);

            if (Time.time > collideTime + 3) {
                character.GetComponent<Controller>().enabled = true;
                GameObject.Find("Levels").GetComponent<LevelLoader>().nextLevel();
            }

        }
	}

    void OnCollisionEnter(Collision collision) {
        character.GetComponent<Controller>().enabled = false;
        collideTime = Time.time;
        collided = true;

        Animator animator = character.GetComponent<Controller>().animator;
        animator.SetBool("isWalking", false);
        animator.SetBool("isRunning", false);

        endPos = Camera.main.transform.position - Camera.main.transform.forward * 8;
        GetComponent<AudioSource>().Play();
    }
}
