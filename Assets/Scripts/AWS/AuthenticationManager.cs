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
   // AWS 리전
   public static Amazon.RegionEndpoint Region = Amazon.RegionEndpoint.APNortheast2;

   // AWS Cognito의 정보 입력
   const string IdentityPool = "ap-northeast-2:7fd7b2e5-c1da-4ad2-9ea5-5033c1d08ef4"; //자격 증명 풀
   const string AppClientID = "1j1m3jfkr5j6rsis9bi76c595h"; //앱 클라이언트 ID
   const string userPoolId = "ap-northeast-2_l9iAkgRJT"; // 사용자 풀

   private AmazonCognitoIdentityProviderClient _provider;
   private CognitoAWSCredentials _cognitoAWSCredentials;
   private static string _userid = "";
   private CognitoUser _user;

   private void Awake() {
      _provider = new AmazonCognitoIdentityProviderClient(new Amazon.Runtime.AnonymousAWSCredentials(), Region);
   }

   //로그인
   public async Task<bool> Login(string email, string password)
   {
      CognitoUserPool userPool = new CognitoUserPool(userPoolId, AppClientID, _provider);
      CognitoUser user = new CognitoUser(email, AppClientID, userPool, _provider);

      InitiateSrpAuthRequest authRequest = new InitiateSrpAuthRequest()
      {
         Password = password
      };

      try
      {
         //로그인 시도
         AuthFlowResponse authFlowResponse = await user.StartWithSrpAuthAsync(authRequest).ConfigureAwait(false);
         //유저 id, 이름 가져오기
         _userid = await GetAttributeFromProvider(authFlowResponse.AuthenticationResult.AccessToken, "sub");
         string _username = await GetAttributeFromProvider(authFlowResponse.AuthenticationResult.AccessToken, "preferred_username");

         UserSessionCache userSessionCache = UserSessionCache.Instance;
         userSessionCache.SetUserSessionCache(
            authFlowResponse.AuthenticationResult.IdToken,
            authFlowResponse.AuthenticationResult.AccessToken,
            authFlowResponse.AuthenticationResult.RefreshToken,
            _userid,
            _username);

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
   //로그 아웃
   public async void SignOut()
   {
      UserSessionCache userSessionCache = UserSessionCache.Instance;
      await userSessionCache.GetUser().GlobalSignOutAsync();
      userSessionCache.SetCredentials(null);
      userSessionCache.SetUser(null);
      Debug.Log("user logged out.");
   }
   //회원가입
   public async Task<bool> Signup(string email, string password, string username)
   {
      //앱 클라이언트, 이메일, 비밀번호 입력
      SignUpRequest signUpRequest = new SignUpRequest()
      {
         ClientId = AppClientID,
         Username = email,
         Password = password
      };
      //이메일, 유저 이름 속성 입력(필수속성)
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
         SignUpResponse signupResponse = await _provider.SignUpAsync(signUpRequest); //회원 가입 메소드
         Debug.Log("Sign up successful");
         return true;
      }
      catch (Exception e)
      {
         Debug.Log("Sign up failed, exception: " + e);
         return false;
      }
   }
   //계정 속성 얻기
   private async Task<string> GetAttributeFromProvider(string accessToken, string attr)
   {
      string returnAttr = "";

      Task<GetUserResponse> responseTask =
         _provider.GetUserAsync(new GetUserRequest
         {
            AccessToken = accessToken
         });

      GetUserResponse responseObject = await responseTask;

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