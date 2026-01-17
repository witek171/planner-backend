-- Przełącz się na bazę danych
USE PlannerDB;
GO

-- Sprawdź czy pracujemy na właściwej bazie
IF DB_NAME() != 'PlannerDB'
BEGIN
    RAISERROR('Błąd: Nie udało się przełączyć na bazę PlannerDB', 16, 1);
    RETURN;
END

PRINT 'Rozpoczynam wypełnianie bazy danych przykładowymi danymi...';
PRINT 'Kontekst bazy danych: ' + DB_NAME();
GO

-- =============================================
-- WYPEŁNIANIE BAZY DANYCH PRZYKŁADOWYMI DANYMI 
-- =============================================

-- =============================================
-- 1. Companies - dodanie firm (klubów sportowych)
-- =============================================

DECLARE @MainCompanyId UNIQUEIDENTIFIER = NEWID();
DECLARE @Branch1Id UNIQUEIDENTIFIER = NEWID();
DECLARE @Branch2Id UNIQUEIDENTIFIER = NEWID();

INSERT INTO Companies (Id, Name, TaxCode, Street, City, PostalCode, Phone, Email, IsParentNode, IsReception)
VALUES
    (@MainCompanyId, 'SportFit Group', 'PL1234567890', 'Sportowa 1', 'Warszawa', '00-001', '+48123456789',
     'contact@sportfitgroup.pl', 1, 0),
    (@Branch1Id, 'SportFit Centrum', 'PL2345678901', 'Centralna 10', 'Warszawa', '00-002', '+48234567890',
     'centrum@sportfit.pl', 0, 1),
    (@Branch2Id, 'SportFit Południe', 'PL3456789012', 'Południowa 20', 'Kraków', '30-001', '+48345678901',
     'poludnie@sportfit.pl', 0, 1);

-- =============================================
-- 2. CompanyHierarchies - dodanie hierarchii firm
-- =============================================

INSERT INTO CompanyHierarchies (CompanyId, ParentCompanyId)
VALUES
    (@Branch1Id, @MainCompanyId),
    (@Branch2Id, @MainCompanyId);

-- =============================================
--  CompanyConfigs
-- =============================================

INSERT INTO CompanyConfigs (CompanyId, BreakTimeStaff, BreakTimeParticipants)
VALUES
    (@MainCompanyId, 0, 0),
    (@Branch1Id, 0, 0),
    (@Branch2Id, 0, 0);

-- =============================================
-- 3. Staff - 15 pracowników dla SportFit Centrum
-- =============================================

-- Pracownicy recepcji (3)
DECLARE @StaffRec1Id UNIQUEIDENTIFIER = NEWID();
DECLARE @StaffRec2Id UNIQUEIDENTIFIER = NEWID();
DECLARE @StaffRec3Id UNIQUEIDENTIFIER = NEWID();

-- Trenerzy (10)
DECLARE @Trainer1Id UNIQUEIDENTIFIER = NEWID();
DECLARE @Trainer2Id UNIQUEIDENTIFIER = NEWID();
DECLARE @Trainer3Id UNIQUEIDENTIFIER = NEWID();
DECLARE @Trainer4Id UNIQUEIDENTIFIER = NEWID();
DECLARE @Trainer5Id UNIQUEIDENTIFIER = NEWID();
DECLARE @Trainer6Id UNIQUEIDENTIFIER = NEWID();
DECLARE @Trainer7Id UNIQUEIDENTIFIER = NEWID();
DECLARE @Trainer8Id UNIQUEIDENTIFIER = NEWID();
DECLARE @Trainer9Id UNIQUEIDENTIFIER = NEWID();
DECLARE @Trainer10Id UNIQUEIDENTIFIER = NEWID();

-- Managerowie (2)
DECLARE @Manager1Id UNIQUEIDENTIFIER = NEWID();
DECLARE @Manager2Id UNIQUEIDENTIFIER = NEWID();

INSERT INTO Staff (Id, Role, Email, Password, FirstName, LastName, Phone)
VALUES
    -- Pracownicy recepcji
    (@StaffRec1Id, 'ReceptionEmployee', 'r@r.pl', '$2a$11$JTCf2we/VdmC1Viuhxqwf.WVyRMRH5gxgt2WvOicBCNzO84VQI.6C', 'Anna', 'Kowalska', '+48700100101'),
    (@StaffRec2Id, 'ReceptionEmployee', 'barbara.nowak@sportfit.pl', '$2a$11$JTCf2we/VdmC1Viuhxqwf.WVyRMRH5gxgt2WvOicBCNzO84VQI.6C', 'Barbara', 'Nowak', '+48700100102'),
    (@StaffRec3Id, 'ReceptionEmployee', 'celina.wisniewski@sportfit.pl', '$2a$11$JTCf2we/VdmC1Viuhxqwf.WVyRMRH5gxgt2WvOicBCNzO84VQI.6C', 'Celina', 'Wiśniewska', '+48700100103'),

    -- Trenerzy (10)
    (@Trainer1Id, 'Trainer', 't@t.pl', '$2a$11$JTCf2we/VdmC1Viuhxqwf.WVyRMRH5gxgt2WvOicBCNzO84VQI.6C', 'Dariusz', 'Malinowski', '+48700200201'),
    (@Trainer2Id, 'Trainer', 'ewa.jablonska@sportfit.pl', '$2a$11$JTCf2we/VdmC1Viuhxqwf.WVyRMRH5gxgt2WvOicBCNzO84VQI.6C', 'Ewa', 'Jabłońska', '+48700200202'),
    (@Trainer3Id, 'Trainer', 'filip.kowalczyk@sportfit.pl', '$2a$11$JTCf2we/VdmC1Viuhxqwf.WVyRMRH5gxgt2WvOicBCNzO84VQI.6C', 'Filip', 'Kowalczyk', '+48700200203'),
    (@Trainer4Id, 'Trainer', 'grazyna.lewandowska@sportfit.pl', '$2a$11$JTCf2we/VdmC1Viuhxqwf.WVyRMRH5gxgt2WvOicBCNzO84VQI.6C', 'Grażyna', 'Lewandowska', '+48700200204'),
    (@Trainer5Id, 'Trainer', 'henryk.wojcik@sportfit.pl', '$2a$11$JTCf2we/VdmC1Viuhxqwf.WVyRMRH5gxgt2WvOicBCNzO84VQI.6C', 'Henryk', 'Wójcik', '+48700200205'),
    (@Trainer6Id, 'Trainer', 'irena.kaminska@sportfit.pl', '$2a$11$JTCf2we/VdmC1Viuhxqwf.WVyRMRH5gxgt2WvOicBCNzO84VQI.6C', 'Irena', 'Kamińska', '+48700200206'),
    (@Trainer7Id, 'Trainer', 'jacek.zielinski@sportfit.pl', '$2a$11$JTCf2we/VdmC1Viuhxqwf.WVyRMRH5gxgt2WvOicBCNzO84VQI.6C', 'Jacek', 'Zieliński', '+48700200207'),
    (@Trainer8Id, 'Trainer', 'karolina.szymanska@sportfit.pl', '$2a$11$JTCf2we/VdmC1Viuhxqwf.WVyRMRH5gxgt2WvOicBCNzO84VQI.6C', 'Karolina', 'Szymańska', '+48700200208'),
    (@Trainer9Id, 'Trainer', 'lukasz.wozniak@sportfit.pl', '$2a$11$JTCf2we/VdmC1Viuhxqwf.WVyRMRH5gxgt2WvOicBCNzO84VQI.6C', 'Łukasz', 'Woźniak', '+48700200209'),
    (@Trainer10Id, 'Trainer', 'magdalena.dabrowski@sportfit.pl', '$2a$11$JTCf2we/VdmC1Viuhxqwf.WVyRMRH5gxgt2WvOicBCNzO84VQI.6C', 'Magdalena', 'Dąbrowska', '+48700200210'),

    -- Managerowie
    (@Manager1Id, 'Manager', 'm@m.pl', '$2a$11$JTCf2we/VdmC1Viuhxqwf.WVyRMRH5gxgt2WvOicBCNzO84VQI.6C', 'Janusz', 'Mazur', '+48700300301'),
    (@Manager2Id, 'Manager', 'kamila.kaczmarek@sportfit.pl', '$2a$11$JTCf2we/VdmC1Viuhxqwf.WVyRMRH5gxgt2WvOicBCNzO84VQI.6C', 'Kamila', 'Kaczmarek', '+48700300302');

