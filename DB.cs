using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UPharmacy
{
    public static class DB
    {
        private static readonly string connectionString = "Data Source=prescriptions.db;Version=3;";

        static DB()
        {
            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                // 테이블 1: 요약
                string createSummaryTable = @"
                    CREATE TABLE IF NOT EXISTS pastPrescriptionList (
                        jumin TEXT,
                        name TEXT,
                        date TEXT,
                        hospital TEXT,
                        doctor TEXT,
                        totalAmountSum INTEGER
                    );"
                ;

                using (var cmd1 = new SQLiteCommand(createSummaryTable, conn))
                {
                    cmd1.ExecuteNonQuery();
                }

                // 테이블 2: 상세
                string createDetailTable = @"
                    CREATE TABLE IF NOT EXISTS previousPrescriptionDetails (
                        jumin TEXT,
                        name TEXT,
                        date TEXT,
                        drugCode TEXT,
                        drugName TEXT,
                        dose TEXT,
                        timesPerDay TEXT,
                        days TEXT,
                        totalAmount TEXT,
                        insulance TEXT,
                        danga INTEGER,
                        amountValue INTEGER
                    );";

                using (var cmd2 = new SQLiteCommand(createDetailTable, conn))
                {
                    cmd2.ExecuteNonQuery(); 
                }
            }
        }

        // 요약 정보 삽입
        public static void InsertSummary(PrescriptionSummary summary)
        {
            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                string query = @"
                    INSERT INTO pastPrescriptionList 
                    (jumin, name, date, hospital, doctor, totalAmountSum)
                    VALUES (@jumin, @name, @date, @hospital, @doctor, @totalAmountSum);";

                using (var cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@jumin", summary.Jumin);
                    cmd.Parameters.AddWithValue("@name", summary.Name);
                    cmd.Parameters.AddWithValue("@date", summary.Date);
                    cmd.Parameters.AddWithValue("@hospital", summary.Hospital);
                    cmd.Parameters.AddWithValue("@doctor", summary.Doctor);
                    cmd.Parameters.AddWithValue("@totalAmountSum", summary.TotalAmountSum);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        // 상세 정보 삽입
        public static void InsertDetail(PrescriptionDetail detail)
        {
            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                string query = @"
                    INSERT INTO previousPrescriptionDetails 
                    (jumin, name, date, drugCode, drugName, dose, timesPerDay, days, totalAmount, insulance, danga, amountValue)
                    VALUES 
                    (@jumin, @name, @date, @drugCode, @drugName, @dose, @timesPerDay, @days, @totalAmount, @insulance, @danga, @amountValue);";

                using (var cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@jumin", detail.Jumin);
                    cmd.Parameters.AddWithValue("@name", detail.Name);
                    cmd.Parameters.AddWithValue("@date", detail.Date);
                    cmd.Parameters.AddWithValue("@drugCode", detail.DrugCode);
                    cmd.Parameters.AddWithValue("@drugName", detail.DrugName);
                    cmd.Parameters.AddWithValue("@dose", detail.Dose);
                    cmd.Parameters.AddWithValue("@timesPerDay", detail.TimesPerDay);
                    cmd.Parameters.AddWithValue("@days", detail.Days);
                    cmd.Parameters.AddWithValue("@totalAmount", detail.TotalAmount);
                    cmd.Parameters.AddWithValue("@insulance", detail.Insulance);
                    cmd.Parameters.AddWithValue("@danga", detail.Danga);
                    cmd.Parameters.AddWithValue("@amountValue", detail.AmountValue);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        // 요약 정보 조회
        public static List<PrescriptionSummary> GetSummariesByJumin(string jumin)
        {
            var list = new List<PrescriptionSummary>();

            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                string query = "SELECT * FROM pastPrescriptionList WHERE jumin = @jumin";
                using (var cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@jumin", jumin);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new PrescriptionSummary
                            {
                                Jumin = reader["jumin"].ToString(),
                                Name = reader["name"].ToString(),
                                Date = reader["date"].ToString(),
                                Hospital = reader["hospital"].ToString(),
                                Doctor = reader["doctor"].ToString(),
                                TotalAmountSum = Convert.ToInt32(reader["totalAmountSum"])
                            });
                        }
                    }
                }
            }

            return list;
        }

        // 상세 정보 조회
        public static List<PrescriptionDetail> GetDetailsByJumin(string jumin)
        {
            var list = new List<PrescriptionDetail>();

            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                string query = "SELECT * FROM previousPrescriptionDetails WHERE jumin = @jumin";
                using (var cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@jumin", jumin);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new PrescriptionDetail
                            {
                                Jumin = reader["jumin"].ToString(),
                                Name = reader["name"].ToString(),
                                Date = reader["date"].ToString(),
                                DrugCode = reader["drugCode"].ToString(),
                                DrugName = reader["drugName"].ToString(),
                                Dose = reader["dose"].ToString(),
                                TimesPerDay = reader["timesPerDay"].ToString(),
                                Days = reader["days"].ToString(),
                                TotalAmount = reader["totalAmount"].ToString(),
                                Insulance = reader["insulance"].ToString(),
                                Danga = Convert.ToInt32(reader["danga"]),
                                AmountValue = Convert.ToInt32(reader["amountValue"])
                            });
                        }
                    }
                }
            }

            return list;
        }
    }

    // 요약 클래스
    public class PrescriptionSummary
    {
        public string Jumin { get; set; }
        public string Name { get; set; }
        public string Date { get; set; }
        public string Hospital { get; set; }
        public string Doctor { get; set; }
        public int TotalAmountSum { get; set; }
    }

    // 상세 클래스
    public class PrescriptionDetail
    {
        public string Jumin { get; set; }
        public string Name { get; set; }
        public string Date { get; set; }
        public string DrugCode { get; set; }
        public string DrugName { get; set; }
        public string Dose { get; set; }
        public string TimesPerDay { get; set; }
        public string Days { get; set; }
        public string TotalAmount { get; set; }
        public string Insulance { get; set; }
        public int Danga { get; set; }
        public int AmountValue { get; set; }
    }
}
