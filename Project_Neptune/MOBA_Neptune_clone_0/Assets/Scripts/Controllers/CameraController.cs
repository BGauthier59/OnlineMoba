using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Controllers
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Transform player;

        private bool cameraLock = true;
        public static CameraController Instance;

        [SerializeField] private Vector3 offset;
        [SerializeField] private float lerpSpeed;

        public void Awake()
        {
            if (Instance != null && Instance != this)
            {
                DestroyImmediate(gameObject);
                return;
            }

            Instance = this;
        }

        public void LinkCamera(Transform target)
        {
            player = target;
            InputManager.PlayerMap.Camera.LockToggle.performed += OnToggleCameraLock;
        }

        public void UnLinkCamera()
        {
            player = null;
            InputManager.PlayerMap.Camera.LockToggle.performed -= OnToggleCameraLock;
        }

        private void OnToggleCameraLock(InputAction.CallbackContext ctx)
        {
            if (!ctx.performed) return;
            cameraLock = !cameraLock;
        }
        
        private void FixedUpdate()
        {
            UpdateCamera(Time.fixedTime);
        }

        private void UpdateCamera(float deltaTime)
        {
            //if the player is not null
            if (!player) return;
            Vector3 nextPos;

            //if the camera is locked the camera follows the player
            if (cameraLock)
            {
                nextPos = player.position + offset;
            }
            else
            {
                nextPos = transform.position;

                // Todo - Camera
            }
            transform.position = Vector3.Lerp(transform.position, nextPos, deltaTime * lerpSpeed);
        }
    }
}