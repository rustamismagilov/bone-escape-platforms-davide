using UnityEngine;

public class TouchingGroundHandler : MonoBehaviour
{
    [SerializeField] Collider2D touchingGroundCollider;
    public bool isTouchingGround { get; private set; }

    // Update is called once per frame
    void Update()
    {
        CheckTouchGround();
    }

    // check if is touching the ground
    void CheckTouchGround()
    {
        isTouchingGround = touchingGroundCollider.IsTouchingLayers(LayerMask.GetMask("Ground", "Enemy"));
    }
}
