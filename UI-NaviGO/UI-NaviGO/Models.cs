using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace UI_NaviGO
{
    // ===============================
    // 🔹 KELAS MODEL OOP
    // ===============================

    // 🧱 Base Class: Encapsulation
    public class RuteKapal
    {
        private string id;
        private string kapal;
        private string rute;
        private DateTime tanggal;
        private string jamBerangkat;
        private string jamTiba;
        private string transit;
        private decimal harga;

        public string ID { get => id; set => id = value; }
        public string Kapal { get => kapal; set => kapal = value; }
        public string Rute { get => rute; set => rute = value; }
        public DateTime Tanggal { get => tanggal; set => tanggal = value; }
        public string JamBerangkat { get => jamBerangkat; set => jamBerangkat = value; }
        public string JamTiba { get => jamTiba; set => jamTiba = value; }
        public string Transit { get => transit; set => transit = value; }
        public decimal Harga
        {
            get => harga;
            set
            {
                if (value < 0)
                    throw new ArgumentException("Harga tidak boleh negatif!");
                harga = value;
            }
        }

        public RuteKapal(string id, string kapal, string rute, DateTime tanggal, string jamBerangkat, string jamTiba, string transit, decimal harga)
        {
            ID = id;
            Kapal = kapal;
            Rute = rute;
            Tanggal = tanggal;
            JamBerangkat = jamBerangkat;
            JamTiba = jamTiba;
            Transit = transit;
            Harga = harga;
        }

        // 🔸 Virtual agar bisa di-override oleh subclass (Polymorphism)
        public virtual string GetInfo()
        {
            return $"{ID} - {Kapal} ({Rute}) pada {Tanggal:dd MMM yyyy} - Rp {Harga:N0}";
        }

        // Method untuk mendapatkan kota asal
        public string GetKotaAsal()
        {
            if (string.IsNullOrEmpty(Rute) || !Rute.Contains("-"))
                return "Unknown";
            return Rute.Split('-')[0].Trim();
        }

        // Method untuk mendapatkan kota tujuan
        public string GetKotaTujuan()
        {
            if (string.IsNullOrEmpty(Rute) || !Rute.Contains("-"))
                return "Unknown";
            return Rute.Split('-')[1].Trim();
        }
    }

    // 🧱 Subclass: Inheritance + Polymorphism
    public class RuteVIP : RuteKapal
    {
        public string FasilitasVIP { get; set; }

        public RuteVIP(string id, string kapal, string rute, DateTime tanggal, string jamBerangkat, string jamTiba, string transit, decimal harga, string fasilitas)
            : base(id, kapal, rute, tanggal, jamBerangkat, jamTiba, transit, harga)
        {
            FasilitasVIP = fasilitas;
        }

        public override string GetInfo()
        {
            return base.GetInfo() + $" | Fasilitas: {FasilitasVIP}";
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
        public static string Email { get; set; } = "felicia.angel@example.com"; // TAMBAH PROPERTY EMAIL

        public static RiwayatTiket TiketReschedule { get; set; }
        public static bool IsRescheduleMode => TiketReschedule != null;
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
        public string ID { get; set; }
        public string Rute { get; set; }
        public DateTime TanggalPesan { get; set; }
        public DateTime TanggalBerangkat { get; set; }
        public string Waktu { get; set; }
        public string Penumpang { get; set; }
        public string Kapal { get; set; }
        public string Kelas { get; set; }
        public decimal TotalHarga { get; set; }
        public string Status { get; set; }
        public string MetodePembayaran { get; set; }// Confirmed, Selesai, Cancelled
    }
}