-- =============================================
-- INICJALIZACJA BAZY DANYCH W KONTENERZE
-- =============================================

-- Użyj master database dla operacji tworzenia bazy
USE master;
GO

-- Sprawdź czy baza danych istnieje i usuń ją jeśli tak
IF EXISTS (SELECT name FROM sys.databases WHERE name = 'PlannerDB')
BEGIN
    ALTER DATABASE PlannerDB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE PlannerDB;
    PRINT 'Stara baza danych PlannerDB została usunięta.';
END

-- Utwórz nową bazę danych
CREATE DATABASE PlannerDB
COLLATE SQL_Latin1_General_CP1_CI_AS;
PRINT 'Baza danych PlannerDB została utworzona.';
GO

-- Przełącz się na nową bazę danych
USE PlannerDB;
GO

-- =============================================
-- UPEWNIENIE SIĘ, ŻE PRACUJEMY NA WŁAŚCIWEJ BAZIE
-- =============================================

-- Dodaj sprawdzenie kontekstu bazy danych
IF DB_NAME() != 'PlannerDB'
BEGIN
    RAISERROR('Błąd: Nie udało się przełączyć na bazę PlannerDB', 16, 1);
    RETURN;
END

PRINT 'Kontekst bazy danych: ' + DB_NAME();
GO

-- =============================================
-- USUWANIE WSZYSTKICH TABEL (w odpowiedniej kolejności)
-- =============================================

PRINT 'Rozpoczynam usuwanie istniejących tabel...';

-- Usuń tabele w odwrotnej kolejności zależności
IF OBJECT_ID('dbo.Messages', 'U') IS NOT NULL DROP TABLE dbo.Messages;
IF OBJECT_ID('dbo.Notifications', 'U') IS NOT NULL DROP TABLE dbo.Notifications;
IF OBJECT_ID('dbo.ReservationParticipants', 'U') IS NOT NULL DROP TABLE dbo.ReservationParticipants;
IF OBJECT_ID('dbo.Reservations', 'U') IS NOT NULL DROP TABLE dbo.Reservations;
IF OBJECT_ID('dbo.EventScheduleStaff', 'U') IS NOT NULL DROP TABLE dbo.EventScheduleStaff;
IF OBJECT_ID('dbo.EventSchedules', 'U') IS NOT NULL DROP TABLE dbo.EventSchedules;
IF OBJECT_ID('dbo.EventTypes', 'U') IS NOT NULL DROP TABLE dbo.EventTypes;
IF OBJECT_ID('dbo.StaffMemberAvailabilities', 'U') IS NOT NULL DROP TABLE dbo.StaffMemberAvailabilities;
IF OBJECT_ID('dbo.StaffMemberSpecializations', 'U') IS NOT NULL DROP TABLE dbo.StaffMemberSpecializations;
IF OBJECT_ID('dbo.Specializations', 'U') IS NOT NULL DROP TABLE dbo.Specializations;
IF OBJECT_ID('dbo.Participants', 'U') IS NOT NULL DROP TABLE dbo.Participants;
IF OBJECT_ID('dbo.StaffMemberCompanies', 'U') IS NOT NULL DROP TABLE dbo.StaffMemberCompanies;
IF OBJECT_ID('dbo.Staff', 'U') IS NOT NULL DROP TABLE dbo.Staff;
IF OBJECT_ID('dbo.CompanyConfigs', 'U') IS NOT NULL DROP TABLE dbo.CompanyConfigs;
IF OBJECT_ID('dbo.CompanyHierarchies', 'U') IS NOT NULL DROP TABLE dbo.CompanyHierarchies;
IF OBJECT_ID('dbo.Companies', 'U') IS NOT NULL DROP TABLE dbo.Companies;

PRINT 'Usunięcie tabel zakończone.';
GO

-- =============================================
-- TWORZENIE TABEL OD NOWA
-- =============================================

PRINT 'Rozpoczynam tworzenie nowych tabel...';

