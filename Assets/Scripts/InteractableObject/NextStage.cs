using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextStage : InteractableObject
{
    RoomController room;
    DataDirector data;


    private void Start() {
        data = DataDirector.Instance;
    }
    protected override void ExitAction(){}
    protected override void Interaction() //다음 스테이지 진출
    {
        data.isLoadedGame = false;
        interactionDirector.gameDirector.ScreenFadeOut(100);
        interactionDirector.gameObject.transform.parent.parent.position = Vector2.zero;
        room = interactionDirector.roomDirector.GetComponent<RoomController>();
        room.NextStage(interactionDirector.soundDirector);
        data.playerPosIndex=0;
        data.stage += 1;
    }
}
