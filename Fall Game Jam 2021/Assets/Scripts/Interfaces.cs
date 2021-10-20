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