-- Powiązania Staff z Companies
INSERT INTO StaffMemberCompanies (StaffMemberId, CompanyId)
VALUES
    (@StaffRec1Id, @Branch1Id),
    (@StaffRec2Id, @Branch1Id),
    (@StaffRec3Id, @Branch1Id),
    (@Trainer1Id, @Branch1Id),
    (@Trainer2Id, @Branch1Id),
    (@Trainer3Id, @Branch1Id),
    (@Trainer4Id, @Branch1Id),
    (@Trainer5Id, @Branch1Id),
    (@Trainer6Id, @Branch1Id),
    (@Trainer7Id, @Branch1Id),
    (@Trainer8Id, @Branch1Id),
    (@Trainer9Id, @Branch1Id),
    (@Trainer10Id, @Branch1Id),
    (@Manager1Id, @MainCompanyId),
    (@Manager1Id, @Branch1Id),
    (@Manager1Id, @Branch2Id),
    (@Manager2Id, @Branch1Id);

-- =============================================
-- 4. Participants - 50 uczestników dla SportFit Centrum
-- =============================================

DECLARE @P1 UNIQUEIDENTIFIER = NEWID();
DECLARE @P2 UNIQUEIDENTIFIER = NEWID();
DECLARE @P3 UNIQUEIDENTIFIER = NEWID();
DECLARE @P4 UNIQUEIDENTIFIER = NEWID();
DECLARE @P5 UNIQUEIDENTIFIER = NEWID();
DECLARE @P6 UNIQUEIDENTIFIER = NEWID();
DECLARE @P7 UNIQUEIDENTIFIER = NEWID();
DECLARE @P8 UNIQUEIDENTIFIER = NEWID();
DECLARE @P9 UNIQUEIDENTIFIER = NEWID();
DECLARE @P10 UNIQUEIDENTIFIER = NEWID();
DECLARE @P11 UNIQUEIDENTIFIER = NEWID();
DECLARE @P12 UNIQUEIDENTIFIER = NEWID();
DECLARE @P13 UNIQUEIDENTIFIER = NEWID();
DECLARE @P14 UNIQUEIDENTIFIER = NEWID();
DECLARE @P15 UNIQUEIDENTIFIER = NEWID();
DECLARE @P16 UNIQUEIDENTIFIER = NEWID();
DECLARE @P17 UNIQUEIDENTIFIER = NEWID();
DECLARE @P18 UNIQUEIDENTIFIER = NEWID();
DECLARE @P19 UNIQUEIDENTIFIER = NEWID();
DECLARE @P20 UNIQUEIDENTIFIER = NEWID();
DECLARE @P21 UNIQUEIDENTIFIER = NEWID();
DECLARE @P22 UNIQUEIDENTIFIER = NEWID();
DECLARE @P23 UNIQUEIDENTIFIER = NEWID();
DECLARE @P24 UNIQUEIDENTIFIER = NEWID();
DECLARE @P25 UNIQUEIDENTIFIER = NEWID();
DECLARE @P26 UNIQUEIDENTIFIER = NEWID();
DECLARE @P27 UNIQUEIDENTIFIER = NEWID();
DECLARE @P28 UNIQUEIDENTIFIER = NEWID();
DECLARE @P29 UNIQUEIDENTIFIER = NEWID();
DECLARE @P30 UNIQUEIDENTIFIER = NEWID();
DECLARE @P31 UNIQUEIDENTIFIER = NEWID();
DECLARE @P32 UNIQUEIDENTIFIER = NEWID();
DECLARE @P33 UNIQUEIDENTIFIER = NEWID();
DECLARE @P34 UNIQUEIDENTIFIER = NEWID();
DECLARE @P35 UNIQUEIDENTIFIER = NEWID();
DECLARE @P36 UNIQUEIDENTIFIER = NEWID();
DECLARE @P37 UNIQUEIDENTIFIER = NEWID();
DECLARE @P38 UNIQUEIDENTIFIER = NEWID();
DECLARE @P39 UNIQUEIDENTIFIER = NEWID();
DECLARE @P40 UNIQUEIDENTIFIER = NEWID();
DECLARE @P41 UNIQUEIDENTIFIER = NEWID();
DECLARE @P42 UNIQUEIDENTIFIER = NEWID();
DECLARE @P43 UNIQUEIDENTIFIER = NEWID();
DECLARE @P44 UNIQUEIDENTIFIER = NEWID();
DECLARE @P45 UNIQUEIDENTIFIER = NEWID();
DECLARE @P46 UNIQUEIDENTIFIER = NEWID();
DECLARE @P47 UNIQUEIDENTIFIER = NEWID();
DECLARE @P48 UNIQUEIDENTIFIER = NEWID();
DECLARE @P49 UNIQUEIDENTIFIER = NEWID();
DECLARE @P50 UNIQUEIDENTIFIER = NEWID();

