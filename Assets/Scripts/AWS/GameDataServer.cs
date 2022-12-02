using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Amazon.DynamoDBv2.DataModel;

[DynamoDBTable("Project2D_Data")]
public class GameDataServer //게임 데이터 클래스
{
    [DynamoDBHashKey] public string UserID {get; set;}

    [DynamoDBProperty] public string Gamedata {get;set;}
}
