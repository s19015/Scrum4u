/* DEFINICJA PROCEDUR */
drop procedure if exists rejestracjaUzytkownika;
delimiter //
create procedure rejestracjaUzytkownika( in uzytkownikEmail varchar(50), in imie varchar(50), in nazwisko varchar(50), in haslo varchar(256), in dataRejestracji timestamp )
begin

/*
1. Dodaj użytkownika do bazy
2. Utworz token rejestracyjny
*/

insert into Uzytkownik( uzytkownikEmail, imie, nazwisko, haslo, rowDate )
values ( uzytkownikEmail, imie, nazwisko, haslo, dataRejestracji );

call utworzTokenRejestracyjny( uzytkownikEmail );

end //
delimiter ;

drop procedure if exists utworzTokenRejestracyjny;
delimiter //
create procedure utworzTokenRejestracyjny( in uzytkownikEmail varchar(50))
begin

insert into RejestracjaTokeny (uzytkownikEmail, token)
values (uzytkownikEmail, cast(md5(rand()) as char(40) ));

SELECT LAST_INSERT_ID() as Token;

/* PROCEDURA POWINNA ZWRÓCIĆ FALSE W PRZYPADKU BŁĘDU? */

end//
delimiter ;

drop function if exists czyKontoAktywneFunc;
delimiter //
create function czyKontoAktywneFunc( inUzytkownikEmail varchar(50) ) returns bool
begin
	return (select kontoAktywne from Uzytkownik where uzytkownikEmail = inUzytkownikEmail);
end//
delimiter ;

drop procedure if exists czyKontoAktywne;
delimiter //
create procedure czyKontoAktywne( in inEmailUzytkownika varchar(50) )
begin
	select czyKontoAktywneFunc(inEmailUzytkownika) as kontoAktywne;
end//
delimiter ;

drop procedure if exists aktywacjaKonta;
delimiter //
create procedure aktywacjaKonta( inUzytkownikEmail varchar(50), token char(40) )
begin
/*
1. sprawdzenie czy konto aktywne
1.1 sprawdzenie czy token nie wygasł??
2. jezeli nie to porownaj tokeny
3. jezeli równe to aktywuj konto
*/

declare czyAktywne bool default 0; #domyslnie zakladamy ze konto jest aktywne
declare tokenPoprawny bool default 0; #domyslnie niepoprawny token
set czyAktywne = czyKontoAktywneFunc(inUzytkownikEmail);
if czyAktywne = 0 then #konto nieaktywne
	begin
		set tokenPoprawny = ( select count(*) from RejestracjaTokeny where uzytkownikEmail = inUzytkownikEmail and token = token );
		if tokenPoprawny then #token prawidlowy
			begin
				update Uzytkownik set kontoAktywne = 1 where uzytkownikEmail = inUzytkownikEmail;
			end;
		else #token nieprawidlowy
			begin
			end;
		end if;
	end;
else #uzytkownik już aktywny lub niepoprawny email
	begin
	
	end;
end if;
call czyKontoAktywne( inUzytkownikEmail );
end//
delimiter ;

drop procedure if exists utworzGrupeRobocza;
delimiter //
create procedure utworzGrupeRobocza(in inUzytkownikEmail varchar(50), in inNazwaGrupy varchar(50))
begin
	insert into GrupyRobocze (uzytkownikEmail, nazwaGrupy)
	values ( inUzytkownikEmail, inNazwaGrupy );
end//
delimiter ;

drop procedure if exists listaGrupRoboczych;
delimiter //
create procedure listaGrupRoboczych( in inUzytkownikEmail varchar(50)) # zwraca listę grup roboczych uzytkownika
begin
	select idGrupyRobocze, nazwaGrupy from GrupyRobocze where uzytkownikEmail = inUzytkownikEmail order by rowDate;
end//
delimiter ;

drop procedure if exists logowanie;
delimiter //
create procedure logowanie( in inUzytkownikEmail varchar(50), in inHaslo varchar(256))
begin
declare czyAktywne bool default 0; #domyslnie zakladamy ze konto jest aktywne
set czyAktywne = czyKontoAktywneFunc(inUzytkownikEmail);
if czyAktywne=0 then
	begin
		select 0 as statusLogowania;
	end;
