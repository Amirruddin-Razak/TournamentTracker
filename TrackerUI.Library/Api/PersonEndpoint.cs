using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TrackerUI.Library.Api.Helper;
using TrackerUI.Library.Models;

namespace TrackerUI.Library.Api;

public class PersonEndpoint : IPersonEndpoint
{
    private readonly IApiConnector _apiConnector;

    public PersonEndpoint(IApiConnector apiConnector)
    {
        _apiConnector = apiConnector;
    }

    public async Task<PersonModel> CreatePersonAsync(PersonModel person)
    {
        try
        {
            using HttpResponseMessage response = await _apiConnector.ApiClient.PostAsJsonAsync("Person/CreatePerson", person);

            if (response.StatusCode == HttpStatusCode.Created)
            {
                var createdPerson = await response.Content.ReadAsAsync<PersonModel>();
                return createdPerson;
            }
            else
            {
                throw new Exception(response.ReasonPhrase);
            }
        }
        catch (Exception)
        {
            // TODO add log
            throw;
        }
    }

    public async Task<List<PersonModel>> GetAllPersonAsync()
    {
        try
        {
            using HttpResponseMessage response = await _apiConnector.ApiClient.GetAsync("Person/GetAll");

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var personList = await response.Content.ReadAsAsync<List<PersonModel>>();
                return personList;
            }
            else if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return new List<PersonModel>();
            }
            else
            {
                throw new Exception(response.ReasonPhrase);
            }
        }
        catch (Exception)
        {
            // TODO add log
            throw;
        }
    }
}