INSERT INTO Participants (Id, CompanyId, Email, FirstName, LastName, Phone, GdprConsent)
VALUES
    (@P1, @Branch1Id, 'marek.adamski@email.com', 'Marek', 'Adamski', '+48800400401', 1),
    (@P2, @Branch1Id, 'natalia.barska@gmail.com', 'Natalia', 'Barska', '+48800400402', 1),
    (@P3, @Branch1Id, 'olgierd.cichocki@outlook.com', 'Olgierd', 'Cichocki', '+48800400403', 1),
    (@P4, @Branch1Id, 'anna.dabrowska@yahoo.com', 'Anna', 'Dąbrowska', '+48800400404', 1),
    (@P5, @Branch1Id, 'piotr.eliasz@email.com', 'Piotr', 'Eliasz', '+48800400405', 1),
    (@P6, @Branch1Id, 'joanna.filipek@gmail.com', 'Joanna', 'Filipek', '+48800400406', 1),
    (@P7, @Branch1Id, 'tomasz.grabowski@outlook.com', 'Tomasz', 'Grabowski', '+48800400407', 1),
    (@P8, @Branch1Id, 'katarzyna.halicka@yahoo.com', 'Katarzyna', 'Halicka', '+48800400408', 1),
    (@P9, @Branch1Id, 'michal.iwanski@email.com', 'Michał', 'Iwański', '+48800400409', 1),
    (@P10, @Branch1Id, 'agnieszka.jasinska@gmail.com', 'Agnieszka', 'Jasińska', '+48800400410', 1),
    (@P11, @Branch1Id, 'robert.krol@outlook.com', 'Robert', 'Król', '+48800400411', 1),
    (@P12, @Branch1Id, 'monika.lewinska@yahoo.com', 'Monika', 'Lewińska', '+48800400412', 1),
    (@P13, @Branch1Id, 'adam.majewski@email.com', 'Adam', 'Majewski', '+48800400413', 1),
    (@P14, @Branch1Id, 'paula.nowakowska@gmail.com', 'Paula', 'Nowakowska', '+48800400414', 1),
    (@P15, @Branch1Id, 'krzysztof.olszewski@outlook.com', 'Krzysztof', 'Olszewski', '+48800400415', 1),
    (@P16, @Branch1Id, 'beata.pawlak@yahoo.com', 'Beata', 'Pawlak', '+48800400416', 1),
    (@P17, @Branch1Id, 'marcin.rataj@email.com', 'Marcin', 'Rataj', '+48800400417', 1),
    (@P18, @Branch1Id, 'ewa.sobczak@gmail.com', 'Ewa', 'Sobczak', '+48800400418', 1),
    (@P19, @Branch1Id, 'jan.tomczak@outlook.com', 'Jan', 'Tomczak', '+48800400419', 1),
    (@P20, @Branch1Id, 'zofia.urbaniak@yahoo.com', 'Zofia', 'Urbaniak', '+48800400420', 1),
    (@P21, @Branch1Id, 'pawel.walczak@email.com', 'Paweł', 'Walczak', '+48800400421', 1),
    (@P22, @Branch1Id, 'marta.zajac@gmail.com', 'Marta', 'Zając', '+48800400422', 1),
    (@P23, @Branch1Id, 'grzegorz.adamczyk@outlook.com', 'Grzegorz', 'Adamczyk', '+48800400423', 1),
    (@P24, @Branch1Id, 'aleksandra.bak@yahoo.com', 'Aleksandra', 'Bąk', '+48800400424', 1),
    (@P25, @Branch1Id, 'wojciech.czajka@email.com', 'Wojciech', 'Czajka', '+48800400425', 1),
    (@P26, @Branch1Id, 'dorota.dudek@gmail.com', 'Dorota', 'Dudek', '+48800400426', 1),
    (@P27, @Branch1Id, 'rafal.gorski@outlook.com', 'Rafał', 'Górski', '+48800400427', 1),
    (@P28, @Branch1Id, 'iwona.hajduk@yahoo.com', 'Iwona', 'Hajduk', '+48800400428', 1),
    (@P29, @Branch1Id, 'stanislaw.janicki@email.com', 'Stanisław', 'Janicki', '+48800400429', 1),
    (@P30, @Branch1Id, 'magdalena.kania@gmail.com', 'Magdalena', 'Kania', '+48800400430', 1),
    (@P31, @Branch1Id, 'bartosz.lis@outlook.com', 'Bartosz', 'Lis', '+48800400431', 1),
    (@P32, @Branch1Id, 'sylwia.mazurek@yahoo.com', 'Sylwia', 'Mazurek', '+48800400432', 1),
    (@P33, @Branch1Id, 'andrzej.niedzielski@email.com', 'Andrzej', 'Niedzielski', '+48800400433', 1),
    (@P34, @Branch1Id, 'renata.olejnik@gmail.com', 'Renata', 'Olejnik', '+48800400434', 1),
    (@P35, @Branch1Id, 'kamil.piotrowski@outlook.com', 'Kamil', 'Piotrowski', '+48800400435', 1),
    (@P36, @Branch1Id, 'justyna.rosa@yahoo.com', 'Justyna', 'Rosa', '+48800400436', 1),
    (@P37, @Branch1Id, 'maciej.sikora@email.com', 'Maciej', 'Sikora', '+48800400437', 1),
    (@P38, @Branch1Id, 'patrycja.turek@gmail.com', 'Patrycja', 'Turek', '+48800400438', 1),
    (@P39, @Branch1Id, 'artur.urban@outlook.com', 'Artur', 'Urban', '+48800400439', 1),
    (@P40, @Branch1Id, 'weronika.wilk@yahoo.com', 'Weronika', 'Wilk', '+48800400440', 1),
    (@P41, @Branch1Id, 'damian.zawadzki@email.com', 'Damian', 'Zawadzki', '+48800400441', 1),
    (@P42, @Branch1Id, 'aneta.blaszczyk@gmail.com', 'Aneta', 'Błaszczyk', '+48800400442', 1),
    (@P43, @Branch1Id, 'sebastian.chmiel@outlook.com', 'Sebastian', 'Chmiel', '+48800400443', 1),
    (@P44, @Branch1Id, 'nina.duda@yahoo.com', 'Nina', 'Duda', '+48800400444', 1),
    (@P45, @Branch1Id, 'oskar.flis@email.com', 'Oskar', 'Flis', '+48800400445', 1),
    (@P46, @Branch1Id, 'kinga.gajda@gmail.com', 'Kinga', 'Gajda', '+48800400446', 1),
    (@P47, @Branch1Id, 'norbert.holda@outlook.com', 'Norbert', 'Hołda', '+48800400447', 1),
    (@P48, @Branch1Id, 'oliwia.janik@yahoo.com', 'Oliwia', 'Janik', '+48800400448', 1),
    (@P49, @Branch1Id, 'patryk.kowal@email.com', 'Patryk', 'Kowal', '+48800400449', 1),
    (@P50, @Branch1Id, 'sandra.lis@gmail.com', 'Sandra', 'Lis', '+48800400450', 1);

-- =============================================
-- 5. Specializations - 15 specjalizacji dla SportFit Centrum
-- =============================================

DECLARE @Spec1Id UNIQUEIDENTIFIER = NEWID();
DECLARE @Spec2Id UNIQUEIDENTIFIER = NEWID();
DECLARE @Spec3Id UNIQUEIDENTIFIER = NEWID();
DECLARE @Spec4Id UNIQUEIDENTIFIER = NEWID();
DECLARE @Spec5Id UNIQUEIDENTIFIER = NEWID();
DECLARE @Spec6Id UNIQUEIDENTIFIER = NEWID();
DECLARE @Spec7Id UNIQUEIDENTIFIER = NEWID();
DECLARE @Spec8Id UNIQUEIDENTIFIER = NEWID();
DECLARE @Spec9Id UNIQUEIDENTIFIER = NEWID();
DECLARE @Spec10Id UNIQUEIDENTIFIER = NEWID();
DECLARE @Spec11Id UNIQUEIDENTIFIER = NEWID();
DECLARE @Spec12Id UNIQUEIDENTIFIER = NEWID();
DECLARE @Spec13Id UNIQUEIDENTIFIER = NEWID();
DECLARE @Spec14Id UNIQUEIDENTIFIER = NEWID();
DECLARE @Spec15Id UNIQUEIDENTIFIER = NEWID();

