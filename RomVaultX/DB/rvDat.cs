﻿using System;
using System.Data.SQLite;

namespace RomVaultX.DB
{
    public class rvDat
    {
        public int DatId;
        public int DirId;
        public string Filename;
        public string Name;
        public string RootDir;
        public string Description;
        public string Category;
        public string Version;
        public string Date;
        public string Author;
        public string Email;
        public string Homepage;
        public string URL;
        public string Comment;


        private static readonly SQLiteCommand SqlWrite;
        private static readonly SQLiteCommand SqlRead;

        static rvDat()
        {
            SqlWrite = new SQLiteCommand(
               @"INSERT INTO DAT ( DirId, Filename, name, rootdir, description, category, version, date, author, email, homepage, url, comment)
                VALUES            (@DirId,@Filename,@name,@rootdir,@description,@category,@version,@date,@author,@email,@homepage,@url,@comment);

                SELECT last_insert_rowid();");

            SqlWrite.Parameters.Add(new SQLiteParameter("DirId"));
            SqlWrite.Parameters.Add(new SQLiteParameter("Filename"));
            SqlWrite.Parameters.Add(new SQLiteParameter("name"));
            SqlWrite.Parameters.Add(new SQLiteParameter("rootdir"));
            SqlWrite.Parameters.Add(new SQLiteParameter("description"));
            SqlWrite.Parameters.Add(new SQLiteParameter("category"));
            SqlWrite.Parameters.Add(new SQLiteParameter("version"));
            SqlWrite.Parameters.Add(new SQLiteParameter("date"));
            SqlWrite.Parameters.Add(new SQLiteParameter("author"));
            SqlWrite.Parameters.Add(new SQLiteParameter("email"));
            SqlWrite.Parameters.Add(new SQLiteParameter("homepage"));
            SqlWrite.Parameters.Add(new SQLiteParameter("url"));
            SqlWrite.Parameters.Add(new SQLiteParameter("comment"));


            SqlRead = new SQLiteCommand(
             @"SELECT DirId,Filename,name,rootdir,description,category,version,date,author,email,homepage,url,comment 
                FROM DAT WHERE DatId=@datId");
            SqlRead.Parameters.Add(new SQLiteParameter("datId"));

        }
        public static void SetConnection(SQLiteConnection connection)
        {
            SqlWrite.Connection = connection;
            SqlRead.Connection = connection;
        }

        public static void MakeDB()
        {

            DataAccessLayer.ExecuteNonQuery(@"
                 CREATE TABLE IF NOT EXISTS [DAT] (
                    [DatId] INTEGER  PRIMARY KEY NOT NULL,
                    [DirId] INTEGER  NOT NULL,
                    [Filename] NVARCHAR(300)  NULL,

                    [name] NVARCHAR(100)  NULL,
                    [rootdir] NVARCHAR(10)  NULL,
                    [description] NVARCHAR(10)  NULL,
                    [category] NVARCHAR(10)  NULL,
                    [version] NVARCHAR(10)  NULL,
                    [date] NVARCHAR(10)  NULL,
                    [author] NVARCHAR(10)  NULL,
                    [email] NVARCHAR(10)  NULL,
                    [homepage] NVARCHAR(10)  NULL,
                    [url] NVARCHAR(10)  NULL,
                    [comment] NVARCHAR(10) NULL
                );");
        }

        public void DBRead(int datId)
        {
            SqlRead.Parameters["DatID"].Value = datId;

            using (SQLiteDataReader dr = SqlRead.ExecuteReader())
            {
                if (dr.Read())
                {
                    DatId = datId;
                    DirId = Convert.ToInt32(dr["DirId"]);
                    Filename = dr["filename"].ToString();
                    Name = dr["name"].ToString();
                    RootDir = dr["rootdir"].ToString();
                    Description = dr["description"].ToString();
                    Category = dr["category"].ToString();
                    Version = dr["version"].ToString();
                    Date = dr["date"].ToString();
                    Author = dr["author"].ToString();
                    Email = dr["email"].ToString();
                    Homepage = dr["homepage"].ToString();
                    URL = dr["url"].ToString();
                    Comment = dr["comment"].ToString();
                }
                dr.Close();
            }
        }

        public void DbWrite()
        {
            SqlWrite.Parameters["DirId"].Value = DirId;
            SqlWrite.Parameters["Filename"].Value = Filename;
            SqlWrite.Parameters["name"].Value = Name;
            SqlWrite.Parameters["rootdir"].Value = RootDir;
            SqlWrite.Parameters["description"].Value = Description;
            SqlWrite.Parameters["category"].Value = Category;
            SqlWrite.Parameters["version"].Value = Version;
            SqlWrite.Parameters["date"].Value = Date;
            SqlWrite.Parameters["author"].Value = Author;
            SqlWrite.Parameters["email"].Value = Email;
            SqlWrite.Parameters["homepage"].Value = Homepage;
            SqlWrite.Parameters["url"].Value = URL;
            SqlWrite.Parameters["comment"].Value = Comment;
            object res = SqlWrite.ExecuteScalar();

            if (res == null || res == DBNull.Value)
                return;
            DatId = Convert.ToInt32(res);
        }
    }
}