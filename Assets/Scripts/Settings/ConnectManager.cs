using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConnectManager : MonoBehaviour
{
    [Header("회원가입")]
    [SerializeField] private InputField email;
    [SerializeField] private InputField username;
    [SerializeField] private InputField password;

    [Header("로그인")]
    [SerializeField] private InputField loginEmail;
    [SerializeField] private InputField loginPassword;

    [SerializeField] private AuthenticationManager connect;

    public SettingNotice notice;
    public SettingDirector director;

    public async void onSignupClicked() //회원 가입 클릭
    {
        bool successfulSignup = await connect.Signup(email.text, password.text, username.text);

        if (successfulSignup)
        {
            notice.SetNotice("회원가입 성공");
        }
        else
        {
            notice.SetNotice("회원가입 실패");
        }
    }

    public async void onLoginClicked() //로그인 클릭
    {
        bool successfulLogin = await connect.Login(loginEmail.text, loginPassword.text);
        if(successfulLogin)
        {
            notice.SetNotice("로그인 성공");
            director.ToggleLogInOut(true);
        }
        else
        {
            notice.SetNotice("로그인 실패");
        }
    }
    public void onLogoutClick() //로그아웃 클릭
    {
        connect.SignOut();
        director.ToggleLogInOut(false);
        notice.SetNotice("로그아웃 되었습니다");
    }
}
