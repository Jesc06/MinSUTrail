# MinSU-BC Student Affairs Services (SAS) Record Management System

The **SAS Record Management System** is a robust, enterprise-grade solution tailored for Mindoro State University - Bongabong Campus. Built on the **Clean Architecture** pattern, this system ensures a scalable, maintainable, and secure environment for managing student records and administrative workflows.

## 🚀 Key Features

* **Clean Architecture Implementation:** Decoupled layers (Domain, Application, Infrastructure, and API) for maximum testability.
* **Identity & Security:** Secure authentication via JWT (JSON Web Tokens) featuring Refresh Token rotation.
* **Automatic Data Seeding:** Seamless initial setup with automated Admin account creation upon system startup.
* **Email-Driven OTP:** Integrated One-Time Password (OTP) system for secure verification and password recovery.
* **Modern API Design:** Built with ASP.NET Core 9, prioritizing high performance and developer productivity.

---

## 🛠️ Technical Stack

* **Backend:** ASP.NET Core 9 (Web API)
* **Database:** Microsoft SQL Server
* **ORM:** Entity Framework Core (Code-First)
* **Security:** JWT Authentication & Role-Based Access Control (RBAC)
* **Communication:** SMTP-based Email Service

---

## ⚙️ Configuration Guide

To get the project running locally, update the `appsettings.json` file in your Web API project with your credentials:

```json
{
  "ConnectionStrings": {
    "default": "Server=YOUR_SERVER;Database=RecordManagementSystem;Trusted_Connection=True;TrustServerCertificate=True"
  },
  "Jwt": {
    "key": "Your_High_Security_Secret_Key_At_Least_32_Chars",
    "Issuer": "RecordManagementSystem",
    "Audience": "Users",
    "DurationInMinutes": 15,
    "RefreshTokenDurationInMinutes": 1440
  },
  "EmailSettings": {
    "SmtpServer": "smtp.gmail.com",
    "SmtpPort": 587,
    "SmtpUser": "your-email@gmail.com",
    "SmtpPassword": "your-app-specific-password"
  },
  "AdminSeededAccount": {
    "Email": "admin@minsu.edu.ph",
    "Password": "SecurePassword123!",
    "FirstName": "Admin",
    "MiddleName": "SAS",
    "LastName": "User"
  }
}

```

---

## 📧 Dependency: Email OTP Service

This project utilizes a specialized email utility. You can find the source code and documentation for the integration module here:
👉 [Email-Service-Asp.NetCore-Web-API](https://github.com/Jesc06/Email-Service-Asp.NetCore-Web-API.git)

---

## 🚧 Project Status

> [!NOTE]
> This project is currently **on hold** as development resources are being diverted to other priority academic requirements. Updates and maintenance will resume at a later date.

---

## 👨‍💻 Author

**John Joshua Manalo Escarez** *Full Stack Developer* Made with ❤️ for the Mindoro State University Community.

---

### Pro-Tips for your Repository:

1. **Add a `.gitignore`:** Ensure you aren't committing your actual `appsettings.json` secrets to GitHub.
2. **License File:** Consider adding an MIT or Apache license to make it look even more official.
3. **API Documentation:** If you have Swagger enabled, you could add a "Usage" section explaining how to access `/swagger`.
