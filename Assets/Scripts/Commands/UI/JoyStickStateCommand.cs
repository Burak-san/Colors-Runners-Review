using Enums;
using UnityEngine;
using UnityEngine.UI;

namespace Commands.UI
{
    public class JoyStickStateCommand
    {
        public void JoyStickUIStateChanger(GameStates state, GameObject joystickOuterCircle, GameObject joystickInnerCircle)
        {
            if (state == GameStates.Idle)
            {
                joystickInnerCircle.GetComponent<Image>().enabled = true;
                joystickOuterCircle.GetComponent<Image>().enabled = true;
            }
            else
            {
                joystickOuterCircle.GetComponent<Image>().enabled = false;
                joystickInnerCircle.GetComponent<Image>().enabled = false;
            }
        }
    }
}