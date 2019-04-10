using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Mono.Data.Sqlite;
using UnityEngine;

public class Castle 
{
    private string tableName = "Castle";
    public int HeroId { get; set; }
    public int KnightsNo { get; set; }
    public int SoldiersNo { get; set; }
    public int ArchersNo { get; set; }

    public int SaveInDb()
    {
        string connection = "URI=file:" + Application.dataPath + "/My_Database";
        IDbConnection dbcon = new SqliteConnection(connection);
        dbcon.Open();
        IDbCommand insert = dbcon.CreateCommand();
        insert.CommandText = string.Format("INSERT INTO " +
            "{0}(ArchersNO,SoldiersNO,KnightsNO,HeroId) " +
            "values(@archers,@soldiers,@knights,@heroId) ", tableName);

        SqliteParameter heroid = new SqliteParameter("@heroId", HeroId);
        SqliteParameter archers = new SqliteParameter("@archers", ArchersNo);
        SqliteParameter soldiers = new SqliteParameter("@soldiers", SoldiersNo);
        SqliteParameter knights = new SqliteParameter("@knights", KnightsNo);
        insert.Parameters.Add(heroid);
        insert.Parameters.Add(archers);
        insert.Parameters.Add(soldiers);
        insert.Parameters.Add(knights);

        Debug.Log(insert.CommandText);
        insert.ExecuteNonQuery();
        string sql = @"select last_insert_rowid()";
        insert.CommandText = "select last_insert_rowid()";
        Int64 LastRowID64 = (Int64)insert.ExecuteScalar();
        int LastRowID = (int)LastRowID64;

        return LastRowID;
    }
}
