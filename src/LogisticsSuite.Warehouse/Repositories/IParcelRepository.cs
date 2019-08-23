using LogisticsSuite.Infrastructure.Dtos;

namespace LogisticsSuite.Warehouse.Repositories
{
	public interface IParcelRepository
	{
		ParcelDto Dequeue();

		void Enqueue(ParcelDto parcel);

		int GetCount();

		int GetNextParcelNo();
	}
}
