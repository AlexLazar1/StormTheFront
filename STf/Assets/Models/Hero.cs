using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using System;

public class Hero
{
    private string tableName = "Hero";
    public string Name { get; set; }
    public int ArchersNo { get; set; }
    public int SoldiersNo { get; set; }
    public int KnightsNo { get; set; }
    public int SiegeNo { get; set; }

    public int SaveInDb()
    {
        string connection = "URI=file:" + Application.dataPath + "/My_Database";
        IDbConnection dbcon = new SqliteConnection(connection);
        dbcon.Open();
        IDbCommand insert = dbcon.CreateCommand();
        insert.CommandText = string.Format("INSERT INTO " +
        	"{0}(Name,ArchersNO,SoldiersNO,KnightsNO,SiegeNO) " +
        	"values(@name,@archers,@soldiers,@knights,@siege) ",tableName);

        SqliteParameter namedb = new SqliteParameter("@name", Name);
        SqliteParameter archers = new SqliteParameter("@archers", ArchersNo);
        SqliteParameter soldiers = new SqliteParameter("@soldiers", SoldiersNo);
        SqliteParameter knights = new SqliteParameter("@knights", KnightsNo);
        SqliteParameter siege = new SqliteParameter("@siege", SiegeNo);
        insert.Parameters.Add(namedb);
        insert.Parameters.Add(archers);
        insert.Parameters.Add(soldiers);
        insert.Parameters.Add(knights);
        insert.Parameters.Add(siege);

        Debug.Log(insert.CommandText);
        insert.ExecuteNonQuery();
        string sql = @"select last_insert_rowid()";
        insert.CommandText = "select last_insert_rowid()";
        Int64 LastRowID64 = (Int64)insert.ExecuteScalar();
        int LastRowID = (int)LastRowID64;

        return LastRowID;

    }

}
