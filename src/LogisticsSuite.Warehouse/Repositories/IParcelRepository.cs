using System.Threading.Tasks;
using LogisticsSuite.Infrastructure.Dtos;

namespace LogisticsSuite.Warehouse.Repositories
{
	public interface IParcelRepository
	{
		Task<ParcelDto> DequeueAsync();

		Task EnqueueAsync(ParcelDto parcel);

		int GetNextParcelNo();
	}
}