INSERT INTO Specializations (Id, CompanyId, Name, Description)
VALUES
    (@Spec1Id, @Branch1Id, 'Joga Hatha', 'Klasyczna joga z naciskiem na pozycje i oddech'),
    (@Spec2Id, @Branch1Id, 'Joga Vinyasa', 'Dynamiczna joga łącząca ruch z oddechem'),
    (@Spec3Id, @Branch1Id, 'Pilates Mat', 'Ćwiczenia pilates na macie'),
    (@Spec4Id, @Branch1Id, 'Pilates Reformer', 'Pilates z wykorzystaniem specjalistycznego sprzętu'),
    (@Spec5Id, @Branch1Id, 'Trening Siłowy', 'Trening z ciężarami wolnymi'),
    (@Spec6Id, @Branch1Id, 'Trening Funkcjonalny', 'Ćwiczenia poprawiające codzienną sprawność'),
    (@Spec7Id, @Branch1Id, 'Crossfit', 'Intensywny trening crossfit'),
    (@Spec8Id, @Branch1Id, 'HIIT', 'Trening interwałowy o wysokiej intensywności'),
    (@Spec9Id, @Branch1Id, 'Spinning', 'Trening na rowerach stacjonarnych'),
    (@Spec10Id, @Branch1Id, 'Zumba', 'Taniec fitness z elementami latin'),
    (@Spec11Id, @Branch1Id, 'Stretching', 'Rozciąganie i mobilność'),
    (@Spec12Id, @Branch1Id, 'Rehabilitacja Ruchowa', 'Ćwiczenia rehabilitacyjne'),
    (@Spec13Id, @Branch1Id, 'Trening Personalny', 'Indywidualne sesje treningowe'),
    (@Spec14Id, @Branch1Id, 'Kickboxing Fitness', 'Trening cardio z elementami sztuk walki'),
    (@Spec15Id, @Branch1Id, 'TRX Suspension', 'Trening z wykorzystaniem taśm TRX');

-- =============================================
-- 6. StaffMemberSpecializations - przypisanie specjalizacji do trenerów
-- =============================================

INSERT INTO StaffMemberSpecializations (Id, CompanyId, StaffMemberId, SpecializationId)
VALUES
    -- Trainer 1 - Joga
    (NEWID(), @Branch1Id, @Trainer1Id, @Spec1Id),
    (NEWID(), @Branch1Id, @Trainer1Id, @Spec2Id),
    (NEWID(), @Branch1Id, @Trainer1Id, @Spec11Id),

    -- Trainer 2
    (NEWID(), @Branch1Id, @Trainer2Id, @Spec3Id),
    (NEWID(), @Branch1Id, @Trainer2Id, @Spec4Id),
    (NEWID(), @Branch1Id, @Trainer2Id, @Spec5Id),
    (NEWID(), @Branch1Id, @Trainer2Id, @Spec1Id),
    (NEWID(), @Branch1Id, @Trainer2Id, @Spec2Id),
    (NEWID(), @Branch1Id, @Trainer2Id, @Spec11Id),
    (NEWID(), @Branch1Id, @Trainer2Id, @Spec6Id),
    (NEWID(), @Branch1Id, @Trainer2Id, @Spec13Id),
    (NEWID(), @Branch1Id, @Trainer2Id, @Spec14Id),
    (NEWID(), @Branch1Id, @Trainer2Id, @Spec15Id),
    (NEWID(), @Branch1Id, @Trainer2Id, @Spec10Id),

    -- Trainer 3 - Siłowy
    (NEWID(), @Branch1Id, @Trainer3Id, @Spec5Id),
    (NEWID(), @Branch1Id, @Trainer3Id, @Spec6Id),
    (NEWID(), @Branch1Id, @Trainer3Id, @Spec13Id),

    -- Trainer 4 - Crossfit/HIIT
    (NEWID(), @Branch1Id, @Trainer4Id, @Spec7Id),
    (NEWID(), @Branch1Id, @Trainer4Id, @Spec8Id),
    (NEWID(), @Branch1Id, @Trainer4Id, @Spec15Id),

    -- Trainer 5 - Cardio
    (NEWID(), @Branch1Id, @Trainer5Id, @Spec9Id),
    (NEWID(), @Branch1Id, @Trainer5Id, @Spec10Id),

    -- Trainer 6 - Rehabilitacja
    (NEWID(), @Branch1Id, @Trainer6Id, @Spec11Id),
    (NEWID(), @Branch1Id, @Trainer6Id, @Spec12Id),
    (NEWID(), @Branch1Id, @Trainer6Id, @Spec3Id),

    -- Trainer 7 - Siłowy/TRX
    (NEWID(), @Branch1Id, @Trainer7Id, @Spec5Id),
    (NEWID(), @Branch1Id, @Trainer7Id, @Spec15Id),
    (NEWID(), @Branch1Id, @Trainer7Id, @Spec6Id),

    -- Trainer 8 - Joga/Pilates
    (NEWID(), @Branch1Id, @Trainer8Id, @Spec1Id),
    (NEWID(), @Branch1Id, @Trainer8Id, @Spec3Id),
    (NEWID(), @Branch1Id, @Trainer8Id, @Spec11Id),

    -- Trainer 9 - Cardio/Kickboxing
    (NEWID(), @Branch1Id, @Trainer9Id, @Spec9Id),
    (NEWID(), @Branch1Id, @Trainer9Id, @Spec14Id),
    (NEWID(), @Branch1Id, @Trainer9Id, @Spec8Id),

    -- Trainer 10 - Zumba/Taniec
    (NEWID(), @Branch1Id, @Trainer10Id, @Spec10Id),
    (NEWID(), @Branch1Id, @Trainer10Id, @Spec14Id),
    (NEWID(), @Branch1Id, @Trainer10Id, @Spec11Id);

-- =============================================
-- 7. StaffMemberAvailabilities - dostępność trenerów (styczeń-marzec 2026)
-- =============================================

-- Generowanie dostępności dla wszystkich trenerów na 3 miesiące
DECLARE @CurrentDate DATE = '2026-01-01';
DECLARE @EndDate DATE = '2026-03-31';

