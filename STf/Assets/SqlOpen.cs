using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using System.IO;

public class SqlOpen : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        string connection = "URI=file:" + Application.dataPath + "/My_Database";
        IDbConnection dbcon = new SqliteConnection(connection);
        dbcon.Open();
        Debug.Log(connection);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
