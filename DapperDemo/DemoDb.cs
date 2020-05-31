using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Dapper;
using DapperDemo.Model;
using Katoa.Queries;
using Microsoft.Data.Sqlite;

namespace DapperDemo
{
    public class DemoDb : IDisposable
    {
        private string _dbPath = "./demo.sqlite";
        private SqliteConnection _connection;

        public void Open()
        {
            if(File.Exists(_dbPath)) File.Delete(_dbPath);
            _connection = new SqliteConnection($"Data Source={_dbPath}");
            CreateDemoData();
        }

        public List<Data> AllData() =>  _connection.Query<Data>(Query.For("AllData")).ToList();
        
        private void CreateDemoData()
        {
            _connection.Execute(Query.For("CreateDemoData"));
        }

        public void Dispose()
        {
            _connection?.Dispose();
        }

        public List<Data> DataLike(string like) =>
            _connection.Query<Data>(Query.For("DataLike"), new {Like = $"%{like}%"}).ToList();
    }
}