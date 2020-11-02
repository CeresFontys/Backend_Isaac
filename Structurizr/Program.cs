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

            var gateway = softwareSystem.AddContainer("API Gateway", "The backend of the application", "ASP.NET Core");
            frontend.Uses(gateway, "");
            
            var accountContainer = softwareSystem.AddContainer("Account Service", "The account service", "ASP.NET Core");
            accountContainer.Uses(mysql, "Processes Data");
            {
                var component = accountContainer.AddComponent("Account Component", "");
                var component2 = accountContainer.AddComponent("Account Controller", "");
                component.Uses(mysql, "Processes Data");
                gateway.Uses(component2, "");
                component.Uses(component2, "");
            }
            

            var authorizationContainer = softwareSystem.AddContainer("Authorization Service", "The account service", "ASP.NET Core");
            authorizationContainer.Uses(mysql, "Stores Data");
            authorizationContainer.AddComponent("Authorization Component", "").Uses(mysql, "Stores Data");
            authorizationContainer.AddComponent("Authorization Controller", "").Uses(gateway, "");
            
            var anomalyContainer = softwareSystem.AddContainer("Sensor Anomaly Service", "The account service", "ASP.NET Core");
            anomalyContainer.Uses(mqtt, "Processed Data");
            anomalyContainer.Uses(mysql, "Stores Errors");
            var comp = anomalyContainer.AddComponent("Sensor Anomaly Component", "");
            comp.Uses(mqtt, "Processed Data");
            comp.Uses(mysql, "Stores Errors");
            anomalyContainer.AddComponent("Sensor Anomaly Controller", "").Uses(gateway, "");
            
            var settingsContainer = softwareSystem.AddContainer("Sensor Settings Service", "The account service", "ASP.NET Core");
            settingsContainer.Uses(influx, "Edits preferences");
            settingsContainer.AddComponent("Sensor Settings Component", "").Uses(influx, "Edits preferences");
            {
                var component = settingsContainer.AddComponent("Sensor Settings Controller", "").Uses(gateway, "");
            }
            
            
            var dataContainer = softwareSystem.AddContainer("Data Management", "The account service", "ASP.NET Core");
            dataContainer.Uses(mqtt, "Processes Data");
            mqtt.Uses(dataContainer, "Receives Data");
            dataContainer.Uses(influx, "Stores Data");

            gateway.Uses(accountContainer,"");
            gateway.Uses(authorizationContainer,"");
            gateway.Uses(anomalyContainer,"");
            gateway.Uses(settingsContainer,"");
            gateway.Uses(dataContainer,"");

            //Frontend components
            //frontend.AddComponent("", "", "", "");
            
            


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
            eventStyle.Shape = Shape.Pipe;

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

            ComponentView componentView = viewSet.CreateComponentView(accountContainer, "Component", "The Component Diagram");
            componentView.AddAllComponents();
            componentView.Add(gateway);
            componentView.Add(frontend);
            componentView.AddAllPeople();
            componentView.Add(mysql);
            
            //Upload
            StructurizrClient structurizrClient = new StructurizrClient(" 8afe380d-d3a2-46a5-9f24-0aa290cb2455", "2219e66c-d2d3-4a6a-bb84-c9700f15d77c");
            structurizrClient.PutWorkspace(59476, workspace);
        }
    }
}