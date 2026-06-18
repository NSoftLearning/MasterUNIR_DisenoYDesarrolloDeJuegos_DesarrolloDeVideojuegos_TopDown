using UnityEngine;

//Unico singleton
public class ComponentLocatorService
{
    public static ComponentsLibrary Components => _componentsLibrary;


    private static ComponentsLibrary _componentsLibrary;

    public static void BuildComponentsLibrary(ComponentsLibrary componentsLibrary)
    {
        _componentsLibrary = componentsLibrary; ;
    }
}
