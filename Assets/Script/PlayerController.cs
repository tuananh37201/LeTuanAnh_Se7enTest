using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Animator animator;
    public Transform mainCamera;
    public float speed;
    public float boundX, boundZ;
    public float turnSmoothTime = 0.1f;

    private Rigidbody rb;
    private float xInput, zInput;
    private float turnSmoothVelocity;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        Movement();
    }

    private void Movement()
    {

        // nhận đầu vào nút di chuyển (WASD)
        xInput = Input.GetAxisRaw("Horizontal");
        zInput = Input.GetAxisRaw("Vertical");
        Vector3 inputDir = new Vector3 (xInput, 0, zInput);

        // nếu đầu vào lớn hơn 0.1
        if(inputDir.magnitude >= 0.1f)
        {
            // tính toán góc xoay của người chơi dựa theo hướng di chuyển
            float targetAngle = Mathf.Atan2(inputDir.x, inputDir.z) * Mathf.Rad2Deg + mainCamera.eulerAngles.y;
            // xoay người chơi theo hướng di chuyển
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            // tạo một vector hướng theo hướng mà người chơi di chuyển dựa trên ý định xoay
            Vector3 moveDir = (Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward).normalized;
            // đặt vật tốc của người chơi dựa trên hướng di chuyển và tốc độ
            rb.velocity = moveDir * speed;

            // giới hạn phạm di vi di chuyển của người chơi trong sân bóng
            Vector3 position = transform.position;
            position.x = Mathf.Clamp(position.x, -boundX, boundX);
            position.z = Mathf.Clamp(position.z, -boundZ, boundZ);
            transform.position = position;

            // chạy animation Run
            animator.SetBool("isRunning", true);
        }
        else
        {
            // đặt vận tốc về 0 khi không có tín hiệu di chuyển đầu vào
            rb.velocity = Vector3.zero;
            // chạy animation Idle
            animator.SetBool("isRunning", false);
        }
    }
}
