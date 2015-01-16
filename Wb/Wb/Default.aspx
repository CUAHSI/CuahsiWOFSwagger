<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Wb._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron">
        <h1>Demonstration Service</h1>
        <p class="lead">This is a demonstration for the GeoWS Earthcube Building Block</p>
        <p><a href="http://workspace.earthcube.org/geows" class="btn btn-primary btn-lg">Learn more &raquo;</a></p>
    </div>

    <div class="row">
       <div class="col-md-8">
            <h2>REST Services URL Builders for the CUAHSI Services</h2>

            <p>
                This is a basic set of services documentaiton for the Earthcube web services project. 
                <b>Basic CUAHSI workflow applies.</b> for details about this go to <a href="http://his.cuahsi.org/">CUAHSI HIS</a>
                    Search HIS Central, and retrieve series, and service information to access these data series.
                    Go to each service, and retrieve data.
            </p>
                            <div> For CUAHSI services the steps to discover and access data are as follows:
                    <ul >
                        <li> <a href="http://hiscentral.cuahsi.org/">HIS Central </a> Discovery: Get listing of terms from GetSearchableConcepts</li>
                        <li> <a href="http://hiscentral.cuahsi.org/">HIS Central </a> Discovery: Search for services in a small bounding</li>
                        <li> <a href="http://hiscentral.cuahsi.org/">HIS Central </a> Discovery: Use a term to search for series in a small bounding</li>
                        <li> <a href="http://his.cuahsi.org/wofws.html">WaterOneFlow</a> : using the series and service inforamtion, retrieve data from distributed data services</li>
                    </ul>
                </div>
 <div> For additional  help see the <a href="http://his.cuahsi.org">CUAHSI HIS Website</a> and details of using
				<a href="http://his.cuahsi.org/wofws.html"> WaterOneFlow web services</a> and <a href="http://hiscentral.cushi.org"> HIS Cenral </a>
				
				</div>

       </div>
         
        <div class="col-md-2">
           
            <p>
                <a runat="server" class="btn btn-default" href="~/swagger/">REST UI for the WaterOneFlow services</a>
            </p>
        </div>
    </div>

</asp:Content>
