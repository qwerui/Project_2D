using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextStage : InteractableObject
{
    RoomController room;
    protected override void ExitAction(){}
    protected override void Interaction()
    {
        interactionDirector.gameObject.transform.parent.parent.position = Vector2.zero;
        room = interactionDirector.roomDirector.GetComponent<RoomController>();
        room.NextStage();
    }
}
