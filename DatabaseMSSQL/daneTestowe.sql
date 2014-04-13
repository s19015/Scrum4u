begin
--delete from TokenyRejestracyjne where uzytkownicy_email = 'jtestowy@test.pl';
--delete from GrupyRobocze where uzytkownicy_email = 'jtestowy@test.pl';
--delete from Uzytkownicy where uzytkownicy_email = 'jtestowy@test.pl';
delete from ProjektyZaproszenia where id_zapraszajacego = 'jtestowy@test.pl';
delete from Projekty where id_menager_projektu = 'jtestowy@test.pl';

--exec rejestracjaUzytkownika @email_uzytkownika = '     jtestowy@test.pl', @imie ='   Jan   ', @nazwisko = '  Testowy  ', @haslo = '20cf0e0caf95a5464ae77ae124829a7a3df03d141d82f532ab75ce6aa17cbe8c';
--exec aktywacjaKonta @email_uzytkownika = 'jtestowy@test.pl', @token = '27AD8B46828B44CA4F375DA8E9AA64A7';
--exec utworzGrupeRobocza @email_uzytkownika = 'jtestowy@test.pl', @nazwa_grupy = 'Testowa Grupa Robocza';
--exec listaGrupRoboczych @email_uzytkownika = 'jtestowy@test.pl';
--exec logowanie @email_uzytkownika = 'jtestowy@test.pl', @haslo = '20cf0e0caf95a5464ae77ae124829a7a3df03d141d82f532ab75ce6aa17cbe8c';
--exec profilUzytkownika @email_uzytkownika = 'jtestowy@test.pl';
--exec rejestracjaUzytkownika @email_uzytkownika = 'stestowy@test.pl', @imie ='Stefan', @nazwisko = 'Testowy', @haslo = '20cf0e0caf95a5464ae77ae124829a7a3df03d141d82f532ab75ce6aa17cbe8c';
--exec aktywacjaKonta @email_uzytkownika = 'stestowy@test.pl', @token ='107EA93CE85BBE1F7325CC48799BE551';
DECLARE @id_grupy_roboczej INT;
SET @id_grupy_roboczej = ( SELECT id_grupy_robocze FROM GrupyRobocze WHERE uzytkownicy_email = 'jtestowy@test.pl' );
exec utworzProjekt @email_uzytkownika = 'jtestowy@test.pl', @id_grupy_robocze = @id_grupy_roboczej, @nazwa_projektu = 'Projekt Testowy', @scrum_master_id = 'stestowy@test.pl';
end;