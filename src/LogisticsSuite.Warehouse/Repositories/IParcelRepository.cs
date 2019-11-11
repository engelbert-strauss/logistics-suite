using System.Threading.Tasks;
using LogisticsSuite.Infrastructure.Dtos;

namespace LogisticsSuite.Warehouse.Repositories
{
	public interface IParcelRepository
	{
		Task<ParcelDto> DeleteAsync();

		int GetNextParcelNo();

		Task InsertAsync(ParcelDto parcel);
	}
}
