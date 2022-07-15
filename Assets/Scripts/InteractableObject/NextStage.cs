using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextStage : InteractableObject
{
    RoomController room;
    DataDirector data;
    public AudioSource sound;
    public AudioClip clip;

    private void Start() {
        data = DataDirector.Instance;
    }
    protected override void ExitAction(){}
    protected override void Interaction()
    {
        interactionDirector.gameDirector.ScreenFadeOut(100);
        interactionDirector.gameObject.transform.parent.parent.position = Vector2.zero;
        room = interactionDirector.roomDirector.GetComponent<RoomController>();
        room.NextStage();
        StartCoroutine(SoundPlay());
        data.stage += 1;
    }
    IEnumerator SoundPlay()
    {
        for (int i = 0; i < 4; i++)
        {
            sound.pitch = 0.8f - (i * 0.2f);
            sound.volume = 0.6f - (i * 0.1f);
            sound.PlayOneShot(clip);
            yield return new WaitForSeconds(0.25f);
        }
    }
}
