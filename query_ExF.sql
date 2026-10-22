CREATE DATABASE ExamenFinal

USE ExamenFinal

select *  from tb_Consumidor
CREATE TABLE tb_paises (
    idpais CHAR(3) PRIMARY KEY,
    nompais VARCHAR(50) NOT NULL
);

Create table tb_Consumidor (
dnicont char(8) primary key,
nomcont varchar(40) not null,
apecont varchar(40) not null,
emailcont varchar(250) unique,
idpais char(3) references tb_paises(idpais)
)

INSERT INTO tb_paises (idpais, nompais) VALUES 
('PER', 'Perú'),
('COL', 'Colombia');


--------------------------------------------------------------------------------
-- 1. PROCEDIMIENTO ALMACENADO: usp_agrega_consumidor
--------------------------------------------------------------------------------
CREATE OR ALTER PROCEDURE usp_agrega_consumidor
    @dnicont CHAR(8),
    @nomcont VARCHAR(40),
    @apecont VARCHAR(40),
    @emailcont VARCHAR(250),
    @idpais CHAR(3),
    @resultado INT OUTPUT 
AS
BEGIN
    -- Validar que el DNI sea numérico y tenga exactamente 8 dígitos
    IF LEN(@dnicont) <> 8 OR @dnicont LIKE '%[^0-9]%'
    BEGIN
        SET @resultado = 0;
        RETURN;
    END

    -- Validar que el nombre y apellido no estén vacíos o en blanco
    IF LTRIM(RTRIM(@nomcont)) = '' OR LTRIM(RTRIM(@apecont)) = ''
    BEGIN
        SET @resultado = 0;
        RETURN;
    END

    -- Intentar la inserción
    BEGIN TRY
        INSERT INTO tb_Consumidor (dnicont, nomcont, apecont, emailcont, idpais)
        VALUES (@dnicont, @nomcont, @apecont, @emailcont, @idpais);
        
        SET @resultado = 1; 
    END TRY
    BEGIN CATCH
        SET @resultado = -1;
    END CATCH
END;


CREATE OR ALTER PROCEDURE sp_obtenerConsumidorDni
@Dni CHAR(8)
AS
BEGIN
SELECT * FROM tb_Consumidor where dnicont = @Dni
END;


--eliminar consumidor
CREATE OR ALTER PROCEDURE usp_eliminar_consumidor
    @dnicont CHAR(8) 
AS
BEGIN
        DELETE FROM tb_Consumidor WHERE dnicont = @dnicont;
END



--listar consumidor
CREATE OR ALTER PROCEDURE usp_listar_consumidor
AS
BEGIN
    SELECT dnicont, nomcont, apecont, emailcont, idpais 
    FROM tb_Consumidor;
END;



CREATE OR ALTER PROCEDURE usp_editar_consumidor
    @dnicont CHAR(8),
    @nomcont VARCHAR(40),
    @apecont VARCHAR(40),
    @emailcont VARCHAR(250),
    @idpais CHAR(3),
    @resultado INT OUTPUT 
AS
BEGIN
    -- Validar que el DNI sea numérico y tenga exactamente 8 dígitos
    IF LEN(@dnicont) <> 8 OR @dnicont LIKE '%[^0-9]%'
    BEGIN
        SET @resultado = 0;
        RETURN;
    END

    -- Validar que el nombre y apellido no estén vacíos o en blanco
    IF LTRIM(RTRIM(@nomcont)) = '' OR LTRIM(RTRIM(@apecont)) = ''
    BEGIN
        SET @resultado = 0;
        RETURN;
    END

    -- Intentar la inserción
    BEGIN TRY
    UPDATE tb_Consumidor SET nomcont = @nomcont, apecont = @apecont, emailcont = @emailcont, idpais = @idpais  Where dnicont = @dnicont
        
     IF @@ROWCOUNT > 0
            SET @resultado = 1;
        else
            SET @resultado = 0;

    END TRY
    BEGIN CATCH
        SET @resultado = -1;
    END CATCH
END;