else
	begin
		select count(*) as statusLogowania from Uzytkownik where uzytkownikEmail = inUzytkownikEmail and haslo = inHaslo;
	end;
end if;
end//
delimiter ;

drop procedure if exists pobierzProfil;
delimiter //
create procedure pobierzProfil( in inUzytkownikEmail varchar(50) )
begin
select uzytkownikEmail, imie, nazwisko from Uzytkownik where uzytkownikEmail = inUzytkownikEmail;
end//
delimiter ;

drop procedure if exists utworzProjekt;
delimiter //
create procedure utworzProjekt( in inUzytkownikEmail varchar(50), in inIdGrupyRoboczej int unsigned, in inNazwaProjektu varchar(50), in inScrumMaster varchar(50))
begin
insert into Projekty ( uzytkownikEmail, idGrupyRobocze, nazwaProjektu, scrumMaster )
values ( inUzytkownikEmail, inIdGrupyRoboczej, inNazwaProjektu, inScrumMaster );
end//
delimiter ;

drop procedure if exists listaProjektow;
delimiter //
create procedure listaProjektow ( in inUzytkownikEmail varchar(50) )
begin
	select idProjekty, nazwaProjektu, scrumMaster 
	from Projekty 
	where uzytkownikEmail = inUzytkownikEmail and status = 0
	order by nazwaProjektu;
end//
delimiter ;

drop procedure if exists utworzGrupeUzytkownikow;
delimiter //
create procedure utworzGrupeUzytkownikow( in inUzytkownikEmail varchar(50), in inIdProjektu int unsigned, in inNazwaGrupy varchar(50), in inStatusGrupy bool )
begin
/*
1. Sprawdzenie czy projekt należy do użytkownika
2. Dodanie grupy
3. Zwrocenie id nowej grupy
*/

declare wlascicielProjektu varchar(50);
declare returnIdGrupy int unsigned;

set wlascicielProjektu = ( select uzytkownikEmail from Projekty where idProjekty = inIdProjektu );

if wlascicielProjektu = inUzytkownikEmail then #uzytkownik poprawny
	begin
		insert into ProjektyGrupyUzytkownikow (idProjekty, nazwaGrupy, statusGrupy)
		values ( inIdProjektu, inNazwaGrupy, inStatusGrupy );
		set returnIdGrupy = last_insert_id();
	end;
else # nie udalo sie utworzyc grupy
	begin
		set returnIdGrupy = null;
	end;
end if;
select returnIdGrupy as idProjektyGrupyUzytkownikow;
end//
delimiter ;

drop procedure if exists zaproszenieDoProjektu;
delimiter //
create procedure zaproszenieDoProjektu( in inUzytkownikEmail varchar(50), in inUzytkownikZapraszany varchar(50), in inIdProjektu int unsigned )
begin
/*
1. Sprawdzenie czy projekt nalezy do zapraszajacego
2. Utworzenie zaproszenia i tokenu
3. Zwrocenie tokenu
*/

declare wlascicielProjektu varchar(50);
declare returnToken char(40);

set wlascicielProjektu = ( select uzytkownikEmail from Projekty where idProjekty = inIdProjektu );
select wlascicielProjektu;
if wlascicielProjektu = inUzytkownikEmail then # email poprawny
	begin
		insert into ProjektyZaproszenia (uzytkownikEmail, idProjekty, Token)
		values ( inUzytkownikZapraszany, inIdProjektu, cast( md5( rand() ) as char(40) ) );
		
		set returnToken = ( select token from ProjektyZaproszenia where uzytkownikEmail = inUzytkownikZapraszany and idProjekty = inIdProjektu );
	end;
else # email niepoprawny
	begin
		set returnToken = null;
	end;
end if;
select returnToken as token;
end//
delimiter ;

drop procedure if exists przyjmijZaproszenieDoProjektu;
delimiter //
create procedure przyjmijZaproszenieDoProjektu( in inUzytkownikEmail varchar(50), in inToken char(40) )
begin
/*
1. Sprawdzenie tokenu
2. Jeżeli poprawny zaakceptuj zaproszenie.
3. Dodaj użytkownika do projektu.
*/

declare idProjektu bool default 0; 

set idProjektu = ( select idProjekty from ProjektyZaproszenia where uzytkownikEmail = inUzytkownikEmail and token = inToken and status = 0 );


