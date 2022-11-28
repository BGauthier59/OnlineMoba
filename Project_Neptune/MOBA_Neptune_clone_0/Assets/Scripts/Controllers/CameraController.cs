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
            //if the player is not null
            if (!player) return;
            Vector3 nextPos;

            //if the camera is locked the camera follows the player
            if (cameraLock)
            {
                nextPos = player.position + offset;
                transform.position = Vector3.Lerp(transform.position, nextPos, Time.fixedDeltaTime * lerpSpeed);
            }
            else
            {
                nextPos = transform.position;
                
                // Todo - Calculer nextPos en fonction de la position relative de la souris et du joueur
                
                transform.position = Vector3.Lerp(transform.position, nextPos, Time.fixedDeltaTime * lerpSpeed);
                
            }
        }
    }
}