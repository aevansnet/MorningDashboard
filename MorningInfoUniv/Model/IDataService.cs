using System.Threading.Tasks;

namespace MorningInfoUniv.Model
{
    public interface IDataService
    {
        Task<DataItem> GetData();
    }
}