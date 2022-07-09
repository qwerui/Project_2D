using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Amazon.CognitoIdentity;
using Amazon.Extensions.CognitoAuthentication;

public class UserSessionCache
{
   private static UserSessionCache instance = null;
   public static UserSessionCache Instance
   {
        get
        {
            if(instance == null)
            {
               instance = new UserSessionCache();
            }
            return instance;
        }
   } 
   public string _idToken;
   public string _accessToken;
   public string _refreshToken;
   public string _userId;
   private CognitoAWSCredentials _cognitoAWSCredentials;
   private CognitoUser _user;

   public void SetUserSessionCache(string idToken, string accessToken, string refreshToken, string userId)
   {
      _idToken = idToken;
      _accessToken = accessToken;
      _refreshToken = refreshToken;
      _userId = userId;
   }
   public void SetCredentials(CognitoAWSCredentials credentials)
   {
        _cognitoAWSCredentials = credentials;
   }
   public CognitoAWSCredentials GetCredentials()
   {
        return _cognitoAWSCredentials;
   }
   public void SetUser(CognitoUser user)
   {
      _user = user;
   }
   public CognitoUser GetUser()
   {
      return _user;
   }

   public string getIdToken()
   {
      return _idToken;
   }

   public string getAccessToken()
   {
      return _accessToken;
   }

   public string getRefreshToken()
   {
      return _refreshToken;
   }

   public string getUserId()
   {
      return _userId;
   }
}