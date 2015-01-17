<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Wb._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron">
        <h1>Hydrologic Data Service</h1>
        <p class="lead">This is a demonstration for the GeoWS Earthcube  Building Block service for hydrologic data available through CUAHSI</p>
        <p><a href="http://workspace.earthcube.org/geows" class="btn btn-primary btn-lg">Learn more &raquo;</a></p>
    </div>
              <div class="row">
          <div class="col-md-8">
                <h2>REST Services URL Builders for the CUAHSI Hydrologic Data  Services</h2>
              </div>
          
      </div>   

    <div class="row">
       <div class="col-md-8">
 
            <p>
 This is a basic set of services providing access to hydrologic data from multiple distributed sources, made available through the CUAHSI Water Data Center. Under the EarthCube web services project we publish GeoWS-compliant REST service wrappers over CUAHSI services

            </p>
           The basic data discovery and retrieval workflow in the CUAHSI HIS system includes two main steps: a) searching HIS central to identify the needed time series and web services that include them, and b) retrieving values of the time series from the identified servers. The main requests include
           <p>
               
           </p>
                    <ul >
                        <li> <a href="http://hiscentral.cuahsi.org/">HIS Central </a> Discovery: Get listing of terms from GetSearchableConcepts</li>
                        <li> <a href="http://hiscentral.cuahsi.org/">HIS Central </a> Discovery: Search for services in a small bounding</li>
                        <li> <a href="http://hiscentral.cuahsi.org/">HIS Central </a> Discovery: Use a term to search for series in a small bounding</li>
                        <li> <a href="http://his.cuahsi.org/wofws.html">WaterOneFlow</a> Data Retreival : using the series and service inforamtion, retrieve data from distributed data services</li>
                    </ul>
                </div>
    <div>    Please visit the  <a href="http://his.cuahsi.org">CUAHSI HIS Website</a> ,
         and the <a href="https://www.cuahsi.org/wdc">CUAHSI Water Data Center </a> for more information. 
        The list of available hydrologic data servers is at <a href="http://hiscentral.cushi.org"> hiscentral.cuahsi.org.  </a> 

				
				</div>
        <div>The Geo-WS REST services for hydrologic data follow the same basic data discovery and retrieval workflow. </div>
       </div>
         
        <div class="col-md-4">
           
            <p>
             <!--   <a runat="server" class="btn btn-default" href="~/swagger/">REST UI for the WaterOneFlow services</a> -->
                  <a runat="server" class="btn btn-primary btn-lg" href="~/SwaggerUI/EcRest.html">REST UI for the Hydrologic Data Services</a>

            </p>
        </div>


</asp:Content>
