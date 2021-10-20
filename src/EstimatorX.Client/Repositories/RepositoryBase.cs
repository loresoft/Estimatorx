using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using FluentRest;

namespace EstimatorX.Client.Repositories
{
    public abstract class RepositoryBase<TModel>
    {
        protected RepositoryBase(HttpClient httpClient)
        {
            HttpClient = httpClient;
        }

        protected HttpClient HttpClient { get; }

        public async Task<TModel> Get(string id, string partition = null)
        {
            var result = await HttpClient.GetAsync<TModel>(b => b
                .AppendPath(GetBasePath())
                .AppendPath(id)
                .AppendPath(partition ?? id)
            );

            return result;
        }

        public async Task<IReadOnlyCollection<TModel>> All()
        {
            var result = await HttpClient.GetAsync<List<TModel>>(b => b
                .AppendPath(GetBasePath())
            );

            return result;
        }

        public async Task<TModel> Create(TModel model)
        {
            var result = await HttpClient.PostAsync<TModel>(b => b
                .AppendPath(GetBasePath())
                .Content(model)
            );

            return result;
        }

        public async Task<TModel> Update(string id, TModel model, string partition = null)
        {
            var result = await HttpClient.PutAsync<TModel>(b => b
                .AppendPath(GetBasePath())
                .AppendPath(id)
                .AppendPath(partition ?? id)
                .Content(model)
            );

            return result;
        }

        public async Task<TModel> Delete(string id, string partition = null)
        {
            var result = await HttpClient.DeleteAsync<TModel>(b => b
                .AppendPath(GetBasePath())
                .AppendPath(id)
                .AppendPath(partition ?? id)
            );

            return result;
        }


        protected abstract string GetBasePath();
    }
}