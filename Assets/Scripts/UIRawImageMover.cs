using UnityEngine;
using UnityEngine.UI;

public class UIRawImageMover : MonoBehaviour
{
    // Public fields for setting points in the Inspector
    public RawImage uiRawImage;    // The UI RawImage you want to move
    public Vector2 pointA;         // Starting point (UI space in Inspector)
    public Vector2 pointB;         // End point (UI space in Inspector)
    public float moveSpeed = 2.0f; // Speed of movement

    private Vector2 targetPosition;
    private bool movingToB = true;

    void Start()
    {
        if (uiRawImage != null)
        {
            // Set the initial target to pointB
            targetPosition = pointA;

            // Initialize the position of the RawImage to pointA
            uiRawImage.rectTransform.anchoredPosition = pointA;
        }
    }

    void Update()
    {
        if (uiRawImage != null)
        {
            // Move the RawImage smoothly towards the target position
            uiRawImage.rectTransform.anchoredPosition = Vector2.MoveTowards(uiRawImage.rectTransform.anchoredPosition, targetPosition, moveSpeed * Time.deltaTime);

            // If the RawImage reaches the target position, switch the target
            if (Vector2.Distance(uiRawImage.rectTransform.anchoredPosition, targetPosition) < 0.01f)
            {
                if (movingToB)
                {
                    targetPosition = pointA; // Switch to move back to pointA
                }
                else
                {
                    targetPosition = pointB; // Switch to move to pointB
                }

                movingToB = !movingToB; // Toggle between moving to pointA and pointB
            }
        }
    }
}
