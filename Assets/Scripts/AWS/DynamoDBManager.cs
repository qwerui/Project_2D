using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Amazon;
using Amazon.CognitoIdentity;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;

public class DynamoDBManager : MonoBehaviour
{
    DynamoDBContext context;
    AmazonDynamoDBClient DBclient;
    CognitoAWSCredentials credentials;
    UserSessionCache session;

    private void Awake()
    {
        session = UserSessionCache.Instance;
        credentials = session.GetCredentials();
        if(credentials == null)
        {
            credentials = new CognitoAWSCredentials("ap-northeast-2:7fd7b2e5-c1da-4ad2-9ea5-5033c1d08ef4", RegionEndpoint.APNortheast2);
        }
        DBclient = new AmazonDynamoDBClient(credentials, Amazon.RegionEndpoint.APNortheast2);
        context = new DynamoDBContext(DBclient);
    }
    public bool UploadRanking()
    {
        LocalRanking myRank = JsonDirector.LoadRanking();
        if(myRank == null)
            return false;
        WorldRank uploadRank = new WorldRank
        {
            UserID = session.getUserId(),
            Username = session.getUserName(),
            Score = myRank.score[0]
        };
        try
        {
            context.SaveAsync(uploadRank);
            Debug.Log("Upload Success");
            return true;
        }
        catch(Exception e)
        {
            Debug.Log("Upload Fail : " + e);
            return false;
        }
        
    }
    public List<WorldRank> LoadRanking()
    {
        AsyncSearch<WorldRank> worldRankData = context.ScanAsync<WorldRank>(null);
        List<WorldRank> worldRankList = worldRankData.GetNextSetAsync().Result;
        return worldRankList;
    }
}
