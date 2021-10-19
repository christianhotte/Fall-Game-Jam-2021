using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interfaces : MonoBehaviour
{
    public interface IAbility
    {
        public void Activate(bool buttonDown);
    }

    public interface IControllable
    {
        public void OnStick(Vector2 value);
        public void OnUseAbility(bool up);
    }
}