WHILE @CurrentDate <= @EndDate
BEGIN
    -- Pomijamy niedziele (DATEPART(dw, @CurrentDate) = 1 w systemie z niedzielą jako 1)
    IF DATEPART(dw, @CurrentDate) != 1
    BEGIN
        -- Trainer 1 - poniedziałek-piątek 8:00-16:00
        IF DATEPART(dw, @CurrentDate) BETWEEN 2 AND 6
            INSERT INTO StaffMemberAvailabilities (Id, CompanyId, StaffMemberId, Date, StartTime, EndTime, IsAvailable)
            VALUES (NEWID(), @Branch1Id, @Trainer1Id, @CurrentDate,
                    CAST(@CurrentDate AS DATETIME) + CAST('08:00:00' AS DATETIME),
                    CAST(@CurrentDate AS DATETIME) + CAST('16:00:00' AS DATETIME), 1);

        -- Trainer 2 - poniedziałek-piątek 10:00-18:00
        IF DATEPART(dw, @CurrentDate) BETWEEN 2 AND 6
            INSERT INTO StaffMemberAvailabilities (Id, CompanyId, StaffMemberId, Date, StartTime, EndTime, IsAvailable)
            VALUES (NEWID(), @Branch1Id, @Trainer2Id, @CurrentDate,
                    CAST(@CurrentDate AS DATETIME) + CAST('10:00:00' AS DATETIME),
                    CAST(@CurrentDate AS DATETIME) + CAST('18:00:00' AS DATETIME), 1);

        -- Trainer 3 - poniedziałek-sobota 12:00-20:00
        IF DATEPART(dw, @CurrentDate) BETWEEN 2 AND 7
            INSERT INTO StaffMemberAvailabilities (Id, CompanyId, StaffMemberId, Date, StartTime, EndTime, IsAvailable)
            VALUES (NEWID(), @Branch1Id, @Trainer3Id, @CurrentDate,
                    CAST(@CurrentDate AS DATETIME) + CAST('12:00:00' AS DATETIME),
                    CAST(@CurrentDate AS DATETIME) + CAST('20:00:00' AS DATETIME), 1);

        -- Trainer 4 - wtorek-sobota 7:00-15:00
        IF DATEPART(dw, @CurrentDate) BETWEEN 3 AND 7
            INSERT INTO StaffMemberAvailabilities (Id, CompanyId, StaffMemberId, Date, StartTime, EndTime, IsAvailable)
            VALUES (NEWID(), @Branch1Id, @Trainer4Id, @CurrentDate,
                    CAST(@CurrentDate AS DATETIME) + CAST('07:00:00' AS DATETIME),
                    CAST(@CurrentDate AS DATETIME) + CAST('15:00:00' AS DATETIME), 1);

        -- Trainer 5 - poniedziałek-piątek 6:00-14:00
        IF DATEPART(dw, @CurrentDate) BETWEEN 2 AND 6
            INSERT INTO StaffMemberAvailabilities (Id, CompanyId, StaffMemberId, Date, StartTime, EndTime, IsAvailable)
            VALUES (NEWID(), @Branch1Id, @Trainer5Id, @CurrentDate,
                    CAST(@CurrentDate AS DATETIME) + CAST('06:00:00' AS DATETIME),
                    CAST(@CurrentDate AS DATETIME) + CAST('14:00:00' AS DATETIME), 1);

        -- Trainer 6 - poniedziałek-piątek 9:00-17:00
        IF DATEPART(dw, @CurrentDate) BETWEEN 2 AND 6
            INSERT INTO StaffMemberAvailabilities (Id, CompanyId, StaffMemberId, Date, StartTime, EndTime, IsAvailable)
            VALUES (NEWID(), @Branch1Id, @Trainer6Id, @CurrentDate,
                    CAST(@CurrentDate AS DATETIME) + CAST('09:00:00' AS DATETIME),
                    CAST(@CurrentDate AS DATETIME) + CAST('17:00:00' AS DATETIME), 1);

        -- Trainer 7 - poniedziałek-sobota 14:00-22:00
        IF DATEPART(dw, @CurrentDate) BETWEEN 2 AND 7
            INSERT INTO StaffMemberAvailabilities (Id, CompanyId, StaffMemberId, Date, StartTime, EndTime, IsAvailable)
            VALUES (NEWID(), @Branch1Id, @Trainer7Id, @CurrentDate,
                    CAST(@CurrentDate AS DATETIME) + CAST('14:00:00' AS DATETIME),
                    CAST(@CurrentDate AS DATETIME) + CAST('22:00:00' AS DATETIME), 1);

        -- Trainer 8 - środa-sobota 8:00-16:00
        IF DATEPART(dw, @CurrentDate) BETWEEN 4 AND 7
            INSERT INTO StaffMemberAvailabilities (Id, CompanyId, StaffMemberId, Date, StartTime, EndTime, IsAvailable)
            VALUES (NEWID(), @Branch1Id, @Trainer8Id, @CurrentDate,
                    CAST(@CurrentDate AS DATETIME) + CAST('08:00:00' AS DATETIME),
                    CAST(@CurrentDate AS DATETIME) + CAST('16:00:00' AS DATETIME), 1);

        -- Trainer 9 - poniedziałek-piątek 16:00-22:00
        IF DATEPART(dw, @CurrentDate) BETWEEN 2 AND 6
            INSERT INTO StaffMemberAvailabilities (Id, CompanyId, StaffMemberId, Date, StartTime, EndTime, IsAvailable)
            VALUES (NEWID(), @Branch1Id, @Trainer9Id, @CurrentDate,
                    CAST(@CurrentDate AS DATETIME) + CAST('16:00:00' AS DATETIME),
                    CAST(@CurrentDate AS DATETIME) + CAST('22:00:00' AS DATETIME), 1);

        -- Trainer 10 - wtorek-sobota 10:00-18:00
        IF DATEPART(dw, @CurrentDate) BETWEEN 3 AND 7
            INSERT INTO StaffMemberAvailabilities (Id, CompanyId, StaffMemberId, Date, StartTime, EndTime, IsAvailable)
            VALUES (NEWID(), @Branch1Id, @Trainer10Id, @CurrentDate,
                    CAST(@CurrentDate AS DATETIME) + CAST('10:00:00' AS DATETIME),
                    CAST(@CurrentDate AS DATETIME) + CAST('18:00:00' AS DATETIME), 1);
    END

    SET @CurrentDate = DATEADD(day, 1, @CurrentDate);
END;

-- =============================================
-- 8. EventTypes - 15 typów wydarzeń dla SportFit Centrum
-- =============================================

DECLARE @ET1 UNIQUEIDENTIFIER = NEWID();
DECLARE @ET2 UNIQUEIDENTIFIER = NEWID();
DECLARE @ET3 UNIQUEIDENTIFIER = NEWID();
DECLARE @ET4 UNIQUEIDENTIFIER = NEWID();
DECLARE @ET5 UNIQUEIDENTIFIER = NEWID();
DECLARE @ET6 UNIQUEIDENTIFIER = NEWID();
DECLARE @ET7 UNIQUEIDENTIFIER = NEWID();
DECLARE @ET8 UNIQUEIDENTIFIER = NEWID();
DECLARE @ET9 UNIQUEIDENTIFIER = NEWID();
DECLARE @ET10 UNIQUEIDENTIFIER = NEWID();
DECLARE @ET11 UNIQUEIDENTIFIER = NEWID();
DECLARE @ET12 UNIQUEIDENTIFIER = NEWID();
DECLARE @ET13 UNIQUEIDENTIFIER = NEWID();
DECLARE @ET14 UNIQUEIDENTIFIER = NEWID();
DECLARE @ET15 UNIQUEIDENTIFIER = NEWID();

