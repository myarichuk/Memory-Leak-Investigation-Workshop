using Raven.Client.Documents;
using Raven.Embedded;
using ShoppingCartService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tests.Infrastructure;

namespace ShoppingCartService
{
    public class DataHandler
    {
        private readonly IDocumentStore _store;

        public DataHandler(IDocumentStore store) => _store = store;

        //should run only once
        public async Task EnsureSampleDataAsync()
        {
            using var session = _store.OpenAsyncSession();
            if(await session.Query<Product>().CountAsync() == 0)
            {
                _store.Maintenance.Send(new CreateSampleDataOperation());
            }

            EmbeddedServer.Instance.OpenStudioInBrowser();
        }

        internal void Put<T>(string id, T document)
        {
            using var session = _store.OpenSession();
            session.Store(document, id);

            session.SaveChanges();
        }

        internal void Put<T>(T document)
        {
            using var session = _store.OpenSession();
            session.Store(document);

            session.SaveChanges();
        }


        internal T GetById<T>(string id)
        {
            using var session = _store.OpenSession();
            return session.Load<T>(id);
        }

        internal IEnumerable<T> Query<T>(int skip = 0, int take = 4096)
        {
            using var session = _store.OpenSession();
            return session.Query<T>().Skip(skip).Take(take).ToList();
        }
    }
}
