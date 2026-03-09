using Microsoft.AspNetCore.Mvc.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace APP.Repositories.Interceptors;

public class AuditDbContextInterceptor : SaveChangesInterceptor
{
	private static readonly Dictionary<EntityState, Action<DbContext, IAuditEntity>> Behaviours = new()
	{
		{ EntityState.Added,AddBehavior},
		{ EntityState.Modified,ModifiedBehavior}
	};

	private static void AddBehavior(DbContext context, IAuditEntity auditEntity)
	{
		auditEntity.Created = DateTime.Now;
		context.Entry(auditEntity).Property(x => x.Updated).IsModified = false;
	}
	private static void ModifiedBehavior(DbContext context, IAuditEntity auditEntity)
	{
		context.Entry(auditEntity).Property(x => x.Created).IsModified = false;
		auditEntity.Updated = DateTime.Now;
	}
	public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
	{

		foreach (var entityEntry in eventData.Context!.ChangeTracker.Entries().ToList())
		{
			if (entityEntry.Entity is not IAuditEntity auditEntity) continue;

			Behaviours[entityEntry.State](eventData.Context, auditEntity);
		}

		return base.SavingChangesAsync(eventData, result, cancellationToken);
	}
}
