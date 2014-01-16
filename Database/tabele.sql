SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='TRADITIONAL,ALLOW_INVALID_DATES';

CREATE SCHEMA IF NOT EXISTS `bb1511_scrum4u` DEFAULT CHARACTER SET utf8 COLLATE utf8_general_ci ;
USE `bb1511_scrum4u` ;

-- -----------------------------------------------------
-- Table `bb1511_scrum4u`.`Uzytkownik`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `bb1511_scrum4u`.`Uzytkownik` (
  `UzytkownikEmail` VARCHAR(50) NOT NULL,
  `Imie` VARCHAR(50) NOT NULL,
  `Nazwisko` VARCHAR(50) NOT NULL,
  `Haslo` CHAR(128) NOT NULL,
  `KontoAktywne` TINYINT(1) NOT NULL DEFAULT 0,
  `RowDate` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT 'DataRejestracji',
  PRIMARY KEY (`UzytkownikEmail`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `bb1511_scrum4u`.`RejestracjaTokeny`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `bb1511_scrum4u`.`RejestracjaTokeny` (
  `UzytkownikEmail` VARCHAR(50) NOT NULL,
  `Token` CHAR(40) NOT NULL,
  `RowDate` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`UzytkownikEmail`),
  UNIQUE INDEX `EmailToken` (`UzytkownikEmail` ASC, `Token` ASC),
  CONSTRAINT `UzytkownikEmail`
    FOREIGN KEY (`UzytkownikEmail`)
    REFERENCES `bb1511_scrum4u`.`Uzytkownik` (`UzytkownikEmail`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `bb1511_scrum4u`.`GrupyRobocze`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `bb1511_scrum4u`.`GrupyRobocze` (
  `idGrupyRobocze` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `UzytkownikEmail` VARCHAR(50) NOT NULL COMMENT 'UzytkownikEmail - właściciel grupy',
  `NazwaGrupy` VARCHAR(50) NOT NULL COMMENT 'Nazwa grupy unikatowa dla użytkownika czyli uzt nie może mieć dwóch tak samo nazwanych grup',
  `RowDate` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`idGrupyRobocze`),
  INDEX `UzytkownikEmail_idx` (`UzytkownikEmail` ASC),
  UNIQUE INDEX `NazwaGrupu` (`UzytkownikEmail` ASC, `NazwaGrupy` ASC),
  CONSTRAINT `uzytkownikGrupyRoboczej`
    FOREIGN KEY (`UzytkownikEmail`)
    REFERENCES `bb1511_scrum4u`.`Uzytkownik` (`UzytkownikEmail`)
    ON DELETE RESTRICT
    ON UPDATE RESTRICT)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `bb1511_scrum4u`.`Projekty`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `bb1511_scrum4u`.`Projekty` (
  `idProjekty` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `UzytkownikEmail` VARCHAR(50) NOT NULL COMMENT 'ProjectManager',
  `idGrupyRobocze` INT UNSIGNED NOT NULL COMMENT 'Id Grupy Roboczej',
  `NazwaProjektu` VARCHAR(50) NOT NULL COMMENT 'Nazwa projektu unikatowa dla grupy roboczej',
  `ScrumMaster` VARCHAR(50) NOT NULL COMMENT 'Unikatowy dla projektu',
  `RowDate` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`idProjekty`),
  INDEX `ProjectManager_idx` (`UzytkownikEmail` ASC),
  INDEX `GrupaRobocza_idx` (`idGrupyRobocze` ASC),
  UNIQUE INDEX `NazwaProjektu` (`idGrupyRobocze` ASC, `NazwaProjektu` ASC),
  UNIQUE INDEX `ScrumMaster` (`idProjekty` ASC, `ScrumMaster` ASC),
  INDEX `ScrumMaster_idx` (`ScrumMaster` ASC),
  CONSTRAINT `ProjectManager`
    FOREIGN KEY (`UzytkownikEmail`)
    REFERENCES `bb1511_scrum4u`.`Uzytkownik` (`UzytkownikEmail`)
    ON DELETE RESTRICT
    ON UPDATE RESTRICT,
  CONSTRAINT `GrupaRobocza`
    FOREIGN KEY (`idGrupyRobocze`)
    REFERENCES `bb1511_scrum4u`.`GrupyRobocze` (`idGrupyRobocze`)
    ON DELETE CASCADE
    ON UPDATE CASCADE,
  CONSTRAINT `ScrumMaster`
    FOREIGN KEY (`ScrumMaster`)
    REFERENCES `bb1511_scrum4u`.`Uzytkownik` (`UzytkownikEmail`)
    ON DELETE RESTRICT
    ON UPDATE RESTRICT)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `bb1511_scrum4u`.`ProjektyUzytkownicy`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `bb1511_scrum4u`.`ProjektyUzytkownicy` (
  `UzytkownicyEmail` VARCHAR(50) NOT NULL,
  `idProjekty` INT UNSIGNED NOT NULL,
  INDEX `ProjektId_idx` (`idProjekty` ASC),
  PRIMARY KEY (`UzytkownicyEmail`, `idProjekty`),
  CONSTRAINT `UzytkownicyEmail`
    FOREIGN KEY (`UzytkownicyEmail`)
    REFERENCES `bb1511_scrum4u`.`Uzytkownik` (`UzytkownikEmail`)
    ON DELETE RESTRICT
    ON UPDATE RESTRICT,
  CONSTRAINT `ProjektId`
    FOREIGN KEY (`idProjekty`)
    REFERENCES `bb1511_scrum4u`.`Projekty` (`idProjekty`)
    ON DELETE RESTRICT
    ON UPDATE RESTRICT)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `bb1511_scrum4u`.`ProjektyZaproszenia`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `bb1511_scrum4u`.`ProjektyZaproszenia` (
  `UzytkownikEmail` VARCHAR(50) NOT NULL,
  `idProjekty` INT UNSIGNED NOT NULL,
  `Token` CHAR(40) NOT NULL,
  `Status` TINYINT(1) NOT NULL DEFAULT 0 COMMENT '0 - niezaakceptowane zaproszenie\n1 - zaakceptowane',
  INDEX `ZaproszonyUzytkownik_idx` (`UzytkownikEmail` ASC),
  INDEX `IdProjektu_idx` (`idProjekty` ASC),
  INDEX `KluczGłowny` (`UzytkownikEmail` ASC, `idProjekty` ASC),
  PRIMARY KEY (`UzytkownikEmail`, `idProjekty`),
  CONSTRAINT `ZaproszonyUzytkownik`
    FOREIGN KEY (`UzytkownikEmail`)
    REFERENCES `bb1511_scrum4u`.`Uzytkownik` (`UzytkownikEmail`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `IdProjektu`
    FOREIGN KEY (`idProjekty`)
    REFERENCES `bb1511_scrum4u`.`Projekty` (`idProjekty`)
    ON DELETE RESTRICT
    ON UPDATE RESTRICT)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `bb1511_scrum4u`.`ProjektyGrupyUzytkownikow`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `bb1511_scrum4u`.`ProjektyGrupyUzytkownikow` (
  `idProjektyGrupyUzytkownikow` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `idProjekty` INT UNSIGNED NOT NULL,
  `NazwaGrupy` VARCHAR(50) NOT NULL,
  `StatusGrupy` BIT NOT NULL DEFAULT 0 COMMENT '1 - Grupa Zamknięta\n0 - Grupa Otwarta',
  PRIMARY KEY (`idProjektyGrupyUzytkownikow`),
  INDEX `IdProjektu_idx` (`idProjekty` ASC),
  CONSTRAINT `idProjektuGrupa`
    FOREIGN KEY (`idProjekty`)
    REFERENCES `bb1511_scrum4u`.`Projekty` (`idProjekty`)
    ON DELETE CASCADE
    ON UPDATE CASCADE)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `bb1511_scrum4u`.`ProjektyUzytkownicyGrupy`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `bb1511_scrum4u`.`ProjektyUzytkownicyGrupy` (
  `idProjektyGrupyUzytkownikow` INT UNSIGNED NOT NULL,
  `UzytkownikEmail` VARCHAR(50) NOT NULL,
  PRIMARY KEY (`idProjektyGrupyUzytkownikow`, `UzytkownikEmail`),
  INDEX `EmailUzytkownika_idx` (`UzytkownikEmail` ASC),
  CONSTRAINT `ProjektyGrupyUzytkownikow`
    FOREIGN KEY (`idProjektyGrupyUzytkownikow`)
    REFERENCES `bb1511_scrum4u`.`ProjektyGrupyUzytkownikow` (`idProjektyGrupyUzytkownikow`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `EmailUzytkownika`
    FOREIGN KEY (`UzytkownikEmail`)
    REFERENCES `bb1511_scrum4u`.`Uzytkownik` (`UzytkownikEmail`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `bb1511_scrum4u`.`Sprinty`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `bb1511_scrum4u`.`Sprinty` (
  `idSprinty` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `idProjekty` INT UNSIGNED NOT NULL,
  `dataRozpoczecia` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `dataZakonczenia` TIMESTAMP NULL,
  `status` BIT NOT NULL DEFAULT 0 COMMENT '0 - otwarty\n1 - zamkniety',
  PRIMARY KEY (`idSprinty`),
  INDEX `idProjektu_idx` (`idProjekty` ASC),
  CONSTRAINT `SprintProjekt`
    FOREIGN KEY (`idProjekty`)
    REFERENCES `bb1511_scrum4u`.`Projekty` (`idProjekty`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `bb1511_scrum4u`.`Zadania`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `bb1511_scrum4u`.`Zadania` (
  `idZadania` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `idSprinty` INT UNSIGNED NOT NULL,
  `Typ` BIT NOT NULL DEFAULT 0 COMMENT '0 - zadanie\n1 - błąd',
  `Tytul` VARCHAR(100) NOT NULL,
  `Opis` TEXT NULL,
  `isDeleted` BIT NOT NULL DEFAULT 0 COMMENT '0 - aktywny\n1 - usunięty',
  `Dodal` VARCHAR(50) NOT NULL COMMENT 'Id użytkownika dodającego zadanie',
  `RowDate` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT 'Data utworzenia zadania',
  `Priorytet` INT NOT NULL DEFAULT 0,
  `idPrzydzielonyUzytkownik` VARCHAR(50) NULL,
  `idPrzydzielonaGrupa` INT UNSIGNED NULL,
  `ZadanieNadrzędne` INT UNSIGNED NULL COMMENT 'id Zadania nadrzędnego.',
  `DataZakonczenia` DATETIME NULL COMMENT 'Jeżeli puste - nieukończone',
  PRIMARY KEY (`idZadania`),
  INDEX `PrzydzielonyUzytkownik_idx` (`idPrzydzielonyUzytkownik` ASC),
  INDEX `PrzydzielonaGrupa_idx` (`idPrzydzielonaGrupa` ASC),
  INDEX `ZadanieNadrzędne_idx` (`ZadanieNadrzędne` ASC),
  INDEX `Dodal_idx` (`Dodal` ASC),
  INDEX `idSprtintu_idx` (`idSprinty` ASC),
  CONSTRAINT `PrzydzielonyUzytkownik`
    FOREIGN KEY (`idPrzydzielonyUzytkownik`)
    REFERENCES `bb1511_scrum4u`.`Uzytkownik` (`UzytkownikEmail`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `PrzydzielonaGrupa`
    FOREIGN KEY (`idPrzydzielonaGrupa`)
    REFERENCES `bb1511_scrum4u`.`ProjektyGrupyUzytkownikow` (`idProjektyGrupyUzytkownikow`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `idSprtintu`
    FOREIGN KEY (`idSprinty`)
    REFERENCES `bb1511_scrum4u`.`Sprinty` (`idSprinty`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `ZadanieNadrzędne`
    FOREIGN KEY (`ZadanieNadrzędne`)
    REFERENCES `bb1511_scrum4u`.`Zadania` (`idZadania`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `Dodal`
    FOREIGN KEY (`Dodal`)
    REFERENCES `bb1511_scrum4u`.`Uzytkownik` (`UzytkownikEmail`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;
