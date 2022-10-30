using UnityEngine;

namespace Utility {

    /// <summary>
    /// class to handle the visibility of the cursor
    /// </summary>
    public static class CursorManager {

        /// <summary>
        /// every possible visibility type
        /// </summary>
        public enum CursorEvent {
            ForceInvisible,
            ForceVisible,
            RemoveForce,
            Invisible,
            Visible
        }

        private static bool _forced;

        /// <summary>
        /// change cursor lockstate for different events
        /// </summary>
        public static void ExecuteCursorEvent(CursorEvent newEvent) {
            if (_forced)
                if (newEvent != CursorEvent.ForceInvisible && newEvent != CursorEvent.ForceVisible &&
                    newEvent != CursorEvent.RemoveForce)
                    return;

            switch (newEvent) {
                case CursorEvent.ForceInvisible:
                    _forced = true;
                    Cursor.lockState = CursorLockMode.Locked;
                    break;

                case CursorEvent.ForceVisible:
                    _forced = true;
                    Cursor.lockState = CursorLockMode.None;
                    break;

                case CursorEvent.RemoveForce:
                    _forced = false;
                    break;

                case CursorEvent.Invisible:
                    Cursor.lockState = CursorLockMode.Locked;
                    break;

                case CursorEvent.Visible:
                    Cursor.lockState = CursorLockMode.None;
                    Debug.Log("setVisible");
                    break;

                default:
                    Debug.LogWarning("[CursorManager]: CursorEvent not found");
                    break;
            }
        }
    }

}
