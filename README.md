# `Mindoro State University Bongabong Campus SAS RecordManagement System`
#### `AppSettings.json Configuration for this project`
```json
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "default": "Server=your server;Database=RecordManagementSystem;Trusted_Connection=True;TrustServerCertificate=True"
  },
"Jwt": {
  "key": "AspDotnet_Core_Clean_Architecture_Dotnet_nine",
  "Issuer": "RecordManagementSystem",
  "Audience": "Users",
  "DurationInMinutes": 1,
  "RefreshTokenDurationInMinutes": 2
},
"EmailSettings": {
  "SmtpServer": "smtp.gmail.com",
  "SmtpPort": 587,
  "SmtpUser": "escarezjohnjoshuamanalo@gmail.com",
  "SmtpPassword": "your Smtp password"
},
"AdminSeededAccount": {
  "Email": "your email",
  "Password": "your password",
  "FirstName": "your firstname",
  "MiddleName": "your middlename",
  "LastName":  "your lastname"
}
```
