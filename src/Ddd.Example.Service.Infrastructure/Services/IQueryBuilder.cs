using System;

namespace Ddd.Example.Service.Infrastructure.Services
{

    public interface IQueryBuilder<in TRequest>
    {

        Uri GetQuery(TRequest request);
    }
}