if idProjektu is not null then #token poprawny
	begin
		#update ProjektyZaproszenia set status = 1 where uzytkownikEmail = inUzytkownikEmail and token = inToken and status = 0;
		#call dodajUzytkownikaDoProjektu( inUzytkownikEmail, idProjektu );
		select idProjektu;
	end;
else
	begin 
		
	end;
end if;

select status as statusAktywacji from ProjektyZaproszenia where uzytkownikEmail = inUzytkownikEmail and token = inToken;

end//
delimiter ;

drop procedure if exists dodajUzytkownikaDoProjektu;
delimiter //
create procedure dodajUzytkownikaDoProjektu ( in inUzytkownikEmail varchar(50), in inIdProjektu int unsigned )
begin
	insert into ProjektyUzytkownicy ( uzytkownikEmail, idProjekty ) values ( inUzytkownikEmail, inIdProjektu );
end//
delimiter ;

drop procedure if exists dodajWpisDoDziennikaWydarzen;
delimiter //
create procedure dodajWpisDoDziennikaWydarzen( in inDziennikZrodlo varchar(200), in inDziennikOpis tinytext, in inDziennikStackTrace text)
begin
insert into DziennikWydarzen ( dziennikZrodlo, dziennikOpis, dziennikStackTrace )
values ( inDziennikZrodlo, inDziennikOpis, inDziennikStackTrace );
end//
delimiter ;

drop procedure if exists pobierzDziennikWydarzen;
delimiter //
create procedure pobierzDziennikWydarzen(  ) 
begin

select idDziennikWydarzen, dziennikData, dziennikZrodlo, dziennikOpis, dziennikStackTrace from DziennikWydarzen order by dziennikData desc limit 50;

end//
delimiter ;

drop procedure if exists dodajDoKolejkiMaili;
delimiter //
create procedure dodajDoKolejkiMaili( in inEmailOd varchar(50), in inEmailDo varchar(50), in inEmailTemat varchar(150), in inEmailTresc text, in inEmailWersja varchar(10), in inEmailDataKolejki timestamp)
begin
insert into KolejkaEmaili ( emailOd, emailDo, emailTemat, emailTresc, emailWersja, emailDataKolejki )
values ( inEmailOd, inEmailDo, inEmailTemat, inEmailTresc, inEmailWersja, inEmailDataKolejki );
end//
delimiter ;

drop procedure if exists pobierzNiewyslaneMaileZKolejki;
delimiter //
create procedure pobierzNiewyslaneMaileZKolejki(in inDataOd timestamp)
begin
select idKolejkaEmaili, emailOd, emailDo, emailTemat, emailTresc, emailWersja, emailWyslany, emailDataKolejki, emailDataWyslania
from KolejkaEmaili
where emailDataKolejki >= inDataOd and emailWyslany = 0;
end//
delimiter ;

drop procedure if exists potwierdzWyslanieEmailaZKolejki;
delimiter //
create procedure potwierdzWyslanieEmailaZKolejki( in inIdKolejkaEmaili int unsigned)
begin
update KolejkaEmaili set emailWyslany = 1, emailDataWyslania = CURRENT_TIMESTAMP
where idKolejkaEmaili = inIdKolejkaEmaili;
end//
delimiter ;

drop procedure if exists czyUzytkownikIstnieje;
delimiter //
create procedure czyUzytkownikIstnieje( in inEmailUzytkownika varchar(50))
begin
select count(*) as czyUzytkownikIstnieje from Uzytkownik where uzytkownikEmail = trim(inEmailUzytkownika);
end//
delimiter ;

drop procedure if exists pobierzTokenResetHasla;
delimiter //
create procedure pobierzTokenResetHasla( in inEmailUzytkownika varchar(50))
begin

select token from ResetowanieHasla where uzytkownikEmail = inEmailUzytkownika and czyAktywny = 1;

end//
delimiter ;

drop procedure if exists utworzTokenResetHasla;
delimiter //
create procedure utworzTokenResetHasla( in inEmailUzytkownika varchar(50) )
begin

insert into ResetowanieHasla( uzytkownikEmail, token )
values ( inEmailUzytkownika, cast(md5(rand()) as char(40) )) ;