-- Tabela Companies (podstawowa) - rozszerzona o funkcjonalność recepcji
CREATE TABLE dbo.Companies
(
    Id           UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    Name         NVARCHAR(255) NOT NULL,
    TaxCode      NVARCHAR(20)  NOT NULL,
    Street       NVARCHAR(255) NOT NULL,
    City         NVARCHAR(100) NOT NULL,
    PostalCode   NVARCHAR(10)  NOT NULL,
    Phone        NVARCHAR(20)  NOT NULL,
    Email        NVARCHAR(255) NOT NULL,
    IsParentNode BIT                          DEFAULT 0,
    IsReception  BIT                          DEFAULT 0,
    CreatedAt    DATETIME2(7)                 DEFAULT SYSUTCDATETIME()
);
PRINT 'Tabela Companies utworzona.';

-- Tabela CompanyHierarchies (hierarchia firm i recepcji)
CREATE TABLE dbo.CompanyHierarchies
(
    CompanyId       UNIQUEIDENTIFIER NOT NULL,
    ParentCompanyId UNIQUEIDENTIFIER NOT NULL,
    PRIMARY KEY (CompanyId),
    CONSTRAINT fk_ch_company FOREIGN KEY (CompanyId)
        REFERENCES dbo.Companies (Id) ON DELETE NO ACTION ON UPDATE NO ACTION,
    CONSTRAINT fk_ch_parent FOREIGN KEY (ParentCompanyId)
        REFERENCES dbo.Companies (Id) ON DELETE NO ACTION ON UPDATE NO ACTION
);
PRINT 'Tabela CompanyHierarchies utworzona.';

-- Tabela CompanyConfigs (konfiguracja firmy)
CREATE TABLE dbo.CompanyConfigs
(
    CompanyId             UNIQUEIDENTIFIER PRIMARY KEY NOT NULL,
    BreakTimeStaff        INT                          NOT NULL DEFAULT 0,
    BreakTimeParticipants INT                          NOT NULL DEFAULT 0,
    CONSTRAINT fk_companyconfig_company FOREIGN KEY (CompanyId)
        REFERENCES dbo.Companies (Id) ON DELETE CASCADE ON UPDATE NO ACTION
);
PRINT 'Tabela CompanyConfigs utworzona.';

-- Tabela Staff (personel) – bez CompanyId
CREATE TABLE dbo.Staff
(
    Id        UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    Role      NVARCHAR(50)  NOT NULL,
    Email     NVARCHAR(255) NOT NULL,
    Password  NVARCHAR(255) NOT NULL,
    FirstName NVARCHAR(100) NOT NULL,
    LastName  NVARCHAR(100) NOT NULL,
    Phone     NVARCHAR(30)  NOT NULL,
    CreatedAt DATETIME2(7)                  DEFAULT SYSUTCDATETIME(),
    IsDeleted BIT                          DEFAULT 0
);
PRINT 'Tabela Staff utworzona.';

-- Tabela StaffMemberCompanies (wiele-do-wielu pomiędzy Staff a Companies)
CREATE TABLE dbo.StaffMemberCompanies
(
    Id        UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    StaffMemberId   UNIQUEIDENTIFIER NOT NULL,
    CompanyId UNIQUEIDENTIFIER NOT NULL,
    CreatedAt DATETIME2(7)                 DEFAULT SYSUTCDATETIME(),
    CONSTRAINT fk_sc_staff FOREIGN KEY (StaffMemberId)
        REFERENCES dbo.Staff (Id) ON DELETE CASCADE ON UPDATE NO ACTION,
    CONSTRAINT fk_sc_company FOREIGN KEY (CompanyId)
        REFERENCES dbo.Companies (Id) ON DELETE CASCADE ON UPDATE NO ACTION,
    CONSTRAINT uq_staff_company UNIQUE (StaffMemberId, CompanyId)
);
PRINT 'Tabela StaffMemberCompanies utworzona.';