INSERT INTO EventTypes (Id, CompanyId, Name, Description, Duration, Price, MaxParticipants, MinStaff)
VALUES
    (@ET1, @Branch1Id, 'Joga Poranna', 'Poranne zajęcia jogi na dobry początek dnia', 60, 45.00, 20, 1),
    (@ET2, @Branch1Id, 'Joga Wieczorna', 'Relaksacyjna joga na zakończenie dnia', 75, 50.00, 18, 1),
    (@ET3, @Branch1Id, 'Pilates Podstawy', 'Pilates dla początkujących', 50, 55.00, 15, 1),
    (@ET4, @Branch1Id, 'Pilates Zaawansowany', 'Pilates dla osób zaawansowanych', 60, 65.00, 12, 1),
    (@ET5, @Branch1Id, 'Trening Siłowy Grupowy', 'Trening siłowy w grupie', 60, 40.00, 16, 1),
    (@ET6, @Branch1Id, 'Trening Personalny', 'Indywidualna sesja z trenerem', 45, 150.00, 1, 1),
    (@ET7, @Branch1Id, 'Crossfit WOD', 'Workout of the Day - crossfit', 60, 55.00, 14, 1),
    (@ET8, @Branch1Id, 'HIIT Express', 'Krótki intensywny trening interwałowy', 30, 35.00, 20, 1),
    (@ET9, @Branch1Id, 'Spinning Classic', 'Klasyczne zajęcia spinning', 45, 40.00, 25, 1),
    (@ET10, @Branch1Id, 'Zumba Party', 'Energetyczna zumba', 55, 35.00, 30, 1),
    (@ET11, @Branch1Id, 'Stretching & Relax', 'Rozciąganie i relaksacja', 45, 30.00, 20, 1),
    (@ET12, @Branch1Id, 'Rehabilitacja Grupowa', 'Ćwiczenia rehabilitacyjne w grupie', 60, 70.00, 10, 1),
    (@ET13, @Branch1Id, 'Kickboxing Fitness', 'Cardio kickboxing', 55, 45.00, 18, 1),
    (@ET14, @Branch1Id, 'TRX Training', 'Trening na taśmach TRX', 45, 50.00, 12, 1),
    (@ET15, @Branch1Id, 'Functional Training', 'Trening funkcjonalny', 50, 45.00, 14, 1);

-- =============================================
-- 9. EventSchedules - 150 wydarzeń (styczeń-marzec 2026)
-- =============================================

-- Tabela tymczasowa do przechowywania ID wydarzeń
CREATE TABLE #EventIds (
    EventId UNIQUEIDENTIFIER,
    EventNumber INT
);

-- Generowanie 150 wydarzeń
DECLARE @EventCounter INT = 1;
DECLARE @EventDate DATE;
DECLARE @EventTypeId UNIQUEIDENTIFIER;
DECLARE @TrainerId UNIQUEIDENTIFIER;
DECLARE @StartHour INT;
DECLARE @PlaceName NVARCHAR(100);
DECLARE @NewEventId UNIQUEIDENTIFIER;

WHILE @EventCounter <= 150
BEGIN
    -- Losowa data w styczniu-marcu 2026
    SET @EventDate = DATEADD(day, ABS(CHECKSUM(NEWID())) % 90, '2026-01-01');

    -- Pomijamy niedziele
    WHILE DATEPART(dw, @EventDate) = 1
        SET @EventDate = DATEADD(day, 1, @EventDate);

    -- Losowy typ wydarzenia
    SET @EventTypeId = CASE (ABS(CHECKSUM(NEWID())) % 15) + 1
        WHEN 1 THEN @ET1 WHEN 2 THEN @ET2 WHEN 3 THEN @ET3 WHEN 4 THEN @ET4 WHEN 5 THEN @ET5
        WHEN 6 THEN @ET6 WHEN 7 THEN @ET7 WHEN 8 THEN @ET8 WHEN 9 THEN @ET9 WHEN 10 THEN @ET10
        WHEN 11 THEN @ET11 WHEN 12 THEN @ET12 WHEN 13 THEN @ET13 WHEN 14 THEN @ET14 ELSE @ET15
    END;

    -- Losowy trener (odpowiedni do typu)
    SET @TrainerId = CASE (ABS(CHECKSUM(NEWID())) % 10) + 1
        WHEN 1 THEN @Trainer1Id WHEN 2 THEN @Trainer2Id WHEN 3 THEN @Trainer3Id WHEN 4 THEN @Trainer4Id
        WHEN 5 THEN @Trainer5Id WHEN 6 THEN @Trainer6Id WHEN 7 THEN @Trainer7Id WHEN 8 THEN @Trainer8Id
        WHEN 9 THEN @Trainer9Id ELSE @Trainer10Id
    END;

    -- Losowa godzina (7-20)
    SET @StartHour = 7 + (ABS(CHECKSUM(NEWID())) % 14);

    -- Losowa sala
    SET @PlaceName = CASE (ABS(CHECKSUM(NEWID())) % 8) + 1
        WHEN 1 THEN 'Sala Fitness A'
        WHEN 2 THEN 'Sala Fitness B'
        WHEN 3 THEN 'Sala Jogi'
        WHEN 4 THEN 'Sala Pilates'
        WHEN 5 THEN 'Sala Spinning'
        WHEN 6 THEN 'Sala Crossfit'
        WHEN 7 THEN 'Sala Taneczna'
        ELSE 'Sala Treningowa'
    END;

    SET @NewEventId = NEWID();

    INSERT INTO EventSchedules (Id, CompanyId, EventTypeId, PlaceName, StartTime, Status)
    VALUES (@NewEventId, @Branch1Id, @EventTypeId, @PlaceName,
            DATEADD(hour, @StartHour, CAST(@EventDate AS DATETIME)), 'Active');

    -- Zapisz ID wydarzenia
    INSERT INTO #EventIds (EventId, EventNumber) VALUES (@NewEventId, @EventCounter);

    -- Przypisz trenera do wydarzenia
    INSERT INTO EventScheduleStaff (Id, CompanyId, EventScheduleId, StaffMemberId)
    VALUES (NEWID(), @Branch1Id, @NewEventId, @TrainerId);

    SET @EventCounter = @EventCounter + 1;
END;

-- =============================================
-- 10. Reservations - 15 rezerwacji z wieloma uczestnikami
-- =============================================

DECLARE @R1 UNIQUEIDENTIFIER = NEWID();
DECLARE @R2 UNIQUEIDENTIFIER = NEWID();
DECLARE @R3 UNIQUEIDENTIFIER = NEWID();
DECLARE @R4 UNIQUEIDENTIFIER = NEWID();
DECLARE @R5 UNIQUEIDENTIFIER = NEWID();
DECLARE @R6 UNIQUEIDENTIFIER = NEWID();
DECLARE @R7 UNIQUEIDENTIFIER = NEWID();
DECLARE @R8 UNIQUEIDENTIFIER = NEWID();
DECLARE @R9 UNIQUEIDENTIFIER = NEWID();
DECLARE @R10 UNIQUEIDENTIFIER = NEWID();
DECLARE @R11 UNIQUEIDENTIFIER = NEWID();
DECLARE @R12 UNIQUEIDENTIFIER = NEWID();
DECLARE @R13 UNIQUEIDENTIFIER = NEWID();
DECLARE @R14 UNIQUEIDENTIFIER = NEWID();
DECLARE @R15 UNIQUEIDENTIFIER = NEWID();

-- Pobierz ID wydarzeń dla rezerwacji
DECLARE @E1 UNIQUEIDENTIFIER, @E2 UNIQUEIDENTIFIER, @E3 UNIQUEIDENTIFIER, @E4 UNIQUEIDENTIFIER, @E5 UNIQUEIDENTIFIER;
DECLARE @E6 UNIQUEIDENTIFIER, @E7 UNIQUEIDENTIFIER, @E8 UNIQUEIDENTIFIER, @E9 UNIQUEIDENTIFIER, @E10 UNIQUEIDENTIFIER;
DECLARE @E11 UNIQUEIDENTIFIER, @E12 UNIQUEIDENTIFIER, @E13 UNIQUEIDENTIFIER, @E14 UNIQUEIDENTIFIER, @E15 UNIQUEIDENTIFIER;

