using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInputMethod
{
    public void GivePlayer(InputMaster.Player player);
    public void RemovePlayer();
    public Vector2 CheckMoveInput();
    public bool CheckAbilityInput();
}
public interface IControllable
{
    public void GivePlayer(InputMaster.Player player);
    public void ReceiveJoystick(Vector2 input);
    public void ReceiveButton(bool pressed);
    public void DestroyPawn();
}
