using System;
using System.Collections.Generic;
using Structurizr;
using Structurizr.Api;


namespace Structurizr
{
    class Program
    {
        static void Main(string[] args)
        {
            Workspace workspace = new Workspace("Getting Started", "This is a model of my software system.");
            Model model = workspace.Model;
            
            //People using the application
            Person visitor = model.AddPerson("Visitor", "A visitor of the site.");
            Person user = model.AddPerson("User", "An authorized user of the site");
            Person admin = model.AddPerson("Admin", "A admin of the site");

            //Our Software System
            SoftwareSystem softwareSystem = model.AddSoftwareSystem("Sensor Application", "A sensor application");
            visitor.Uses(softwareSystem, "Uses");
            user.Uses(softwareSystem, "Uses");
            admin.Uses(softwareSystem, "Uses");
            
            SoftwareSystem externalSensorService = model.AddSoftwareSystem(Location.External,"Isaac sensor service", "MQTT Input data");
            softwareSystem.Uses(externalSensorService, "Receives Data");
            //Internal Software Containers
            var mqtt = softwareSystem.AddContainer("Event Bus", "pub/sub Event Bus", "MQTT");
            mqtt.Uses(externalSensorService, "Receives Data");
            mqtt.AddTags(new []{"Event"});
            var mysql = softwareSystem.AddContainer("Business Database", "Business database", "MySQL");
            mysql.AddTags(new []{"Database"});
            var influx = softwareSystem.AddContainer("Sensor Database", "Sensor time series database", "InfluxDB");
            influx.AddTags(new []{"Database"});

            //Containers of the software system
            var frontend = softwareSystem.AddContainer("SPA Frontend", "The frontend of the application", "React.js (hosted by nginx)");
            visitor.Uses(frontend, "Interacts");
            user.Uses(frontend, "Interacts");
            admin.Uses(frontend, "Manages");

            frontend.Uses(mqtt, "Receives Data");
            
            var backend = softwareSystem.AddContainer("API Backend","The backend of the application", "ASP.NET Core");
            frontend.Uses(backend, "Uses");
            
            backend.Uses(mqtt, "Processes Data");
            mqtt.Uses(backend, "Receives Data");
            backend.Uses(mysql, "Stores Information");
            backend.Uses(influx, "Stores Data");
            
            //Frontend components
            //frontend.AddComponent("", "", "", "");
            
            
            //Backend components
            var accountService = backend.AddComponent("Account Service", "This component handles account interactions", "ASP.NET Core API Controller");
            accountService.Uses(mysql, "Stores Data");
            var authorizationService = backend.AddComponent("Authorization Service", "This component handles permissions", "ASP.NET Core API Controller");
            authorizationService.Uses(mysql, "Stores Data");
            var anomalyService = backend.AddComponent("Sensor Anomaly Service", "this component detects anomalies and logs them", "ASP.NET Core API Controller");
            anomalyService.Uses(mqtt, "Processed Data");
            anomalyService.Uses(mysql, "Stores Errors");
            var settingsService = backend.AddComponent("Sensor Settings Service", "this components managing changing settings related to sensors", "ASP.NET Core API Controller");
            settingsService.Uses(influx, "Edits preferences");
            var dataService = backend.AddComponent("Data Management", "This extracts mqtt data and populates the influxdb and then repopulates mqtt", "ASP.NET Core API Controller");
            dataService.Uses(mqtt, "Processes Data");
            dataService.Uses(influx, "Stores Data");

            frontend.Uses(accountService, "");
            frontend.Uses(authorizationService, "");
            frontend.Uses(anomalyService, "");
            frontend.Uses(settingsService, "");
            frontend.Uses(dataService, "");

            mqtt.Uses(dataService, "Receives Data");


            //Styles
            ViewSet viewSet = workspace.Views;
            Styles styles = viewSet.Configuration.Styles;

            var softwareSystemStyle = new ElementStyle(Tags.SoftwareSystem);
            softwareSystemStyle.Background = "#9bbede";
            softwareSystemStyle.Color = "#ffffff";

            var personStyle = new ElementStyle(Tags.Person);
            personStyle.Background = "#08427b";
            personStyle.Color = "#ffffff";
            personStyle.Shape = Shape.Person;
            
            var databaseStyle = new ElementStyle("Database");
            databaseStyle.Background = "#7aaede";
            databaseStyle.Shape = Shape.Cylinder;
            
            var eventStyle = new ElementStyle("Event");
            eventStyle.Background = "#9bbede";
            eventStyle.Shape = Shape.Cylinder;

            viewSet.Configuration.Styles.Add(softwareSystemStyle);
            viewSet.Configuration.Styles.Add(personStyle);
            viewSet.Configuration.Styles.Add(databaseStyle);
            viewSet.Configuration.Styles.Add(eventStyle);
            
            //Views

            SystemContextView contextView = viewSet.CreateSystemContextView(softwareSystem, "SystemContext", "The System Context diagram");
            contextView.AddAllSoftwareSystems();
            contextView.AddAllPeople();
            
            ContainerView containerView = viewSet.CreateContainerView(softwareSystem, "Container", "The Container Diagram");
            containerView.AddAllPeople();
            containerView.AddAllContainers();
            containerView.AddAllSoftwareSystems();


            ComponentView backendView =
                viewSet.CreateComponentView(backend, "Component", "The Backend Diagram");
            backendView.AddNearestNeighbours(backend);
            foreach (Component comp in backend.Components)
            {
                backendView.Add(comp);
            }

            backendView.Remove(backend);
            
            //Upload
            StructurizrClient structurizrClient = new StructurizrClient(" 8afe380d-d3a2-46a5-9f24-0aa290cb2455", "2219e66c-d2d3-4a6a-bb84-c9700f15d77c");
            structurizrClient.PutWorkspace(59476, workspace);
        }
    }
}