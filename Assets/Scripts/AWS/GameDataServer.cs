using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Amazon.DynamoDBv2.DataModel;

[DynamoDBTable("Project2D_Data")]
public class GameDataServer
{
    [DynamoDBHashKey] public string UserID {get; set;}

    [DynamoDBProperty] public string Gamedata {get;set;}
}
