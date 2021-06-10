/*
* Drop database
*/

IF ( DB_ID( '$DatabaseName' ) IS NOT NULL ) 
    BEGIN
        EXEC(' ALTER DATABASE $DatabaseName SET SINGLE_USER WITH ROLLBACK IMMEDIATE' )
        EXEC(' DROP DATABASE $DatabaseName' )

        PRINT 'Droped database'
    END