-- Tabela Participants (uczestnicy)
CREATE TABLE dbo.Participants
(
    Id          UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    CompanyId   UNIQUEIDENTIFIER NOT NULL,
    Email       NVARCHAR(255)    NOT NULL,
    FirstName   NVARCHAR(100)    NOT NULL,
    LastName    NVARCHAR(100)    NOT NULL,
    Phone       NVARCHAR(30)     NOT NULL,
    GdprConsent BIT              NOT NULL,
    CreatedAt   DATETIME2(7)                  DEFAULT SYSUTCDATETIME(),
    CONSTRAINT fk_participants_company FOREIGN KEY (CompanyId)
        REFERENCES dbo.Companies (Id) ON DELETE CASCADE ON UPDATE NO ACTION
);
PRINT 'Tabela Participants utworzona.';

-- Tabela Specializations (specjalizacje)
CREATE TABLE dbo.Specializations
(
    Id          UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    CompanyId   UNIQUEIDENTIFIER NOT NULL,
    Name        NVARCHAR(100)    NOT NULL,
    Description NVARCHAR(MAX),
    CONSTRAINT fk_specializations_company FOREIGN KEY (CompanyId)
        REFERENCES dbo.Companies (Id) ON DELETE CASCADE ON UPDATE NO ACTION
);
PRINT 'Tabela Specializations utworzona.';

-- Tabela StaffMemberSpecializations (specjalizacje personelu)
CREATE TABLE dbo.StaffMemberSpecializations
(
    Id               UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    CompanyId        UNIQUEIDENTIFIER NOT NULL,
    StaffMemberId    UNIQUEIDENTIFIER NOT NULL,
    SpecializationId UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT fk_staffspec_company FOREIGN KEY (CompanyId)
        REFERENCES dbo.Companies (Id) ON DELETE CASCADE ON UPDATE NO ACTION,
    CONSTRAINT fk_staffspec_staff FOREIGN KEY (StaffMemberId)
        REFERENCES dbo.Staff (Id) ON DELETE NO ACTION ON UPDATE NO ACTION,
    CONSTRAINT fk_staffspec_specialization FOREIGN KEY (SpecializationId)
        REFERENCES dbo.Specializations (Id) ON DELETE NO ACTION ON UPDATE NO ACTION
);
PRINT 'Tabela StaffMemberSpecializations utworzona.';

-- Tabela StaffMemberAvailabilities (dostępność personelu)
CREATE TABLE dbo.StaffMemberAvailabilities
(
    Id            UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    CompanyId     UNIQUEIDENTIFIER NOT NULL,
    StaffMemberId UNIQUEIDENTIFIER NOT NULL,
    Date          DATE             NOT NULL,
    StartTime     DATETIME2(7)     NOT NULL,
    EndTime       DATETIME2(7)     NOT NULL,
    IsAvailable   BIT              NOT NULL,
    CONSTRAINT fk_staffavail_company FOREIGN KEY (CompanyId)
        REFERENCES dbo.Companies (Id) ON DELETE CASCADE ON UPDATE NO ACTION,
    CONSTRAINT fk_staffavail_staff FOREIGN KEY (StaffMemberId)
        REFERENCES dbo.Staff (Id) ON DELETE NO ACTION ON UPDATE NO ACTION
);
PRINT 'Tabela StaffMemberAvailabilities utworzona.';

-- Tabela EventTypes (typy wydarzeń)
CREATE TABLE dbo.EventTypes
(
    Id              UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    CompanyId       UNIQUEIDENTIFIER NOT NULL,
    Name            NVARCHAR(100)    NOT NULL,
    Description     NVARCHAR(MAX),
    Duration        INT              NOT NULL,
    Price           DECIMAL(10, 2)   NOT NULL,
    MaxParticipants INT,
    MinStaff        INT                          DEFAULT 1,
    IsDeleted       BIT                          DEFAULT 0,
    CONSTRAINT fk_eventtypes_company FOREIGN KEY (CompanyId)
        REFERENCES dbo.Companies (Id) ON DELETE CASCADE ON UPDATE NO ACTION
);
PRINT 'Tabela EventTypes utworzona.';

