using Benchy.DapperVsEfCore.Database.Repositories;

namespace Benchy.DapperVsEfCore.Database.Factories;

public static class RepositoryFactory
{
    private static readonly IRepository EfSplitQueryRepository = new EfCoreRepository();
    private static readonly IRepository DapperSplitQueryRepository = new DapperRepository();

    public static IRepository EfCore() => EfSplitQueryRepository;
    public static IRepository Dapper() => DapperSplitQueryRepository;
}