using UnityEngine;

public class EyesController : MonoBehaviour {
    public GameObject[] eyelids;
    private Animator[] eyelidAnimators;
    private float timeToNextBlink = 5.0f;

	// Use this for initialization
	void Start () {

        if (eyelids.Length > 0) {
            eyelidAnimators = new Animator[eyelids.Length];
            for (int index = 0; index < eyelids.Length; index++) {
                GameObject eyelid = eyelids[index];
                Animator eyelidAnimator = eyelid.GetComponent<Animator>();
                if (eyelidAnimator != null) {
                    eyelidAnimators[index] = eyelidAnimator;
                }
                else {
                    Debug.LogError("Failed to find animator on object " + eyelid.name);
                }
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
		if(eyelidAnimators.Length > 0) {
            timeToNextBlink -= Time.deltaTime;

            if(timeToNextBlink <= 0.0f) {
                for(int index = 0; index < eyelidAnimators.Length; index++) {
                    eyelidAnimators[index].SetTrigger("blink");
                }

                timeToNextBlink = Random.Range(2.0f, 5.0f);
            }
        }
	}
}
