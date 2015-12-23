REST Services URL Builders for the CUAHSI Hydrologic Data Services
========
This is a basic set of services providing access to hydrologic data from multiple distributed sources, made available through the CUAHSI Water Data Center. Under the EarthCube web services project we publish GeoWS-compliant REST service wrappers over CUAHSI services

The basic data discovery and retrieval workflow in the CUAHSI HIS system includes two main steps: a) searching HIS central to identify the needed time series and web services that include them, and b) retrieving values of the time series from the identified servers. The main requests include
* HIS Central Discovery: Get listing of terms from GetSearchableConcepts
* HIS Central Discovery: Search for services in a small bounding
* HIS Central Discovery: Use a term to search for series in a small bounding
* WaterOneFlow Data Retreival : using the series and service inforamtion, retrieve data from distributed data services

Simply,
* It  proxies CUAHSI services to provide a set of query parameters defined by ECWS, and return ECWS CSV files.
* It provides a SwaggerUI client side interface to allow users to explore the REST services.

It also returns the original XML, and JSON
It provides paging over large result sets.

http://ecrest.azurewebsites.net/

Intitial prototype at: http://ecrest.azurewebsites.net/swagger/
Swagger spec: http://ecrest.azurewebsites.net/swagger/docs/1.0
