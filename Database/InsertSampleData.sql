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

-- SportFit Group
DECLARE
	@MainCompanyId UNIQUEIDENTIFIER = NEWID();
DECLARE
	@Branch1Id UNIQUEIDENTIFIER = NEWID();
DECLARE
	@Branch2Id UNIQUEIDENTIFIER = NEWID();
DECLARE
	@Branch3Id UNIQUEIDENTIFIER = NEWID();
DECLARE
	@Branch4Id UNIQUEIDENTIFIER = NEWID();
DECLARE
	@Branch5Id UNIQUEIDENTIFIER = NEWID();

-- FitZone Network
DECLARE
	@FitZoneMainId UNIQUEIDENTIFIER = NEWID();
DECLARE
	@FitZone1Id UNIQUEIDENTIFIER = NEWID();
DECLARE
	@FitZone2Id UNIQUEIDENTIFIER = NEWID();
DECLARE
	@FitZone3Id UNIQUEIDENTIFIER = NEWID();

-- AquaFit Centers
DECLARE
	@AquaFitMainId UNIQUEIDENTIFIER = NEWID();
DECLARE
	@AquaFit1Id UNIQUEIDENTIFIER = NEWID();
DECLARE
	@AquaFit2Id UNIQUEIDENTIFIER = NEWID();

-- PowerGym Chain
DECLARE
	@PowerGymMainId UNIQUEIDENTIFIER = NEWID();
DECLARE
	@PowerGym1Id UNIQUEIDENTIFIER = NEWID();
DECLARE
	@PowerGym2Id UNIQUEIDENTIFIER = NEWID();

-- FlexYoga Studios
DECLARE
	@FlexYogaMainId UNIQUEIDENTIFIER = NEWID();
DECLARE
	@FlexYoga1Id UNIQUEIDENTIFIER = NEWID();

INSERT INTO Companies (Id, Name, TaxCode, Street, City, PostalCode, Phone, Email, IsParentNode, IsReception)
VALUES
	-- SportFit Group - firma główna (nie jest recepcją)
	(@MainCompanyId, 'SportFit Group', 'PL1234567890', 'Sportowa 1', 'Warszawa', '00-001', '+48123456789',
	 'contact@sportfitgroup.pl', 1, 0),
	-- SportFit - recepcje (są recepcjami)
	(@Branch1Id, 'SportFit Centrum', 'PL2345678901', 'Centralna 10', 'Warszawa', '00-002', '+48234567890',
	 'centrum@sportfit.pl', 0, 1),
	(@Branch2Id, 'SportFit Południe', 'PL3456789012', 'Południowa 20', 'Kraków', '30-001', '+48345678901',
	 'poludnie@sportfit.pl', 0, 1),
	(@Branch3Id, 'SportFit Północ', 'PL4567890123', 'Północna 30', 'Gdańsk', '80-001', '+48456789012',
	 'polnoc@sportfit.pl', 0, 1),
	(@Branch4Id, 'SportFit Wschód', 'PL5678901234', 'Wschodnia 40', 'Lublin', '20-001', '+48567890123',
	 'wschod@sportfit.pl', 0, 1),
	(@Branch5Id, 'SportFit Zachód', 'PL6789012345', 'Zachodnia 50', 'Wrocław', '50-001', '+48678901234',
	 'zachod@sportfit.pl', 0, 1),

	-- FitZone Network - firma główna (nie jest recepcją)
	(@FitZoneMainId, 'FitZone Network', 'PL7890123456', 'Fitness 5', 'Warszawa', '02-001', '+48789012345',
	 'contact@fitzonenetwork.pl', 1, 0),
	-- FitZone - recepcje (są recepcjami)
	(@FitZone1Id, 'FitZone Mokotów', 'PL8901234567', 'Mokotowska 15', 'Warszawa', '02-002', '+48890123456',
	 'mokotow@fitzone.pl', 0, 1),
	(@FitZone2Id, 'FitZone Katowice', 'PL9012345678', 'Śląska 25', 'Katowice', '40-001', '+48901234567',
	 'katowice@fitzone.pl', 0, 1),
	(@FitZone3Id, 'FitZone Poznań', 'PL0123456789', 'Wielkopolska 35', 'Poznań', '60-001', '+48012345678',
	 'poznan@fitzone.pl', 0, 1),

	-- AquaFit Centers - firma główna (nie jest recepcją)
	(@AquaFitMainId, 'AquaFit Centers', 'PL1357924680', 'Wodna 8', 'Gdynia', '81-001', '+48135792468',
	 'contact@aquafitcenters.pl', 1, 0),
	-- AquaFit - recepcje (są recepcjami)
	(@AquaFit1Id, 'AquaFit Marina', 'PL2468135790', 'Portowa 12', 'Gdynia', '81-002', '+48246813579',
	 'marina@aquafit.pl', 0, 1),
	(@AquaFit2Id, 'AquaFit Sopot', 'PL3579246801', 'Plażowa 7', 'Sopot', '81-700', '+48357924680', 'sopot@aquafit.pl',
	 0, 1),

	-- PowerGym Chain - firma główna (nie jest recepcją)
	(@PowerGymMainId, 'PowerGym Chain', 'PL4680357912', 'Siłowa 3', 'Łódź', '90-001', '+48468035791',
	 'contact@powergymchain.pl', 1, 0),
	-- PowerGym - recepcje (są recepcjami)
	(@PowerGym1Id, 'PowerGym Center', 'PL5791468023', 'Centralna 45', 'Łódź', '90-002', '+48579146802',
	 'center@powergym.pl', 0, 1),
	(@PowerGym2Id, 'PowerGym Bydgoszcz', 'PL6802579134', 'Kujawska 18', 'Bydgoszcz', '85-001', '+48680257913',
	 'bydgoszcz@powergym.pl', 0, 1),

	-- FlexYoga Studios - firma główna (nie jest recepcją)
	(@FlexYogaMainId, 'FlexYoga Studios', 'PL7913680245', 'Relaksacyjna 22', 'Warszawa', '01-001', '+48791368024',
	 'contact@flexyogastudios.pl', 1, 0),
	-- FlexYoga - recepcja (jest recepcją))
	(@FlexYoga1Id, 'FlexYoga Wilanów', 'PL8024791356', 'Spokójna 33', 'Warszawa', '02-958', '+48802479135',
	 'wilanow@flexyoga.pl', 0, 1);

-- =============================================
-- 2. CompanyHierarchies - dodanie hierarchii firm
-- =============================================

INSERT INTO CompanyHierarchies (CompanyId, ParentCompanyId)
VALUES
	-- SportFit - recepcje podległe firmie głównej
	(@Branch1Id, @MainCompanyId),
	(@Branch2Id, @MainCompanyId),
	(@Branch3Id, @MainCompanyId),
	(@Branch4Id, @MainCompanyId),
	(@Branch5Id, @MainCompanyId),

	-- FitZone - recepcje podległe firmie głównej
	(@FitZone1Id, @FitZoneMainId),
	(@FitZone2Id, @FitZoneMainId),
	(@FitZone3Id, @FitZoneMainId),

	-- AquaFit - recepcje podległe firmie głównej
	(@AquaFit1Id, @AquaFitMainId),
	(@AquaFit2Id, @AquaFitMainId),

	-- PowerGym - recepcje podległe firmie głównej
	(@PowerGym1Id, @PowerGymMainId),
	(@PowerGym2Id, @PowerGymMainId),

	-- FlexYoga - recepcja podległa firmie głównej
	(@FlexYoga1Id, @FlexYogaMainId);

-- =============================================
-- 3. Staff - personel
-- =============================================

-- Pracownicy recepcji
DECLARE @StaffRec1Id UNIQUEIDENTIFIER = NEWID();
DECLARE @StaffRec2Id UNIQUEIDENTIFIER = NEWID();
DECLARE @StaffRec3Id UNIQUEIDENTIFIER = NEWID();
DECLARE @StaffRec4Id UNIQUEIDENTIFIER = NEWID();
DECLARE @StaffRec5Id UNIQUEIDENTIFIER = NEWID();
DECLARE @StaffRec6Id UNIQUEIDENTIFIER = NEWID();
DECLARE @StaffRec7Id UNIQUEIDENTIFIER = NEWID();
DECLARE @StaffRec8Id UNIQUEIDENTIFIER = NEWID();
DECLARE @StaffRec9Id UNIQUEIDENTIFIER = NEWID();
DECLARE @StaffRec10Id UNIQUEIDENTIFIER = NEWID();
DECLARE @StaffRec11Id UNIQUEIDENTIFIER = NEWID();
DECLARE @StaffRec12Id UNIQUEIDENTIFIER = NEWID();
DECLARE @StaffRec13Id UNIQUEIDENTIFIER = NEWID();

-- Trenerzy
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
DECLARE @Trainer11Id UNIQUEIDENTIFIER = NEWID();
DECLARE @Trainer12Id UNIQUEIDENTIFIER = NEWID();
DECLARE @Trainer13Id UNIQUEIDENTIFIER = NEWID();
DECLARE @Trainer14Id UNIQUEIDENTIFIER = NEWID();
DECLARE @Trainer15Id UNIQUEIDENTIFIER = NEWID();
DECLARE @Trainer16Id UNIQUEIDENTIFIER = NEWID();
DECLARE @Trainer17Id UNIQUEIDENTIFIER = NEWID();
DECLARE @Trainer18Id UNIQUEIDENTIFIER = NEWID();
DECLARE @Trainer19Id UNIQUEIDENTIFIER = NEWID();
DECLARE @Trainer20Id UNIQUEIDENTIFIER = NEWID();

-- Managerowie
DECLARE @Manager1Id UNIQUEIDENTIFIER = NEWID();
DECLARE @Manager2Id UNIQUEIDENTIFIER = NEWID();
DECLARE @Manager3Id UNIQUEIDENTIFIER = NEWID();
DECLARE @Manager4Id UNIQUEIDENTIFIER = NEWID();
DECLARE @Manager5Id UNIQUEIDENTIFIER = NEWID();
DECLARE @Manager6Id UNIQUEIDENTIFIER = NEWID();
DECLARE @Manager7Id UNIQUEIDENTIFIER = NEWID();
DECLARE @Manager8Id UNIQUEIDENTIFIER = NEWID();
DECLARE @Manager9Id UNIQUEIDENTIFIER = NEWID();
DECLARE @Manager10Id UNIQUEIDENTIFIER = NEWID();

-- =============================================
-- CompanyConfig - konfiguracja dla firm
-- =============================================

INSERT INTO CompanyConfigs (CompanyId, BreakTimeStaff, BreakTimeParticipants)
VALUES
	-- SportFit Group (firma główna + oddziały)
	(@MainCompanyId, 15, 10),
	(@Branch1Id, 10, 5),
	(@Branch2Id, 12, 6),
	(@Branch3Id, 10, 5),
	(@Branch4Id, 15, 7),
	(@Branch5Id, 10, 5),

	-- FitZone Network (firma główna + oddziały)
	(@FitZoneMainId, 20, 15),
	(@FitZone1Id, 10, 5),
	(@FitZone2Id, 12, 6),
	(@FitZone3Id, 15, 8),

	-- AquaFit Centers (firma główna + oddziały)
	(@AquaFitMainId, 20, 10),
	(@AquaFit1Id, 10, 5),
	(@AquaFit2Id, 12, 6),

	-- PowerGym Chain (firma główna + oddziały)
	(@PowerGymMainId, 15, 10),
	(@PowerGym1Id, 10, 5),
	(@PowerGym2Id, 12, 6),

	-- FlexYoga Studios (firma główna + oddział)
	(@FlexYogaMainId, 15, 10),
	(@FlexYoga1Id, 10, 5);

