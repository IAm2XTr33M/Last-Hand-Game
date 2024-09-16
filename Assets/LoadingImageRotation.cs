using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingImageRotation : MonoBehaviour
{
    [SerializeField] float speed = 270;

    RawImage img;
    void Start()
    {
        img = GetComponent<RawImage>();
    }

    void Update()
    {
        img.rectTransform.eulerAngles -= new Vector3(0, 0, Time.deltaTime * speed);
    }
}
