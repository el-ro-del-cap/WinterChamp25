// TopDownInputController.cs (Modified)
using UnityEngine;
using UnityEngine.InputSystem;
// Remove 'using UnityEngine.Events;' as it's no longer needed here

namespace TopDownCharacter2D.Controllers
{
    /// <summary>
    ///     This class encapsulate all the input processing for a player using Unity's new input system
    /// </summary>
    public class TopDownInputController : TopDownCharacterController
    {
        private bool movementEnabled = true;

        public void SetMovementEnabled(bool enabled)
        {
            movementEnabled = enabled;
        }
        private Camera _camera;

        // REMOVE THESE LINES:
        // private readonly UnityEvent onInteractEvent = new UnityEvent();
        // public UnityEvent OnInteractEvent => onInteractEvent;

        protected override void Awake()
        {
            base.Awake();
            _camera = Camera.main;
        }

        #region Methods called by unity input events

        /// <summary>
        ///     Method called when the user input a movement
        /// </summary>
        /// <param name="value"> The value of the input </param>
        public void OnMove(InputValue value)
        {
            if (!movementEnabled)
            {
                OnMoveEvent.Invoke(Vector2.zero);
                return;
            }
            Vector2 moveInput = value.Get<Vector2>().normalized;
            OnMoveEvent.Invoke(moveInput);
        }

        /// <summary>
        ///     Method called when the user enter a look input
        /// </summary>
        /// <param name="value"> The value of the input </param>
        public void OnLook(InputValue value)
        {
            Vector2 newAim = value.Get<Vector2>();
            if (!(newAim.normalized == newAim))
            {
                Vector2 worldPos = _camera.ScreenToWorldPoint(newAim);
                newAim = (worldPos - (Vector2) transform.position).normalized;
            }

            if (newAim.magnitude >= .9f)
            {
                LookEvent.Invoke(newAim);
            }
        }

        /// <summary>
        ///     Method called when the user enter a fire input
        /// </summary>
        /// <param name="value"> The value of the input </param>
        public void OnFire(InputValue value)
        {
            IsAttacking = value.isPressed;
        }

        // REMOVE THIS ENTIRE METHOD:
        /*
        public void OnInteract(InputValue value)
        {
            if (value.isPressed)
            {
                Debug.Log("--- OnInteract (E key) pressed! Invoking event. ---");
                onInteractEvent.Invoke();
            }
        }
        */

        #endregion
    }
}