SELECT @E1 = EventId FROM #EventIds WHERE EventNumber = 1;
SELECT @E2 = EventId FROM #EventIds WHERE EventNumber = 5;
SELECT @E3 = EventId FROM #EventIds WHERE EventNumber = 10;
SELECT @E4 = EventId FROM #EventIds WHERE EventNumber = 15;
SELECT @E5 = EventId FROM #EventIds WHERE EventNumber = 20;
SELECT @E6 = EventId FROM #EventIds WHERE EventNumber = 30;
SELECT @E7 = EventId FROM #EventIds WHERE EventNumber = 40;
SELECT @E8 = EventId FROM #EventIds WHERE EventNumber = 50;
SELECT @E9 = EventId FROM #EventIds WHERE EventNumber = 60;
SELECT @E10 = EventId FROM #EventIds WHERE EventNumber = 70;
SELECT @E11 = EventId FROM #EventIds WHERE EventNumber = 80;
SELECT @E12 = EventId FROM #EventIds WHERE EventNumber = 90;
SELECT @E13 = EventId FROM #EventIds WHERE EventNumber = 100;
SELECT @E14 = EventId FROM #EventIds WHERE EventNumber = 120;
SELECT @E15 = EventId FROM #EventIds WHERE EventNumber = 140;

INSERT INTO Reservations (Id, CompanyId, EventScheduleId, Status, Notes, CreatedAt, CancelledAt, IsPaid, PaidAt)
VALUES
    -- Rezerwacja 1 - 12 uczestników (Joga grupowa)
    (@R1, @Branch1Id, @E1, 'Confirmed', 'Duża grupa przyjaciół na jogę poranną',
     '2026-01-02 09:00:00', NULL, 1, '2026-01-02 09:30:00'),

    -- Rezerwacja 2 - 8 uczestników (Pilates)
    (@R2, @Branch1Id, @E2, 'Confirmed', 'Grupa koleżanek z pracy',
     '2026-01-03 14:00:00', NULL, 1, '2026-01-03 14:15:00'),

    -- Rezerwacja 3 - 15 uczestników (Zumba)
    (@R3, @Branch1Id, @E3, 'Confirmed', 'Wieczór panieński - zumba party',
     '2026-01-05 11:00:00', NULL, 1, '2026-01-05 11:30:00'),

    -- Rezerwacja 4 - 6 uczestników (Crossfit)
    (@R4, @Branch1Id, @E4, 'Confirmed', 'Drużyna crossfit amatorów',
     '2026-01-07 08:30:00', NULL, 1, '2026-01-07 09:00:00'),

    -- Rezerwacja 5 - 10 uczestników (HIIT)
    (@R5, @Branch1Id, @E5, 'Confirmed', 'Poranna ekipa HIIT',
     '2026-01-10 06:00:00', NULL, 1, '2026-01-10 06:20:00'),

    -- Rezerwacja 6 - 14 uczestników (Spinning)
    (@R6, @Branch1Id, @E6, 'Confirmed', 'Klub rowerowy - trening zimowy',
     '2026-01-15 16:00:00', NULL, 1, '2026-01-15 16:30:00'),

    -- Rezerwacja 7 - 5 uczestników (TRX)
    (@R7, @Branch1Id, @E7, 'Confirmed', 'Mała grupa TRX dla początkujących',
     '2026-01-20 10:00:00', NULL, 1, '2026-01-20 10:15:00'),

    -- Rezerwacja 8 - 9 uczestników (Stretching)
    (@R8, @Branch1Id, @E8, 'Confirmed', 'Seniorzy - stretching poranny',
     '2026-01-25 09:00:00', NULL, 1, '2026-01-25 09:30:00'),

    -- Rezerwacja 9 - 11 uczestników (Functional Training)
    (@R9, @Branch1Id, @E9, 'Confirmed', 'Drużyna piłkarska - trening uzupełniający',
     '2026-02-01 15:00:00', NULL, 1, '2026-02-01 15:20:00'),

    -- Rezerwacja 10 - 7 uczestników (Kickboxing)
    (@R10, @Branch1Id, @E10, 'Confirmed', 'Kickboxing dla kobiet',
     '2026-02-05 18:00:00', NULL, 1, '2026-02-05 18:15:00'),

    -- Rezerwacja 11 - 13 uczestników (Joga wieczorna)
    (@R11, @Branch1Id, @E11, 'Confirmed', 'Joga antystresowa po pracy',
     '2026-02-10 17:00:00', NULL, 1, '2026-02-10 17:30:00'),

    -- Rezerwacja 12 - 4 uczestników (Rehabilitacja)
    (@R12, @Branch1Id, @E12, 'Confirmed', 'Rehabilitacja kręgosłupa',
     '2026-02-15 11:00:00', NULL, 1, '2026-02-15 11:20:00'),

    -- Rezerwacja 13 - 16 uczestników (Siłowy grupowy)
    (@R13, @Branch1Id, @E13, 'Confirmed', 'Trening siłowy - ekipa firmowa',
     '2026-02-20 12:00:00', NULL, 1, '2026-02-20 12:30:00'),

    -- Rezerwacja 14 - 8 uczestników (Pilates zaawansowany)
    (@R14, @Branch1Id, @E14, 'Pending', 'Pilates - zaawansowana technika',
     '2026-03-01 14:00:00', NULL, 0, NULL),

    -- Rezerwacja 15 - 10 uczestników (HIIT)
    (@R15, @Branch1Id, @E15, 'Pending', 'HIIT weekendowy',
     '2026-03-10 10:00:00', NULL, 0, NULL);

-- =============================================
-- 11. ReservationParticipants - przypisanie uczestników
-- =============================================

-- Rezerwacja 1 - 12 uczestników
INSERT INTO ReservationParticipants (CompanyId, ReservationId, ParticipantId) VALUES
    (@Branch1Id, @R1, @P1), (@Branch1Id, @R1, @P2), (@Branch1Id, @R1, @P3), (@Branch1Id, @R1, @P4),
    (@Branch1Id, @R1, @P5), (@Branch1Id, @R1, @P6), (@Branch1Id, @R1, @P7), (@Branch1Id, @R1, @P8),
    (@Branch1Id, @R1, @P9), (@Branch1Id, @R1, @P10), (@Branch1Id, @R1, @P11), (@Branch1Id, @R1, @P12);

-- Rezerwacja 2 - 8 uczestników
INSERT INTO ReservationParticipants (CompanyId, ReservationId, ParticipantId) VALUES
    (@Branch1Id, @R2, @P13), (@Branch1Id, @R2, @P14), (@Branch1Id, @R2, @P15), (@Branch1Id, @R2, @P16),
    (@Branch1Id, @R2, @P17), (@Branch1Id, @R2, @P18), (@Branch1Id, @R2, @P19), (@Branch1Id, @R2, @P20);

-- Rezerwacja 3 - 15 uczestników
INSERT INTO ReservationParticipants (CompanyId, ReservationId, ParticipantId) VALUES
    (@Branch1Id, @R3, @P21), (@Branch1Id, @R3, @P22), (@Branch1Id, @R3, @P23), (@Branch1Id, @R3, @P24),
    (@Branch1Id, @R3, @P25), (@Branch1Id, @R3, @P26), (@Branch1Id, @R3, @P27), (@Branch1Id, @R3, @P28),
    (@Branch1Id, @R3, @P29), (@Branch1Id, @R3, @P30), (@Branch1Id, @R3, @P31), (@Branch1Id, @R3, @P32),
    (@Branch1Id, @R3, @P33), (@Branch1Id, @R3, @P34), (@Branch1Id, @R3, @P35);

