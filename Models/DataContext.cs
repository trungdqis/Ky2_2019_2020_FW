using System.Runtime.InteropServices;
using System.Data;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Ky2_2019_2020.Models
{
    public class DataContext {
        public string ConnectionString { get; set; }
        
        public DataContext(string connectionString) {
            this.ConnectionString = connectionString;
        }

        private SqlConnection getConnection() {
            return new SqlConnection(ConnectionString);
        }

        public int sqlInsertDiemCachLy(DiemCachLyModel newDiemCachLy) {
            using (SqlConnection conn = getConnection()) {
                conn.Open();
                
                var query = "INSERT INTO DIEMCACHLY VALUES (@MaDiemCachLy, @TenDiemCachLy, @DiaChi)";
                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("MaDiemCachLy", newDiemCachLy.MaDiemCachLy);
                cmd.Parameters.AddWithValue("TenDiemCachLy", newDiemCachLy.TenDiemCachLy);
                cmd.Parameters.AddWithValue("DiaChi", newDiemCachLy.DiaChi);

                return cmd.ExecuteNonQuery();
            }
        }

        public List<object> sqlListByTrieuChungCongNhan(int soTc) {
            List<object> result = new List<object>();

            using (SqlConnection conn = getConnection()) {
                conn.Open();

                var query = @"SELECT cn.TenCongNhan, cn.NamSinh, cn.NuocVe, COUNT(*) as SoTrieuChung
                                FROM CONGNHAN cn
                                JOIN CN_TC ct ON ct.MaCongNhan = cn.MaCongNhan
                                GROUP BY cn.TenCongNhan, cn.NamSinh, cn.NuocVe
                                HAVING COUNT(*) >= @soTcInput";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("soTcInput", soTc);

                using (var reader = cmd.ExecuteReader()) {
                    while (reader.Read()) {
                        result.Add(new {
                            TenCongNhan = reader["TenCongNhan"].ToString(),
                            NamSinh = Convert.ToInt32(reader["NamSinh"]),
                            NuocVe = reader["NuocVe"].ToString(),
                            SoTrieuChung = Convert.ToInt32(reader["SoTrieuChung"])
                        });                   
                    }
                    reader.Close();
                }               
                conn.Close();
            }
            return result;
        }

        public List<DiemCachLyModel> sqlListDiemCachLy() {
            List<DiemCachLyModel> result = new List<DiemCachLyModel>();

            using (SqlConnection conn = getConnection()) {
                conn.Open();

                var query = "SELECT MaDiemCachLy, TenDiemCachLy FROM DIEMCACHLY";

                SqlCommand cmd = new SqlCommand(query, conn);

                using (var reader = cmd.ExecuteReader()) {
                    while (reader.Read()) {
                        DiemCachLyModel diemCachLy = new DiemCachLyModel();
                        diemCachLy.MaDiemCachLy = reader["MaDiemCachLy"].ToString();
                        diemCachLy.TenDiemCachLy = reader["TenDiemCachLy"].ToString();

                        result.Add(diemCachLy);
                    }
                    reader.Close();
                }
                conn.Close();
            }
            return result;
        }

        public List<CongNhanModel> sqlListByDiemCachLyCongNhan(string maDiemCachLy) {
            List<CongNhanModel> result = new List<CongNhanModel>();

            using (SqlConnection conn = getConnection()) {
                conn.Open();

                var query = @"SELECT cn.MaCongNhan, cn.TenCongNhan
                                FROM CONGNHAN cn
                                JOIN DIEMCACHLY dcl ON dcl.MaDiemCachLy = cn.MaDiemCachLy
                                AND cn.MaDiemCachLy = @maDiemCachLyInput";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("maDiemCachLyInput", maDiemCachLy);

                using (var reader = cmd.ExecuteReader()) {
                    while (reader.Read()) {
                        CongNhanModel congNhan = new CongNhanModel();

                        congNhan.MaCongNhan = reader["MaCongNhan"].ToString();
                        congNhan.TenCongNhan = reader["TenCongNhan"].ToString();

                        result.Add(congNhan);
                    }
                    reader.Close();
                }
                conn.Close();
            }
            return result;
        }

        public CongNhanModel sqlFindCongNhanByMaCN(string maCongNhan) {
            CongNhanModel congNhan = new CongNhanModel();

            using (SqlConnection conn = getConnection()) {
                conn.Open();

                var query = @"SELECT MaCongNhan, TenCongNhan, GioiTinh, NamSinh, NuocVe
                                FROM CONGNHAN 
                                WHERE MaCongNhan = @maCongNhanInput";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("maCongNhanInput", maCongNhan);

                using (var reader = cmd.ExecuteReader()) {
                    while (reader.Read()) {
                        congNhan.MaCongNhan = reader["MaCongNhan"].ToString();
                        congNhan.TenCongNhan = reader["TenCongNhan"].ToString();
                        congNhan.GioiTinh = Convert.ToBoolean(reader["GioiTinh"]);
                        congNhan.NamSinh = Convert.ToInt32(reader["NamSinh"]);
                        congNhan.NuocVe = reader["NuocVe"].ToString();
                    }
                    reader.Close();
                }
                conn.Close();
            }
            return congNhan;
        }

        public int sqlDeleteCongNhanByMaCN(string maCongNhan) {
            using (SqlConnection conn = getConnection()) {
                conn.Open();

                sqlDeleteCNTCByMaCN(maCongNhan);

                var query = @"DELETE FROM CONGNHAN 
                                WHERE MaCongNhan = @maCongNhanInput";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("maCongNhanInput", maCongNhan);
                
                return cmd.ExecuteNonQuery();
            }
        }

        public void sqlDeleteCNTCByMaCN(string maCongNhan) {
            using (SqlConnection conn = getConnection()) {
                conn.Open();

                var query = @"DELETE FROM CN_TC 
                                WHERE MaCongNhan = @maCongNhanInput";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("maCongNhanInput", maCongNhan);

                cmd.ExecuteNonQuery();
            }
        }
    } 
}