using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioGenerate : MonoBehaviour
{
    public float duration = 2f;

    private void Start()
    {
        StartCoroutine(WaitTime(duration));
    }

    IEnumerator WaitTime(float sec)
    {
        yield return new WaitForSeconds(sec);

        Destroy(this.gameObject);
    }
}