-- Rezerwacja 4 - 6 uczestników
INSERT INTO ReservationParticipants (CompanyId, ReservationId, ParticipantId) VALUES
    (@Branch1Id, @R4, @P36), (@Branch1Id, @R4, @P37), (@Branch1Id, @R4, @P38),
    (@Branch1Id, @R4, @P39), (@Branch1Id, @R4, @P40), (@Branch1Id, @R4, @P41);

-- Rezerwacja 5 - 10 uczestników
INSERT INTO ReservationParticipants (CompanyId, ReservationId, ParticipantId) VALUES
    (@Branch1Id, @R5, @P42), (@Branch1Id, @R5, @P43), (@Branch1Id, @R5, @P44), (@Branch1Id, @R5, @P45),
    (@Branch1Id, @R5, @P46), (@Branch1Id, @R5, @P47), (@Branch1Id, @R5, @P48), (@Branch1Id, @R5, @P49),
    (@Branch1Id, @R5, @P50), (@Branch1Id, @R5, @P1);

-- Rezerwacja 6 - 14 uczestników
INSERT INTO ReservationParticipants (CompanyId, ReservationId, ParticipantId) VALUES
    (@Branch1Id, @R6, @P2), (@Branch1Id, @R6, @P3), (@Branch1Id, @R6, @P4), (@Branch1Id, @R6, @P5),
    (@Branch1Id, @R6, @P6), (@Branch1Id, @R6, @P7), (@Branch1Id, @R6, @P8), (@Branch1Id, @R6, @P9),
    (@Branch1Id, @R6, @P10), (@Branch1Id, @R6, @P11), (@Branch1Id, @R6, @P12), (@Branch1Id, @R6, @P13),
    (@Branch1Id, @R6, @P14), (@Branch1Id, @R6, @P15);

-- Rezerwacja 7 - 5 uczestników
INSERT INTO ReservationParticipants (CompanyId, ReservationId, ParticipantId) VALUES
    (@Branch1Id, @R7, @P16), (@Branch1Id, @R7, @P17), (@Branch1Id, @R7, @P18),
    (@Branch1Id, @R7, @P19), (@Branch1Id, @R7, @P20);

-- Rezerwacja 8 - 9 uczestników
INSERT INTO ReservationParticipants (CompanyId, ReservationId, ParticipantId) VALUES
    (@Branch1Id, @R8, @P21), (@Branch1Id, @R8, @P22), (@Branch1Id, @R8, @P23), (@Branch1Id, @R8, @P24),
    (@Branch1Id, @R8, @P25), (@Branch1Id, @R8, @P26), (@Branch1Id, @R8, @P27), (@Branch1Id, @R8, @P28),
    (@Branch1Id, @R8, @P29);

-- Rezerwacja 9 - 11 uczestników
INSERT INTO ReservationParticipants (CompanyId, ReservationId, ParticipantId) VALUES
    (@Branch1Id, @R9, @P30), (@Branch1Id, @R9, @P31), (@Branch1Id, @R9, @P32), (@Branch1Id, @R9, @P33),
    (@Branch1Id, @R9, @P34), (@Branch1Id, @R9, @P35), (@Branch1Id, @R9, @P36), (@Branch1Id, @R9, @P37),
    (@Branch1Id, @R9, @P38), (@Branch1Id, @R9, @P39), (@Branch1Id, @R9, @P40);

-- Rezerwacja 10 - 7 uczestników
INSERT INTO ReservationParticipants (CompanyId, ReservationId, ParticipantId) VALUES
    (@Branch1Id, @R10, @P41), (@Branch1Id, @R10, @P42), (@Branch1Id, @R10, @P43), (@Branch1Id, @R10, @P44),
    (@Branch1Id, @R10, @P45), (@Branch1Id, @R10, @P46), (@Branch1Id, @R10, @P47);

-- Rezerwacja 11 - 13 uczestników
INSERT INTO ReservationParticipants (CompanyId, ReservationId, ParticipantId) VALUES
    (@Branch1Id, @R11, @P48), (@Branch1Id, @R11, @P49), (@Branch1Id, @R11, @P50), (@Branch1Id, @R11, @P1),
    (@Branch1Id, @R11, @P2), (@Branch1Id, @R11, @P3), (@Branch1Id, @R11, @P4), (@Branch1Id, @R11, @P5),
    (@Branch1Id, @R11, @P6), (@Branch1Id, @R11, @P7), (@Branch1Id, @R11, @P8), (@Branch1Id, @R11, @P9),
    (@Branch1Id, @R11, @P10);

-- Rezerwacja 12 - 4 uczestników
INSERT INTO ReservationParticipants (CompanyId, ReservationId, ParticipantId) VALUES
    (@Branch1Id, @R12, @P11), (@Branch1Id, @R12, @P12), (@Branch1Id, @R12, @P13), (@Branch1Id, @R12, @P14);

-- Rezerwacja 13 - 16 uczestników
INSERT INTO ReservationParticipants (CompanyId, ReservationId, ParticipantId) VALUES
    (@Branch1Id, @R13, @P15), (@Branch1Id, @R13, @P16), (@Branch1Id, @R13, @P17), (@Branch1Id, @R13, @P18),
    (@Branch1Id, @R13, @P19), (@Branch1Id, @R13, @P20), (@Branch1Id, @R13, @P21), (@Branch1Id, @R13, @P22),
    (@Branch1Id, @R13, @P23), (@Branch1Id, @R13, @P24), (@Branch1Id, @R13, @P25), (@Branch1Id, @R13, @P26),
    (@Branch1Id, @R13, @P27), (@Branch1Id, @R13, @P28), (@Branch1Id, @R13, @P29), (@Branch1Id, @R13, @P30);

-- Rezerwacja 14 - 8 uczestników
INSERT INTO ReservationParticipants (CompanyId, ReservationId, ParticipantId) VALUES
    (@Branch1Id, @R14, @P31), (@Branch1Id, @R14, @P32), (@Branch1Id, @R14, @P33), (@Branch1Id, @R14, @P34),
    (@Branch1Id, @R14, @P35), (@Branch1Id, @R14, @P36), (@Branch1Id, @R14, @P37), (@Branch1Id, @R14, @P38);

-- Rezerwacja 15 - 10 uczestników
INSERT INTO ReservationParticipants (CompanyId, ReservationId, ParticipantId) VALUES
    (@Branch1Id, @R15, @P39), (@Branch1Id, @R15, @P40), (@Branch1Id, @R15, @P41), (@Branch1Id, @R15, @P42),
    (@Branch1Id, @R15, @P43), (@Branch1Id, @R15, @P44), (@Branch1Id, @R15, @P45), (@Branch1Id, @R15, @P46),
    (@Branch1Id, @R15, @P47), (@Branch1Id, @R15, @P48);

-- Czyszczenie tabeli tymczasowej
DROP TABLE #EventIds;

PRINT ''
PRINT 'Skrypt zakończony pomyślnie!'
GO