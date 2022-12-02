using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;

    public float smoothSpeed = 3;
    public Vector2 offset;
    public float limitMinX, limitMaxX, limitMinY, limitMaxY;
    float cameraHalfWidth, cameraHalfHeight;

    private void Start()
    {
        cameraHalfWidth = Camera.main.aspect * Camera.main.orthographicSize;
        cameraHalfHeight = Camera.main.orthographicSize;
    }

    //매 프레임 끝 날때 카메라 위치 변경
    private void LateUpdate()
    {
        Vector3 desiredPosition = new Vector3(
            Mathf.Clamp(target.position.x + offset.x, limitMinX + cameraHalfWidth, limitMaxX - cameraHalfWidth),   // X
            Mathf.Clamp(target.position.y + offset.y, limitMinY + cameraHalfHeight, limitMaxY - cameraHalfHeight), // Y
            -10);                                                                                                  // Z
        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * smoothSpeed);
    }
    //최대 카메라 위치
    public void SetCameraLimit(RoomInfo room, int maxTile)
    {
        limitMinX = (room.pos.x-(int)(maxTile/2))*50-25;
        limitMaxX = (room.pos.x-(int)(maxTile/2))*50+25;
        limitMaxY = (room.pos.y-(int)(maxTile/2))*50+25;
        limitMinY = (room.pos.y-(int)(maxTile/2))*50-25;
    }
    //카메라 위치 초기화
    public void SetCameraPosition()
    {
        transform.position = new Vector3(
            Mathf.Clamp(target.position.x + offset.x, limitMinX + cameraHalfWidth, limitMaxX - cameraHalfWidth),   // X
            Mathf.Clamp(target.position.y + offset.y, limitMinY + cameraHalfHeight, limitMaxY - cameraHalfHeight), // Y
            -10);
    }
}
