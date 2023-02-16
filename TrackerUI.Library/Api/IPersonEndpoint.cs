using TrackerUI.Library.Models;

namespace TrackerUI.Library.Api;

public interface IPersonEndpoint
{
    Task<PersonModel> CreatePersonAsync(PersonModel person);

    Task<List<PersonModel>> GetAllPersonAsync();
}