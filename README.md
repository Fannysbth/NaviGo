# NaviGo
NaviGo aplikasi desktop C# (.NET, WPF/WinForms) untuk pemesanan tiket kapal dengan fitur booking, pembayaran online via Payment Gateway API, e-ticket (PDF), serta manajemen jadwal dan transaksi berbasis PostgreSQL.

# Shipers
- Ketua Kelompok: Fanny Elisabeth Panjaitan
- Anggota 1: Agatha Husna Amalia
- Anggota 2: Gabriele Ghea De Palma -  23/512218/TK/56306

# Fitur Utama
1. Login & Registrasi
Validasi email dan password
Menyimpan data pengguna ke database
2. Cek Jadwal & Rute Kapal
Filter berdasarkan asal, tujuan, tanggal
Menampilkan tabel jadwal lengkap
3. Input Data Penumpang
Tambah/Hapus penumpang
Kategori dewasa/anak/bayi
4. Pembayaran Tiket
Memilih kelas (Ekonomi/Bisnis/VIP)
Menghitung total harga otomatis
Konfirmasi nominal pembayaran
5. Pengiriman E-Ticket
Menggunakan SendGrid API
Format PDF lengkap dengan detail perjalanan & status LUNAS
6. Riwayat Pemesanan
Menampilkan transaksi sebelumnya

# Tech Stack
Frontend
C# Windows Forms (WinForms)
Figma untuk mengedit dan eksplorasi UI

Backend
Menggunakan C# OOP
DotNetEnv untuk load file .env

Database
PostgreSQL 
Hosting menggunakan supabase

External API
SendGrid Email API = Untuk mengirim E-Ticket PDF ke email pengguna.

Tools
Visual Studio 2022
Git dan GitHub
Supabase

# Desain UIUX
<img src="./Screenshot 2025-09-08 143116.png" alt="Class Diagram" width="600"/>

# Domain Model
Domain model awal mencakup beberapa entitas utama yang merepresentasikan objek dalam aplikasi:

- **User**
- **Product**
- **Order**
- **Payment**

Setelah diskusi kelompok, domain model dikembangkan menjadi **class diagram** lengkap dengan atribut dan method tambahan.


# Class Diagram Lengkap
Berikut adalah gambar class diagram hasil pengembangan:

<img src="./Class Diagram.drawio.png" alt="Class Diagram" width="600"/>

# Diagram ERD Lengkap
Berikut adalah gambar ERD database hasil pengembangan:

<img src="./NaviGo ERD.png" alt="Class Diagram" width="600"/>

# Instalasi Aplikasi
1.	Unduh dan ekstrak file NaviGO.zip
[Download NaviGo.zip](https://github.com/Fannysbth/NaviGo/blob/main/NaviGo.zip)
atau dapat diakses di gdrive berikut https://drive.google.com/drive/folders/11AUQDAAfamNmdxzPuExxE311N3r7CPtQ?usp=share_link
3.	Klik dua kali NaviGo.exe.
4.	Buka Folder NaviGo
5.	Klik dua kali NaviGO.exe (jalankan aplikasi)
6.	Tunggu hingga jendela utama muncul.
7.	Jika muncul peringatan keamanan Windows, klik More Info → Run Anyway.
8.	Halaman Login akan terbuka.
9.	Masukkan Akun demo berikut.
Email: sarikusuma@gmail.com
Password: sari123
10.	Eksplore Aplikasi NaviGo


