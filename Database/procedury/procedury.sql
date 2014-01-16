/* DEFINICJA PROCEDUR */
drop procedure if exists rejestracjaUzytkownika;
delimiter //
create procedure rejestracjaUzytkownika( in uzytkownikEmail varchar(50), in imie varchar(50), in nazwisko varchar(50), in haslo varchar(256), in dataRejestracji timestamp )
begin

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

end//
delimiter ;

drop procedure if exists czyKontoAktywne;
delimiter //
create procedure czyKontoAktywne( in inEmailUzytkownika varchar(50) )
begin
	select kontoAktywne from Uzytkownik where uzytkownikEmail = uzytkownikEmail;
end//
delimiter ;

drop procedure if exists aktywacjaKonta;
delimiter //
create procedure aktywacjaKonta( uzytkownikEmail varchar(50), token char(40) )
begin
/*
1. sprawdzenie czy konto aktywne
1.1 sprawdzenie czy token nie wygasł??
2. jezeli nie to porownaj tokeny
3. jezeli równe to aktywuj konto
*/

declare czyAktywne bool default 0; #domyslnie zakladamy ze konto jest aktywne
declare tokenPoprawny bool default 0; #domyslnie niepoprawny token
set czyAktywne = czyKontoAktywne(uzytkownikEmail);
if czyAktywne = 0 then #konto nieaktywne
	begin
		set tokenPoprawny = ( select count(*) from RejestracjaTokeny where uzytkownikEmail = uzytkownikEmail and token = token );
		if tokenPoprawny then #token prawidlowy
			begin
				update Uzytkownik set kontoAktywne = 1 where uzytkownikEmail = uzytkownikEmail;
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
call czyKontoAktywne( uzytkownikEmail );
end//
delimiter ;

/* DEFINICJA PROCEDUR */

/* DANE TESTOWE */

SET SQL_SAFE_UPDATES=0;
truncate table RejestracjaTokeny;
truncate table Uzytkownik;
SET SQL_SAFE_UPDATES=1;
call rejestracjaUzytkownika( 'jtestowy@test.pl', 'Jan', 'Testowy', '20cf0e0caf95a5464ae77ae124829a7a3df03d141d82f532ab75ce6aa17cbe8c', CURRENT_TIMESTAMP );
call aktywacjaKonta( 'jtestowy@test.pl', (select token from RejestracjaTokeny where uzytkownikEmail = 'jtestowy@test.pl') );
/* DANE TESTOWE */