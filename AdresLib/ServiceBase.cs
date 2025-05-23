using AdresLib.Entidades;
using Dapper;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdresLib
{
    public abstract class ServiceBase
    {
        protected SqliteConnection GetConnectionx()
        {
            // Implementation for creating and returning a SqliteConnection  
            return new SqliteConnection(@"Data Source=db\adres.db");
        }

        protected SqliteConnection GetConnection()
        {
            //string dbPath = Path.Combine(Environment.CurrentDirectory, "db", "adres.db");
            string dbPath = @"Data Source=db\adres.db";
            return new SqliteConnection(dbPath);
        }

        public List<T> Listar<T>(string sql)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                return conn.Query<T>(sql).ToList();
            }

        }

        public List<IdNombre> IdNombreListar(string sql)
        {
            return Listar<IdNombre>(sql);
        }
    }
}