-- Tabela EventSchedules (harmonogramy wydarzeń)
CREATE TABLE dbo.EventSchedules
(
    Id          UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    CompanyId   UNIQUEIDENTIFIER NOT NULL,
    EventTypeId UNIQUEIDENTIFIER NOT NULL,
    PlaceName   NVARCHAR(255)    NOT NULL,
    StartTime   DATETIME2(7)     NOT NULL,
    CreatedAt   DATETIME2(7)                  DEFAULT SYSUTCDATETIME(),
    Status      NVARCHAR(20)                 DEFAULT 'Active',
    CONSTRAINT fk_eventschedules_company FOREIGN KEY (CompanyId)
        REFERENCES dbo.Companies (Id) ON DELETE CASCADE ON UPDATE NO ACTION,
    CONSTRAINT fk_eventschedules_eventtype FOREIGN KEY (EventTypeId)
        REFERENCES dbo.EventTypes (Id) ON DELETE NO ACTION ON UPDATE NO ACTION
);
PRINT 'Tabela EventSchedules utworzona.';

-- Tabela EventScheduleStaff (personel przypisany do wydarzeń)
CREATE TABLE dbo.EventScheduleStaff
(
    Id              UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    CompanyId       UNIQUEIDENTIFIER NOT NULL,
    EventScheduleId UNIQUEIDENTIFIER NOT NULL,
    StaffMemberId   UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT fk_eventstaff_company FOREIGN KEY (CompanyId)
        REFERENCES dbo.Companies (Id) ON DELETE CASCADE ON UPDATE NO ACTION,
    CONSTRAINT fk_eventstaff_schedule FOREIGN KEY (EventScheduleId)
        REFERENCES dbo.EventSchedules (Id) ON DELETE NO ACTION ON UPDATE NO ACTION,
    CONSTRAINT fk_eventstaff_staff FOREIGN KEY (StaffMemberId)
        REFERENCES dbo.Staff (Id) ON DELETE NO ACTION ON UPDATE NO ACTION
);
PRINT 'Tabela EventScheduleStaff utworzona.';

-- Tabela Reservations (rezerwacje)
CREATE TABLE dbo.Reservations
(
    Id              UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    CompanyId       UNIQUEIDENTIFIER NOT NULL,
    EventScheduleId UNIQUEIDENTIFIER NOT NULL,
    Status          NVARCHAR(20)                 DEFAULT 'Confirmed',
    Notes           NVARCHAR(MAX),
    CreatedAt       DATETIME2(7)                 DEFAULT SYSUTCDATETIME(),
    CancelledAt     DATETIME2(7)     NULL,
    IsPaid          BIT                          DEFAULT 0,
    PaidAt          DATETIME2(7)     NULL,
    CONSTRAINT fk_reservations_company FOREIGN KEY (CompanyId)
        REFERENCES dbo.Companies (Id) ON DELETE CASCADE ON UPDATE NO ACTION,
    CONSTRAINT fk_reservations_eventschedule FOREIGN KEY (EventScheduleId)
        REFERENCES dbo.EventSchedules (Id) ON DELETE NO ACTION ON UPDATE NO ACTION
);
PRINT 'Tabela Reservations utworzona.';

-- Tabela ReservationParticipants (tabela asocjacyjna)
CREATE TABLE dbo.ReservationParticipants
(
    Id            UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    CompanyId     UNIQUEIDENTIFIER NOT NULL,
    ReservationId UNIQUEIDENTIFIER NOT NULL,
    ParticipantId UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT fk_resparticipants_company FOREIGN KEY (CompanyId)
        REFERENCES dbo.Companies (Id) ON DELETE CASCADE ON UPDATE NO ACTION,
    CONSTRAINT fk_resparticipants_reservation FOREIGN KEY (ReservationId)
        REFERENCES dbo.Reservations (Id) ON DELETE NO ACTION ON UPDATE NO ACTION,
    CONSTRAINT fk_resparticipants_participant FOREIGN KEY (ParticipantId)
        REFERENCES dbo.Participants (Id) ON DELETE NO ACTION ON UPDATE NO ACTION,
    CONSTRAINT uk_reservation_participant UNIQUE (ReservationId, ParticipantId)
);
PRINT 'Tabela ReservationParticipants utworzona.';