-- =============================================
-- Wstawiamy personel (bez CompanyId)
-- =============================================
INSERT INTO Staff (Id, Role, Email, Password, FirstName, LastName, Phone)
VALUES
	-- Pracownicy recepcji
	(@StaffRec1Id, 'ReceptionEmployee', 'kowalska@sportfit.pl', '$2a$11$4r/77UPSpJvAbAkQt2U5oOpG5vzSHlxyPZQiUi5aN0biwTut0ewsm', 'Anna', 'Kowalska', '+48700100101'),
	(@StaffRec2Id, 'ReceptionEmployee', 'nowak@sportfit.pl', '$2a$11$4r/77UPSpJvAbAkQt2U5oOpG5vzSHlxyPZQiUi5aN0biwTut0ewsm', 'Barbara', 'Nowak', '+48700100102'),
	(@StaffRec3Id, 'ReceptionEmployee', 'wisniewska@sportfit.pl', '$2a$11$4r/77UPSpJvAbAkQt2U5oOpG5vzSHlxyPZQiUi5aN0biwTut0ewsm', 'Celina', 'Wiśniewska','+48700100103'),
	(@StaffRec4Id, 'ReceptionEmployee', 'kaminska@sportfit.pl', '$2a$11$4r/77UPSpJvAbAkQt2U5oOpG5vzSHlxyPZQiUi5aN0biwTut0ewsm', 'Diana', 'Kamińska', '+48700100104'),
	(@StaffRec5Id, 'ReceptionEmployee', 'lewandowska@sportfit.pl', '$2a$11$4r/77UPSpJvAbAkQt2U5oOpG5vzSHlxyPZQiUi5aN0biwTut0ewsm', 'Ewa', 'Lewandowska', '+48700100105'),
	(@StaffRec6Id, 'ReceptionEmployee', 'zielinska@fitzone.pl', '$2a$11$4r/77UPSpJvAbAkQt2U5oOpG5vzSHlxyPZQiUi5aN0biwTut0ewsm', 'Fatima', 'Zielińska', '+48700100106'),
	(@StaffRec7Id, 'ReceptionEmployee', 'szymanska@fitzone.pl', '$2a$11$4r/77UPSpJvAbAkQt2U5oOpG5vzSHlxyPZQiUi5aN0biwTut0ewsm', 'Gabriela', 'Szymańska', '+48700100107'),
	(@StaffRec8Id, 'ReceptionEmployee', 'wojcik@fitzone.pl', '$2a$11$4r/77UPSpJvAbAkQt2U5oOpG5vzSHlxyPZQiUi5aN0biwTut0ewsm', 'Hanna', 'Wójcik', '+48700100108'),
	(@StaffRec9Id, 'ReceptionEmployee', 'kowalczyk@aquafit.pl', '$2a$11$4r/77UPSpJvAbAkQt2U5oOpG5vzSHlxyPZQiUi5aN0biwTut0ewsm', 'Irena', 'Kowalczyk', '+48700100109'),
	(@StaffRec10Id, 'ReceptionEmployee', 'kozlowska@aquafit.pl', '$2a$11$4r/77UPSpJvAbAkQt2U5oOpG5vzSHlxyPZQiUi5aN0biwTut0ewsm', 'Justyna', 'Kozłowska', '+48700100110'),
	(@StaffRec11Id, 'ReceptionEmployee', 'jankowska@powergym.pl', '$2a$11$4r/77UPSpJvAbAkQt2U5oOpG5vzSHlxyPZQiUi5aN0biwTut0ewsm', 'Klara', 'Jankowska', '+48700100111'),
	(@StaffRec12Id, 'ReceptionEmployee', 'zawadzka@powergym.pl', '$2a$11$4r/77UPSpJvAbAkQt2U5oOpG5vzSHlxyPZQiUi5aN0biwTut0ewsm', 'Laura', 'Zawadzka', '+48700100112'),
	(@StaffRec13Id, 'ReceptionEmployee', 'mazur@flexyoga.pl', '$2a$11$4r/77UPSpJvAbAkQt2U5oOpG5vzSHlxyPZQiUi5aN0biwTut0ewsm', 'Monika', 'Mazur', '+48700100113'),

	-- Trenerzy
	(@Trainer1Id, 'Trainer', 'malinowski@sportfit.pl', '$2a$11$4r/77UPSpJvAbAkQt2U5oOpG5vzSHlxyPZQiUi5aN0biwTut0ewsm', 'Dariusz', 'Malinowski', '+48700200201'),
	(@Trainer2Id, 'Trainer', 'jablonska@sportfit.pl', '$2a$11$4r/77UPSpJvAbAkQt2U5oOpG5vzSHlxyPZQiUi5aN0biwTut0ewsm', 'Ewa', 'Jabłońska', '+48700200202'),
	(@Trainer3Id, 'Trainer', 'kowalczyk@sportfit.pl', '$2a$11$4r/77UPSpJvAbAkQt2U5oOpG5vzSHlxyPZQiUi5aN0biwTut0ewsm', 'Filip', 'Kowalczyk', '+48700200203'),
	(@Trainer4Id, 'Trainer', 'lewandowska2@sportfit.pl', '$2a$11$4r/77UPSpJvAbAkQt2U5oOpG5vzSHlxyPZQiUi5aN0biwTut0ewsm', 'Grażyna', 'Lewandowska', '+48700200204'),
	(@Trainer5Id, 'Trainer', 'zielinski@sportfit.pl', '$2a$11$4r/77UPSpJvAbAkQt2U5oOpG5vzSHlxyPZQiUi5aN0biwTut0ewsm', 'Henryk', 'Zieliński', '+48700200205'),
	(@Trainer6Id, 'Trainer', 'szymanska2@sportfit.pl', '$2a$11$4r/77UPSpJvAbAkQt2U5oOpG5vzSHlxyPZQiUi5aN0biwTut0ewsm', 'Iwona', 'Szymańska', '+48700200206'),
	(@Trainer7Id, 'Trainer', 'borkowski@sportfit.pl', '$2a$11$4r/77UPSpJvAbAkQt2U5oOpG5vzSHlxyPZQiUi5aN0biwTut0ewsm', 'Jacek', 'Borkowski', '+48700200207'),
	(@Trainer8Id, 'Trainer', 'krawczyk@sportfit.pl', '$2a$11$4r/77UPSpJvAbAkQt2U5oOpG5vzSHlxyPZQiUi5aN0biwTut0ewsm', 'Kinga', 'Krawczyk', '+48700200208'),
	(@Trainer9Id, 'Trainer', 'nowicki@fitzone.pl', '$2a$11$4r/77UPSpJvAbAkQt2U5oOpG5vzSHlxyPZQiUi5aN0biwTut0ewsm', 'Łukasz', 'Nowicki', '+48700200209'),
	(@Trainer10Id, 'Trainer', 'pawlak@fitzone.pl', '$2a$11$4r/77UPSpJvAbAkQt2U5oOpG5vzSHlxyPZQiUi5aN0biwTut0ewsm', 'Marta', 'Pawlak', '+48700200210'),
	(@Trainer11Id, 'Trainer', 'michalski@fitzone.pl', '$2a$11$4r/77UPSpJvAbAkQt2U5oOpG5vzSHlxyPZQiUi5aN0biwTut0ewsm', 'Norbert', 'Michalski', '+48700200211'),
	(@Trainer12Id, 'Trainer', 'olszewska@fitzone.pl', '$2a$11$4r/77UPSpJvAbAkQt2U5oOpG5vzSHlxyPZQiUi5aN0biwTut0ewsm', 'Oliwia', 'Olszewska', '+48700200212'),
	(@Trainer13Id, 'Trainer', 'adamczyk@aquafit.pl', '$2a$11$4r/77UPSpJvAbAkQt2U5oOpG5vzSHlxyPZQiUi5aN0biwTut0ewsm', 'Piotr', 'Adamczyk', '+48700200213'),
	(@Trainer14Id, 'Trainer', 'rutkowska@aquafit.pl', '$2a$11$4r/77UPSpJvAbAkQt2U5oOpG5vzSHlxyPZQiUi5aN0biwTut0ewsm', 'Renata', 'Rutkowska', '+48700200214'),
	(@Trainer15Id, 'Trainer', 'sikora@aquafit.pl', '$2a$11$4r/77UPSpJvAbAkQt2U5oOpG5vzSHlxyPZQiUi5aN0biwTut0ewsm', 'Sebastian', 'Sikora', '+48700200215'),
	(@Trainer16Id, 'Trainer', 'baran@powergym.pl', '$2a$11$4r/77UPSpJvAbAkQt2U5oOpG5vzSHlxyPZQiUi5aN0biwTut0ewsm', 'Tomasz', 'Baran', '+48700200216'),
	(@Trainer17Id, 'Trainer', 'urbanska@powergym.pl', '$2a$11$4r/77UPSpJvAbAkQt2U5oOpG5vzSHlxyPZQiUi5aN0biwTut0ewsm', 'Urszula', 'Urbańska', '+48700200217'),
	(@Trainer18Id, 'Trainer', 'walczak@powergym.pl', '$2a$11$4r/77UPSpJvAbAkQt2U5oOpG5vzSHlxyPZQiUi5aN0biwTut0ewsm', 'Władysław', 'Walczak', '+48700200218'),
	(@Trainer19Id, 'Trainer', 'zakrzewski@flexyoga.pl', '$2a$11$4r/77UPSpJvAbAkQt2U5oOpG5vzSHlxyPZQiUi5aN0biwTut0ewsm', 'Yolanda', 'Zakrzewska', '+48700200219'),
	(@Trainer20Id, 'Trainer', 'adamski@flexyoga.pl', '$2a$11$4r/77UPSpJvAbAkQt2U5oOpG5vzSHlxyPZQiUi5aN0biwTut0ewsm', 'Zbigniew', 'Adamski', '+48700200220'),

	-- Managerowie
	(@Manager1Id, 'Manager', 'mazur.manager@sportfit.pl', '$2a$11$4r/77UPSpJvAbAkQt2U5oOpG5vzSHlxyPZQiUi5aN0biwTut0ewsm', 'Janusz', 'Mazur', '+48700300301'),
	(@Manager2Id, 'Manager', 'kaczmarek@sportfit.pl', '$2a$11$4r/77UPSpJvAbAkQt2U5oOpG5vzSHlxyPZQiUi5aN0biwTut0ewsm', 'Kamila', 'Kaczmarek', '+48700300302'),
	(@Manager3Id, 'Manager', 'grabowski@sportfit.pl', '$2a$11$4r/77UPSpJvAbAkQt2U5oOpG5vzSHlxyPZQiUi5aN0biwTut0ewsm', 'Leszek', 'Grabowski', '+48700300303'),
	(@Manager4Id, 'Manager', 'kowalski@sportfit.pl', '$2a$11$4r/77UPSpJvAbAkQt2U5oOpG5vzSHlxyPZQiUi5aN0biwTut0ewsm', 'Marcin', 'Kowalski', '+48700300304'),
	(@Manager5Id, 'Manager', 'nowakowska@sportfit.pl', '$2a$11$4r/77UPSpJvAbAkQt2U5oOpG5vzSHlxyPZQiUi5aN0biwTut0ewsm', 'Nina', 'Nowakowska', '+48700300305'),
	(@Manager6Id, 'Manager', 'pawlowski@fitzone.pl', '$2a$11$4r/77UPSpJvAbAkQt2U5oOpG5vzSHlxyPZQiUi5aN0biwTut0ewsm', 'Oscar', 'Pawłowski', '+48700300306'),
	(@Manager7Id, 'Manager', 'piotrowska@fitzone.pl', '$2a$11$4r/77UPSpJvAbAkQt2U5oOpG5vzSHlxyPZQiUi5aN0biwTut0ewsm', 'Patrycja', 'Piotrowska', '+48700300307'),
	(@Manager8Id, 'Manager', 'rybak@aquafit.pl', '$2a$11$4r/77UPSpJvAbAkQt2U5oOpG5vzSHlxyPZQiUi5aN0biwTut0ewsm', 'Robert', 'Rybak', '+48700300308'),
	(@Manager9Id, 'Manager', 'sokolowska@powergym.pl', '$2a$11$4r/77UPSpJvAbAkQt2U5oOpG5vzSHlxyPZQiUi5aN0biwTut0ewsm', 'Sylwia', 'Sokołowska', '+48700300309'),
	(@Manager10Id, 'Manager', 'tomaszewski@flexyoga.pl', '$2a$11$4r/77UPSpJvAbAkQt2U5oOpG5vzSHlxyPZQiUi5aN0biwTut0ewsm', 'Tomasz', 'Tomaszewski', '+48700300310');

