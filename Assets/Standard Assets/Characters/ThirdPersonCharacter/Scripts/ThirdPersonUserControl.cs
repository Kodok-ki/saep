using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets.Characters.ThirdPerson
{
    [RequireComponent(typeof(ThirdPersonCharacter))]
    public class ThirdPersonUserControl : MonoBehaviour
    {
        private ThirdPersonCharacter m_Character; // A reference to the ThirdPersonCharacter on the object
        private Transform m_Cam;                  // A reference to the main camera in the scenes transform
        private Vector3 m_CamForward;             // The current forward direction of the camera
        private Vector3 m_Move;                   // the world-relative desired move direction, calculated from the camForward and user input.
        private bool m_Jump;                      //Ability to jump (from ground)
        private bool dJump;                       //Ability to jump (in midair)
        private bool sprinting;                   // Whether or not PC is currently sprinting



        private void Start()
        {
            // get the transform of the main camera
            if (Camera.main != null)
            {
                m_Cam = Camera.main.transform;
            }
            else
            {
                Debug.LogWarning(
                    "Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.");
                // we use self-relative controls in this case, which probably isn't what the user wants, but hey, we warned them!
            }

            // get the third person character ( this should never be null due to require component )
            m_Character = GetComponent<ThirdPersonCharacter>();
            dJump = false;
        }


        private void Update()
        {
            if (!m_Jump) //Check for 'jumpable'
            {
                if (!CheckforDoubleJump())
                {
                    m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
                }
                else
                {
                    dJump = CrossPlatformInputManager.GetButtonDown("Jump");
                }
            }
        }


        // Fixed update is called in sync with physics
        private void FixedUpdate()
        {
            // read inputs
            float h = CrossPlatformInputManager.GetAxis("Horizontal");
            float v = CrossPlatformInputManager.GetAxis("Vertical");
            bool crouch = Input.GetKey(KeyCode.C);

            // calculate move direction to pass to character
            if (m_Cam != null)
            {
                // calculate camera relative direction to move:
                m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
                m_Move = v * m_CamForward + h * m_Cam.right;
            }
            else
            {
                // we use world-relative directions in the case of no main camera
                m_Move = v * Vector3.forward + h * Vector3.right;
            }
#if !MOBILE_INPUT
            // walk speed multiplier
            if (Input.GetKey(KeyCode.LeftShift)) sprinting = true;
            else sprinting = false;
#endif

            // pass all parameters to the character control script
            m_Character.Move(m_Move, crouch, m_Jump, sprinting);
            m_Jump = false;
            if (dJump)
            {
                m_Character.airJump(dJump, (m_Move + Vector3.up));
            }
        }
        /// <summary>
        /// Method to check whether or not the player is in midair.
        /// </summary>
        bool CheckforDoubleJump(){
            RaycastHit hitInfo;

            if (!Physics.Raycast((transform.position + Vector3.up * 0.1f), Vector3.down, out hitInfo, 0.1f)) { //if in midair
                return true;
            }
            return false;
        }

    }
}
