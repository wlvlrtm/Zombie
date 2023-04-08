using UnityEngine;

// 플레이어 캐릭터를 사용자 입력에 따라 움직이는 스크립트
public class PlayerMovement : MonoBehaviour {
    public float moveSpeed = 5f; // 앞뒤 움직임의 속도
    public float rotateSpeed = 180f; // 좌우 회전 속도

    private PlayerInput playerInput; // 플레이어 입력을 알려주는 컴포넌트
    private Rigidbody playerRigidbody; // 플레이어 캐릭터의 리지드바디
    private Animator playerAnimator; // 플레이어 캐릭터의 애니메이터

    private Plane plane;


    private void Start() {
        // 사용할 컴포넌트들의 참조를 가져오기
        playerInput = GetComponent<PlayerInput>();
        playerRigidbody = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();

        plane = new Plane(transform.up, transform.position);
    }

    // FixedUpdate는 물리 갱신 주기에 맞춰 실행됨
    private void FixedUpdate() {
        // 물리 갱신 주기마다 움직임, 회전, 애니메이션 처리 실행
        Move();
        Rotate();

    }

    // 입력값에 따라 캐릭터를 앞뒤로 움직임
    private void Move() {
        Vector3 camFront = Camera.main.transform.forward;
        Vector3 camRight = Camera.main.transform.right;
        
        camFront.y = 0;
        camRight.y = 0;

        camFront = camFront.normalized;
        camRight = camRight.normalized;

        Vector3 moveDirection = (camFront * playerInput.verticalValue) + (camRight * playerInput.horizontalValue);
        moveDirection.y = 0;

        Vector3 moveDistance = moveDirection * moveSpeed * Time.deltaTime;
        playerRigidbody.MovePosition(transform.position + moveDistance);

        float move;

        if (playerInput.verticalValue != 0 || playerInput.horizontalValue != 0) {
            move = (playerInput.verticalValue == 0) ? playerInput.horizontalValue : playerInput.verticalValue;
        } 
        else {
            move = 0;
        }

        playerAnimator.SetFloat("Move", move);
    }

    // 입력값에 따라 캐릭터를 좌우로 회전
    private void Rotate() {
        Vector3 hitPoint;
        Vector3 lookDirection;
        float enter;

        plane.Raycast(playerInput.mousePositionRay, out enter); // enter == distance
        
        hitPoint = playerInput.mousePositionRay.GetPoint(enter);
        lookDirection = hitPoint - transform.position;
        
        lookDirection.y = 0;

        transform.localRotation = Quaternion.LookRotation(lookDirection);
    }
}