-- =============================================
-- Powiązania Staff z Companies (wiele-do-wielu)
-- =============================================
INSERT INTO StaffMemberCompanies (StaffMemberId, CompanyId)
VALUES
	-- Pracownicy recepcji
	(@StaffRec1Id, @Branch1Id),
	(@StaffRec2Id, @Branch2Id),
	(@StaffRec3Id, @Branch3Id),
	(@StaffRec4Id, @Branch4Id),
	(@StaffRec5Id, @Branch5Id),
	(@StaffRec6Id, @FitZone1Id),
	(@StaffRec7Id, @FitZone2Id),
	(@StaffRec8Id, @FitZone3Id),
	(@StaffRec9Id, @AquaFit1Id),
	(@StaffRec10Id, @AquaFit2Id),
	(@StaffRec11Id, @PowerGym1Id),
	(@StaffRec12Id, @PowerGym2Id),
	(@StaffRec13Id, @FlexYoga1Id),

	-- Trenerzy
	(@Trainer1Id, @Branch1Id),
	(@Trainer2Id, @Branch1Id),
	(@Trainer3Id, @Branch2Id),
	(@Trainer4Id, @Branch2Id),
	(@Trainer5Id, @Branch3Id),
	(@Trainer6Id, @Branch3Id),
	(@Trainer7Id, @Branch4Id),
	(@Trainer8Id, @Branch5Id),
	(@Trainer9Id, @FitZone1Id),
	(@Trainer10Id, @FitZone1Id),
	(@Trainer11Id, @FitZone2Id),
	(@Trainer12Id, @FitZone3Id),
	(@Trainer13Id, @AquaFit1Id),
	(@Trainer14Id, @AquaFit1Id),
	(@Trainer15Id, @AquaFit2Id),
	(@Trainer16Id, @PowerGym1Id),
	(@Trainer17Id, @PowerGym1Id),
	(@Trainer18Id, @PowerGym2Id),
	(@Trainer19Id, @FlexYoga1Id),
	(@Trainer20Id, @FlexYoga1Id),

	-- Managerowie
	(@Manager1Id, @Branch1Id),
	(@Manager2Id, @Branch2Id),
	(@Manager3Id, @Branch3Id),
	(@Manager4Id, @Branch4Id),
	(@Manager5Id, @Branch5Id),
	(@Manager6Id, @FitZone1Id),
	(@Manager7Id, @FitZone2Id),
	(@Manager8Id, @AquaFit1Id),
	(@Manager9Id, @PowerGym1Id),
	(@Manager10Id, @FlexYoga1Id);

-- =============================================
-- 4. Participants - uczestnicy
-- =============================================

DECLARE
	@Participant1Id UNIQUEIDENTIFIER = NEWID();
DECLARE
	@Participant2Id UNIQUEIDENTIFIER = NEWID();
DECLARE
	@Participant3Id UNIQUEIDENTIFIER = NEWID();
DECLARE
	@Participant4Id UNIQUEIDENTIFIER = NEWID();
DECLARE
	@Participant5Id UNIQUEIDENTIFIER = NEWID();
DECLARE
	@Participant6Id UNIQUEIDENTIFIER = NEWID();
DECLARE
	@Participant7Id UNIQUEIDENTIFIER = NEWID();
DECLARE
	@Participant8Id UNIQUEIDENTIFIER = NEWID();
DECLARE
	@Participant9Id UNIQUEIDENTIFIER = NEWID();
DECLARE
	@Participant10Id UNIQUEIDENTIFIER = NEWID();
DECLARE
	@Participant11Id UNIQUEIDENTIFIER = NEWID();
DECLARE
	@Participant12Id UNIQUEIDENTIFIER = NEWID();
DECLARE
	@Participant13Id UNIQUEIDENTIFIER = NEWID();
DECLARE
	@Participant14Id UNIQUEIDENTIFIER = NEWID();
DECLARE
	@Participant15Id UNIQUEIDENTIFIER = NEWID();
DECLARE
	@Participant16Id UNIQUEIDENTIFIER = NEWID();
DECLARE
	@Participant17Id UNIQUEIDENTIFIER = NEWID();
DECLARE
	@Participant18Id UNIQUEIDENTIFIER = NEWID();
DECLARE
	@Participant19Id UNIQUEIDENTIFIER = NEWID();
DECLARE
	@Participant20Id UNIQUEIDENTIFIER = NEWID();
DECLARE
	@Participant21Id UNIQUEIDENTIFIER = NEWID();
DECLARE
	@Participant22Id UNIQUEIDENTIFIER = NEWID();
DECLARE
	@Participant23Id UNIQUEIDENTIFIER = NEWID();
DECLARE
	@Participant24Id UNIQUEIDENTIFIER = NEWID();
DECLARE
	@Participant25Id UNIQUEIDENTIFIER = NEWID();
DECLARE
	@Participant26Id UNIQUEIDENTIFIER = NEWID();
DECLARE
	@Participant27Id UNIQUEIDENTIFIER = NEWID();
DECLARE
	@Participant28Id UNIQUEIDENTIFIER = NEWID();
DECLARE
	@Participant29Id UNIQUEIDENTIFIER = NEWID();
DECLARE
	@Participant30Id UNIQUEIDENTIFIER = NEWID();

INSERT INTO Participants (Id, CompanyId, Email, FirstName, LastName, Phone, GdprConsent)
VALUES
	-- Uczestnicy SportFit Centrum
	(@Participant1Id, @Branch1Id, 'marek.adamski@email.com', 'Marek', 'Adamski', '+48800400401', 1),
	(@Participant2Id, @Branch1Id, 'natalia.barska@gmail.com', 'Natalia', 'Barska', '+48800400402', 1),
	(@Participant3Id, @Branch1Id, 'olgierd.cichocki@outlook.com', 'Olgierd', 'Cichocki', '+48800400403', 1),
	(@Participant4Id, @Branch1Id, 'anna.dabrowska@yahoo.com', 'Anna', 'Dąbrowska', '+48800400404', 1),

	-- Uczestnicy SportFit Południe
	(@Participant5Id, @Branch2Id, 'patrycja.dabrowska@email.com', 'Patrycja', 'Dąbrowska', '+48800400405', 1),
	(@Participant6Id, @Branch2Id, 'rafal.eski@gmail.com', 'Rafał', 'Eski', '+48800400406', 1),
	(@Participant7Id, @Branch2Id, 'sylwia.frankowska@outlook.com', 'Sylwia', 'Frankowska', '+48800400407', 1),
	(@Participant8Id, @Branch2Id, 'krzysztof.glowacki@yahoo.com', 'Krzysztof', 'Głowacki', '+48800400408', 1),

	-- Uczestnicy SportFit Północ
	(@Participant9Id, @Branch3Id, 'tomasz.gorski@email.com', 'Tomasz', 'Górski', '+48800400409', 1),
	(@Participant10Id, @Branch3Id, 'urszula.horak@gmail.com', 'Urszula', 'Horak', '+48800400410', 1),
	(@Participant11Id, @Branch3Id, 'wiktor.iwanski@outlook.com', 'Wiktor', 'Iwański', '+48800400411', 1),

	-- Uczestnicy SportFit Wschód
	(@Participant12Id, @Branch4Id, 'karolina.jaworska@email.com', 'Karolina', 'Jaworska', '+48800400412', 1),
	(@Participant13Id, @Branch4Id, 'lukasz.kowalski@gmail.com', 'Łukasz', 'Kowalski', '+48800400413', 1),

	-- Uczestnicy SportFit Zachód
	(@Participant14Id, @Branch5Id, 'magdalena.lewicz@outlook.com', 'Magdalena', 'Lewicz', '+48800400414', 1),
	(@Participant15Id, @Branch5Id, 'norbert.mazurek@yahoo.com', 'Norbert', 'Mazurek', '+48800400415', 1),

	-- Uczestnicy FitZone Mokotów
	(@Participant16Id, @FitZone1Id, 'oliwia.nowak@email.com', 'Oliwia', 'Nowak', '+48800400416', 1),
	(@Participant17Id, @FitZone1Id, 'pawel.osinski@gmail.com', 'Paweł', 'Osiński', '+48800400417', 1),
	(@Participant18Id, @FitZone1Id, 'renata.piotrkowska@outlook.com', 'Renata', 'Piotrkowska', '+48800400418', 1),

	-- Uczestnicy FitZone Katowice
	(@Participant19Id, @FitZone2Id, 'sebastian.rutkowski@email.com', 'Sebastian', 'Rutkowski', '+48800400419', 1),
	(@Participant20Id, @FitZone2Id, 'tatiana.sikorska@gmail.com', 'Tatiana', 'Sikorska', '+48800400420', 1),

	-- Uczestnicy FitZone Poznań
	(@Participant21Id, @FitZone3Id, 'urszula.tomczak@outlook.com', 'Urszula', 'Tomczak', '+48800400421', 1),
	(@Participant22Id, @FitZone3Id, 'viktor.wisniewski@yahoo.com', 'Viktor', 'Wiśniewski', '+48800400422', 1),

	-- Uczestnicy AquaFit Marina
	(@Participant23Id, @AquaFit1Id, 'wanda.zalewski@email.com', 'Wanda', 'Zalewski', '+48800400423', 1),
	(@Participant24Id, @AquaFit1Id, 'xavier.adamczyk@gmail.com', 'Xavier', 'Adamczyk', '+48800400424', 1),

	-- Uczestnicy AquaFit Sopot
	(@Participant25Id, @AquaFit2Id, 'yvonne.bednarska@outlook.com', 'Yvonne', 'Bednarska', '+48800400425', 1),
	(@Participant26Id, @AquaFit2Id, 'zbigniew.czarnecki@yahoo.com', 'Zbigniew', 'Czarnecki', '+48800400426', 1),

	-- Uczestnicy PowerGym Center
	(@Participant27Id, @PowerGym1Id, 'agata.dabek@email.com', 'Agata', 'Dąbek', '+48800400427', 1),
	(@Participant28Id, @PowerGym1Id, 'bartosz.eliasz@gmail.com', 'Bartosz', 'Eliasz', '+48800400428', 1),

	-- Uczestnicy PowerGym Bydgoszcz
	(@Participant29Id, @PowerGym2Id, 'celina.filipiak@outlook.com', 'Celina', 'Filipiak', '+48800400429', 1),
	(@Participant30Id, @PowerGym2Id, 'damian.gorzynski@yahoo.com', 'Damian', 'Górzyński', '+48800400430', 1);

-- =============================================
-- 5. Specializations - specjalizacje
-- =============================================

DECLARE
	@Spec1Id UNIQUEIDENTIFIER = NEWID();
DECLARE
	@Spec2Id UNIQUEIDENTIFIER = NEWID();
DECLARE
	@Spec3Id UNIQUEIDENTIFIER = NEWID();
DECLARE
	@Spec4Id UNIQUEIDENTIFIER = NEWID();
DECLARE
	@Spec5Id UNIQUEIDENTIFIER = NEWID();
DECLARE
	@Spec6Id UNIQUEIDENTIFIER = NEWID();
DECLARE
	@Spec7Id UNIQUEIDENTIFIER = NEWID();
DECLARE
	@Spec8Id UNIQUEIDENTIFIER = NEWID();
DECLARE
	@Spec9Id UNIQUEIDENTIFIER = NEWID();
DECLARE
	@Spec10Id UNIQUEIDENTIFIER = NEWID();
DECLARE
	@Spec11Id UNIQUEIDENTIFIER = NEWID();
DECLARE
	@Spec12Id UNIQUEIDENTIFIER = NEWID();
DECLARE
	@Spec13Id UNIQUEIDENTIFIER = NEWID();
DECLARE
	@Spec14Id UNIQUEIDENTIFIER = NEWID();
DECLARE
	@Spec15Id UNIQUEIDENTIFIER = NEWID();
DECLARE
	@Spec16Id UNIQUEIDENTIFIER = NEWID();
DECLARE
	@Spec17Id UNIQUEIDENTIFIER = NEWID();
