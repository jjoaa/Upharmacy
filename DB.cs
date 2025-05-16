using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace UPharmacy
{
    public static class DB
    {
        private static readonly string connectionString = "Server=127.0.0.1; Port=3306; Database=upharmacy; Uid=root; Pwd=000000; Charset=utf8;";

        static DB()
        {
            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    string createSummaryTable = @"
                        CREATE TABLE IF NOT EXISTS pastPrescriptionList (
                            jumin VARCHAR(20),
                            name VARCHAR(50),
                            date DATE,
                            hospital VARCHAR(100),
                            doctor VARCHAR(50),
                            totalAmountSum INT
                        );";

                    using (var cmd1 = new MySqlCommand(createSummaryTable, conn))
                    {
                        cmd1.ExecuteNonQuery();
                    }

                    string createDetailTable = @"
                        CREATE TABLE IF NOT EXISTS previousPrescriptionDetails (
                            jumin VARCHAR(20),
                            name VARCHAR(50),
                            date DATE,
                            drugCode VARCHAR(50),
                            drugName VARCHAR(100),
                            dose VARCHAR(20),
                            timesPerDay VARCHAR(20),
                            days VARCHAR(20),
                            totalAmount VARCHAR(20),
                            insulance VARCHAR(20),
                            danga INT,
                            amountValue INT
                        );";

                    using (var cmd2 = new MySqlCommand(createDetailTable, conn))
                    {
                        cmd2.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("DB 초기화 오류: " + ex.Message);
            }
        }

        public static void InsertSummary(PrescriptionSummary summary)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                string query = @"
                    INSERT INTO pastPrescriptionList 
                    (jumin, name, date, hospital, doctor, totalAmountSum)
                    VALUES (@jumin, @name, @date, @hospital, @doctor, @totalAmountSum);";

                using (var cmd = new MySqlCommand(query, conn))
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

        public static void InsertDetail(PrescriptionDetail detail)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                string query = @"
                    INSERT INTO previousPrescriptionDetails 
                    (jumin, name, date, drugCode, drugName, dose, timesPerDay, days, totalAmount, insulance, danga, amountValue)
                    VALUES 
                    (@jumin, @name, @date, @drugCode, @drugName, @dose, @timesPerDay, @days, @totalAmount, @insulance, @danga, @amountValue);";

                using (var cmd = new MySqlCommand(query, conn))
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

        public static List<PrescriptionSummary> GetSummariesByJumin(string jumin)
        {
            var list = new List<PrescriptionSummary>();

            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                string query = "SELECT * FROM pastPrescriptionList WHERE jumin = @jumin";
                using (var cmd = new MySqlCommand(query, conn))
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
                                Date = Convert.ToDateTime(reader["date"]).ToString("yyyy-MM-dd"),
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

        public static List<PrescriptionDetail> GetDetailsByJumin(string jumin)
        {
            var list = new List<PrescriptionDetail>();

            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                string query = "SELECT * FROM previousPrescriptionDetails WHERE jumin = @jumin";
                using (var cmd = new MySqlCommand(query, conn))
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
                                Date = Convert.ToDateTime(reader["date"]).ToString("yyyy-MM-dd"),
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

    public class PrescriptionSummary
    {
        public string Jumin { get; set; }
        public string Name { get; set; }
        public string Date { get; set; }
        public string Hospital { get; set; }
        public string Doctor { get; set; }
        public int TotalAmountSum { get; set; }
    }

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
