using System;
using System.Collections.Generic;
using EfSqlServerIdentity.Model;

var context = new MyContext();
await context.Audits.AddAsync( new()
{
    AuthorUserName = "test",
    ChangeDateTime = DateTimeOffset.Now,
    ChangeType = 1,
    ClrTypeName = "MyType",
    RecordId = 111,
    SqlTableName = "MyTypes",
    SqlTableSchema = "dbo",
    AuditDetails = new List<AuditDetail>
    {
        new()
        {
            ClrTypeName = "String",
            OriginalValue = null,
            NewValue = "NewValue",
            PropertyName = "MyProperty"
        }
    }
} );
try
{
    await context.SaveChangesAsync();
}
catch ( Exception ex )
{
    Console.WriteLine( ex );
}

Console.WriteLine( "Done" );
Console.ReadLine();

/*
Query 1:
exec sp_executesql N'SET NOCOUNT ON;
INSERT INTO [Audits] ([AuthorUserName], [ChangeDateTime], [ChangeType], [ClrTypeName], [RecordId], [SqlTableName], [SqlTableSchema])
VALUES (@p0, @p1, @p2, @p3, @p4, @p5, @p6);
SELECT [AuditId]
FROM [Audits]
WHERE @@ROWCOUNT = 1 AND [AuditId] = scope_identity();

',N'@p0 varchar(255),@p1 datetimeoffset(7),@p2 int,@p3 varchar(255),@p4 bigint,@p5 varchar(255),@p6 varchar(255)',@p0='test',@p1='2021-06-10 21:37:31.3890125 +02:00',@p2=1,@p3='MyType',@p4=111,@p5='MyTypes',@p6='dbo'


Query 2:
exec sp_executesql N'SET NOCOUNT ON;
INSERT INTO [AuditDetails] ([AuditId], [ClrTypeName], [NewValue], [OriginalValue], [PropertyName])
VALUES (@p7, @p8, @p9, @p10, @p11);
SELECT [AuditDetailId]
FROM [AuditDetails]
WHERE @@ROWCOUNT = 1 AND [AuditDetailId] = scope_identity();

',N'@p7 bigint,@p8 varchar(255),@p9 nvarchar(4000),@p10 nvarchar(4000),@p11 varchar(255)',@p7=-9223372036854774807,@p8='String',@p9=N'NewValue',@p10=NULL,@p11='MyProperty'



-- @p7 has an invalid value
*/

/*
Microsoft.EntityFrameworkCore.DbUpdateException: An error occurred while updating the entries. See the inner exception for details.
 ---> Microsoft.Data.SqlClient.SqlException (0x80131904): The INSERT statement conflicted with the FOREIGN KEY constraint "FK_dbo.AuditDetails_dbo.Audits". The conflict occurred in database "TestDb", table "dbo.Audits", column 'AuditId'.
The statement has been terminated.
   at Microsoft.Data.SqlClient.SqlCommand.<>c.<ExecuteDbDataReaderAsync>b__188_0(Task`1 result)
   at System.Threading.Tasks.ContinuationResultTaskFromResultTask`2.InnerInvoke()
   at System.Threading.Tasks.Task.<>c.<.cctor>b__277_0(Object obj)
   at System.Threading.ExecutionContext.RunInternal(ExecutionContext executionContext, ContextCallback callback, Object state)
--- End of stack trace from previous location ---
   at System.Threading.ExecutionContext.RunInternal(ExecutionContext executionContext, ContextCallback callback, Object state)
   at System.Threading.Tasks.Task.ExecuteWithThreadLocal(Task& currentTaskSlot, Thread threadPoolThread)
--- End of stack trace from previous location ---
   at Microsoft.EntityFrameworkCore.Storage.RelationalCommand.ExecuteReaderAsync(RelationalCommandParameterObject parameterObject, CancellationToken cancellationToken)
   at Microsoft.EntityFrameworkCore.Storage.RelationalCommand.ExecuteReaderAsync(RelationalCommandParameterObject parameterObject, CancellationToken cancellationToken)
   at Microsoft.EntityFrameworkCore.Update.ReaderModificationCommandBatch.ExecuteAsync(IRelationalConnection connection, CancellationToken cancellationToken)
ClientConnectionId:f9d752a0-6348-4793-b763-4a38f38cb364
Error Number:547,State:0,Class:16
   --- End of inner exception stack trace ---
   at Microsoft.EntityFrameworkCore.Update.ReaderModificationCommandBatch.ExecuteAsync(IRelationalConnection connection, CancellationToken cancellationToken)
   at Microsoft.EntityFrameworkCore.Update.Internal.BatchExecutor.ExecuteAsync(IEnumerable`1 commandBatches, IRelationalConnection connection, CancellationToken cancellationToken)
   at Microsoft.EntityFrameworkCore.Update.Internal.BatchExecutor.ExecuteAsync(IEnumerable`1 commandBatches, IRelationalConnection connection, CancellationToken cancellationToken)
   at Microsoft.EntityFrameworkCore.Update.Internal.BatchExecutor.ExecuteAsync(IEnumerable`1 commandBatches, IRelationalConnection connection, CancellationToken cancellationToken)
   at Microsoft.EntityFrameworkCore.ChangeTracking.Internal.StateManager.SaveChangesAsync(IList`1 entriesToSave, CancellationToken cancellationToken)
   at Microsoft.EntityFrameworkCore.ChangeTracking.Internal.StateManager.SaveChangesAsync(DbContext _, Boolean acceptAllChangesOnSuccess, CancellationToken cancellationToken)
   at Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal.SqlServerExecutionStrategy.ExecuteAsync[TState,TResult](TState state, Func`4 operation, Func`4 verifySucceeded, CancellationToken cancellationToken)
   at Microsoft.EntityFrameworkCore.DbContext.SaveChangesAsync(Boolean acceptAllChangesOnSuccess, CancellationToken cancellationToken)
   at Microsoft.EntityFrameworkCore.DbContext.SaveChangesAsync(Boolean acceptAllChangesOnSuccess, CancellationToken cancellationToken)
   at <Program>$.<<Main>$>d__0.MoveNext() in C:\_git\efIdentity\EfSqlServerIdentity\EfSqlServerIdentity\Program.cs:line 28
Done

*/