DECLARE
	@Spec18Id UNIQUEIDENTIFIER = NEWID();
DECLARE
	@Spec19Id UNIQUEIDENTIFIER = NEWID();
DECLARE
	@Spec20Id UNIQUEIDENTIFIER = NEWID();

INSERT INTO Specializations (Id, CompanyId, Name, Description)
VALUES
	-- SportFit Centrum
	(@Spec1Id, @Branch1Id, 'Joga', 'Zajęcia jogi dla początkujących i zaawansowanych'),
	(@Spec2Id, @Branch1Id, 'Trening siłowy', 'Zajęcia na siłowni z trenerem personalnym'),
	(@Spec3Id, @Branch1Id, 'Pilates', 'Ćwiczenia wzmacniające i rozciągające'),

	-- SportFit Południe
	(@Spec4Id, @Branch2Id, 'Crossfit', 'Intensywny trening crossfit dla wszystkich poziomów'),
	(@Spec5Id, @Branch2Id, 'Spinning', 'Zajęcia na rowerach stacjonarnych'),
	(@Spec6Id, @Branch2Id, 'Zumba', 'Energiczne zajęcia taneczne'),

	-- SportFit Północ
	(@Spec7Id, @Branch3Id, 'Pływanie', 'Nauka pływania i doskonalenie techniki'),
	(@Spec8Id, @Branch3Id, 'Aqua aerobik', 'Ćwiczenia w wodzie'),

	-- SportFit Wschód
	(@Spec9Id, @Branch4Id, 'Box', 'Trening bokserski dla różnych poziomów'),
	(@Spec10Id, @Branch4Id, 'Kickboxing', 'Sztuki walki z elementami cardio'),

	-- SportFit Zachód
	(@Spec11Id, @Branch5Id, 'TRX', 'Trening funkcjonalny z linami TRX'),
	(@Spec12Id, @Branch5Id, 'Stretching', 'Zajęcia rozciągające i relaksacyjne'),

	-- FitZone Mokotów
	(@Spec13Id, @FitZone1Id, 'HIIT', 'Trening interwałowy wysokiej intensywności'),
	(@Spec14Id, @FitZone1Id, 'Body Pump', 'Zajęcia z ciężarkami do muzyki'),

	-- FitZone Katowice
	(@Spec15Id, @FitZone2Id, 'Tabata', 'Krótkie, intensywne treningi'),

	-- FitZone Poznań
	(@Spec16Id, @FitZone3Id, 'Functional Training', 'Trening funkcjonalny całego ciała'),

	-- AquaFit Marina
	(@Spec17Id, @AquaFit1Id, 'Pływanie sportowe', 'Zaawansowane techniki pływackie'),

	-- AquaFit Sopot
	(@Spec18Id, @AquaFit2Id, 'Aqua fitness', 'Kompleksowy trening w wodzie'),

	-- PowerGym Center
	(@Spec19Id, @PowerGym1Id, 'Powerlifting', 'Trening siłowy - martwy ciąg, przysiad, wyciskanie'),

	-- FlexYoga Wilanów
	(@Spec20Id, @FlexYoga1Id, 'Hatha Yoga', 'Klasyczna joga z naciskiem na pozycje');

-- =============================================
-- 6. StaffMemberSpecializations - przypisanie specjalizacji do trenerów
-- =============================================

INSERT INTO StaffMemberSpecializations (Id, CompanyId, StaffMemberId, SpecializationId)
VALUES
	-- SportFit Centrum
	(NEWID(), @Branch1Id, @Trainer1Id, @Spec1Id),
	(NEWID(), @Branch1Id, @Trainer1Id, @Spec2Id),
	(NEWID(), @Branch1Id, @Trainer2Id, @Spec2Id),
	(NEWID(), @Branch1Id, @Trainer2Id, @Spec3Id),

	-- SportFit Południe
	(NEWID(), @Branch2Id, @Trainer3Id, @Spec4Id),
	(NEWID(), @Branch2Id, @Trainer3Id, @Spec6Id),
	(NEWID(), @Branch2Id, @Trainer4Id, @Spec5Id),
	(NEWID(), @Branch2Id, @Trainer4Id, @Spec6Id),

	-- SportFit Północ
	(NEWID(), @Branch3Id, @Trainer5Id, @Spec7Id),
	(NEWID(), @Branch3Id, @Trainer5Id, @Spec8Id),
	(NEWID(), @Branch3Id, @Trainer6Id, @Spec7Id),

	-- SportFit Wschód
	(NEWID(), @Branch4Id, @Trainer7Id, @Spec9Id),
	(NEWID(), @Branch4Id, @Trainer7Id, @Spec10Id),

	-- SportFit Zachód
	(NEWID(), @Branch5Id, @Trainer8Id, @Spec11Id),
	(NEWID(), @Branch5Id, @Trainer8Id, @Spec12Id),

	-- FitZone Mokotów
	(NEWID(), @FitZone1Id, @Trainer9Id, @Spec13Id),
	(NEWID(), @FitZone1Id, @Trainer10Id, @Spec14Id),
	(NEWID(), @FitZone1Id, @Trainer10Id, @Spec13Id),

	-- FitZone Katowice
	(NEWID(), @FitZone2Id, @Trainer11Id, @Spec15Id),

	-- FitZone Poznań
	(NEWID(), @FitZone3Id, @Trainer12Id, @Spec16Id),

	-- AquaFit Marina
	(NEWID(), @AquaFit1Id, @Trainer13Id, @Spec17Id),
	(NEWID(), @AquaFit1Id, @Trainer14Id, @Spec17Id),

	-- AquaFit Sopot
	(NEWID(), @AquaFit2Id, @Trainer15Id, @Spec18Id),

	-- PowerGym Center
	(NEWID(), @PowerGym1Id, @Trainer16Id, @Spec19Id),
	(NEWID(), @PowerGym1Id, @Trainer17Id, @Spec19Id),

	-- PowerGym Bydgoszcz
	(NEWID(), @PowerGym2Id, @Trainer18Id, @Spec19Id),

	-- FlexYoga Wilanów
	(NEWID(), @FlexYoga1Id, @Trainer19Id, @Spec20Id),
	(NEWID(), @FlexYoga1Id, @Trainer20Id, @Spec20Id);

-- =============================================
-- 7. StaffMemberAvailabilities - dodanie dostępności personelu
-- =============================================

INSERT INTO StaffMemberAvailabilities (Id, CompanyId, StaffMemberId, Date, StartTime, EndTime, IsAvailable)
VALUES
	-- Dostępność trenerów SportFit Centrum
	(NEWID(), @Branch1Id, @Trainer1Id, '2025-07-28', '2025-07-28 08:00:00', '2025-07-28 16:00:00', 1),
	(NEWID(), @Branch1Id, @Trainer1Id, '2025-07-29', '2025-07-29 08:00:00', '2025-07-29 16:00:00', 1),
	(NEWID(), @Branch1Id, @Trainer1Id, '2025-07-30', '2025-07-30 08:00:00', '2025-07-30 16:00:00', 1),
	(NEWID(), @Branch1Id, @Trainer1Id, '2025-07-31', '2025-07-31 10:00:00', '2025-07-31 18:00:00', 1),
	(NEWID(), @Branch1Id, @Trainer1Id, '2025-08-01', '2025-08-01 08:00:00', '2025-08-01 16:00:00', 1),

	(NEWID(), @Branch1Id, @Trainer2Id, '2025-07-28', '2025-07-28 10:00:00', '2025-07-28 18:00:00', 1),
	(NEWID(), @Branch1Id, @Trainer2Id, '2025-07-29', '2025-07-29 10:00:00', '2025-07-29 18:00:00', 1),
	(NEWID(), @Branch1Id, @Trainer2Id, '2025-07-30', '2025-07-30 10:00:00', '2025-07-30 18:00:00', 1),
	(NEWID(), @Branch1Id, @Trainer2Id, '2025-07-31', '2025-07-31 12:00:00', '2025-07-31 20:00:00', 1),
	(NEWID(), @Branch1Id, @Trainer2Id, '2025-08-01', '2025-08-01 10:00:00', '2025-08-01 18:00:00', 1),

	-- Dostępność trenerów SportFit Południe
	(NEWID(), @Branch2Id, @Trainer3Id, '2025-07-28', '2025-07-28 09:00:00', '2025-07-28 17:00:00', 1),
	(NEWID(), @Branch2Id, @Trainer3Id, '2025-07-29', '2025-07-29 09:00:00', '2025-07-29 17:00:00', 1),
	(NEWID(), @Branch2Id, @Trainer3Id, '2025-07-30', '2025-07-30 09:00:00', '2025-07-30 17:00:00', 1),
	(NEWID(), @Branch2Id, @Trainer3Id, '2025-07-31', '2025-07-31 11:00:00', '2025-07-31 19:00:00', 1),
	(NEWID(), @Branch2Id, @Trainer3Id, '2025-08-01', '2025-08-01 09:00:00', '2025-08-01 17:00:00', 1),

	(NEWID(), @Branch2Id, @Trainer4Id, '2025-07-28', '2025-07-28 11:00:00', '2025-07-28 19:00:00', 1),
	(NEWID(), @Branch2Id, @Trainer4Id, '2025-07-29', '2025-07-29 11:00:00', '2025-07-29 19:00:00', 1),
	(NEWID(), @Branch2Id, @Trainer4Id, '2025-07-30', '2025-07-30 11:00:00', '2025-07-30 19:00:00', 1),
	(NEWID(), @Branch2Id, @Trainer4Id, '2025-07-31', '2025-07-31 09:00:00', '2025-07-31 17:00:00', 1),
	(NEWID(), @Branch2Id, @Trainer4Id, '2025-08-01', '2025-08-01 11:00:00', '2025-08-01 19:00:00', 1),

	-- Dostępność trenerów SportFit Północ
	(NEWID(), @Branch3Id, @Trainer5Id, '2025-07-28', '2025-07-28 07:00:00', '2025-07-28 15:00:00', 1),
	(NEWID(), @Branch3Id, @Trainer5Id, '2025-07-29', '2025-07-29 07:00:00', '2025-07-29 15:00:00', 1),
	(NEWID(), @Branch3Id, @Trainer5Id, '2025-07-30', '2025-07-30 07:00:00', '2025-07-30 15:00:00', 1),
	(NEWID(), @Branch3Id, @Trainer5Id, '2025-07-31', '2025-07-31 08:00:00', '2025-07-31 16:00:00', 1),
	(NEWID(), @Branch3Id, @Trainer5Id, '2025-08-01', '2025-08-01 07:00:00', '2025-08-01 15:00:00', 1),

	(NEWID(), @Branch3Id, @Trainer6Id, '2025-07-28', '2025-07-28 13:00:00', '2025-07-28 21:00:00', 1),
	(NEWID(), @Branch3Id, @Trainer6Id, '2025-07-29', '2025-07-29 13:00:00', '2025-07-29 21:00:00', 1),
	(NEWID(), @Branch3Id, @Trainer6Id, '2025-07-30', '2025-07-30 13:00:00', '2025-07-30 21:00:00', 1),
	(NEWID(), @Branch3Id, @Trainer6Id, '2025-07-31', '2025-07-31 15:00:00', '2025-07-31 21:00:00', 1),
	(NEWID(), @Branch3Id, @Trainer6Id, '2025-08-01', '2025-08-01 13:00:00', '2025-08-01 21:00:00', 1),

	-- Dostępność trenerów FitZone
	(NEWID(), @FitZone1Id, @Trainer9Id, '2025-07-28', '2025-07-28 06:00:00', '2025-07-28 14:00:00', 1),
	(NEWID(), @FitZone1Id, @Trainer9Id, '2025-07-29', '2025-07-29 06:00:00', '2025-07-29 14:00:00', 1),
	(NEWID(), @FitZone1Id, @Trainer9Id, '2025-07-30', '2025-07-30 06:00:00', '2025-07-30 14:00:00', 1),

	(NEWID(), @FitZone1Id, @Trainer10Id, '2025-07-28', '2025-07-28 14:00:00', '2025-07-28 22:00:00', 1),
	(NEWID(), @FitZone1Id, @Trainer10Id, '2025-07-29', '2025-07-29 14:00:00', '2025-07-29 22:00:00', 1),
	(NEWID(), @FitZone1Id, @Trainer10Id, '2025-07-30', '2025-07-30 14:00:00', '2025-07-30 22:00:00', 1),

	-- Dostępność trenerów AquaFit
	(NEWID(), @AquaFit1Id, @Trainer13Id, '2025-07-28', '2025-07-28 06:00:00', '2025-07-28 14:00:00', 1),
	(NEWID(), @AquaFit1Id, @Trainer13Id, '2025-07-29', '2025-07-29 06:00:00', '2025-07-29 14:00:00', 1),
	(NEWID(), @AquaFit1Id, @Trainer13Id, '2025-07-30', '2025-07-30 06:00:00', '2025-07-30 14:00:00', 1),

	(NEWID(), @AquaFit1Id, @Trainer14Id, '2025-07-28', '2025-07-28 14:00:00', '2025-07-28 22:00:00', 1),
	(NEWID(), @AquaFit1Id, @Trainer14Id, '2025-07-29', '2025-07-29 14:00:00', '2025-07-29 22:00:00', 1),
	(NEWID(), @AquaFit1Id, @Trainer14Id, '2025-07-30', '2025-07-30 14:00:00', '2025-07-30 22:00:00', 1),

	-- Dostępność trenerów PowerGym
	(NEWID(), @PowerGym1Id, @Trainer16Id, '2025-07-28', '2025-07-28 05:00:00', '2025-07-28 13:00:00', 1),
	(NEWID(), @PowerGym1Id, @Trainer16Id, '2025-07-29', '2025-07-29 05:00:00', '2025-07-29 13:00:00', 1),
	(NEWID(), @PowerGym1Id, @Trainer16Id, '2025-07-30', '2025-07-30 05:00:00', '2025-07-30 13:00:00', 1),

	(NEWID(), @PowerGym1Id, @Trainer17Id, '2025-07-28', '2025-07-28 13:00:00', '2025-07-28 21:00:00', 1),
	(NEWID(), @PowerGym1Id, @Trainer17Id, '2025-07-29', '2025-07-29 13:00:00', '2025-07-29 21:00:00', 1),
	(NEWID(), @PowerGym1Id, @Trainer17Id, '2025-07-30', '2025-07-30 13:00:00', '2025-07-30 21:00:00', 1),

	-- Dostępność trenerów FlexYoga
	(NEWID(), @FlexYoga1Id, @Trainer19Id, '2025-07-28', '2025-07-28 08:00:00', '2025-07-28 16:00:00', 1),
	(NEWID(), @FlexYoga1Id, @Trainer19Id, '2025-07-29', '2025-07-29 08:00:00', '2025-07-29 16:00:00', 1),
	(NEWID(), @FlexYoga1Id, @Trainer19Id, '2025-07-30', '2025-07-30 08:00:00', '2025-07-30 16:00:00', 1),

	(NEWID(), @FlexYoga1Id, @Trainer20Id, '2025-07-28', '2025-07-28 16:00:00', '2025-07-28 22:00:00', 1),
	(NEWID(), @FlexYoga1Id, @Trainer20Id, '2025-07-29', '2025-07-29 16:00:00', '2025-07-29 22:00:00', 1),
	(NEWID(), @FlexYoga1Id, @Trainer20Id, '2025-07-30', '2025-07-30 16:00:00', '2025-07-30 22:00:00', 1);

