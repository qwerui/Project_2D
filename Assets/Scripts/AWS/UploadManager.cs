using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Amazon;
using Amazon.CognitoIdentity;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;

public class UploadManager : MonoBehaviour
{
    DynamoDBContext context;
    AmazonDynamoDBClient DBclient;
    CognitoAWSCredentials credentials;
    UserSessionCache session;

    public SettingNotice notice;

    //세이브 데이터 업로드
    public void UploadData()
    {
        string localData = JsonDirector.LoadEncryptedSave();
        if(localData == null)
        {
            return;
        }
        GameDataServer data = new GameDataServer
        {
            UserID = session.getUserId(),
            Gamedata = localData
        };
        try
        {
            context.SaveAsync(data);
            Debug.Log("Upload Success");
            notice.SetNotice("업로드 성공");
        }
        catch(Exception e)
        {
            Debug.Log("Upload Fail : " + e);
            notice.SetNotice("업로드 실패");
        }
    }
    //세이브 데이터 다운로드
    public void DownloadData()
    {
        if(JsonDirector.SaveEncryptedSave(context.LoadAsync<GameDataServer>(session.getUserId()).Result.Gamedata))
        {
            notice.SetNotice("다운로드 성공");
        }
        else
        {
            notice.SetNotice("다운로드 실패");
        }
    }

    //DynamoDB 연결
    public void SetDyanmoDB(bool login)
    {
        if(login)
        {
            session = UserSessionCache.Instance;
            credentials = session.GetCredentials();
            DBclient = new AmazonDynamoDBClient(credentials, Amazon.RegionEndpoint.APNortheast2);
            context = new DynamoDBContext(DBclient);
        }
        else
        {
            credentials = null;
            DBclient = null;
            context = null;
            session = null;
        }
    }
}
