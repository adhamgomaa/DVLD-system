# 🚗 DVLD – Driving & Vehicle License Management System

## 📌 Description
DVLD (Driving & Vehicle License Department) is a **desktop management system** built using **C# WinForms** and **SQL Server**.  
The system is designed to manage driving license services such as issuing new licenses, renewals, replacements, tests, and driver records.

This project is implemented as **Project 1** based on real-world requirements of driving license departments.

---

## ⚙️ Technologies Used
- C#
- WinForms
- SQL Server
- ADO.NET
- .NET Framework

---

## 🧩 System Modules & Features

### 👤 People Management
- Add, update, delete, and search people by National Number
- Prevent duplicate persons using National Number
- Store personal information (name, birth date, address, phone, email, nationality, photo)

### 👥 Users Management
- Add system users linked to existing persons
- Update user information
- Disable / freeze user accounts
- Manage user permissions
- Track actions performed by users

---

### 📝 Applications Management
- Create and manage service applications
- Application types:
  - New Driving License
  - Retake Test
  - License Renewal
  - Replacement for Lost License
  - Replacement for Damaged License
  - Release Detained License
  - International Driving License
- Track application status (New, Cancelled, Completed)
- Prevent duplicate active applications of the same type

---

### 🚘 License Management
- Issue driving licenses based on license classes
- License classes include:
  - Motorcycles (small & heavy)
  - Private vehicles
  - Commercial vehicles (Taxi / Limousine)
  - Agricultural vehicles
  - Buses
  - Heavy trucks
- Validate minimum age and license rules
- Support multiple license classes per person
- Track license issue, expiry, and status

---

### 🧪 Tests Management
- Vision Test
- Theory Test
- Practical Driving Test
- Schedule tests manually
- Record test results (Pass / Fail)
- Allow retake tests with new applications and fees
- Prevent invalid test sequences

---

### 🌍 International License
- Issue international driving licenses
- Available only for valid private vehicle licenses
- Prevent issuing multiple active international licenses
- Maintain history of issued international licenses

---

### 🚫 License Detention
- Detain licenses with reason, fine, and date
- Release detained licenses after fine payment
- Track detention and release history

---

## 🛠 Database Setup
1. Open **SQL Server Management Studio**
2. Create a new empty database (e.g. `DVLD_DB`)
3. Run the database schema script:

> ⚠️ The script includes **schema only** (no real data).

---

## ▶️ How to Run the Project
1. Clone the repository
2. Open the solution file:
3. Update the connection string in `App.config`
4. Run the project using Visual Studio

---

## 🔒 Security Notes
- Sensitive files are excluded using `.gitignore`
- No passwords or real personal data are included
- Local configuration files are not tracked in GitHub