-- =============================================
-- 8. EventTypes - dodanie typów wydarzeń 
-- =============================================

DECLARE
	@EventType1Id UNIQUEIDENTIFIER = NEWID();
DECLARE
	@EventType2Id UNIQUEIDENTIFIER = NEWID();
DECLARE
	@EventType3Id UNIQUEIDENTIFIER = NEWID();
DECLARE
	@EventType4Id UNIQUEIDENTIFIER = NEWID();
DECLARE
	@EventType5Id UNIQUEIDENTIFIER = NEWID();
DECLARE
	@EventType6Id UNIQUEIDENTIFIER = NEWID();
DECLARE
	@EventType7Id UNIQUEIDENTIFIER = NEWID();
DECLARE
	@EventType8Id UNIQUEIDENTIFIER = NEWID();
DECLARE
	@EventType9Id UNIQUEIDENTIFIER = NEWID();
DECLARE
	@EventType10Id UNIQUEIDENTIFIER = NEWID();
DECLARE
	@EventType11Id UNIQUEIDENTIFIER = NEWID();
DECLARE
	@EventType12Id UNIQUEIDENTIFIER = NEWID();
DECLARE
	@EventType13Id UNIQUEIDENTIFIER = NEWID();
DECLARE
	@EventType14Id UNIQUEIDENTIFIER = NEWID();
DECLARE
	@EventType15Id UNIQUEIDENTIFIER = NEWID();
DECLARE
	@EventType16Id UNIQUEIDENTIFIER = NEWID();
DECLARE
	@EventType17Id UNIQUEIDENTIFIER = NEWID();
DECLARE
	@EventType18Id UNIQUEIDENTIFIER = NEWID();
DECLARE
	@EventType19Id UNIQUEIDENTIFIER = NEWID();
DECLARE
	@EventType20Id UNIQUEIDENTIFIER = NEWID();
DECLARE
	@EventType21Id UNIQUEIDENTIFIER = NEWID();
DECLARE
	@EventType22Id UNIQUEIDENTIFIER = NEWID();
DECLARE
	@EventType23Id UNIQUEIDENTIFIER = NEWID();
DECLARE
	@EventType24Id UNIQUEIDENTIFIER = NEWID();
DECLARE
	@EventType25Id UNIQUEIDENTIFIER = NEWID();

INSERT INTO EventTypes (Id, CompanyId, Name, Description, Duration, Price, MaxParticipants, MinStaff)
VALUES
	-- SportFit Centrum
	(@EventType1Id, @Branch1Id, 'Joga dla początkujących', 'Zajęcia jogi dla osób początkujących', 60, 50.00, 15, 1),
	(@EventType2Id, @Branch1Id, 'Trening personalny', 'Indywidualne sesje z trenerem', 45, 120.00, 1, 1),
	(@EventType3Id, @Branch1Id, 'Pilates grupowy', 'Zajęcia pilates w małej grupie', 50, 45.00, 12, 1),

	-- SportFit Południe
	(@EventType4Id, @Branch2Id, 'Crossfit grupowy', 'Intensywny trening crossfit w grupie', 60, 60.00, 12, 1),
	(@EventType5Id, @Branch2Id, 'Spinning', 'Zajęcia na rowerach stacjonarnych', 45, 45.00, 20, 1),
	(@EventType6Id, @Branch2Id, 'Zumba party', 'Energiczne zajęcia taneczne', 55, 40.00, 25, 1),

	-- SportFit Północ
	(@EventType7Id, @Branch3Id, 'Nauka pływania', 'Lekcje pływania dla różnych poziomów', 60, 80.00, 8, 1),
	(@EventType8Id, @Branch3Id, 'Aqua aerobik', 'Ćwiczenia aerobowe w wodzie', 45, 55.00, 15, 1),

	-- SportFit Wschód
	(@EventType9Id, @Branch4Id, 'Trening bokserski', 'Podstawy boksu i trening kondycyjny', 60, 70.00, 10, 1),
	(@EventType10Id, @Branch4Id, 'Kickboxing', 'Sztuki walki z elementami cardio', 55, 65.00, 12, 1),

	-- SportFit Zachód
	(@EventType11Id, @Branch5Id, 'TRX Functional', 'Trening funkcjonalny z linami TRX', 50, 55.00, 14, 1),
	(@EventType12Id, @Branch5Id, 'Stretching & Relax', 'Zajęcia rozciągające i relaksacyjne', 45, 35.00, 20, 1),

	-- FitZone Mokotów
	(@EventType13Id, @FitZone1Id, 'HIIT Power', 'Trening interwałowy wysokiej intensywności', 45, 60.00, 16, 1),
	(@EventType14Id, @FitZone1Id, 'Body Pump', 'Zajęcia z ciężarkami do muzyki', 55, 50.00, 18, 1),

	-- FitZone Katowice
	(@EventType15Id, @FitZone2Id, 'Tabata Express', 'Krótkie, intensywne treningi', 30, 40.00, 20, 1),

	-- FitZone Poznań
	(@EventType16Id, @FitZone3Id, 'Functional Training', 'Trening funkcjonalny całego ciała', 60, 55.00, 14, 1),

	-- AquaFit Marina
	(@EventType17Id, @AquaFit1Id, 'Pływanie sportowe', 'Zaawansowane techniki pływackie', 60, 90.00, 6, 1),
	(@EventType18Id, @AquaFit1Id, 'Trening personalny pływanie', 'Indywidualne lekcje pływania', 45, 150.00, 1, 1),

	-- AquaFit Sopot
	(@EventType19Id, @AquaFit2Id, 'Aqua fitness', 'Kompleksowy trening w wodzie', 50, 60.00, 12, 1),
	(@EventType20Id, @AquaFit2Id, 'Aqua jogging', 'Bieganie w wodzie', 40, 45.00, 15, 1),

	-- PowerGym Center
	(@EventType21Id, @PowerGym1Id, 'Powerlifting', 'Trening siłowy - martwy ciąg, przysiad, wyciskanie', 90, 80.00, 8,
	 1),
	(@EventType22Id, @PowerGym1Id, 'Strongman training', 'Trening siłaczy', 75, 75.00, 10, 1),

	-- PowerGym Bydgoszcz
	(@EventType23Id, @PowerGym2Id, 'Bodybuilding', 'Trening na masę mięśniową', 75, 70.00, 12, 1),

	-- FlexYoga Wilanów
	(@EventType24Id, @FlexYoga1Id, 'Hatha Yoga', 'Klasyczna joga z naciskiem na pozycje', 75, 60.00, 16, 1),
	(@EventType25Id, @FlexYoga1Id, 'Yoga Nidra', 'Joga relaksacyjna i medytacyjna', 60, 55.00, 20, 1);

-- =============================================
-- 9. EventSchedules - dodanie harmonogramu wydarzeń 
-- =============================================

DECLARE
	@Event1Id UNIQUEIDENTIFIER = NEWID();
DECLARE
	@Event2Id UNIQUEIDENTIFIER = NEWID();
DECLARE
	@Event3Id UNIQUEIDENTIFIER = NEWID();
DECLARE
	@Event4Id UNIQUEIDENTIFIER = NEWID();
DECLARE
	@Event5Id UNIQUEIDENTIFIER = NEWID();
DECLARE
	@Event6Id UNIQUEIDENTIFIER = NEWID();
DECLARE
	@Event7Id UNIQUEIDENTIFIER = NEWID();
DECLARE
	@Event8Id UNIQUEIDENTIFIER = NEWID();
DECLARE
	@Event9Id UNIQUEIDENTIFIER = NEWID();
DECLARE
	@Event10Id UNIQUEIDENTIFIER = NEWID();
DECLARE
	@Event11Id UNIQUEIDENTIFIER = NEWID();
DECLARE
	@Event12Id UNIQUEIDENTIFIER = NEWID();
DECLARE
	@Event13Id UNIQUEIDENTIFIER = NEWID();
DECLARE
	@Event14Id UNIQUEIDENTIFIER = NEWID();
DECLARE
	@Event15Id UNIQUEIDENTIFIER = NEWID();
DECLARE
	@Event16Id UNIQUEIDENTIFIER = NEWID();
DECLARE
	@Event17Id UNIQUEIDENTIFIER = NEWID();
DECLARE
	@Event18Id UNIQUEIDENTIFIER = NEWID();
DECLARE
	@Event19Id UNIQUEIDENTIFIER = NEWID();
DECLARE
	@Event20Id UNIQUEIDENTIFIER = NEWID();
DECLARE
	@Event21Id UNIQUEIDENTIFIER = NEWID();
DECLARE
	@Event22Id UNIQUEIDENTIFIER = NEWID();
DECLARE
	@Event23Id UNIQUEIDENTIFIER = NEWID();
