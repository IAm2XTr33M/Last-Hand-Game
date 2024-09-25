using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayingCard : MonoBehaviour
{
    [SerializeField] int number;
    [SerializeField] float animationTime = 2;
    [SerializeField] AnimationCurve positionAnimationCurve;
    [SerializeField] AnimationCurve jumpAnimationCurve;
    [SerializeField] float jumpHeight = 1;
    [SerializeField] AnimationCurve flipAnimationCurve;
    [SerializeField] Vector3 normalFlip = Vector3.zero;
    [SerializeField] Vector3 hiddenFlip = Vector3.zero;

    [SerializeField] Vector3 testPos;

    bool isDrawn = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DrawCard(testPos);
        }
    }

    IEnumerator LerpPosition(Vector3 targetPos)
    {
        Vector3 startPos = transform.position;
        targetPos = new Vector3(targetPos.x, startPos.y, targetPos.z);

        WaitForEndOfFrame wait = new WaitForEndOfFrame();
        float currentAnimationTime = 0;

        while (currentAnimationTime < 1)
        {
            currentAnimationTime += Time.deltaTime / animationTime;
            yield return wait;

            Vector3 lerpedPos = Vector3.Lerp(startPos, targetPos, positionAnimationCurve.Evaluate(currentAnimationTime));
            transform.position = new Vector3(lerpedPos.x, transform.position.y, lerpedPos.z);
        }
        transform.position = new Vector3(targetPos.x, transform.position.y, targetPos.z);
    }

    IEnumerator LerpJump()
    {
        float startYPos = transform.position.y;
        float targetYPos = transform.position.y + jumpHeight;

        WaitForEndOfFrame wait = new WaitForEndOfFrame();
        float currentAnimationTime = 0;
        while (currentAnimationTime < 1)
        {
            currentAnimationTime += Time.deltaTime / animationTime;
            yield return wait;

            float yPos = Mathf.Lerp(startYPos, targetYPos, jumpAnimationCurve.Evaluate(currentAnimationTime));
            transform.position = new Vector3(transform.position.x, yPos, transform.position.z);
        }
        transform.position = new Vector3(transform.position.x, startYPos, transform.position.z);
    }

    IEnumerator LerpFlip(Vector3 flip)
    {
        Vector3 startRot = transform.eulerAngles;
        Vector3 endRot = transform.eulerAngles + flip;

        WaitForEndOfFrame wait = new WaitForEndOfFrame();
        float currentAnimationTime = 0;
        while (currentAnimationTime < 1)
        {
            currentAnimationTime += Time.deltaTime / animationTime;
            yield return wait;

            transform.eulerAngles = Vector3.Lerp(startRot, endRot, flipAnimationCurve.Evaluate(currentAnimationTime));
        }
        transform.eulerAngles = endRot;
    }

    public void DrawCard(Vector3 pos, bool hidden = false , bool me = false)
    {
        if (!isDrawn)
        {
            isDrawn = true;
            StartCoroutine(LerpPosition(pos));
            StartCoroutine(LerpJump());
            if (!hidden)
            {
                if (me)
                {
                    StartCoroutine(LerpFlip(normalFlip));
                }
                else
                {
                    StartCoroutine(LerpFlip(-normalFlip));
                }
            }
            else
            {
                if (me)
                {
                    StartCoroutine(LerpFlip(hiddenFlip));
                }
                else
                {
                    StartCoroutine(LerpFlip(-hiddenFlip));
                }
            }
        }
    }
}