-- Tabela Notifications (powiadomienia)
CREATE TABLE dbo.Notifications
(
    Id            UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    CompanyId     UNIQUEIDENTIFIER NOT NULL,
    ReservationId UNIQUEIDENTIFIER NOT NULL,
    EmailStatus   NVARCHAR(20)                 DEFAULT 'Pending',
    SmsStatus     NVARCHAR(20)                 DEFAULT 'Pending',
    EmailSentAt   DATETIME2(7)     NULL,
    SmsSentAt     DATETIME2(7)     NULL,
    EmailContent  NVARCHAR(MAX),
    SmsContent    NVARCHAR(MAX),
    CONSTRAINT fk_notifications_company FOREIGN KEY (CompanyId)
        REFERENCES dbo.Companies (Id) ON DELETE CASCADE ON UPDATE NO ACTION,
    CONSTRAINT fk_notifications_reservation FOREIGN KEY (ReservationId)
        REFERENCES dbo.Reservations (Id) ON DELETE NO ACTION ON UPDATE NO ACTION
);
PRINT 'Tabela Notifications utworzona.';

-- Tabela Messages (wiadomości)
CREATE TABLE dbo.Messages
(
    Id         UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    CompanyId  UNIQUEIDENTIFIER NOT NULL,
    SenderId   UNIQUEIDENTIFIER,
    ReceiverId UNIQUEIDENTIFIER,
    Content    NVARCHAR(MAX)    NOT NULL,
    CreatedAt  DATETIME2(7)                  DEFAULT SYSUTCDATETIME(),
    CONSTRAINT fk_messages_company FOREIGN KEY (CompanyId)
        REFERENCES dbo.Companies (Id) ON DELETE CASCADE ON UPDATE NO ACTION,
    CONSTRAINT fk_messages_sender FOREIGN KEY (SenderId)
        REFERENCES dbo.Staff (Id) ON DELETE NO ACTION ON UPDATE NO ACTION,
    CONSTRAINT fk_messages_receiver FOREIGN KEY (ReceiverId)
        REFERENCES dbo.Staff (Id) ON DELETE NO ACTION ON UPDATE NO ACTION
);
PRINT 'Tabela Messages utworzona.';
GO

-- =============================================
-- INDEKSY DODATKOWE DLA OPTYMALIZACJI
-- =============================================

PRINT 'Tworzenie indeksów...';

-- Indeks na EventSchedules
CREATE NONCLUSTERED INDEX idx_event_schedule_datetime ON dbo.EventSchedules (StartTime);

-- Indeks na flagę IsReception dla szybkiego wyszukiwania recepcji
CREATE NONCLUSTERED INDEX idx_companies_is_reception ON dbo.Companies (IsReception);

-- Indeks na CompanyHierarchies dla zapytań hierarchicznych
CREATE NONCLUSTERED INDEX idx_company_hierarchy_parent ON dbo.CompanyHierarchies (ParentCompanyId);

-- Indeks na ReservationParticipants dla szybkiego wyszukiwania uczestników rezerwacji
CREATE NONCLUSTERED INDEX idx_reservation_participants_reservation ON dbo.ReservationParticipants (ReservationId);
CREATE NONCLUSTERED INDEX idx_reservation_participants_participant ON dbo.ReservationParticipants (ParticipantId);

-- Dodatkowe indeksy dla wydajności
CREATE NONCLUSTERED INDEX idx_staff_email ON dbo.Staff (Email);
CREATE NONCLUSTERED INDEX idx_participants_email ON dbo.Participants (Email);
CREATE NONCLUSTERED INDEX idx_reservations_status ON dbo.Reservations (Status);
CREATE NONCLUSTERED INDEX idx_eventschedules_status ON dbo.EventSchedules (Status);

PRINT 'Indeksy utworzone.';
GO

-- =============================================
-- TRIGGER: usuwa powiązane dane i na końcu usuwa firmę
-- =============================================

PRINT 'Tworzenie triggerów...';

-- Usuń trigger jeśli istnieje
IF OBJECT_ID('dbo.trg_delete_company', 'TR') IS NOT NULL
    DROP TRIGGER dbo.trg_delete_company;
GO

