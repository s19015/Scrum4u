begin
--delete from TokenyRejestracyjne where uzytkownicy_email = 'jtestowy@test.pl';
--delete from GrupyRobocze where uzytkownicy_email = 'jtestowy@test.pl';
--delete from Uzytkownicy where uzytkownicy_email = 'jtestowy@test.pl';

--exec rejestracjaUzytkownika @email_uzytkownika = '     jtestowy@test.pl', @imie ='   Jan   ', @nazwisko = '  Testowy  ', @haslo = '20cf0e0caf95a5464ae77ae124829a7a3df03d141d82f532ab75ce6aa17cbe8c';
--exec aktywacjaKonta @email_uzytkownika = 'jtestowy@test.pl', @token = '27AD8B46828B44CA4F375DA8E9AA64A7';
--exec utworzGrupeRobocza @email_uzytkownika = 'jtestowy@test.pl', @nazwa_grupy = 'Testowa Grupa Robocza';
--exec listaGrupRoboczych @email_uzytkownika = 'jtestowy@test.pl';
--exec logowanie @email_uzytkownika = 'jtestowy@test.pl', @haslo = '20cf0e0caf95a5464ae77ae124829a7a3df03d141d82f532ab75ce6aa17cbe8c';
--exec profilUzytkownika @email_uzytkownika = 'jtestowy@test.pl';
end;