using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateEffect : MonoBehaviour
{
    [SerializeField] private float rotateSpeed = 300f;
    [SerializeField] private bool isPlay = true;

    private void Update()
    {
        if (isPlay == false) return;
        transform.Rotate(Vector3.forward, rotateSpeed * Time.unscaledDeltaTime);
    }
}
