# Weather API

## Client-Side Webpage

### Description
A simple HTML page that allows the client to choose a city and get the weather information for the selected city.
Calls the server-side API to get the cities, with pagination to avoid loading all the cities at once.
Requests the weather information for the selected city from the server-side API.

## Server-Side API
Populates a list of cities from a csv file (to be done using a database in future changes).
Provides the pages of cities to the client-side API.
Provides the weather information for a selected city to the client-side API, using the Open Weather Map API.


## Testing

### Development Environment

Initially everything was testing locally, buy running the ASP.NET Core application on a local host and accessing the webpage from the browser through a live server. Some issues were found due to the CORS policy, but were treated.

### Production Environment

The API was deployed to Azure App Service, and the webpage was deployed to Azure Static Web Apps. Only things needed were to change the endpoints as required.