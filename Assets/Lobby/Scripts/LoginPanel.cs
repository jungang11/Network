using Photon.Pun;
using TMPro;
using UnityEngine;
using MySql.Data.MySqlClient;
using System;
using UnityEditor.Experimental.GraphView;
using System.Runtime.InteropServices;
using System.Text;
using Unity.VisualScripting;

public class LoginPanel : MonoBehaviour
{
    [SerializeField] GameObject registerPanel;

    [SerializeField] TMP_InputField idInputField;
    [SerializeField] TMP_InputField passwordInputField;

    private MySqlConnection con;
    private MySqlDataReader reader;

    private void Start()
    {
        con = GameManager.Data.con;
        reader = GameManager.Data.reader;
    }

    // Login ��ư ���� �Լ�
    public void Login()
    {
        try
        {
            string id = idInputField.text;
            string pass = passwordInputField.text;

            string sqlCommand = string.Format($"SELECT ID,Password FROM user_info WHERE ID='{id}'");
            MySqlCommand cmd = new MySqlCommand(sqlCommand, con);
            reader = cmd.ExecuteReader();

            if(reader.HasRows)
            {
                while (reader.Read()) // ���̺� ��ü�� �б�. read => true : ���� �ڷᰡ ���� / false : �� ����
                {
                    string readID = reader["ID"].ToString();
                    string readPass = reader["Password"].ToString();

                    Debug.Log($"ID : {readID}, Password : {readPass}");

                    if(pass == readPass)
                    {
                        PhotonNetwork.LocalPlayer.NickName = id;
                        PhotonNetwork.ConnectUsingSettings(); // ���� ��û
                        if (!reader.IsClosed)
                            reader.Close();
                        return;
                    }
                    else
                    {
                        Debug.Log("Wrong Password");
                        if (!reader.IsClosed)
                            reader.Close();
                        return;
                    }
                }
            }
            else
            {
                Debug.Log("There is no player id");
            }
            if (!reader.IsClosed)
                reader.Close();
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }
}
