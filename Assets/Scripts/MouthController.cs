using UnityEngine;

public class MouthController : MonoBehaviour {
    public GameObject[] mouths;
    private Animator[] mouthAnimators;

    private float timeToNextEmotion = 5.0f;
    private int emotionValue = 0;

    // Use this for initialization
    void Start() {
        if (mouths.Length > 0) {
            mouthAnimators = new Animator[mouths.Length];
            for (int index = 0; index < mouths.Length; index++) {
                GameObject mouth = mouths[index];
                Animator mouthAnimator = mouth.GetComponent<Animator>();
                if (mouthAnimator != null) {
                    mouthAnimators[index] = mouthAnimator;
                }
                else {
                    Debug.LogError("Failed to find animator on object " + mouth.name);
                }
            }
        }
    }

    // Update is called once per frame
    void Update() {
        if (mouthAnimators.Length > 0) {
            UpdateMouth();
        }
    }

    private void UpdateMouth() {
        timeToNextEmotion -= Time.deltaTime;

        if (timeToNextEmotion <= 0.0f) {
            emotionValue = 1 - emotionValue; //Random.Range(0, 1);
            Debug.Log("Change emotion - " + emotionValue);

            for (int index = 0; index < mouthAnimators.Length; index++) {
                mouthAnimators[index].SetInteger("emotion", emotionValue);
            }

            timeToNextEmotion = Random.Range(5.0f, 10.0f);
        }
    }
}
