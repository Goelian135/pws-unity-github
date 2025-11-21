using UnityEngine;

public class SwordFollow : MonoBehaviour
{
    public Transform anchor;
    public CharacterController2D controller;

    [Header("Offsets")]
    public float offsetX = 1.5f;
    public float offsetY = 0.5f;

    [Header("Smoothness")]
    public float smoothTime = 0.15f;        

    private Vector3 velocity = Vector3.zero;

    private float targetOffsetX;

    // Houdt bij of het wapen VISUEEL naar rechts of links flipped
    private int visualFacing = 1;


    void Start()
    {
        // Initial setup
        visualFacing = controller.facing;
        targetOffsetX = offsetX * controller.facing;
    }

    void LateUpdate()
    {
        int facing = controller.facing; // 1 = rechts, -1 = links

        // Offset target (maar nog NIET flippen)
        targetOffsetX = offsetX * facing;

        // Bereken target positie
        Vector3 desiredPosition =
            anchor.position + new Vector3(targetOffsetX, offsetY, 0);

        // Smooth volgen
        transform.position = Vector3.SmoothDamp(
            transform.position,
            desiredPosition,
            ref velocity,
            smoothTime
        );

        // -----------------------------------------------------
        // ⭐ MID-SWING FLIP LOGICA
        // -----------------------------------------------------
        if (visualFacing != facing)
        {
            // Van links naar rechts draaien → wapen moet rechts van anchor komen
            if (facing == 1 && transform.position.x > anchor.position.x)
            {
                ApplyFlip(1);
            }
            // Van rechts naar links draaien → wapen moet links van anchor komen
            else if (facing == -1 && transform.position.x < anchor.position.x)
            {
                ApplyFlip(-1);
            }
        }
    }


    void ApplyFlip(int newFacing)
    {
        visualFacing = newFacing;

        // Flip sprite
        transform.localScale = new Vector3(newFacing, 1, 1);
    }
}