CREATE TRIGGER dbo.trg_delete_company
    ON dbo.Companies
    INSTEAD OF DELETE
    AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @DeletedCompanies TABLE (Id UNIQUEIDENTIFIER);
    INSERT INTO @DeletedCompanies (Id) SELECT Id FROM deleted;

    PRINT 'Uruchamianie triggera usuwania firmy...';

    BEGIN TRY
        -- Usuń wiadomości
        DELETE FROM dbo.Messages WHERE CompanyId IN (SELECT Id FROM @DeletedCompanies);

        -- Usuń powiadomienia
        DELETE FROM dbo.Notifications WHERE CompanyId IN (SELECT Id FROM @DeletedCompanies);

        -- Usuń uczestników rezerwacji
        DELETE FROM dbo.ReservationParticipants WHERE CompanyId IN (SELECT Id FROM @DeletedCompanies);

        -- Usuń rezerwacje
        DELETE FROM dbo.Reservations WHERE CompanyId IN (SELECT Id FROM @DeletedCompanies);

        -- Usuń przypisania personelu do wydarzeń
        DELETE FROM dbo.EventScheduleStaff WHERE CompanyId IN (SELECT Id FROM @DeletedCompanies);

        -- Usuń harmonogramy wydarzeń
        DELETE FROM dbo.EventSchedules WHERE CompanyId IN (SELECT Id FROM @DeletedCompanies);

        -- Usuń typy wydarzeń
        DELETE FROM dbo.EventTypes WHERE CompanyId IN (SELECT Id FROM @DeletedCompanies);

        -- Usuń dostępność personelu
        DELETE FROM dbo.StaffMemberAvailabilities WHERE CompanyId IN (SELECT Id FROM @DeletedCompanies);

        -- Usuń specjalizacje personelu
        DELETE FROM dbo.StaffMemberSpecializations WHERE CompanyId IN (SELECT Id FROM @DeletedCompanies);

        -- Usuń specjalizacje
        DELETE FROM dbo.Specializations WHERE CompanyId IN (SELECT Id FROM @DeletedCompanies);

        -- Usuń uczestników
        DELETE FROM dbo.Participants WHERE CompanyId IN (SELECT Id FROM @DeletedCompanies);

        -- Usuń przypisania pracowników do firm
        DELETE FROM dbo.StaffMemberCompanies WHERE CompanyId IN (SELECT Id FROM @DeletedCompanies);

        -- Usuń hierarchię firm
        DELETE FROM dbo.CompanyHierarchies WHERE CompanyId IN (SELECT Id FROM @DeletedCompanies);
        DELETE FROM dbo.CompanyHierarchies WHERE ParentCompanyId IN (SELECT Id FROM @DeletedCompanies);

        -- Usuń konfigurację firmy
        DELETE FROM dbo.CompanyConfigs WHERE CompanyId IN (SELECT Id FROM @DeletedCompanies);

        -- Na końcu usuń samą firmę
        DELETE FROM dbo.Companies WHERE Id IN (SELECT Id FROM @DeletedCompanies);

        PRINT 'Trigger usuwania firmy zakończony pomyślnie.';
    END TRY
    BEGIN CATCH
        PRINT 'Błąd w triggerze usuwania firmy: ' + ERROR_MESSAGE();
        THROW;
    END CATCH
END;
GO

PRINT 'Trigger utworzony.';
GO

-- =============================================
-- SPRAWDZENIE POPRAWNOŚCI UTWORZENIA TABEL
-- =============================================

PRINT 'Sprawdzanie poprawności utworzenia tabel...';

DECLARE @TableCount INT;
SELECT @TableCount = COUNT(*)
FROM INFORMATION_SCHEMA.TABLES
WHERE TABLE_TYPE = 'BASE TABLE' AND TABLE_SCHEMA = 'dbo';

PRINT 'Utworzono ' + CAST(@TableCount AS VARCHAR(10)) + ' tabel.';

-- =============================================
-- KOMUNIKAT KOŃCOWY
-- =============================================

PRINT '==============================================';
PRINT 'INICJALIZACJA BAZY DANYCH ZAKOŃCZONA POMYŚLNIE!';
PRINT 'Baza danych: ' + DB_NAME();
PRINT 'Data utworzenia: ' + CONVERT(VARCHAR(19), SYSUTCDATETIME(), 120);
PRINT '==============================================';