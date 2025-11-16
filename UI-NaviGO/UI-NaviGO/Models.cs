using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace UI_NaviGO
{
    // ===============================
    // 🔹 KELAS MODEL OOP
    // ===============================

    // 🧱 Base Class: Encapsulation
    // RuteKapal.cs
    public class RuteKapal
    {
        public string ID { get; set; }
        public string Kapal { get; set; }
        public string Rute { get; set; }
        public DateTime Tanggal { get; set; }
        public string JamBerangkat { get; set; }
        public string JamTiba { get; set; }
        public string Transit { get; set; }
        public decimal Harga { get; set; }
        public int ScheduleID { get; set; } // TAMBAH INI
        public string ShipID { get; set; }

        public RuteKapal(string id, string kapal, string rute, DateTime tanggal,
                        string jamBerangkat, string jamTiba, string transit,
                        decimal harga, int scheduleID)
        {
            ID = id;
            Kapal = kapal;
            Rute = rute;
            Tanggal = tanggal;
            JamBerangkat = jamBerangkat;
            JamTiba = jamTiba;
            Transit = transit;
            Harga = harga;
            ScheduleID = scheduleID;
        }

        public string GetKotaAsal()
        {
            return Rute.Split('-')[0].Trim();
        }

        public string GetKotaTujuan()
        {
            return Rute.Split('-')[1].Trim();
        }

        public virtual string GetInfo()
        {
            return $"{Kapal} | {Rute} | {Tanggal:dd MMM yyyy} | {JamBerangkat}-{JamTiba} | Harga: {Harga}";
        }

    }

    // 🧱 Subclass: Inheritance + Polymorphism
    public class RuteVIP : RuteKapal
    {
        public string FasilitasVIP { get; set; }

        public RuteVIP(string id, string kapal, string rute, DateTime tanggal,
                       string jamBerangkat, string jamTiba, string transit,
                       decimal harga, int scheduleID, string fasilitas)
            : base(id, kapal, rute, tanggal, jamBerangkat, jamTiba, transit, harga, scheduleID)
        {
            FasilitasVIP = fasilitas;
        }
    }


    public class PenumpangData
    {
        public string Kategori { get; set; }
        public string Nama { get; set; }
        public string NIK { get; set; }
    }

    // ===============================
    // 🔹 STATIC DATA STORAGE
    // ===============================
    public static class SelectedTicketData
    {
        public static RuteKapal SelectedRute { get; set; }
        public static string KelasDipilih { get; set; }
        public static decimal HargaKelas { get; set; }
        public static int JumlahDewasa { get; set; } = 1;
        public static int JumlahAnak { get; set; } = 0;
        public static int JumlahBayi { get; set; } = 0;
        public static string MetodePembayaran { get; set; }
        public static List<PenumpangData> Penumpang { get; set; } = new List<PenumpangData>();
        public static string Username { get; set; } = "Pengunjung NaviGo";
        public static string Email { get; set; } = "felicia.angel@example.com";
        public static int UserID { get; set; } = UserSession.UserId; // TAMBAHKAN INI - default value

        public static RiwayatTiket TiketReschedule { get; set; }
        public static bool IsRescheduleMode => TiketReschedule != null;
        public static int BookingID { get; set; }

        public static void Reset()
        {
            SelectedRute = null;
            KelasDipilih = null;
            HargaKelas = 0;
            JumlahDewasa = 1;
            JumlahAnak = 0;
            JumlahBayi = 0;
            Penumpang.Clear();
        }

    }

    // ===============================
    // 🔹 TIKET MANAGER (UNTUK HISTORY)
    // ===============================
    public static class TiketManager
    {
        public static List<RiwayatTiket> DaftarTiket { get; set; } = new List<RiwayatTiket>();

        public static void TambahTiket(RiwayatTiket tiket)
        {
            DaftarTiket.Add(tiket);
        }

        public static RiwayatTiket DapatkanTiketById(string id)
        {
            return DaftarTiket.Find(t => t.ID == id);
        }
    }

    // ===============================
    // 🔹 CLASS UNTUK HISTORY
    // ===============================
    public class RiwayatTiket
    {
        public int BookingID { get; set; }
        public string ID { get; set; }  // booking_reference
        public string Rute { get; set; }  // route_name / departure - arrival
        public DateTime TanggalPesan { get; set; }  // booking_date
        public DateTime TanggalBerangkat { get; set; }  // departure_date
        public string Waktu { get; set; }  // departure_time
        public string Penumpang { get; set; }  // gabungkan nama penumpang & kategori
        public string Kapal { get; set; }  // ship_name
        public string Kelas { get; set; }  // selected_class
        public decimal TotalHarga { get; set; }  // total_price
        public string Status { get; set; }  // payment_status
        public string MetodePembayaran { get; set; }

        public string RingkasanPenumpang
        {
            get
            {
                if (string.IsNullOrEmpty(Penumpang)) return "0";

                // Penumpang disimpan seperti: "John Doe (Dewasa), Jane Doe (Anak-anak), Bob (Dewasa)"
                var kategoriCount = new Dictionary<string, int>();

                var penumpangList = Penumpang.Split(',');
                foreach (var p in penumpangList)
                {
                    var start = p.IndexOf('(');
                    var end = p.IndexOf(')');

                    string kategori = "Lainnya";
                    if (start >= 0 && end > start)
                    {
                        kategori = p.Substring(start + 1, end - start - 1).Trim();
                    }

                    if (!kategoriCount.ContainsKey(kategori))
                        kategoriCount[kategori] = 0;

                    kategoriCount[kategori]++;
                }

                // Buat string ringkasan: "2 (Dewasa), 1 (Anak-anak)"
                return string.Join(", ", kategoriCount.Select(kvp => $"{kvp.Value} ({kvp.Key})"));
            }
        }

    }

}