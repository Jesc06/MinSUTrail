# MinSU (RMS)

The **SAS Record Management System** is a specialized administrative solution developed for Mindoro State University – Bongabong Campus. It is designed to streamline student record-keeping and administrative workflows through a secure, high-performance web API.

## Core Features

* **Secure Authentication:** Robust Identity management using JWT (JSON Web Tokens) with secure refresh token rotation.
* **OTP Verification:** Integrated One-Time Password (OTP) system via email for identity verification and account recovery.
* **Role-Based Access Control (RBAC):** Granular permission management to ensure data integrity and security.
* **Automated Provisioning:** Built-in data seeding for immediate system deployment and administrative setup.
* **Performance-Oriented:** Developed with a focus on low latency and efficient data handling.

## Technical Stack

* **Framework:** ASP.NET Core 9 (Web API)
* **Database:** Microsoft SQL Server
* **Data Access:** Entity Framework Core (Repository Pattern)
* **Security:** JWT, Refresh Tokens, and BCrypt Encryption
* **Communication:** SMTP-based Email Integration

## Configuration Guide

To set up the project locally, update the `appsettings.json` in the API project with your credentials:

```json
{
  "ConnectionStrings": {
    "default": "Server=YOUR_SERVER;Database=RecordManagementSystem;Trusted_Connection=True;TrustServerCertificate=True"
  },
  "Jwt": {
    "key": "Your_High_Security_Secret_Key_At_Least_32_Chars",
    "Issuer": "RecordManagementSystem",
    "Audience": "Users"
  },
  "EmailSettings": {
    "SmtpServer": "smtp.gmail.com",
    "SmtpPort": 587,
    "SmtpUser": "your-email@gmail.com",
    "SmtpPassword": "your-app-specific-password"
  }
}

```

## 📦 Dependencies

This system utilizes a custom email utility for OTP handling. You can view the core logic here:
[Email-Service-Asp.NetCore-Web-API](https://github.com/Jesc06/Email-Service-Asp.NetCore-Web-API.git)


---


Built with ❤️ by Joshuaesc
