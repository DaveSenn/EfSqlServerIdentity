/*
* CREATE database
*/
IF ( DB_ID( N'$(DbName)' ) IS NULL )
    BEGIN
        PRINT 'Database $(DbName) does not exist. Creating database $(DbName)...'

        DECLARE @createStatement VARCHAR(max) = '
            CREATE DATABASE $(DbName) ON
            (
                FILENAME = ''' + CONVERT( VARCHAR(MAX), SERVERPROPERTY( 'InstanceDefaultDataPath' ) ) + '$(DbName).mdf'',
                NAME = $(DbName),
                SIZE = $(DbSize),
                MAXSIZE = UNLIMITED,
                FILEGROWTH = $(DbFileGrowth)
            )
            LOG ON
            (
                FILENAME = ''' + CONVERT( VARCHAR(MAX), SERVERPROPERTY( 'InstanceDefaultLogPath' ) ) + '$(DbName)_log.ldf'',
                NAME = $(DbName)_Log,
                SIZE = $(LogSize),
                MAXSIZE = UNLIMITED,
                FILEGROWTH = $(LogFileGrowth)
            )
            COLLATE $(Collation)
        ';
        EXEC( @createStatement );
        
        -- Set AUTO_CLOSE database option to OFF for better performance
        EXEC( 'ALTER DATABASE $(DbName) SET AUTO_CLOSE OFF') ;
    END
GO