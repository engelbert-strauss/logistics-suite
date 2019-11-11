using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace LogisticsSuite.Infrastructure.Persistence
{
	public static class MongoHelper
	{
		public static IMongoCollection<BacklogDocument> AddBacklogCollection(IServiceProvider serviceProvider) =>
			serviceProvider.GetService<IMongoClient>().GetDatabase("erp").GetCollection<BacklogDocument>("orders");

		public static IMongoClient AddClient(IServiceProvider serviceProvider) =>
			new MongoClient(serviceProvider.GetService<IConfiguration>()["MongoConnection:ConnectionString"]);

		public static IMongoCollection<OrderDocument> AddOrderCollection(IServiceProvider serviceProvider) =>
			serviceProvider.GetService<IMongoClient>().GetDatabase("warehouse").GetCollection<OrderDocument>("orders");

		public static IMongoCollection<ParcelDocument> AddParcelCollection(IServiceProvider serviceProvider) =>
			serviceProvider.GetService<IMongoClient>().GetDatabase("warehouse").GetCollection<ParcelDocument>("parcels");
	}
}