DECLARE
	@Event24Id UNIQUEIDENTIFIER = NEWID();
DECLARE
	@Event25Id UNIQUEIDENTIFIER = NEWID();
DECLARE
	@Event26Id UNIQUEIDENTIFIER = NEWID();
DECLARE
	@Event27Id UNIQUEIDENTIFIER = NEWID();
DECLARE
	@Event28Id UNIQUEIDENTIFIER = NEWID();
DECLARE
	@Event29Id UNIQUEIDENTIFIER = NEWID();
DECLARE
	@Event30Id UNIQUEIDENTIFIER = NEWID();
DECLARE
	@Event31Id UNIQUEIDENTIFIER = NEWID();
DECLARE
	@Event32Id UNIQUEIDENTIFIER = NEWID();
DECLARE
	@Event33Id UNIQUEIDENTIFIER = NEWID();
DECLARE
	@Event34Id UNIQUEIDENTIFIER = NEWID();
DECLARE
	@Event35Id UNIQUEIDENTIFIER = NEWID();

INSERT INTO EventSchedules (Id, CompanyId, EventTypeId, PlaceName, StartTime, Status)
VALUES
	-- Wydarzenia SportFit Centrum
	(@Event1Id, @Branch1Id, @EventType1Id, 'Sala Fitness 1', '2025-07-28 10:00:00', 'Active'),
	(@Event2Id, @Branch1Id, @EventType2Id, 'Sala Treningowa 2', '2025-07-29 14:00:00', 'Active'),
	(@Event3Id, @Branch1Id, @EventType3Id, 'Sala Pilates', '2025-07-30 18:00:00', 'Active'),
	(@Event4Id, @Branch1Id, @EventType1Id, 'Sala Fitness 1', '2025-07-31 09:00:00', 'Active'),
	(@Event5Id, @Branch1Id, @EventType2Id, 'Sala Treningowa 2', '2025-08-01 15:00:00', 'Active'),

	-- Wydarzenia SportFit Południe
	(@Event6Id, @Branch2Id, @EventType4Id, 'Sala Crossfit', '2025-07-28 18:00:00', 'Active'),
	(@Event7Id, @Branch2Id, @EventType5Id, 'Sala Spinning', '2025-07-29 11:00:00', 'Active'),
	(@Event8Id, @Branch2Id, @EventType6Id, 'Sala Taneczna', '2025-07-30 19:00:00', 'Active'),
	(@Event9Id, @Branch2Id, @EventType4Id, 'Sala Crossfit', '2025-07-31 17:00:00', 'Active'),
	(@Event10Id, @Branch2Id, @EventType5Id, 'Sala Spinning', '2025-08-01 12:00:00', 'Active'),

	-- Wydarzenia SportFit Północ
	(@Event11Id, @Branch3Id, @EventType7Id, 'Basen - tor 1-2', '2025-07-28 09:00:00', 'Active'),
	(@Event12Id, @Branch3Id, @EventType8Id, 'Basen - cały', '2025-07-29 16:00:00', 'Active'),
	(@Event13Id, @Branch3Id, @EventType7Id, 'Basen - tor 3-4', '2025-07-30 10:00:00', 'Active'),
	(@Event14Id, @Branch3Id, @EventType8Id, 'Basen - cały', '2025-07-31 17:00:00', 'Active'),
	(@Event15Id, @Branch3Id, @EventType7Id, 'Basen - tor 1-2', '2025-08-01 08:00:00', 'Active'),

	-- Wydarzenia SportFit Wschód
	(@Event16Id, @Branch4Id, @EventType9Id, 'Sala Bokserska', '2025-07-28 18:00:00', 'Active'),
	(@Event17Id, @Branch4Id, @EventType10Id, 'Sala Kickboxing', '2025-07-29 19:00:00', 'Active'),
	(@Event18Id, @Branch4Id, @EventType9Id, 'Sala Bokserska', '2025-07-30 17:00:00', 'Active'),

	-- Wydarzenia SportFit Zachód
	(@Event19Id, @Branch5Id, @EventType11Id, 'Sala TRX', '2025-07-28 17:00:00', 'Active'),
	(@Event20Id, @Branch5Id, @EventType12Id, 'Sala Relaks', '2025-07-29 20:00:00', 'Active'),
	(@Event21Id, @Branch5Id, @EventType11Id, 'Sala TRX', '2025-07-30 16:00:00', 'Active'),

	-- Wydarzenia FitZone Mokotów
	(@Event22Id, @FitZone1Id, @EventType13Id, 'Sala HIIT', '2025-07-28 07:00:00', 'Active'),
	(@Event23Id, @FitZone1Id, @EventType14Id, 'Sala Body Pump', '2025-07-29 19:00:00', 'Active'),
	(@Event24Id, @FitZone1Id, @EventType13Id, 'Sala HIIT', '2025-07-30 08:00:00', 'Active'),
	(@Event25Id, @FitZone2Id, @EventType15Id, 'Sala Express', '2025-07-28 12:00:00', 'Active'),
	(@Event26Id, @FitZone2Id, @EventType15Id, 'Sala Express', '2025-07-29 18:00:00', 'Active'),

	-- Wydarzenia FitZone Poznań
	(@Event27Id, @FitZone3Id, @EventType16Id, 'Sala Funkcjonalna', '2025-07-28 16:00:00', 'Active'),
	(@Event28Id, @FitZone3Id, @EventType16Id, 'Sala Funkcjonalna', '2025-07-30 17:00:00', 'Active'),

	-- Wydarzenia AquaFit Marina
	(@Event29Id, @AquaFit1Id, @EventType17Id, 'Basen olimpijski - tor 1-3', '2025-07-28 07:00:00', 'Active'),
	(@Event30Id, @AquaFit1Id, @EventType18Id, 'Basen olimpijski - tor 4', '2025-07-29 15:00:00', 'Active'),
	(@Event31Id, @AquaFit1Id, @EventType17Id, 'Basen olimpijski - tor 1-3', '2025-07-30 08:00:00', 'Active'),

	-- Wydarzenia AquaFit Sopot
	(@Event32Id, @AquaFit2Id, @EventType19Id, 'Basen rekreacyjny', '2025-07-28 16:00:00', 'Active'),
	(@Event33Id, @AquaFit2Id, @EventType20Id, 'Basen sportowy', '2025-07-29 17:00:00', 'Active'),

	-- Wydarzenia PowerGym Center
	(@Event34Id, @PowerGym1Id, @EventType21Id, 'Sala Powerlifting', '2025-07-28 06:00:00', 'Active'),
	(@Event35Id, @PowerGym1Id, @EventType22Id, 'Sala Strongman', '2025-07-29 20:00:00', 'Active');

-- =============================================
-- 10. EventScheduleStaff - przypisanie personelu do wydarzeń 
-- =============================================

INSERT INTO EventScheduleStaff (Id, CompanyId, EventScheduleId, StaffMemberId)
VALUES
	-- Przypisanie trenerów SportFit Centrum
	(NEWID(), @Branch1Id, @Event1Id, @Trainer1Id),
	(NEWID(), @Branch1Id, @Event2Id, @Trainer2Id),
	(NEWID(), @Branch1Id, @Event3Id, @Trainer2Id),
	(NEWID(), @Branch1Id, @Event4Id, @Trainer1Id),
	(NEWID(), @Branch1Id, @Event5Id, @Trainer2Id),

	-- Przypisanie trenerów SportFit Południe
	(NEWID(), @Branch2Id, @Event6Id, @Trainer3Id),
	(NEWID(), @Branch2Id, @Event7Id, @Trainer4Id),
	(NEWID(), @Branch2Id, @Event8Id, @Trainer3Id),
	(NEWID(), @Branch2Id, @Event9Id, @Trainer3Id),
	(NEWID(), @Branch2Id, @Event10Id, @Trainer4Id),

	-- Przypisanie trenerów SportFit Północ
	(NEWID(), @Branch3Id, @Event11Id, @Trainer5Id),
	(NEWID(), @Branch3Id, @Event12Id, @Trainer5Id),
	(NEWID(), @Branch3Id, @Event13Id, @Trainer6Id),
	(NEWID(), @Branch3Id, @Event14Id, @Trainer5Id),
	(NEWID(), @Branch3Id, @Event15Id, @Trainer6Id),

	-- Przypisanie trenerów SportFit Wschód
	(NEWID(), @Branch4Id, @Event16Id, @Trainer7Id),
	(NEWID(), @Branch4Id, @Event17Id, @Trainer7Id),
	(NEWID(), @Branch4Id, @Event18Id, @Trainer7Id),

	-- Przypisanie trenerów SportFit Zachód
	(NEWID(), @Branch5Id, @Event19Id, @Trainer8Id),
	(NEWID(), @Branch5Id, @Event20Id, @Trainer8Id),
	(NEWID(), @Branch5Id, @Event21Id, @Trainer8Id),

	-- Przypisanie trenerów FitZone
	(NEWID(), @FitZone1Id, @Event22Id, @Trainer9Id),
	(NEWID(), @FitZone1Id, @Event23Id, @Trainer10Id),
	(NEWID(), @FitZone1Id, @Event24Id, @Trainer9Id),
	(NEWID(), @FitZone2Id, @Event25Id, @Trainer11Id),
	(NEWID(), @FitZone2Id, @Event26Id, @Trainer11Id),
	(NEWID(), @FitZone3Id, @Event27Id, @Trainer12Id),
	(NEWID(), @FitZone3Id, @Event28Id, @Trainer12Id),

	-- Przypisanie trenerów AquaFit
	(NEWID(), @AquaFit1Id, @Event29Id, @Trainer13Id),
	(NEWID(), @AquaFit1Id, @Event30Id, @Trainer14Id),
	(NEWID(), @AquaFit1Id, @Event31Id, @Trainer13Id),
	(NEWID(), @AquaFit2Id, @Event32Id, @Trainer15Id),
	(NEWID(), @AquaFit2Id, @Event33Id, @Trainer15Id),

	-- Przypisanie trenerów PowerGym
	(NEWID(), @PowerGym1Id, @Event34Id, @Trainer16Id),
	(NEWID(), @PowerGym1Id, @Event35Id, @Trainer17Id);

-- =============================================
-- 11. Reservations - dodanie rezerwacji 
-- =============================================

DECLARE
	@Reservation1Id UNIQUEIDENTIFIER = NEWID();
DECLARE
	@Reservation2Id UNIQUEIDENTIFIER = NEWID();
DECLARE
	@Reservation3Id UNIQUEIDENTIFIER = NEWID();
DECLARE
	@Reservation4Id UNIQUEIDENTIFIER = NEWID();
DECLARE
	@Reservation5Id UNIQUEIDENTIFIER = NEWID();
DECLARE
	@Reservation6Id UNIQUEIDENTIFIER = NEWID();
DECLARE
	@Reservation7Id UNIQUEIDENTIFIER = NEWID();
DECLARE
	@Reservation8Id UNIQUEIDENTIFIER = NEWID();
DECLARE
	@Reservation9Id UNIQUEIDENTIFIER = NEWID();
DECLARE
	@Reservation10Id UNIQUEIDENTIFIER = NEWID();
DECLARE
	@Reservation11Id UNIQUEIDENTIFIER = NEWID();
DECLARE
	@Reservation12Id UNIQUEIDENTIFIER = NEWID();
DECLARE
	@Reservation13Id UNIQUEIDENTIFIER = NEWID();
DECLARE
	@Reservation14Id UNIQUEIDENTIFIER = NEWID();
DECLARE
	@Reservation15Id UNIQUEIDENTIFIER = NEWID();
DECLARE
	@Reservation16Id UNIQUEIDENTIFIER = NEWID();
DECLARE
	@Reservation17Id UNIQUEIDENTIFIER = NEWID();
DECLARE
	@Reservation18Id UNIQUEIDENTIFIER = NEWID();
DECLARE
	@Reservation19Id UNIQUEIDENTIFIER = NEWID();
DECLARE
	@Reservation20Id UNIQUEIDENTIFIER = NEWID();
DECLARE
	@Reservation21Id UNIQUEIDENTIFIER = NEWID();