call pobierzTokenResetHasla( inEmailUzytkownika );

end//
delimiter ;


drop procedure if exists resetujHaslo;
delimiter //
create procedure resetujHaslo( in inEmailUzytkownika varchar(50), in inToken char(40), in inNoweHaslo varchar(256) )
begin
	update Uzytkownik set haslo = inNoweHaslo
	where uzytkownikEmail = inEmailUzytkownika and uzytkownikEmail = ( select uzytkownikEmail from ResetowanieHasla where uzytkownikEmail = inEmailUzytkownika and token = inToken and czyAktywny = 1 );

	update ResetowanieHasla set czyAktywny = 0 where uzytkownikEmail = inEmailUzytkownika and token = inToken;
end//
delimiter ;

/* DEFINICJA PROCEDUR */

/* DANE TESTOWE */

SET SQL_SAFE_UPDATES=0;
truncate table RejestracjaTokeny;
truncate table ProjektyUzytkownicyGrupy;
truncate table ProjektyZaproszenia;
truncate table GrupyRobocze;
truncate table ProjektyZaproszenia;
truncate table Projekty;
truncate table ProjektyUzytkownicy;
truncate table ResetowanieHasla;

truncate table Uzytkownik;
truncate table KolejkaEmaili;

SET SQL_SAFE_UPDATES=1;

call rejestracjaUzytkownika( 'jtestowy@test.pl', 'Jan', 'Testowy', '20cf0e0caf95a5464ae77ae124829a7a3df03d141d82f532ab75ce6aa17cbe8c', CURRENT_TIMESTAMP );
call aktywacjaKonta( 'jtestowy@test.pl', (select token from RejestracjaTokeny where uzytkownikEmail = 'jtestowy@test.pl') );
#call logowanie( 'jtestowy@test.pl', (select haslo from Uzytkownik where uzytkownikEmail = 'jtestowy@tsest.pl' ) );
#call pobierzProfil( 'jtestowy@test.pl' );
call utworzGrupeRobocza( 'jtestowy@test.pl', 'jtestowyWG' );
#call listaGrupRoboczych( 'jtestowy@test.pl' );
call rejestracjaUzytkownika( 'smaster@test.pl', 'Scrum', 'Master', '63a0f5c4bf31cc31f135a62f6ad58abbd5ccb5c51ac032e69fcde034c6c50254', CURRENT_TIMESTAMP );
call utworzProjekt( 'jtestowy@test.pl', ( select idGrupyRobocze from GrupyRobocze where uzytkownikEmail = 'jtestowy@test.pl' limit 1 ) ,'Projekt Testowy', 'smaster@test.pl' );
#call listaProjektow( 'jtestowy@test.pl' );
call utworzGrupeUzytkownikow( 'jtestoswy@test.pl', (select idProjekty from Projekty where uzytkownikEmail = 'jtestowy@test.pl' limit 1), 'Grupa testowa', 0 );
call zaproszenieDoProjektu( 'jtestowy@test.pl', 'smaster@test.pl', (select idProjekty from Projekty where uzytkownikEmail = 'jtestowy@test.pl' limit 1));
call przyjmijZaproszenieDoProjektu( 'smaster@test.pl', (select token from ProjektyZaproszenia where uzytkownikEmail = 'smaster@test.pl') );
call dodajWpisDoDziennikaWydarzen( 'http://testowa.sciezka.pl', 'Testowy opis', 'Testowy StackTrace' );
call pobierzDziennikWydarzen();
call dodajDoKolejkiMaili( 'testOd@test.pl', 'testDo@test.pl', 'test temat', 'test treść', 'html', CURRENT_TIMESTAMP );
call pobierzNiewyslaneMaileZKolejki( '2014-01-01' );
call czyUzytkownikIstnieje( 'jtestowy@test.pl' );
call potwierdzWyslanieEmailaZKolejki( 1 );
call utworzTokenResetHasla( 'jtestowy@test.pl' );
call resetujHaslo( 'jtestowy@test.pl', ( select token from ResetowanieHasla where uzytkownikEmail = 'jtestowy@test.pl' and czyAktywny = 1 ), '49673d1fa83a4adc62877657f6ce0adaff67be8b92889e59bfd4d7f38027ed1a' ); #nowehaslo
/* DANE TESTOWE */