using UnityEngine;
using System.Collections.Generic;
using Amazon;
using Amazon.Extensions.CognitoAuthentication;
using Amazon.CognitoIdentity;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using System;
using System.Threading.Tasks;
using System.Net;

public class AuthenticationManager : MonoBehaviour
{
   // the AWS region of where your services live
   public static Amazon.RegionEndpoint Region = Amazon.RegionEndpoint.APNortheast2;

   // In production, should probably keep these in a config file
   const string IdentityPool = "ap-northeast-2:7fd7b2e5-c1da-4ad2-9ea5-5033c1d08ef4"; //insert your Cognito User Pool ID, found under General Settings
   const string AppClientID = "1j1m3jfkr5j6rsis9bi76c595h"; //insert App client ID, found under App Client Settings
   const string userPoolId = "ap-northeast-2_l9iAkgRJT";

   private AmazonCognitoIdentityProviderClient _provider;
   private CognitoAWSCredentials _cognitoAWSCredentials;
   private static string _userid = "";
   private CognitoUser _user;

   private void Awake() {
      //Amazon.AWSConfigs.HttpClient = Amazon.AWSConfigs.HttpClientOption.UnityWebRequest;
      _provider = new AmazonCognitoIdentityProviderClient(new Amazon.Runtime.AnonymousAWSCredentials(), Region);
   }

   public async Task<bool> Login(string email, string password)
   {
      // Debug.Log("Login: " + email + ", " + password);

      CognitoUserPool userPool = new CognitoUserPool(userPoolId, AppClientID, _provider);
      CognitoUser user = new CognitoUser(email, AppClientID, userPool, _provider);

      InitiateSrpAuthRequest authRequest = new InitiateSrpAuthRequest()
      {
         Password = password
      };

      try
      {
         AuthFlowResponse authFlowResponse = await user.StartWithSrpAuthAsync(authRequest).ConfigureAwait(false);

         _userid = await GetAttributeFromProvider(authFlowResponse.AuthenticationResult.AccessToken, "sub");
         string _username = await GetAttributeFromProvider(authFlowResponse.AuthenticationResult.AccessToken, "preferred_username");

         UserSessionCache userSessionCache = UserSessionCache.Instance;
         userSessionCache.SetUserSessionCache(
            authFlowResponse.AuthenticationResult.IdToken,
            authFlowResponse.AuthenticationResult.AccessToken,
            authFlowResponse.AuthenticationResult.RefreshToken,
            _userid,
            _username);

         // This how you get credentials to use for accessing other services.
         // This IdentityPool is your Authorization, so if you tried to access using an
         // IdentityPool that didn't have the policy to access your target AWS service, it would fail.
         _cognitoAWSCredentials = user.GetCognitoAWSCredentials(IdentityPool, Region);
         userSessionCache.SetCredentials(_cognitoAWSCredentials);
         userSessionCache.SetUser(user);

         return true;
      }
      catch (Exception e)
      {
         Debug.Log("Login failed, exception: " + e);
         return false;
      }
   }

   public async void SignOut()
   {
      UserSessionCache userSessionCache = UserSessionCache.Instance;
      await userSessionCache.GetUser().GlobalSignOutAsync();
      userSessionCache.SetCredentials(null);
      userSessionCache.SetUser(null);
      Debug.Log("user logged out.");
   }

   public async Task<bool> Signup(string email, string password, string username)
   {
      // Debug.Log("SignUpRequest: " + username + ", " + email + ", " + password);

      SignUpRequest signUpRequest = new SignUpRequest()
      {
         ClientId = AppClientID,
         Username = email,
         Password = password
      };

      // must provide all attributes required by the User Pool that you configured
      List<AttributeType> attributes = new List<AttributeType>()
      {
         new AttributeType(){
            Name = "email", Value = email
         },
         new AttributeType(){
            Name = "preferred_username", Value = username
         }
      };
      signUpRequest.UserAttributes = attributes;

      try
      {
         SignUpResponse signupResponse = await _provider.SignUpAsync(signUpRequest);
         Debug.Log("Sign up successful");
         return true;
      }
      catch (Exception e)
      {
         Debug.Log("Sign up failed, exception: " + e);
         return false;
      }
   }
   private async Task<string> GetAttributeFromProvider(string accessToken, string attr)
   {
      string returnAttr = "";

      Task<GetUserResponse> responseTask =
         _provider.GetUserAsync(new GetUserRequest
         {
            AccessToken = accessToken
         });

      GetUserResponse responseObject = await responseTask;

      // set the user id
      foreach (var attribute in responseObject.UserAttributes)
      {
         if (attribute.Name == attr)
         {
            returnAttr = attribute.Value;
            break;
         }
      }

      return returnAttr;
   }
}