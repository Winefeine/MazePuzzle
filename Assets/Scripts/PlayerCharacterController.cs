using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Rendering;

[RequireComponent(typeof(CharacterController),typeof(PlayerInputHandler))]
public class PlayerCharacterController : MonoBehaviour
{
    [Header("References")] 
    public Camera PlayerCamera;
    public AudioSource AudioSource;
    [Header("General")]
    public float GravityDownForce = 20f;
    public LayerMask GroundCheckLayers = -1;
    public float GroundCheckDistance = 0.05f;
    [Header("Camera")]
    public float CameraHeightRatio = 0.9f;
    [Header("Movement")]
    public float MaxSpeedOnGround = 10f;
    public float MaxSpeedInAir = 10f;
    public float MovementSharpnessOnGround = 15f;
    public float AccelerationSpeedInAir = 25f;
    public float RotationSpeed = 200f;
    public float RotationMultiplier = 0.4f;
    [Header("Jump")]
    public float JumpForce = 9f;
    
    public Vector3 CharacterVelocity { get; set; }
    public bool IsGrounded { get; private set; }
    public bool HasJumpedThisFrame { get; private set; }

    [Header("Test")]
    public bool ground;

    PlayerInputHandler inputHandler;
    CharacterController controller;
    CameraController cameraController;
    Camera playerCamera;
    Vector3 groundNormal;
    Vector3 latestImpactSpeed;
    float lastTimeJumped = 0f;

    const float jumpGroundingPreventionTime = 0.2f;
    const float groundCheckDistanceInAir = 0.07f;

    private void Start()
    {
        inputHandler = GetComponent<PlayerInputHandler>();
        controller = GetComponent<CharacterController>();
        cameraController = GetComponentInChildren<CameraController>();
        cameraController.GetCameraHeightRatio(CameraHeightRatio);
        playerCamera = GetComponentInChildren<Camera>();

    }


    // Update is called once per frame
    void Update()
    {
        HasJumpedThisFrame = false;

        bool wasGrounded = IsGrounded;
        GroundCheck();

        //land
        if (IsGrounded && !wasGrounded)
        {
            //play land audio sfx
        
        }

        HandleCharacterMovement();
        ground = IsGrounded;
    }

    void GroundCheck()
    {
        float chosenGroundCheckDistance =
            IsGrounded ? (controller.skinWidth + GroundCheckDistance) : groundCheckDistanceInAir;

        //reset values
        IsGrounded = false;
        groundNormal = Vector3.up;

        //detect ground
        if (Time.time >= lastTimeJumped + jumpGroundingPreventionTime)
        {
            if (Physics.CapsuleCast(GetCapsuleBottomHemisphere(), GetCapsuleTopHemisphere(controller.height),
                controller.radius, Vector3.down, out RaycastHit hit, chosenGroundCheckDistance, GroundCheckLayers,
                QueryTriggerInteraction.Ignore))
            {
                groundNormal = hit.normal;
                if (Vector3.Dot(hit.normal, transform.up) > 0f &&
                    IsNormalUnderSlopeLimit(groundNormal))
                {
                    IsGrounded = true;
                    if (hit.distance > controller.skinWidth)
                    {
                        controller.Move(Vector3.down * hit.distance);
                    }
                }
            }
        }
    }

    void HandleCharacterMovement()
    {
        // horizontal character rotation
        transform.Rotate(
            new Vector3(0f, (inputHandler.GetLookInputsHorizontal() * RotationSpeed),
                0f), Space.Self);  
        //PlayerCamera.transform.localEulerAngles = m_CameraController.CameraRotation(m_InputHandler.GetLookInputsVertical() * RotationSpeed * RotationMultiplier);
        // PlayerCamera.transform.localRotation = Quaternion.Euler(inputHandler.GetLookInputsHorizontal()+inputHandler.GetLookInputsHorizontal(),
        //     transform.localEulerAngles.y,transform.localEulerAngles.z);
        //PlayerCamera.transform.localEulerAngles = cameraController.CameraRotation( * RotationSpeed);
        //cameraController.CameraRotation(inputHandler.GetLookInputsHorizontal(),inputHandler.GetLookInputsVertical());
        playerCamera.transform.localEulerAngles = cameraController.CameraRotation(inputHandler.GetLookInputsVertical()*RotationSpeed*RotationMultiplier);

        //character movement handling
        float speedModifier = 1f;
        
        Vector3 worldspaceMoveInput = transform.TransformVector(inputHandler.GetMoveInput());

        if (IsGrounded)
        {
            Vector3 targetVelocity = worldspaceMoveInput * MaxSpeedOnGround * speedModifier;

            CharacterVelocity = Vector3.Lerp(CharacterVelocity, targetVelocity,
                    MovementSharpnessOnGround * Time.deltaTime);

            //jumping
            if (IsGrounded && inputHandler.GetJumpInputDown())
            {
                CharacterVelocity = new Vector3(CharacterVelocity.x, 0f, CharacterVelocity.z);
                CharacterVelocity += Vector3.up * JumpForce;

                //playJumpAudio

                lastTimeJumped = Time.time;
                HasJumpedThisFrame = true;

                IsGrounded = false;
                groundNormal = Vector3.up;
            }
        }else
        {
            //add air acceleration
            CharacterVelocity += worldspaceMoveInput * AccelerationSpeedInAir * Time.deltaTime;
                
            //horizontal air speed limit
            float verticalVelocity = CharacterVelocity.y;
            Vector3 horizontalVelocity = Vector3.ProjectOnPlane(CharacterVelocity, Vector3.up);
            horizontalVelocity = Vector3.ClampMagnitude(horizontalVelocity, MaxSpeedInAir * speedModifier);
            CharacterVelocity = horizontalVelocity + (Vector3.up * verticalVelocity);

            //gravity
            CharacterVelocity += Vector3.down * GravityDownForce * Time.deltaTime;    
        }

        //final velocity
        Vector3 capsuleBottomBeforeMove = GetCapsuleBottomHemisphere();
        Vector3 capsuleTopBeforeMove = GetCapsuleTopHemisphere(controller.height);
        controller.Move(CharacterVelocity * Time.deltaTime);

        // detect obstructions 
        latestImpactSpeed = Vector3.zero;
        if (Physics.CapsuleCast(capsuleBottomBeforeMove, capsuleTopBeforeMove, controller.radius,
            CharacterVelocity.normalized, out RaycastHit hit, CharacterVelocity.magnitude * Time.deltaTime, -1,
            QueryTriggerInteraction.Ignore))
        {
            latestImpactSpeed = CharacterVelocity;

            CharacterVelocity = Vector3.ProjectOnPlane(CharacterVelocity, hit.normal);
        }
    }

    bool IsNormalUnderSlopeLimit(Vector3 normal)
    {
        return Vector3.Angle(transform.up, normal) <= controller.slopeLimit;
    }

    Vector3 GetCapsuleBottomHemisphere()
    {
        return transform.position + (transform.up * controller.radius);
    }
    
    Vector3 GetCapsuleTopHemisphere(float atHeight)
    {
        return transform.position + (transform.up * (atHeight - controller.radius));
    }


}
