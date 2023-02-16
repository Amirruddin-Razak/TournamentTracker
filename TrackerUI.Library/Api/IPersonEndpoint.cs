using TrackerUI.Library.Models;

namespace TrackerUI.Library.Api;

public interface IPersonEndpoint
{
    Task<int> CreatePersonAsync(PersonModel person);

    Task<List<PersonModel>> GetAllPersonAsync();
}