using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using System;

public class Hero
{
    public int id { get; set; }
    private static string tableName = "Hero";
    public string Name { get; set; }
    public int ArchersNo { get; set; }
    public int SoldiersNo { get; set; }
    public int KnightsNo { get; set; }
    public int SiegeNo { get; set; }
    static string connection = "URI=file:" + Application.dataPath + "/My_Database";


    public int SaveInDb()
    {
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

    public static Hero GetPlayer(int id)
    {
        IDbConnection db = new SqliteConnection(connection);
        db.Open();
        IDbCommand command = db.CreateCommand();
        command.CommandText = string.Format("SELECT * FROM {0} WHERE id = {1}", tableName, id);
        IDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            return new Hero
            {
                id = int.Parse(reader["ID"].ToString()),
                Name = reader["Name"].ToString(),
                ArchersNo = int.Parse(reader["ArchersNo"].ToString()),
                SoldiersNo = int.Parse(reader["SoldiersNO"].ToString()),
                KnightsNo = int.Parse(reader["KnightsNO"].ToString()),
                SiegeNo = int.Parse(reader["SiegeNO"].ToString()),
            };
        }
        db.Close();
        return null;
    }

    public void printHero()
    {
        Debug.Log(Name);
        Debug.Log(ArchersNo);
        Debug.Log(SoldiersNo);
        Debug.Log(KnightsNo);
        Debug.Log(SiegeNo);
    }

    public static List<string> GetAllHeroNames()
    {
        List<String> heroNames = new List<string>();
        var db = new SqliteConnection(connection);
        db.Open();
        var command = db.CreateCommand();
        command.CommandText = string.Format("SELECT Name FROM {0}", tableName);
        var reader = command.ExecuteReader();
        while (reader.Read())
        {
            heroNames.Add(reader["Name"].ToString());
        }
        db.Close();
        return heroNames;
    }

    public static Hero GetByName(String name)
    {
        Hero hero = new Hero();
        var db = new SqliteConnection(connection);
        db.Open();
        var command = db.CreateCommand();
        command.CommandText = string.Format("SELECT * FROM {0} WHERE Name = '{1}'", tableName, name);
        var reader = command.ExecuteReader();
        while (reader.Read())
        {
            hero.id = int.Parse(reader["ID"].ToString());
            hero.Name = reader["Name"].ToString();
            hero.ArchersNo = int.Parse(reader["ArchersNo"].ToString());
            hero.SoldiersNo = int.Parse(reader["SoldiersNO"].ToString());
            hero.KnightsNo = int.Parse(reader["KnightsNO"].ToString());
            hero.SiegeNo = int.Parse(reader["SiegeNO"].ToString());

            return hero;

        }
        db.Close();
        return null;
    }

}
