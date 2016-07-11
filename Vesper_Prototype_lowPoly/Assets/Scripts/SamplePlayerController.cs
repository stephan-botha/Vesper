/************************************************************************************

Copyright   :   Copyright 2014 Oculus VR, LLC. All Rights reserved.

Licensed under the Oculus VR Rift SDK License Version 3.2 (the "License");
you may not use the Oculus VR Rift SDK except in compliance with the License,
which is provided at the time of installation or download, or which
otherwise accompanies this software in either electronic or hard copy form.

You may obtain a copy of the License at

http://www.oculusvr.com/licenses/LICENSE-3.2

Unless required by applicable law or agreed to in writing, the Oculus VR SDK
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.

************************************************************************************/
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Controls the player's movement in virtual reality.
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class SamplePlayerController : OVRPlayerController
{
      

    public OVRInput.Button runButton = OVRInput.Button.One;
    public OVRInput.Button speedDecrease = OVRInput.Button.PrimaryIndexTrigger;
    public OVRInput.Button speedIncrease = OVRInput.Button.SecondaryIndexTrigger;


    public float maxFlySpeed = 32.0f;
    public float minFlySpeed = 6.0f;
    float currentFlySpeed = 0.0f;
    public float FPS = 0.0f;
    public Vector3 cn;

    public Vector3 pn;
    public Quaternion playerDirection1;
    
    public Vector3 moveDirection1;


    public bool rotationSnap  = false;
    float PendingRotation = 0;
    float SimulationRate_ = 60f;
    private bool prevHatLeft_;
    private bool prevHatRight_;
    private OVRPose? InitialPose_;
    private Vector3 MoveThrottle_ = Vector3.zero;
    private float FallSpeed_ = 0.0f;
	private float InitialYRotation_ = 0.0f;

    public float axisDeadZone = 0.9f;
    
    float rotationAnimation = 0;
    float targetYaw = 0;
    // float animationStartAngle;
    bool animating;

    float lastFPSValue = 0.0f;
    int lastFrameIndex = 0;
    float lastFrameRateUpdateTime = 0.0f;
    float frameRateUpdateInterval = 1.5f;


    void Awake()
    { 
        Controller = gameObject.GetComponent<CharacterController>();

        if (Controller == null)
            Debug.LogWarning("OVRPlayerController: No CharacterController attached.");

        // We use OVRCameraRig to set rotations to cameras,
        // and to be influenced by rotation
        List<OVRCameraRig> cameraRigs = new List<OVRCameraRig>();
        foreach (Transform child in transform)
        {
            OVRCameraRig childCameraRig = child.gameObject.GetComponent<OVRCameraRig>();
            if (childCameraRig != null)
            {
                cameraRigs.Add(childCameraRig);
            }
        }

        if (cameraRigs.Count == 0)
            Debug.LogWarning("OVRPlayerController: No OVRCameraRig attached.");
        else if (cameraRigs.Count > 1)
            Debug.LogWarning("OVRPlayerController: More then 1 OVRCameraRig attached.");
        else
            CameraRig = cameraRigs[0];

        InitialYRotation_ = transform.rotation.eulerAngles.y;

        //currentFlySpeed = minFlySpeed;
    }

    protected new void Update()
    {
        //if (useProfileData)
        //{
        //    if (InitialPose_ == null)
        //    {
        //        InitialPose_ = new OVRPose()
        //        {
        //            position = CameraRig.transform.localPosition,
        //            orientation = CameraRig.transform.localRotation
        //        };
        //    }

        //    var p = CameraRig.transform.localPosition;
        //    p.y = OVRManager.profile.eyeHeight - 0.5f * Controller.height;
        //    p.z = OVRManager.profile.eyeDepth;
        //    CameraRig.transform.localPosition = p;
        //}
        //else if (InitialPose_ != null)
        //{
        //    CameraRig.transform.localPosition = InitialPose_.Value.position;
        //    CameraRig.transform.localRotation = InitialPose_.Value.orientation;
        //    InitialPose_ = null;
        //}

        UpdateMovement();

        Vector3 moveDirection = Vector3.zero;
        

        float motorDamp = (1.0f + (Damping * SimulationRate_ * Time.deltaTime));

        MoveThrottle_.x /= motorDamp;
       // MoveThrottle_.y = (MoveThrottle_.y > 0.0f) ? (MoveThrottle_.y / motorDamp) : MoveThrottle_.y;
        MoveThrottle_.z /= motorDamp;
        MoveThrottle_.y /= motorDamp;

        moveDirection += MoveThrottle_ * SimulationRate_ * Time.deltaTime;
        moveDirection1 = moveDirection;

        // Gravity
        //if (Controller.isGrounded && FallSpeed_ <= 0)
        //    FallSpeed_ = ((Physics.gravity.y * (GravityModifier * 0.002f)));
        //else
        //    FallSpeed_ += ((Physics.gravity.y * (GravityModifier * 0.002f)) * SimulationRate_ * Time.deltaTime);

        //moveDirection.y += FallSpeed_ * SimulationRate_ * Time.deltaTime;

        //// Offset correction for uneven ground
        //float bumpUpOffset = 0.0f;

        //if (Controller.isGrounded && MoveThrottle_.y <= 0.001f)
        //{
        //    bumpUpOffset = Mathf.Max(Controller.stepOffset, new Vector3(moveDirection.x, 0, moveDirection.z).magnitude);
        //    moveDirection -= bumpUpOffset * Vector3.up;
        //}

        Vector3 predictedXZ = Vector3.Scale((Controller.transform.localPosition + moveDirection), new Vector3(1, 1, 1));

        // Move contoller
        Controller.Move(moveDirection);

        Vector3 actualXZ = Vector3.Scale(Controller.transform.localPosition, new Vector3(1, 1, 1));

        if (predictedXZ != actualXZ)
            MoveThrottle_ += (actualXZ - predictedXZ) / (SimulationRate_ * Time.deltaTime);

        updateFPS();
        
    }

    void updateFPS()
    {
        if (Time.unscaledTime > lastFrameRateUpdateTime + frameRateUpdateInterval)
        {
            lastFPSValue = (Time.frameCount - lastFrameIndex) / frameRateUpdateInterval;
            lastFrameIndex = Time.frameCount;
            lastFrameRateUpdateTime = Time.unscaledTime;
            FPS = lastFPSValue;
        }
    }

    void OnEnable()
    {
    }

    void OnDisable()
    {
    }
    float AngleDifference(float a, float b)
    {
        float diff = (360 + a - b) % 360;
        if (diff > 180)
            diff -= 360;
        return diff;
    }
    public override void UpdateMovement()
    {
        bool HaltUpdateMovement = false;
        GetHaltUpdateMovement(ref HaltUpdateMovement);
        if (HaltUpdateMovement)
            return;

        float MoveScaleMultiplier = 1;
        GetMoveScaleMultiplier(ref MoveScaleMultiplier);

        float RotationScaleMultiplier = 1;
        GetRotationScaleMultiplier(ref RotationScaleMultiplier);

        bool SkipMouseRotation = false;
        GetSkipMouseRotation(ref SkipMouseRotation);

        float MoveScale = 1.0f;
        // No positional movement if we are in the air
        //if (!Controller.isGrounded)
        //    MoveScale = 0.0f;

        MoveScale *= SimulationRate_ * Time.deltaTime;

        //Quaternion playerDirection = new Quaternion(            
        //    CameraRig.centerEyeAnchor.rotation.x,
        //    transform.rotation.y,
        //    CameraRig.centerEyeAnchor.rotation.z,
        //    CameraRig.centerEyeAnchor.rotation.w
        //    );

        Quaternion playerDirection = ((HmdRotatesY) ? CameraRig.centerEyeAnchor.rotation : transform.rotation);

        //remove any pitch + yaw components
        //playerDirection = Quaternion.Euler(
        //    playerDirection.eulerAngles.x,
        //    playerDirection.eulerAngles.y,
        //    playerDirection.eulerAngles.z);

       
        playerDirection = Quaternion.Euler(
           CameraRig.centerEyeAnchor.rotation.eulerAngles.x,
           transform.rotation.eulerAngles.y,
            CameraRig.centerEyeAnchor.rotation.eulerAngles.z);
        



        //playerDirection = Quaternion.identity;

        Vector3 euler = transform.rotation.eulerAngles;

        bool stepLeft = false;
        bool stepRight = false;
        stepLeft = OVRInput.GetDown(OVRInput.Button.PrimaryShoulder) || Input.GetKeyDown(KeyCode.Q);
        stepRight = OVRInput.GetDown(OVRInput.Button.SecondaryShoulder) || Input.GetKeyDown(KeyCode.E);


        float rotateInfluence = SimulationRate_ * Time.deltaTime * RotationAmount * RotationScaleMultiplier;

#if !UNITY_ANDROID 
        if (!SkipMouseRotation)
        {
            PendingRotation += Input.GetAxis("Mouse X") * rotateInfluence * 3.25f;
        }
#endif
        //float rightAxisX = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick).x;

        // Quaternion cameraRotation = CameraRig.centerEyeAnchor.rotation;
        //// float cameraTheta = Mathf.Acos(cameraRotation.w) * 2.0f;
        // Quaternion playerRotation = transform.rotation;
        //// float playerTheta = Mathf.Acos(playerRotation.w) * 2.0f;

        // cn = cameraRotation.eulerAngles;
        // pn = transform.rotation.eulerAngles;

        // float rightAxisX = 0.0f;


        

        Quaternion cameraRotation = CameraRig.centerEyeAnchor.rotation;
        // float cameraTheta = Mathf.Acos(cameraRotation.w) * 2.0f;
        Quaternion playerRotation = transform.rotation;
        // float playerTheta = Mathf.Acos(playerRotation.w) * 2.0f;

        cn = cameraRotation.eulerAngles;
        pn = transform.rotation.eulerAngles;

        float rightAxisX = 0.0f;
        float angleDeadZone = 10.0f;

        //to turn by tilting head
        rightAxisX = AngleDifference(0.0f, cn.z) / 45f;

        // to turn by turning head
        // rightAxisX = AngleDifference(cn.y, pn.y) / 45.0f;


        if (Mathf.Abs(rightAxisX) < axisDeadZone || currentFlySpeed == 0.0f)
            rightAxisX = 0;

        PendingRotation += rightAxisX * rotateInfluence;


        if (rotationSnap)
        {
            if (Mathf.Abs(PendingRotation) > RotationRatchet)
            {
                if (PendingRotation > 0)
                    stepRight = true;
                else
                    stepLeft = true;
                PendingRotation -= Mathf.Sign(PendingRotation) * RotationRatchet;
            }
        }
        else
        {
            euler.y += PendingRotation;
            PendingRotation = 0;
        }



        if (rotationAnimation > 0 && animating)
        {
            float speed = Mathf.Max(rotationAnimation, 3);

            float diff = AngleDifference(targetYaw, euler.y);
            // float done = AngleDifference(euler.y, animationStartAngle);

            euler.y += Mathf.Sign(diff) * speed * Time.deltaTime;

            if ((AngleDifference(targetYaw, euler.y) < 0) != (diff < 0))
            {
                animating = false;
                euler.y = targetYaw;
            }
        }
        if (stepLeft ^ stepRight)
        {
            float change = stepRight ? RotationRatchet : -RotationRatchet;

            if (rotationAnimation > 0)
            {
                targetYaw = (euler.y + change) % 360;
                animating = true;
                // animationStartAngle = euler.y;
            }
            else
            {
                euler.y += change;
            }
        }

        float moveInfluence = SimulationRate_ * Time.deltaTime * Acceleration * 0.1f * MoveScale * MoveScaleMultiplier;

        // Run!
        if (OVRInput.Get(runButton))
        {
            moveInfluence *= 2.0f;
        }

        {
            // controller left thumb stick speed control
         //float leftAxisX = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick).x;
         //float leftAxisY = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick).y;

            //if (Mathf.Abs(leftAxisY) > axisDeadZone)
            //{            
            //    currentFlySpeed += 0.1f * Mathf.Sign(leftAxisY);
            //    if (currentFlySpeed > maxFlySpeed)
            //    {
            //        currentFlySpeed = maxFlySpeed;
            //    }
            //    else if (currentFlySpeed < minFlySpeed)
            //    {
            //        currentFlySpeed = minFlySpeed;
            //    }            
            //}

            //if (Mathf.Abs(leftAxisX) < axisDeadZone)
            //    leftAxisX = 0;

            //if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            //    leftAxisY = 1;
            //if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            //    leftAxisY = -1;
            //if (input.getkey(keycode.a) || input.getkey(keycode.leftarrow))
            //    leftaxisx = -1;
            //if (input.getkey(keycode.d) || input.getkey(keycode.rightarrow))
            //    leftaxisx = 1;

            //leftAxisY = currentFlySpeed;

            //if (leftAxisY > 0.0f)
            //{            
            //    MoveThrottle_ += leftAxisY * (playerDirection * (Vector3.forward * moveInfluence));
            //}
            //else if (leftAxisY < 0.0f)
            //{            
            //    MoveThrottle_ += Mathf.Abs(leftAxisY) * (playerDirection * (Vector3.back * moveInfluence));
            //}

            //if (leftAxisX < 0.0f)
            //{
            //    MoveThrottle_3 = Mathf.Abs(leftAxisX)
            //        * (playerDirection * (Vector3.left * moveInfluence));
            //    MoveThrottle_ += MoveThrottle_3;
            //}
            //if (leftAxisX > 0.0f)
            //{
            //    MoveThrottle_4 = leftAxisX
            //        * (playerDirection * (Vector3.right * moveInfluence));
            //    MoveThrottle_ += MoveThrottle_4;
            //}
        }

        float speedChange = 0.0f;
        if (OVRInput.Get(speedDecrease))
        {
            speedChange = -0.25f;
        }
        else if (OVRInput.Get(speedIncrease))
        {
            speedChange = 0.25f;
        }

        currentFlySpeed += speedChange;
        if (currentFlySpeed > maxFlySpeed)
        {
            currentFlySpeed = maxFlySpeed;
        }
        else if (currentFlySpeed < minFlySpeed && currentFlySpeed != 0.0f)
        {
            currentFlySpeed = minFlySpeed;
        }
        

        float leftAxisY = currentFlySpeed;

        if (leftAxisY > 0.0f)
        {
            MoveThrottle_ += leftAxisY * (playerDirection * (Vector3.forward * moveInfluence));
        }
        else if (leftAxisY < 0.0f)
        {
            MoveThrottle_ += Mathf.Abs(leftAxisY) * (playerDirection * (Vector3.back * moveInfluence));
        }


        transform.rotation = Quaternion.Euler(euler);
    }


    public void SetRotationSnap(bool value)
    {
        rotationSnap = value;
        PendingRotation = 0;
    }

    public void SetRotationAnimation(float value)
    {
        rotationAnimation = value;
        PendingRotation = 0;
    }

    /// <summary>
    /// Resets the player look rotation when the device orientation is reset.
    /// </summary>
    public new void ResetOrientation()
    {
        if (HmdResetsY)
        {
            Vector3 euler = transform.rotation.eulerAngles;
            euler.y = InitialYRotation_;
            transform.rotation = Quaternion.Euler(euler);
        }
    }

    void Reset()
    {
        // Prefer to not reset Y when HMD position reset
        HmdResetsY = false;
    }
}

