using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] float maxLookX = 30;
    [SerializeField] float maxLookY = 5;

    [SerializeField] bool fancyFollow = true;
    [SerializeField] float fancyFollowDelay = 0.5f;
    bool startFollow = false;

    List<Vector3> rotations = new List<Vector3>();

    private void Start()
    {
        if (fancyFollow)
        {
            StartCoroutine(StartFancyFollow());
        }
    }

    IEnumerator StartFancyFollow() { yield return new WaitForSeconds(fancyFollowDelay); startFollow = true; }
    
    void Update()
    {
        Vector3 mousePosition = Input.mousePosition;

        float xPercentage = Mathf.Clamp((mousePosition.x / Screen.width) * 100f, 0, 100);
        float yPercentage = Mathf.Clamp((mousePosition.y / Screen.height) * 100f, 0, 100);

        float xRot = ((xPercentage - 50) * 2) / 100 * maxLookX;
        float yRot = ((yPercentage - 50) * 2) / 100 * maxLookY;

        if (fancyFollow)
        {
            rotations.Add(new Vector3(-yRot, xRot, 0));
            if (startFollow)
            {
                transform.localEulerAngles = rotations[0];
                rotations.RemoveAt(0);
            }
        }
        else
        {
            transform.localEulerAngles = new Vector3(-yRot, xRot, 0);
        }

    }
}