DECLARE
	@Reservation22Id UNIQUEIDENTIFIER = NEWID();
DECLARE
	@Reservation23Id UNIQUEIDENTIFIER = NEWID();
DECLARE
	@Reservation24Id UNIQUEIDENTIFIER = NEWID();
DECLARE
	@Reservation25Id UNIQUEIDENTIFIER = NEWID();

-- Dodanie rezerwacji (bez ParticipantId i ParticipantCount)
INSERT INTO Reservations (Id, CompanyId, EventScheduleId, Status, Notes, CreatedAt, CancelledAt, IsPaid, PaidAt)
VALUES
	-- Rezerwacje SportFit Centrum
	(@Reservation1Id, @Branch1Id, @Event1Id, 'Confirmed', 'Pierwszy raz na zajęciach jogi',
	 '2025-07-25 14:00:00', NULL, 1, '2025-07-25 14:30:00'),
	(@Reservation2Id, @Branch1Id, @Event2Id, 'Confirmed',
	 'Trening personalny - cel: budowa masy mięśniowej', '2025-07-26 08:45:00', NULL, 1, '2025-07-26 09:15:00'),
	(@Reservation3Id, @Branch1Id, @Event3Id, 'Confirmed',
	 'Zajęcia pilates - problemy z kręgosłupem', '2025-07-27 16:15:00', NULL, 1, '2025-07-27 16:45:00'),
	(@Reservation4Id, @Branch1Id, @Event4Id, 'Confirmed', 'Kontynuacja zajęć jogi',
	 '2025-07-28 07:50:00', NULL, 1, '2025-07-28 08:20:00'),

	-- Rezerwacje SportFit Południe
	(@Reservation5Id, @Branch2Id, @Event6Id, 'Confirmed', 'Doświadczony w crossfit',
	 '2025-07-25 11:15:00', NULL, 1, '2025-07-25 11:45:00'),
	(@Reservation6Id, @Branch2Id, @Event7Id, 'Confirmed', 'Lubię spinning - świetna muzyka',
	 '2025-07-26 13:00:00', NULL, 1, '2025-07-26 13:30:00'),
	(@Reservation7Id, @Branch2Id, @Event8Id, 'Confirmed', 'Pierwszy raz na zumbie',
	 '2025-07-27 17:45:00', NULL, 1, '2025-07-27 18:15:00'),
	(@Reservation8Id, @Branch2Id, @Event9Id, 'Confirmed', 'Oczekiwanie na potwierdzenie',
	 '2025-07-28 09:30:00', NULL, 0, NULL),

	-- Rezerwacje SportFit Północ
	(@Reservation9Id, @Branch3Id, @Event11Id, 'Confirmed', 'Początkujący - nauka podstaw pływania',
	 '2025-07-25 15:50:00', NULL, 1, '2025-07-25 16:20:00'),
	(@Reservation10Id, @Branch3Id, @Event12Id, 'Confirmed',
	 'Aqua aerobik - rehabilitacja po kontuzji', '2025-07-26 10:15:00', NULL, 1, '2025-07-26 10:45:00'),
	(@Reservation11Id, @Branch3Id, @Event13Id, 'Cancelled', 'Zmiana planów - choroba',
	 '2025-07-27 14:30:00', '2025-07-27 15:45:00', 0, NULL),
	(@Reservation12Id, @Branch3Id, @Event15Id, 'Confirmed', 'Kontynuacja nauki pływania',
	 '2025-07-28 07:00:00', NULL, 1, '2025-07-28 07:30:00'),

	-- Rezerwacje SportFit Wschód
	(@Reservation13Id, @Branch4Id, @Event16Id, 'Confirmed', 'Chcę nauczyć się boksu',
	 '2025-07-26 15:15:00', NULL, 1, '2025-07-26 15:45:00'),
	(@Reservation14Id, @Branch4Id, @Event17Id, 'Confirmed', 'Kickboxing na spalanie kalorii',
	 '2025-07-27 16:50:00', NULL, 1, '2025-07-27 17:20:00'),

	-- Rezerwacje SportFit Zachód
	(@Reservation15Id, @Branch5Id, @Event19Id, 'Confirmed', 'TRX - trening funkcjonalny',
	 '2025-07-26 13:40:00', NULL, 1, '2025-07-26 14:10:00'),
	(@Reservation16Id, @Branch5Id, @Event20Id, 'Confirmed', 'Potrzebuję relaksu po pracy',
	 '2025-07-27 19:00:00', NULL, 1, '2025-07-27 19:30:00'),

	-- Rezerwacje FitZone Mokotów
	(@Reservation17Id, @FitZone1Id, @Event22Id, 'Confirmed', 'HIIT - chcę szybko spalić kalorie',
	 '2025-07-25 19:45:00', NULL, 1, '2025-07-25 20:15:00'),
	(@Reservation18Id, @FitZone1Id, @Event23Id, 'Confirmed', 'Body Pump - budowanie siły',
	 '2025-07-27 16:15:00', NULL, 1, '2025-07-27 16:45:00'),
	(@Reservation19Id, @FitZone1Id, @Event24Id, 'Confirmed', 'Kolejne HIIT - jestem uzależniona!',
	 '2025-07-28 06:00:00', NULL, 1, '2025-07-28 06:30:00'),

	-- Rezerwacje FitZone Katowice
	(@Reservation20Id, @FitZone2Id, @Event25Id, 'Confirmed', 'Tabata - krótko i intensywnie',
	 '2025-07-26 10:50:00', NULL, 1, '2025-07-26 11:20:00'),
	(@Reservation21Id, @FitZone2Id, @Event26Id, 'Confirmed', 'Wieczorna sesja Tabata',
	 '2025-07-27 17:10:00', NULL, 1, '2025-07-27 17:40:00'),

	-- Rezerwacje AquaFit Marina
	(@Reservation22Id, @AquaFit1Id, @Event29Id, 'Confirmed',
	 'Pływanie sportowe - przygotowanie do zawodów', '2025-07-26 18:00:00', NULL, 1, '2025-07-26 18:30:00'),
	(@Reservation23Id, @AquaFit1Id, @Event30Id, 'Confirmed', 'Trening personalny - technika kraul',
	 '2025-07-27 11:45:00', NULL, 1, '2025-07-27 12:15:00'),

	-- Rezerwacje PowerGym Center
	(@Reservation24Id, @PowerGym1Id, @Event34Id, 'Confirmed', 'Powerlifting - chcę bić rekordy',
	 '2025-07-26 05:15:00', NULL, 1, '2025-07-26 05:45:00'),
	(@Reservation25Id, @PowerGym1Id, @Event35Id, 'Confirmed', 'Strongman - siłacz w sobie',
	 '2025-07-27 19:20:00', NULL, 1, '2025-07-27 19:50:00');

-- =============================================
-- 12. ReservationParticipants - przypisanie uczestników do rezerwacji
-- =============================================

INSERT INTO ReservationParticipants (CompanyId, ReservationId, ParticipantId)
VALUES
	-- Uczestnicy SportFit Centrum
	(@Branch1Id, @Reservation1Id, @Participant1Id),
	(@Branch1Id, @Reservation2Id, @Participant2Id),
	(@Branch1Id, @Reservation3Id, @Participant3Id),
	(@Branch1Id, @Reservation4Id, @Participant4Id),

	-- Uczestnicy SportFit Południe
	(@Branch2Id, @Reservation5Id, @Participant5Id),
	(@Branch2Id, @Reservation6Id, @Participant6Id),
	(@Branch2Id, @Reservation7Id, @Participant7Id),
	(@Branch2Id, @Reservation8Id, @Participant8Id),

	-- Uczestnicy SportFit Północ
	(@Branch3Id, @Reservation9Id, @Participant9Id),
	(@Branch3Id, @Reservation10Id, @Participant10Id),
	(@Branch3Id, @Reservation11Id, @Participant11Id),
	(@Branch3Id, @Reservation12Id, @Participant9Id), -- Ten sam uczestnik w kolejnej rezerwacji

	-- Uczestnicy SportFit Wschód
	(@Branch4Id, @Reservation13Id, @Participant12Id),
	(@Branch4Id, @Reservation14Id, @Participant13Id),

	-- Uczestnicy SportFit Zachód
	(@Branch5Id, @Reservation15Id, @Participant14Id),
	(@Branch5Id, @Reservation16Id, @Participant15Id),

	-- Uczestnicy FitZone Mokotów
	(@FitZone1Id, @Reservation17Id, @Participant16Id),
	(@FitZone1Id, @Reservation18Id, @Participant17Id),
	(@FitZone1Id, @Reservation19Id, @Participant18Id),

	-- Uczestnicy FitZone Katowice
	(@FitZone2Id, @Reservation20Id, @Participant19Id),
	(@FitZone2Id, @Reservation21Id, @Participant20Id),

	-- Uczestnicy AquaFit Marina
	(@AquaFit1Id, @Reservation22Id, @Participant23Id),
	(@AquaFit1Id, @Reservation23Id, @Participant24Id),

	-- Uczestnicy PowerGym Center
	(@PowerGym1Id, @Reservation24Id, @Participant27Id),
	(@PowerGym1Id, @Reservation25Id, @Participant28Id);

-- =============================================
-- PRZYKŁAD: Rezerwacja grupowa z wieloma uczestnikami
-- =============================================

-- Dodatkowa rezerwacja grupowa dla demonstracji
DECLARE @GroupReservationId UNIQUEIDENTIFIER = NEWID();

INSERT INTO Reservations (Id, CompanyId, EventScheduleId, Status, Notes, CreatedAt, CancelledAt, IsPaid, PaidAt)
VALUES (@GroupReservationId, @Branch1Id, @Event1Id, 'Confirmed', 'Rezerwacja grupowa - zajęcia jogi dla przyjaciół',
        '2025-07-29 10:00:00', NULL, 1, '2025-07-29 10:30:00');

-- Dodanie wielu uczestników do jednej rezerwacji
INSERT INTO ReservationParticipants (CompanyId, ReservationId, ParticipantId)
VALUES (@Branch1Id, @GroupReservationId, @Participant1Id), -- Anna Kowalska
       (@Branch1Id, @GroupReservationId, @Participant2Id), -- Jan Nowak
       (@Branch1Id, @GroupReservationId, @Participant3Id);
-- Maria Wiśniewska

-- =============================================
-- 12. Notifications - dodanie powiadomień 
-- =============================================

INSERT INTO Notifications (Id, CompanyId, ReservationId, EmailStatus, SmsStatus, EmailSentAt, SmsSentAt, EmailContent,
                           SmsContent)
