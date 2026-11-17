CREATE TABLE IF NOT EXISTS `__EFMigrationsHistory` (
    `MigrationId` varchar(150) CHARACTER SET utf8mb4 NOT NULL,
    `ProductVersion` varchar(32) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK___EFMigrationsHistory` PRIMARY KEY (`MigrationId`)
) CHARACTER SET=utf8mb4;

START TRANSACTION;
DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261105182629_CriacaoInicial') THEN

    ALTER DATABASE CHARACTER SET utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261105182629_CriacaoInicial') THEN

    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20261105182629_CriacaoInicial', '9.0.0');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261106163742_CriarTabelasAcademicas') THEN

    CREATE TABLE `Alunos` (
        `Id` int NOT NULL AUTO_INCREMENT,
        `Nome` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
        `CPF` varchar(14) CHARACTER SET utf8mb4 NOT NULL,
        `Email` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
        `Matricula` varchar(20) CHARACTER SET utf8mb4 NOT NULL,
        `DataNascimento` datetime(6) NOT NULL,
        `Ativo` tinyint(1) NOT NULL,
        CONSTRAINT `PK_Alunos` PRIMARY KEY (`Id`)
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261106163742_CriarTabelasAcademicas') THEN

    CREATE TABLE `Disciplinas` (
        `Id` int NOT NULL AUTO_INCREMENT,
        `Nome` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
        `Descricao` varchar(300) CHARACTER SET utf8mb4 NOT NULL,
        `CargaHoraria` int NOT NULL,
        `Ativa` tinyint(1) NOT NULL,
        CONSTRAINT `PK_Disciplinas` PRIMARY KEY (`Id`)
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261106163742_CriarTabelasAcademicas') THEN

    CREATE TABLE `Professores` (
        `Id` int NOT NULL AUTO_INCREMENT,
        `Nome` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
        `CPF` varchar(14) CHARACTER SET utf8mb4 NOT NULL,
        `Email` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
        `Especialidade` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
        `Ativo` tinyint(1) NOT NULL,
        CONSTRAINT `PK_Professores` PRIMARY KEY (`Id`)
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261106163742_CriarTabelasAcademicas') THEN

    CREATE TABLE `Turmas` (
        `Id` int NOT NULL AUTO_INCREMENT,
        `Nome` varchar(50) CHARACTER SET utf8mb4 NOT NULL,
        `Ano` int NOT NULL,
        `Semestre` int NOT NULL,
        `ProfessorId` int NOT NULL,
        `DisciplinaId` int NOT NULL,
        CONSTRAINT `PK_Turmas` PRIMARY KEY (`Id`),
        CONSTRAINT `FK_Turmas_Disciplinas_DisciplinaId` FOREIGN KEY (`DisciplinaId`) REFERENCES `Disciplinas` (`Id`) ON DELETE CASCADE,
        CONSTRAINT `FK_Turmas_Professores_ProfessorId` FOREIGN KEY (`ProfessorId`) REFERENCES `Professores` (`Id`) ON DELETE CASCADE
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261106163742_CriarTabelasAcademicas') THEN

    CREATE TABLE `Matriculas` (
        `Id` int NOT NULL AUTO_INCREMENT,
        `AlunoId` int NOT NULL,
        `TurmaId` int NOT NULL,
        `DataMatricula` datetime(6) NOT NULL,
        `Ativa` tinyint(1) NOT NULL,
        CONSTRAINT `PK_Matriculas` PRIMARY KEY (`Id`),
        CONSTRAINT `FK_Matriculas_Alunos_AlunoId` FOREIGN KEY (`AlunoId`) REFERENCES `Alunos` (`Id`) ON DELETE CASCADE,
        CONSTRAINT `FK_Matriculas_Turmas_TurmaId` FOREIGN KEY (`TurmaId`) REFERENCES `Turmas` (`Id`) ON DELETE CASCADE
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261106163742_CriarTabelasAcademicas') THEN

    CREATE TABLE `Notas` (
        `Id` int NOT NULL AUTO_INCREMENT,
        `MatriculaId` int NOT NULL,
        `Valor` decimal(65,30) NOT NULL,
        `Descricao` varchar(100) CHARACTER SET utf8mb4 NULL,
        `DataLancamento` datetime(6) NOT NULL,
        CONSTRAINT `PK_Notas` PRIMARY KEY (`Id`),
        CONSTRAINT `FK_Notas_Matriculas_MatriculaId` FOREIGN KEY (`MatriculaId`) REFERENCES `Matriculas` (`Id`) ON DELETE CASCADE
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261106163742_CriarTabelasAcademicas') THEN

    CREATE INDEX `IX_Matriculas_AlunoId` ON `Matriculas` (`AlunoId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261106163742_CriarTabelasAcademicas') THEN

    CREATE INDEX `IX_Matriculas_TurmaId` ON `Matriculas` (`TurmaId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261106163742_CriarTabelasAcademicas') THEN

    CREATE INDEX `IX_Notas_MatriculaId` ON `Notas` (`MatriculaId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261106163742_CriarTabelasAcademicas') THEN

    CREATE INDEX `IX_Turmas_DisciplinaId` ON `Turmas` (`DisciplinaId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261106163742_CriarTabelasAcademicas') THEN

    CREATE INDEX `IX_Turmas_ProfessorId` ON `Turmas` (`ProfessorId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261106163742_CriarTabelasAcademicas') THEN

    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20261106163742_CriarTabelasAcademicas', '9.0.0');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261110192949_AdicionarTabelaUsuarios') THEN

    CREATE TABLE `Usuarios` (
        `Id` int NOT NULL AUTO_INCREMENT,
        `Nome` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
        `Email` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
        `SenhaHash` longtext CHARACTER SET utf8mb4 NOT NULL,
        `Ativo` tinyint(1) NOT NULL,
        `DataCriacao` datetime(6) NOT NULL,
        CONSTRAINT `PK_Usuarios` PRIMARY KEY (`Id`)
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261110192949_AdicionarTabelaUsuarios') THEN

    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20261110192949_AdicionarTabelaUsuarios', '9.0.0');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261111191946_AjustesRelacionamentosEValidacoes') THEN

    ALTER TABLE `Matriculas` DROP FOREIGN KEY `FK_Matriculas_Alunos_AlunoId`;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261111191946_AjustesRelacionamentosEValidacoes') THEN

    ALTER TABLE `Matriculas` DROP FOREIGN KEY `FK_Matriculas_Turmas_TurmaId`;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261111191946_AjustesRelacionamentosEValidacoes') THEN

    ALTER TABLE `Notas` DROP FOREIGN KEY `FK_Notas_Matriculas_MatriculaId`;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261111191946_AjustesRelacionamentosEValidacoes') THEN

    ALTER TABLE `Turmas` DROP FOREIGN KEY `FK_Turmas_Disciplinas_DisciplinaId`;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261111191946_AjustesRelacionamentosEValidacoes') THEN

    ALTER TABLE `Turmas` DROP FOREIGN KEY `FK_Turmas_Professores_ProfessorId`;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261111191946_AjustesRelacionamentosEValidacoes') THEN

    ALTER TABLE `Notas` MODIFY COLUMN `Valor` decimal(5,2) NOT NULL;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261111191946_AjustesRelacionamentosEValidacoes') THEN

    CREATE UNIQUE INDEX `IX_Usuarios_Email` ON `Usuarios` (`Email`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261111191946_AjustesRelacionamentosEValidacoes') THEN

    ALTER TABLE `Matriculas` ADD CONSTRAINT `FK_Matriculas_Alunos_AlunoId` FOREIGN KEY (`AlunoId`) REFERENCES `Alunos` (`Id`) ON DELETE RESTRICT;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261111191946_AjustesRelacionamentosEValidacoes') THEN

    ALTER TABLE `Matriculas` ADD CONSTRAINT `FK_Matriculas_Turmas_TurmaId` FOREIGN KEY (`TurmaId`) REFERENCES `Turmas` (`Id`) ON DELETE RESTRICT;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261111191946_AjustesRelacionamentosEValidacoes') THEN

    ALTER TABLE `Notas` ADD CONSTRAINT `FK_Notas_Matriculas_MatriculaId` FOREIGN KEY (`MatriculaId`) REFERENCES `Matriculas` (`Id`) ON DELETE RESTRICT;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261111191946_AjustesRelacionamentosEValidacoes') THEN

    ALTER TABLE `Turmas` ADD CONSTRAINT `FK_Turmas_Disciplinas_DisciplinaId` FOREIGN KEY (`DisciplinaId`) REFERENCES `Disciplinas` (`Id`) ON DELETE RESTRICT;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261111191946_AjustesRelacionamentosEValidacoes') THEN

    ALTER TABLE `Turmas` ADD CONSTRAINT `FK_Turmas_Professores_ProfessorId` FOREIGN KEY (`ProfessorId`) REFERENCES `Professores` (`Id`) ON DELETE RESTRICT;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261111191946_AjustesRelacionamentosEValidacoes') THEN

    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20261111191946_AjustesRelacionamentosEValidacoes', '9.0.0');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

COMMIT;

