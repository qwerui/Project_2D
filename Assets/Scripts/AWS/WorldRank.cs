using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Amazon.DynamoDBv2.DataModel;

[DynamoDBTable("Project2D_Ranking")]
public class WorldRank
{
    [DynamoDBHashKey] public string UserID {get; set;}

    [DynamoDBProperty] public string Username {get;set;}
    [DynamoDBProperty] public int Score {get;set;}
}
