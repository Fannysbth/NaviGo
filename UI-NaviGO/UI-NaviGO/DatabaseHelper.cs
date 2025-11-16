// DatabaseHelper.cs
using Npgsql;
using System;
using System.Windows.Forms;

namespace UI_NaviGO
{
    public static class DatabaseHelper
    {
        public static string ConnectionString =>
            "Host=aws-1-ap-southeast-1.pooler.supabase.com;" +
            "Port=5432;" +
            "Username=postgres.zsktvbvfquecdmndgyrz;" +
            "Password=agathahusna;" +
            "Database=postgres;" +
            "Ssl Mode=Require;" +
            "Trust Server Certificate=true;" +
            "Pooling=true;" +
            "Timeout=30;";

        public static NpgsqlConnection GetConnection()
        {
            return new NpgsqlConnection(ConnectionString);
        }

        public static bool TestConnection()
        {
            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Koneksi database gagal: {ex.Message}");
                return false;
            }
        }
    }
}