using UnityEngine;
using UnityEngine.InputSystem;

public class InputPlayer : MonoBehaviour
{
    Vector2 moveInput;
    public float moveSpeed = 4f;
    Rigidbody2D rb;

    [SerializeField] private bool flip;
    [SerializeField] private float flipThreshold = 0.2f;
    public bool cantMove = false;

    void Awake() => rb = GetComponent<Rigidbody2D>();

    void FixedUpdate()
    {
        if (Time.timeScale == 0f) return;
        if (cantMove) return;

        // 물리 이동
        rb.velocity = new Vector2(moveInput.x * moveSpeed, rb.velocity.y);

        if (Mathf.Abs(moveInput.x) > flipThreshold)
        {
            if (moveInput.x > 0f && flip)       
                Flip();
            else if (moveInput.x < 0f && !flip) 
                Flip();
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed) rb.AddForce(Vector2.up * 15f, ForceMode2D.Impulse);
    }

    private void Flip()
    {
        flip = !flip;
        Vector3 scale = transform.localScale;
        scale.x *= -1f;
        transform.localScale = scale;
    }
}