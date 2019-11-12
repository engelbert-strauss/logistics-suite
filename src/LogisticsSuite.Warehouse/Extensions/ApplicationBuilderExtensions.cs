using LogisticsSuite.Infrastructure.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

// ReSharper disable once CheckNamespace
namespace Microsoft.AspNetCore.Builder
{
	public static class ApplicationBuilderExtensions
	{
		public static void CreateIndices(this IApplicationBuilder app)
		{
			var stocksCollection = app.ApplicationServices.GetService<IMongoCollection<StocksDocument>>();

			stocksCollection.Indexes.CreateMany(
				new[]
				{
					new CreateIndexModel<StocksDocument>(
						Builders<StocksDocument>.IndexKeys.Ascending(x => x.ArticleNo),
						new CreateIndexOptions { Unique = true, Background = true }),
					new CreateIndexModel<StocksDocument>(
						Builders<StocksDocument>.IndexKeys.Ascending(x => x.ReplenishmentRequested),
						new CreateIndexOptions { Background = true }),
				});
		}

		public static void InitializeStocks(this IApplicationBuilder app)
		{
			var stocksCollection = app.ApplicationServices.GetService<IMongoCollection<StocksDocument>>();
			var configuration = app.ApplicationServices.GetService<IConfiguration>();
			var initialStock = configuration.GetValue<int>("InitialStock");
			var articleNos = configuration.GetSection("Articles").Get<int[]>();

			foreach (int articleNo in articleNos)
			{
				stocksCollection.ReplaceOne(
					x => x.ArticleNo == articleNo,
					new StocksDocument
					{
						ReplenishmentRequested = false,
						ArticleNo = articleNo,
						Quantity = initialStock,
						Reserved = 0,
						Threshhold = initialStock / 2,
					},
					new UpdateOptions { IsUpsert = true });
			}
		}
	}
}
