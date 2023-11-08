using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VurmaSoundEffect : MonoBehaviour
{
    [SerializeField]
    AudioClip vurmaEffect1;
    [SerializeField]
    AudioClip vurmaEffect2;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void vurmaEfekti()
    {
        int randomizeEffect = Random.Range(1, 3);

        if (randomizeEffect == 1)
        {
            AudioSource.PlayClipAtPoint(vurmaEffect1, Camera.main.transform.position);
        }
        else
        {
            AudioSource.PlayClipAtPoint(vurmaEffect2, Camera.main.transform.position);
        }
    }
}
