using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace UI_NaviGO
{
    // ===============================
    // 🔹 KELAS MODEL OOP - PERBAIKAN
    // ===============================

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
        public string PaymentStatus { get; set; }
        public int ScheduleID { get; set; }
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

    public class RuteData
    {
        public string Rute { get; set; }
        public string Kapal { get; set; }
        public string ID { get; set; }
        public DateTime Tanggal { get; set; }
        public string JamBerangkat { get; set; }
        public string JamTiba { get; set; }
        public decimal Harga { get; set; }
        public int ScheduleID { get; set; }
        public string DepartureCity { get; set; }
        public string ArrivalCity { get; set; }
        public string ShipName { get; set; }
        public string ShipId { get; set; }
    }

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
    // 🔹 RIWAYAT TIKET - PERBAIKAN
    // ===============================
    public class RiwayatTiket
    {
        public int Id { get; set; } // TAMBAHKAN INI
        public string PaymentStatus { get; set; }
        public int BookingID { get; set; }
        public string ID { get; set; }
        public string Rute { get; set; }
        public DateTime TanggalPesan { get; set; }
        public DateTime TanggalBerangkat { get; set; }
        public string Waktu { get; set; }
        public List<PenumpangData> Penumpang { get; set; } = new List<PenumpangData>(); // UBAH KE LIST
        public string Kapal { get; set; }
        public string Kelas { get; set; }
        public decimal TotalHarga { get; set; }
        public string Status { get; set; }
        public string MetodePembayaran { get; set; }

        public string RingkasanPenumpang
        {
            get
            {
                if (Penumpang == null || Penumpang.Count == 0) return "0";

                var kategoriCount = new Dictionary<string, int>();
                foreach (var p in Penumpang)
                {
                    string kategori = p.Kategori ?? "Lainnya";
                    if (!kategoriCount.ContainsKey(kategori))
                        kategoriCount[kategori] = 0;

                    kategoriCount[kategori]++;
                }

                return string.Join(", ", kategoriCount.Select(kvp => $"{kvp.Value} ({kvp.Key})"));
            }
        }
    }

    // ===============================
    // 🔹 SELECTED TICKET DATA - PERBAIKAN
    // ===============================
    public static class SelectedTicketData
    {
        public static RuteKapal SelectedRute { get; set; }
        public static RuteData SelectedRutee { get; set; }
        public static string KelasDipilih { get; set; }
        public static string PaymentStatus { get; set; }
        
        public static decimal HargaKelas { get; set; }
        public static BookingDataa UpdatedBookingData { get; set; }
        public static decimal OriginalTotalPrice { get; set; }
        public static int JumlahDewasa { get; set; } = 1;
        public static int JumlahAnak { get; set; } = 0;
        public static int JumlahBayi { get; set; } = 0;
        public static string MetodePembayaran { get; set; }
        public static List<PenumpangData> Penumpang { get; set; } = new List<PenumpangData>();
        public static string Username { get; set; } = "Pengunjung NaviGo";
        public static string Email { get; set; } = "felicia.angel@example.com";
        public static int UserID { get; set; } = UserSession.UserId;

        // PERBAIKAN: Tambahkan properti yang missing
        public static RiwayatTiket TiketReschedule { get; set; }
        public static bool IsRescheduleMode => TiketReschedule != null;
        public static bool IsEdit { get; set; }
        public static decimal BasePrice { get; set; }
        public static int TicketID { get; set; } // TAMBAHKAN INI
        public static int BookingID { get; set; }

        public static void Reset()
        {
            SelectedRute = null;
            SelectedRutee = null;
            KelasDipilih = null;
            HargaKelas = 0;
            JumlahDewasa = 1;
            JumlahAnak = 0;
            JumlahBayi = 0;
            Penumpang.Clear();
            TiketReschedule = null;
            IsEdit = false;
            BasePrice = 0;
            TicketID = 0;
            BookingID = 0;
        }
    }


    // ===============================
    // 🔹 TIKET MANAGER
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
}