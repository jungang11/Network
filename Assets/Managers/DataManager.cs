using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public MySqlConnection con;
    public MySqlDataReader reader;

    private void Start()
    {
        ConnectDataBase();
    }

    private void ConnectDataBase()
    {
        try
        {
            string serverInfo = "Server=127.0.0.1; DataBase=userdata; Uid=root; Pwd=1234; Port=3306; CharSet=utf8; ";
            con = new MySqlConnection(serverInfo);
            con.Open();

            // �������� ��
            Debug.Log("DataBase connect Success");
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    public Dictionary<int, Stat> StatDict { get; private set; } = new Dictionary<int, Stat>();

    public void Init()
    {
        StatDict = LoadJson<StatData, int, Stat>("StatData").MakeDict();
    }

    Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
    {
        TextAsset textAsset = GameManager.Resource.Load<TextAsset>($"{path}"); // Hard Coding
        return JsonUtility.FromJson<Loader>(textAsset.text);
    }
}
