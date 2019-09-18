using System.Threading.Tasks;

namespace LogisticsSuite.Infrastructure.Caching
{
	/// <summary>
	///     This interface will allow the caller to use any distributed cache in an abstract way.
	/// </summary>
	public interface IDistributedCache
	{
		/// <summary>
		///     Gets a value from the distributed cache.
		/// </summary>
		/// <param name="key">The lookup key inside the distributed cache.</param>
		/// <returns>The possible value retrieves from the distributed cache.</returns>
		Task<int?> GetValueAsync(string key);

		/// <summary>
		///     Sets a value inside the distributed cache.
		/// </summary>
		/// <param name="key">The lookup key inside the distributed cache.</param>
		/// <param name="value">The new value for the associated <paramref name="key" /> inside the distributed cache.</param>
		Task SetValueAsync(string key, int value);
	}
}
