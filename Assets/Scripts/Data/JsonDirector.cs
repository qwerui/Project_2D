using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System;

public static class JsonDirector //Json활용 데이터 저장
{
    private static string SavePath => Application.persistentDataPath + "/saves/"; //저장 경로
    private static string SecurityKey = "dongyangmiraeuniversityproject2d"; //암호화 키

    //게임 데이터 저장
    public static void SaveGameData(GameData data)
    {
        if (!Directory.Exists(SavePath))
        {
            Directory.CreateDirectory(SavePath);
        }
        string saveJson = JsonUtility.ToJson(data);

        //saveJson = Encrypt(saveJson, SecurityKey);
        string saveFilePath = SavePath + "gamesave" + ".json";
        File.WriteAllText(saveFilePath, saveJson);
    }
    //게임 랭킹 저장
    public static void SaveRanking(LocalRanking data)
    {
        if (!Directory.Exists(SavePath))
        {
            Directory.CreateDirectory(SavePath);
        }
        string saveJson = JsonUtility.ToJson(data);

        saveJson = Encrypt(saveJson, SecurityKey);
        string saveFilePath = SavePath + "ranking" + ".json";
        File.WriteAllText(saveFilePath, saveJson);
    }
    //게임 데이터 불러오기
    public static GameData LoadGameData()
    {
        string saveFilePath = SavePath + "gamesave" + ".json";
        if (!Directory.Exists(SavePath))
        {
            return null;
        }
        else if (!File.Exists(saveFilePath))
        {
            return null;
        }
        else
        {
            try
            {
                string saveFile = File.ReadAllText(saveFilePath);
                //saveFile = Decrypt(saveFile, SecurityKey);
                GameData saveData = JsonUtility.FromJson<GameData>(saveFile);
                return saveData;
            }
            catch (System.Exception)
            {

                throw new System.Exception("GameData Load Fail");
            }
        }
    }
    //랭킹 불러오기
    public static LocalRanking LoadRanking()
    {
        string saveFilePath = SavePath + "ranking" + ".json";
        if (!Directory.Exists(SavePath))
        {
            return new LocalRanking();
        }
        else if (!File.Exists(saveFilePath))
        {
            return new LocalRanking();
        }
        else
        {
            try
            {
                string saveFile = File.ReadAllText(saveFilePath);
                saveFile = Decrypt(saveFile, SecurityKey);
                LocalRanking saveData = JsonUtility.FromJson<LocalRanking>(saveFile);
                return saveData;
            }
            catch (System.Exception)
            {

                throw new System.Exception("Ranking Load Fail");
            }
        }
    }
    //데이터 복호화
    public static string Decrypt(string textToDecrypt, string key)
    {
        RijndaelManaged rijndaelCipher = new RijndaelManaged();

        rijndaelCipher.Mode = CipherMode.CBC;
        rijndaelCipher.Padding = PaddingMode.PKCS7;

        rijndaelCipher.KeySize = 128;
        rijndaelCipher.BlockSize = 128;

        byte[] encryptedData = Convert.FromBase64String(textToDecrypt);
        byte[] pwdBytes = Encoding.UTF8.GetBytes(key);
        byte[] keyBytes = new byte[16];

        int len = pwdBytes.Length;

        if (len > keyBytes.Length)
        {
            len = keyBytes.Length;
        }

        Array.Copy(pwdBytes, keyBytes, len);
        rijndaelCipher.Key = keyBytes;
        rijndaelCipher.IV = keyBytes;
        byte[] plainText = rijndaelCipher.CreateDecryptor().TransformFinalBlock(encryptedData, 0, encryptedData.Length);

        return Encoding.UTF8.GetString(plainText);
    }
    //데이터 암호화
    public static string Encrypt(string textToEncrypt, string key)
    {
        RijndaelManaged rijndaelCipher = new RijndaelManaged();

        rijndaelCipher.Mode = CipherMode.CBC;
        rijndaelCipher.Padding = PaddingMode.PKCS7;
        rijndaelCipher.KeySize = 128;
        rijndaelCipher.BlockSize = 128;

        byte[] pwdBytes = Encoding.UTF8.GetBytes(key);
        byte[] keyBytes = new byte[16];
        int len = pwdBytes.Length;

        if (len > keyBytes.Length)
        {
            len = keyBytes.Length;
        }

        Array.Copy(pwdBytes, keyBytes, len);
        rijndaelCipher.Key = keyBytes;
        rijndaelCipher.IV = keyBytes;
        ICryptoTransform transform = rijndaelCipher.CreateEncryptor();
        byte[] plainText = Encoding.UTF8.GetBytes(textToEncrypt);
        return Convert.ToBase64String(transform.TransformFinalBlock(plainText, 0, plainText.Length));
    }
    //세이브 파일 존재 여부 확인
    public static bool CheckSaveFile()
    {
        string saveFilePath = SavePath + "gamesave" + ".json";
        if (!Directory.Exists(SavePath))
        {
            return false;
        }
        else if (!File.Exists(saveFilePath))
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    //세이브 파일 삭제
    public static void DeleteSaveFile()
    {
        string saveFilePath = SavePath + "gamesave" + ".json";
        if(!Directory.Exists(SavePath))
        {
            return;
        }
        else if(!File.Exists(saveFilePath))
        {
            return;
        }
        else
        {
            try
            {
                File.Delete(saveFilePath);
            }
            catch (System.Exception)
            {
                throw new System.Exception("Delete File Fail");
            }
        }
    }
    //암호화 상태 파일 불러오기(서버와 통신에 사용)
    public static string LoadEncryptedSave()
    {
        string saveFilePath = SavePath + "gamesave" + ".json";
        if (!Directory.Exists(SavePath))
        {
            return null;
        }
        else if (!File.Exists(saveFilePath))
        {
            return null;
        }
        else
        {
            try
            {
                string saveFile = File.ReadAllText(saveFilePath);
                return saveFile;
            }
            catch (System.Exception)
            {

                throw new System.Exception("GameData String Load Fail");
            }
        }
    }
    //암호화 상태 파일 저장(서버와 통신에 사용)
    public static bool SaveEncryptedSave(string encryptedData)
    {
        try
        {
            if (!Directory.Exists(SavePath))
            {
                Directory.CreateDirectory(SavePath);
            }
            string saveFilePath = SavePath + "gamesave" + ".json";
            File.WriteAllText(saveFilePath, encryptedData);
            return true;
        }
        catch(System.Exception)
        {
            return false;
        }
        
    }
}