VALUES
	-- Powiadomienia SportFit Centrum
	(NEWID(), @Branch1Id, @Reservation1Id, 'Sent', 'Sent', '2025-07-25 14:35:00', '2025-07-25 14:35:00',
	 'Potwierdzenie rezerwacji na zajęcia jogi dla początkujących w dniu 28.07.2025 o godz. 10:00. Dziękujemy za wybór SportFit!',
	 'SportFit: Joga 28.07.2025, 10:00 - Sala Fitness 1'),
	(NEWID(), @Branch1Id, @Reservation2Id, 'Sent', 'Sent', '2025-07-26 09:20:00', '2025-07-26 09:20:00',
	 'Potwierdzenie rezerwacji na trening personalny w dniu 29.07.2025 o godz. 14:00. Trener: Ewa Jabłońska.',
	 'SportFit: Trening personalny 29.07.2025, 14:00 - Ewa Jabłońska'),
	(NEWID(), @Branch1Id, @Reservation3Id, 'Sent', 'Sent', '2025-07-27 16:50:00', '2025-07-27 16:50:00',
	 'Potwierdzenie rezerwacji na pilates grupowy w dniu 30.07.2025 o godz. 18:00. Sala Pilates.',
	 'SportFit: Pilates 30.07.2025, 18:00 - Sala Pilates'),

	-- Powiadomienia SportFit Południe
	(NEWID(), @Branch2Id, @Reservation5Id, 'Sent', 'Sent', '2025-07-25 11:50:00', '2025-07-25 11:50:00',
	 'Potwierdzenie rezerwacji na crossfit grupowy w dniu 28.07.2025 o godz. 18:00. Przygotuj się na intensywny trening!',
	 'SportFit: Crossfit 28.07.2025, 18:00 - Sala Crossfit'),
	(NEWID(), @Branch2Id, @Reservation6Id, 'Sent', 'Sent', '2025-07-26 13:35:00', '2025-07-26 13:35:00',
	 'Potwierdzenie rezerwacji na spinning w dniu 29.07.2025 o godz. 11:00. Przynieś ręcznik i butelkę wody.',
	 'SportFit: Spinning 29.07.2025, 11:00 - Sala Spinning'),
	(NEWID(), @Branch2Id, @Reservation7Id, 'Sent', 'Sent', '2025-07-27 18:20:00', '2025-07-27 18:20:00',
	 'Potwierdzenie rezerwacji na zumba party w dniu 30.07.2025 o godz. 19:00. Tańcz i baw się!',
	 'SportFit: Zumba 30.07.2025, 19:00 - Sala Taneczna'),

	-- Powiadomienia SportFit Północ
	(NEWID(), @Branch3Id, @Reservation9Id, 'Sent', 'Sent', '2025-07-25 16:25:00', '2025-07-25 16:25:00',
	 'Potwierdzenie rezerwacji na naukę pływania w dniu 28.07.2025 o godz. 09:00. Basen - tor 1-2.',
	 'SportFit: Pływanie 28.07.2025, 09:00 - Basen tor 1-2'),
	(NEWID(), @Branch3Id, @Reservation10Id, 'Sent', 'Sent', '2025-07-26 10:50:00', '2025-07-26 10:50:00',
	 'Potwierdzenie rezerwacji na aqua aerobik w dniu 29.07.2025 o godz. 16:00. Trener: Henryk Zieliński.',
	 'SportFit: Aqua aerobik 29.07.2025, 16:00 - Henryk Zieliński'),
	(NEWID(), @Branch3Id, @Reservation11Id, 'Sent', 'Sent', '2025-07-28 09:00:00', '2025-07-28 09:00:00',
	 'Potwierdzenie anulowania rezerwacji na naukę pływania w dniu 30.07.2025 o godz. 10:00. Życzymy szybkiego powrotu do zdrowia!',
	 'SportFit: Anulowanie - Pływanie 30.07.2025, 10:00'),

	-- Powiadomienia FitZone
	(NEWID(), @FitZone1Id, @Reservation17Id, 'Sent', 'Sent', '2025-07-25 20:20:00', '2025-07-25 20:20:00',
	 'Potwierdzenie rezerwacji na HIIT Power w dniu 28.07.2025 o godz. 07:00. FitZone Mokotów - Sala HIIT.',
	 'FitZone: HIIT 28.07.2025, 07:00 - Sala HIIT'),
	(NEWID(), @FitZone1Id, @Reservation18Id, 'Sent', 'Sent', '2025-07-27 16:50:00', '2025-07-27 16:50:00',
	 'Potwierdzenie rezerwacji na Body Pump w dniu 29.07.2025 o godz. 19:00. Trener: Marta Pawlak.',
	 'FitZone: Body Pump 29.07.2025, 19:00 - Marta Pawlak'),

	-- Powiadomienia AquaFit
	(NEWID(), @AquaFit1Id, @Reservation22Id, 'Sent', 'Sent', '2025-07-26 18:35:00', '2025-07-26 18:35:00',
	 'Potwierdzenie rezerwacji na pływanie sportowe w dniu 28.07.2025 o godz. 07:00. AquaFit Marina - tor 1-3.',
	 'AquaFit: Pływanie sportowe 28.07.2025, 07:00'),
	(NEWID(), @AquaFit1Id, @Reservation23Id, 'Sent', 'Sent', '2025-07-27 12:20:00', '2025-07-27 12:20:00',
	 'Potwierdzenie rezerwacji na trening personalny pływanie w dniu 29.07.2025 o godz. 15:00. Trener: Renata Rutkowska.',
	 'AquaFit: Trening personalny 29.07.2025, 15:00'),

	-- Powiadomienia PowerGym
	(NEWID(), @PowerGym1Id, @Reservation24Id, 'Sent', 'Sent', '2025-07-26 05:50:00', '2025-07-26 05:50:00',
	 'Potwierdzenie rezerwacji na powerlifting w dniu 28.07.2025 o godz. 06:00. PowerGym Center - Sala Powerlifting.',
	 'PowerGym: Powerlifting 28.07.2025, 06:00'),
	(NEWID(), @PowerGym1Id, @Reservation25Id, 'Sent', 'Sent', '2025-07-27 19:55:00', '2025-07-27 19:55:00',
	 'Potwierdzenie rezerwacji na strongman training w dniu 29.07.2025 o godz. 20:00. Trener: Urszula Urbańska.',
	 'PowerGym: Strongman 29.07.2025, 20:00 - Urszula Urbańska');

-- =============================================
-- 13. Messages - dodanie wiadomości 
-- =============================================

INSERT INTO Messages (Id, CompanyId, SenderId, ReceiverId, Content)
VALUES
	-- Wiadomości SportFit
	(NEWID(), @Branch1Id, @Manager1Id, @Trainer1Id,
	 'Proszę o przygotowanie planu zajęć jogi na sierpień. Zwiększone zainteresowanie!'),
	(NEWID(), @Branch1Id, @Trainer1Id, @Manager1Id,
	 'Plan zajęć jogi na sierpień będzie gotowy do piątku. Dodaję 2 dodatkowe grupy.'),
	(NEWID(), @Branch1Id, @StaffRec1Id, @Manager1Id, 'Recepcja raportuje: 15 nowych członków w tym tygodniu!'),
	(NEWID(), @Branch1Id, @Manager1Id, @Trainer2Id,
	 'Świetna opinia o Twoich treningach personalnych. Kontynuuj dobrą pracę!'),
	(NEWID(), @Branch1Id, @Trainer2Id, @Manager1Id,
	 'Dziękuję za uznanie. Czy mogę poprowadzić warsztaty pilates w weekend?'),

	(NEWID(), @Branch2Id, @Manager2Id, @Trainer3Id, 'Czy możesz poprowadzić dodatkowe zajęcia crossfit w sobotę?'),
	(NEWID(), @Branch2Id, @Trainer3Id, @Manager2Id,
	 'Tak, mogę poprowadzić dodatkowe zajęcia crossfit w sobotę o 16:00'),
	(NEWID(), @Branch2Id, @StaffRec2Id, @Manager2Id,
	 'Sala spinning wymaga serwisu rowerów - zgłoszenia od uczestników'),
	(NEWID(), @Branch2Id, @Manager2Id, @StaffRec2Id, 'Dzięki za info. Zamawiam serwis na jutro rano przed zajęciami.'),

	(NEWID(), @Branch3Id, @StaffRec3Id, @Manager3Id,
	 'Komplet zapisów na zajęcia pływania w przyszłym tygodniu. Rozważamy dodatkową grupę?'),
	(NEWID(), @Branch3Id, @Manager3Id, @Trainer5Id,
	 'Henryk, czy mógłbyś poprowadzić dodatkową grupę pływania w czwartek?'),
	(NEWID(), @Branch3Id, @Trainer5Id, @Manager3Id, 'Oczywiście! Czwartek 18:00 będzie idealny dla dodatkowej grupy.'),

	-- Wiadomości FitZone
	(NEWID(), @FitZone1Id, @Manager6Id, @Trainer9Id,
	 'Świetny feedback na zajęcia HIIT! Rozważamy zwiększenie częstotliwości.'),
	(NEWID(), @FitZone1Id, @Trainer9Id, @Manager6Id, 'Cieszę się! Mogę dodać sesje HIIT w środy i piątki.'),
	(NEWID(), @FitZone1Id, @StaffRec6Id, @Manager6Id,
	 'Nowy członek pyta o zajęcia dla seniorów. Czy planujemy taką grupę?'),
	(NEWID(), @FitZone1Id, @Manager6Id, @StaffRec6Id, 'Dobry pomysł! Porozmawiam z trenerami o programie 50+'),

	(NEWID(), @FitZone2Id, @Manager7Id, @Trainer11Id, 'Tabata cieszy się ogromną popularnością! Brawo!'),
	(NEWID(), @FitZone2Id, @Trainer11Id, @Manager7Id,
	 'Dziękuję! Uczestnicy są bardzo zmotywowani. Może warsztaty weekendowe?'),

	-- Wiadomości AquaFit
	(NEWID(), @AquaFit1Id, @Manager8Id, @Trainer13Id, 'Zawodnik z naszych zajęć wygrał regionalne zawody! Gratulacje!'),
	(NEWID(), @AquaFit1Id, @Trainer13Id, @Manager8Id, 'To wspaniała wiadomość! Ciężka praca się opłaciła.'),
	(NEWID(), @AquaFit1Id, @StaffRec9Id, @Manager8Id, 'Zapytania o obozy pływackie na wakacje. Organizujemy?'),
	(NEWID(), @AquaFit1Id, @Manager8Id, @Trainer14Id, 'Renata, czy chciałabyś współorganizować obóz pływacki?'),
	(NEWID(), @AquaFit1Id, @Trainer14Id, @Manager8Id, 'Z przyjemnością! Mam już pomysły na program.'),

	-- Wiadomości PowerGym
	(NEWID(), @PowerGym1Id, @Manager9Id, @Trainer16Id, 'Tomasz, świetne wyniki uczestników w powerlifting!'),
	(NEWID(), @PowerGym1Id, @Trainer16Id, @Manager9Id, 'Dziękuję! Planujemy udział w zawodach wojewódzkich.'),
	(NEWID(), @PowerGym1Id, @StaffRec11Id, @Manager9Id, 'Prośba o dodanie zajęć dla kobiet zainteresowanych siłownią'),
	(NEWID(), @PowerGym1Id, @Manager9Id, @Trainer17Id, 'Urszula, czy poprowadzisz grupę "Ladies Power"?'),
	(NEWID(), @PowerGym1Id, @Trainer17Id, @Manager9Id, 'Świetny pomysł! Kobiety potrzebują dedykowanych zajęć.'),

	-- Wiadomości FlexYoga
	(NEWID(), @FlexYoga1Id, @Manager10Id, @Trainer19Id, 'Meditation workshop otrzymał fantastyczne recenzje!'),
	(NEWID(), @FlexYoga1Id, @Trainer19Id, @Manager10Id, 'Dziękuję! Planujemy cykl warsztatów mindfulness.'),
	(NEWID(), @FlexYoga1Id, @Trainer20Id, @Manager10Id, 'Zbigniew, czy mógłbyś poprowadzić zajęcia jogi dla par?'),
	(NEWID(), @FlexYoga1Id, @Manager10Id, @Trainer20Id, 'Interesujący pomysł na walentynki! Przygotujemy program.');

-- =============================================
-- KONIEC SKRYPTU
-- =============================================

PRINT
	'Dane przykładowe zostały pomyślnie wstawione do bazy danych!'
PRINT 'Utworzono:'
PRINT '- 18 firm (Companies) - w tym 5 firm głównych i 13 recepcji'
PRINT '- Hierarchię firm (CompanyHierarchies)'
PRINT '- 43 pracowników (Staff) - 13 recepcjonistów, 20 trenerów, 10 managerów'
PRINT '- 30 uczestników (Participants)'
PRINT '- 20 specjalizacji (Specializations)'
PRINT '- Przypisania specjalizacji do trenerów (StaffMemberSpecializations)'
PRINT '- Dostępność personelu (StaffMemberAvailabilities)'
PRINT '- 25 typów wydarzeń (EventTypes)'
PRINT '- 35 zaplanowanych wydarzeń (EventSchedules)'
PRINT '- Przypisania personelu do wydarzeń (EventScheduleStaff)'
PRINT '- 25 rezerwacji (Reservations)'
PRINT '- 15 powiadomień (Notifications)'
PRINT '- 32 wiadomości (Messages)'
PRINT ''
PRINT 'Wszystkie tabele używają CompanyId zamiast ReceptionId!'
PRINT 'Wszystkie numery telefonów są unikalne!'
PRINT 'Skrypt zakończony pomyślnie.'