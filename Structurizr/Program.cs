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
            
            
            //Objects
            Person visitor = model.AddPerson("Visitor", "A visitor of the site.");
            Person user = model.AddPerson("User", "An authorized user of the site");
            Person admin = model.AddPerson("Admin", "A admin of the site");

            SoftwareSystem softwareSystem = model.AddSoftwareSystem("Sensor Application", "A video site");
            visitor.Uses(softwareSystem, "Uses");
            user.Uses(softwareSystem, "Uses");
            admin.Uses(softwareSystem, "Uses");
            

            //Views
            ViewSet viewSet = workspace.Views;
            
            SystemContextView contextView = viewSet.CreateSystemContextView(softwareSystem, "SystemContext", "The System Context diagram");
            contextView.AddAllSoftwareSystems();
            contextView.AddAllPeople();
            
            ContainerView containerView = viewSet.CreateContainerView(softwareSystem, "Container", "The Container Diagram");
            containerView.AddAllPeople();
            containerView.AddAllContainers();
            containerView.AddAllSoftwareSystems();
            
            /*
            ComponentView componentView =
                viewSet.CreateComponentView(container, "Component", "A Component Diagram (of an Container)");
            componentView.AddNearestNeighbours(container);
            foreach (Component comp in container)
            {
                componentView.Add(comp);
            }
            */
            
            //Styles
            Styles styles = viewSet.Configuration.Styles;

            var elementStyles = new List<ElementStyle>();
            
            var softwareSystemStyle = new ElementStyle(Tags.SoftwareSystem);
            softwareSystemStyle.Background = "#1168bd";
            softwareSystemStyle.Color = "#ffffff";

            var personStyle = new ElementStyle(Tags.Person);
            personStyle.Background = "#08427b";
            personStyle.Color = "#ffffff";
            personStyle.Shape = Shape.Person;

            styles.Elements.Add(softwareSystemStyle);
            styles.Elements.Add(personStyle);
            
            //Upload
            StructurizrClient structurizrClient = new StructurizrClient(" 8afe380d-d3a2-46a5-9f24-0aa290cb2455", "2219e66c-d2d3-4a6a-bb84-c9700f15d77c");
            structurizrClient.PutWorkspace(59476, workspace);
        }
    }
}