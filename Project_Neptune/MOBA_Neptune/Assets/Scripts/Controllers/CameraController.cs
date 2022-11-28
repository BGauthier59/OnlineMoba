using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Controllers
{
    public class CameraController : MonoBehaviour
    {
        //Script for the camera to follow the player
        [SerializeField] private Transform player;

        [SerializeField] private float cameraSpeed = 0.1f;

        private bool cameraLock = true;
        public static CameraController Instance;

        public bool isBattlerite = true; 
        [SerializeField] private Vector3 offset;
        [SerializeField] private float lerpSpeed;
        [SerializeField] private float rotationY;
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

        /// <summary>
        /// Actions Performed on Toggle CameraLock
        /// </summary>
        /// <param name="ctx"></param>
        private void OnToggleCameraLock(InputAction.CallbackContext ctx)
        {
            if (!ctx.performed) return;
            cameraLock = !cameraLock;
            Debug.Log("Camera Lock Toggled");
        }

        private void LateUpdate()
        {
            if(!isBattlerite)
            UpdateCamera(Time.deltaTime);
        }

        private void FixedUpdate()
        {
            if(isBattlerite)
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
                transform.position = Vector3.Lerp(transform.position, nextPos, deltaTime * lerpSpeed);
            }
            else
            {
                nextPos = transform.position;

                if (Input.mousePosition.x >= Screen.width - 1)
                {
                    nextPos += transform.right * cameraSpeed;
                }

                if (Input.mousePosition.x <= 0)
                {
                    nextPos -= transform.right * cameraSpeed;
                }

                if (Input.mousePosition.y >= Screen.height - 1)
                {
                    nextPos += transform.forward * cameraSpeed;
                }

                if (Input.mousePosition.y <= 0)
                {
                    nextPos -= transform.forward * cameraSpeed;
                }

                transform.position = Vector3.Lerp(transform.position, nextPos, deltaTime* lerpSpeed);
            }

            transform.rotation = Quaternion.Euler(transform.rotation.x, rotationY, transform.rotation.z);
        }
    }
}