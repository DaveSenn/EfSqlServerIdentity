SET XACT_ABORT ON
SET NOCOUNT ON

CREATE TABLE dbo.Audits (
    AuditId bigint IDENTITY(0,1) NOT NULL,
    -- AuditId bigint IDENTITY(1,1) NOT NULL, -- This works
    AuthorUserName varchar(255) NOT NULL,
    ChangeDateTime datetimeoffset(7) NOT NULL,
    ChangeType int NOT NULL,
    RecordId bigint NOT NULL,
    ClrTypeName varchar(255) NOT NULL,
    SqlTableName varchar(255) NOT NULL,
    SqlTableSchema varchar(255) NOT NULL,
    CONSTRAINT [PK_dbo.Audits] PRIMARY KEY CLUSTERED ( AuditId ),
)
CREATE INDEX [IX_Audits_ChangeDateTime]
    ON dbo.Audits( ChangeDateTime )
    INCLUDE ( ChangeType, RecordId, AuthorUserName )

CREATE TABLE dbo.AuditDetails (
    AuditDetailId bigint IDENTITY(1,1) NOT NULL,
    PropertyName varchar(255) NOT NULL,
    ClrTypeName varchar(255) NOT NULL,
    OriginalValue nvarchar(max),
    NewValue nvarchar(max),
    AuditId bigint NOT NULL,
    CONSTRAINT [PK_dbo.AuditDetails] PRIMARY KEY CLUSTERED ( AuditDetailId ),
    CONSTRAINT [FK_dbo.AuditDetails_dbo.Audits] FOREIGN KEY ( AuditId )
        REFERENCES dbo.Audits( AuditId )
        ON DELETE CASCADE
)
CREATE INDEX [IX_AuditDetails_AuditId_Include]
    ON dbo.AuditDetails( AuditId )
    INCLUDE ( PropertyName, OriginalValue, NewValue )