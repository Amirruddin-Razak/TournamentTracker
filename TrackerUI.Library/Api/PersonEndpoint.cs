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

    public async Task<int> CreatePersonAsync(PersonModel person)
    {
        try
        {
            using HttpResponseMessage response = await _apiConnector.ApiClient.PostAsJsonAsync("Person/CreatePerson", person);

            if (response.StatusCode == HttpStatusCode.Created)
            {
                int createdId = await response.Content.ReadAsAsync<int>();
                return createdId;
            }
            else
            {
                throw new Exception(response.ReasonPhrase);
            }
        }
        catch (Exception ex)
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

            if (response.IsSuccessStatusCode)
            {
                var personList = await response.Content.ReadAsAsync<List<PersonModel>>();
                return personList;
            }
            else
            {
                throw new Exception(response.ReasonPhrase);
            }
        }
        catch (Exception ex)
        {
            // TODO add log
            throw;
        }
    }
}
