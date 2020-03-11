using UnityEngine;
using System.Collections;

public class LevelLoader : MonoBehaviour {

    public int level = 0;
    private GameObject character;

	// Use this for initialization
	void Start () {
        for (int i = 0; i < transform.childCount; i++) {
            transform.GetChild(i).gameObject.SetActive(false);
        }
        transform.GetChild(level).gameObject.SetActive(true);

        character = GameObject.Find("CharContainer");

	}

    public void nextLevel() {
        if (level >= 0)
            transform.GetChild(level).gameObject.SetActive(false);
        level++;
        reset();

        if (level < transform.childCount)
            transform.GetChild(level).gameObject.SetActive(true);
        else
            Application.LoadLevel(Application.loadedLevel + 1);

      
    }

    public void reset() {
        character.transform.position = new Vector3(0, 0, 0);
        character.GetComponent<Controller>().shiftChar(false);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
