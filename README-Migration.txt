Fundoo five-project structure migration

This solution preserves the original endpoint paths and business behavior as closely as possible.
The old separate Message Service API is replaced by Middleware/Services/Email/SmtpEmailSender.cs,
which sends SMTP mail directly using the same Gmail SMTP settings pattern.

Before running, configure these settings in User Secrets for the Fundoo project:
ConnectionStrings:DefaultConnection
Jwt:Key
EmailSettings:Email
EmailSettings:Password

The original API UserSecretsId was retained for connection/JWT settings. Email credentials previously
stored under the separate Message Service project secrets will need to be added to Fundoo's secrets.
Do not commit real credentials to appsettings.json.

EF Core migration files were moved to RepositoryLayer/Migrations and mechanically updated for the
FundooContext type and moved CLR namespaces. No migration operations were intentionally changed.
