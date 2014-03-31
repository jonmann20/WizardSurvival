using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Author: Jon Wiedmann
// 
// The gamepad to keyboard and mouse mappings.


namespace InControl {
    public class DefaultProfile : UnityInputDeviceProfile {
        public DefaultProfile() {
            Name = "Default Keyboard and Mouse Mappings";
            Meta = "A keyboard and mouse combination profile.";

            SupportedPlatforms = new[]
			{
				"Windows",
				"Mac",
				"Linux"
			};

            Sensitivity = 1.0f;
            LowerDeadZone = 0.0f;

            ButtonMappings = new[]
			{
				new InputControlMapping
				{
					Handle = "Fire",
					Target = InputControlType.RightBumper,
					Source = MouseButton0
				},
				new InputControlMapping
				{
					Handle = "Special",
					Target = InputControlType.RightTrigger,
					Source = MouseButton1
				},
				new InputControlMapping
				{
					Handle = "Jump",
					Target = InputControlType.Action1,
					Source = KeyCodeButton(KeyCode.Space)
				},
				new InputControlMapping
				{
					Handle = "Start",
					Target = InputControlType.Start,
					Source = KeyCodeButton(KeyCode.Return)
				},
				new InputControlMapping
				{
					Handle = "DPadL",
					Target = InputControlType.DPadLeft,
					Source = KeyCodeButton(KeyCode.Q)
				},
				new InputControlMapping
				{
					Handle = "DPadR",
					Target = InputControlType.DPadRight,
					Source = KeyCodeButton(KeyCode.R)
				}
			};

            AnalogMappings = new[]
			{
				new InputControlMapping
				{
					Handle = "Move X",
					Target = InputControlType.LeftStickX,
					Source = KeyCodeAxis(KeyCode.A, KeyCode.D)
				},
				new InputControlMapping
				{
					Handle = "Move Y",
					Target = InputControlType.LeftStickY,
					Source = KeyCodeAxis(KeyCode.S, KeyCode.W)
				},
				new InputControlMapping
				{
					Handle = "Look X",
					Target = InputControlType.RightStickX,
					Source = MouseXAxis,
					Raw    = true
				},
				new InputControlMapping
				{
					Handle = "Look Y",
					Target = InputControlType.RightStickY,
					Source = MouseYAxis,
					Raw    = true
				},
				new InputControlMapping
				{
					Handle = "Look Z",
					Target = InputControlType.ScrollWheel,
					Source = MouseScrollWheel,
					Raw    = true
				}
			};
        }
    }
}

