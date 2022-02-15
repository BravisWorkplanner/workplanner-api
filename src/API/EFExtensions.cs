using Microsoft.EntityFrameworkCore;

namespace API;

public static class EFExtensions
{
    public static void Clear<T>(this DbSet<T> set) where T : class => set.RemoveRange(set);
}