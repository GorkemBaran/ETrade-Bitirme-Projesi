using AppCore.Records.Bases;
using AppCore.Results.Bases;

namespace AppCore.Business.Services.Bases
{
    // Controller -> (Model) ->  Service -> (Entity) -> Repo -> (DbContext) -> DB
    // Controller <- (Model) <-  Service <- (Entity) <- Repo <- (DbContext) <- DB
    public interface IService<TModel> : IDisposable where TModel : RecordBase, new()
    {
        IQueryable<TModel> Query();
        Result Add(TModel model);
        Result Update(TModel model);
        Result Delete(int id);
    }
}
