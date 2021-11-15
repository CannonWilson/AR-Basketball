using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class TriggerListener : MonoBehaviour
{
    List<InputDevice> leftDevices = new List<InputDevice>();
    List<InputDevice> rightDevices = new List<InputDevice>();
    private InputDevice leftTarget;
    private InputDevice rightTarget;
    public GameObject leftHandAnchor;
    public GameObject rightHandAnchor;
    private GameObject ball;
    private GameObject goal;
    public static bool canSpawnBall = true;
    private bool canSpawnGoal = true;
    private bool goalWasSpawned = false;
    public GameObject ballPrefab;
    public GameObject goalPrefab;


    // Get a reference to controller characteristics and use them to reference the left and right controllers if they are found
    void Start()
    {
        InputDeviceCharacteristics leftControllerCharacteristics = InputDeviceCharacteristics.Left | InputDeviceCharacteristics.Controller;
        InputDevices.GetDevicesWithCharacteristics(leftControllerCharacteristics, leftDevices);

        InputDeviceCharacteristics rightControllerCharacteristics = InputDeviceCharacteristics.Right | InputDeviceCharacteristics.Controller;
        InputDevices.GetDevicesWithCharacteristics(rightControllerCharacteristics, rightDevices);    

        if (leftDevices.Count > 0) {
            leftTarget = leftDevices[0];
        }
        if (rightDevices.Count > 0) {
            rightTarget = rightDevices[0];
        } 
    }


    // FixedUpdate is used here instead of Update because FixedUpdate triggers when the physics engine is triggered while Update triggers once a frame
    // Listens to trigger/grip input from controllers and spawns or releases balls or the goal
    void FixedUpdate()
    {
        // Left grip pressed
        if (leftTarget.TryGetFeatureValue(CommonUsages.grip, out float leftGripValue) && leftGripValue > 0.1f) {
            if (canSpawnGoal) {
                if (!goalWasSpawned) {
                    goal = Instantiate(goalPrefab, leftHandAnchor.transform); // Spawn basketball goal
                    goalWasSpawned = true;
                }
                goal.transform.parent = leftHandAnchor.transform; // Make goal start following left controller movement
            }
        }

        // Left grip released 
        if (leftTarget.TryGetFeatureValue(CommonUsages.grip, out float leftGripVal) && leftGripVal < 0.05f) {
            if (goalWasSpawned) {
                goal.transform.parent = null; // Make the goal stop following left controller movement
            }
        }

        // Left trigger pressed
        if (leftTarget.TryGetFeatureValue(CommonUsages.trigger, out float leftTriggerValue) && leftTriggerValue > 0.1f) {
            if (!IsBallWithinReach(leftHandAnchor)) {
                if (canSpawnBall) {
                    SpawnBall(leftHandAnchor);
                }
            }
        }

        // Left trigger released
        if (leftTarget.TryGetFeatureValue(CommonUsages.trigger, out float leftTriggerVal) && leftTriggerVal < 0.05f) {
            if (!canSpawnBall) { // Easy way to see if ball was spawned already
                ThrowBall(leftTarget);
            }
        }
        
        
        // Right trigger pressed
        if (rightTarget.TryGetFeatureValue(CommonUsages.trigger, out float rightTriggerValue) && rightTriggerValue > 0.1f) {
            if (!IsBallWithinReach(rightHandAnchor)) {
                if (canSpawnBall) {
                    SpawnBall("left");
                }
            }
        }

        // Right trigger released
        if (rightTarget.TryGetFeatureValue(CommonUsages.trigger, out float rightTriggerVal) && rightTriggerVal < 0.05f) {
            if (!canSpawnBall) { // Easy way to see if ball was spawned already
                ThrowBall(rightTarget);
            }
        }
    }

    // Check if there is a ball within reach of the hand
    void IsBallWithinReach(handAnchor) {
        if (ball.scene.IsValid()) { // If a ball exists
            let distanceToHand = Vector3.Distance(ball.transform.position, handAnchor.position);
            if (distanceToHand < 5) { // This value will require tweaking
                ball.transform.parent = handAnchor.transform; // Make ball track hand
                canSpawnBall = false;
                return true;
            }
        }
        return false;
    }


    // SpawnBall is called when a trigger is pressed
    void SpawnBall(handAnchor) {
        canSpawnBall = false;
        if (ball.scene.IsValid()) { // If a ball exists 
            Destroy(ball); 
        }

        ball = Instantiate(ballPrefab, handAnchor.transform); // Spawn ball at left hand
        ball.transform.parent = handAnchor.transform;
        ball.GetComponent<Rigidbody>().useGravity = false;
    }

    void ThrowBall(handTarget) {
        ball.transform.parent = null; // Make the ball stop following controller movement
        ball.GetComponent<Rigidbody>().useGravity = true;
        if (handTarget.TryGetFeatureValue(CommonUsages.deviceVelocity, out Vector3 deviceVelocity)) {
            float forceMultiplier = 120; // This value just requires testing and tweaking            
            ball.GetComponent<Rigidbody>().AddRelativeForce(deviceVelocity * forceMultiplier);
        }
        canSpawnBall = true;
    }
}