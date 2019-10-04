using System;
using System.Linq;
using System.Reflection;
using StructureMap;
using StructureMap.Graph;
using StructureMap.Graph.Scanning;
using Torch.API.Managers;
using RPGCrewPlugin.Data;
using RPGCrewPlugin.Managers;

namespace RPGCrewPlugin
{
    internal class SingletonConvention<TPluginFamily> : IRegistrationConvention
    {
        public void ScanTypes(TypeSet types, Registry registry)
        {
            foreach (var type in types.FindTypes(TypeClassification.Concretes | TypeClassification.Closed)
                .Where(type => typeof(TPluginFamily).IsAssignableFrom(type)))
            {
                registry.ForSingletonOf(type);
                registry.For(typeof(TPluginFamily)).Use(c => c.GetInstance(type));
                //registry.For(type).Use(c => c.GetInstance(type));
            }
        }
    }
    
    internal static class IoC
    {
        public static void Initialize(this IContainer container)
        {
            container.Configure(_ =>
            {
                if (RPGCrewPlugin.Instance.Config.MySQL)
                {
                    _.For<IConnectionFactory>()
                        .Use<MysqlConnectionFactory>();
                }
                else if(RPGCrewPlugin.Instance.Config.SqlLite)
                {
                    _.For<IConnectionFactory>()
                        .Use<SqliteConnectionFactory>();
                }

                _.Scan(s =>
                {
                    // Assembly to scan.
                    var assemblies = AppDomain
                        .CurrentDomain
                        .GetAssemblies()
                        .Where(a => a.GetName().Name.StartsWith("RPGCrewPlugin.") 
                                    || a.GetName().Name == "RPGCrewPlugin")
                        .ToList();
                    foreach (var assembly in assemblies)
                        s.Assembly(assembly);

                    // All managers should be singletons.
                    s.Convention<SingletonConvention<BaseManager>>();
                });
                
                _.Scan(s =>
                {
                    // Assembly to scan.
                    var assemblies = AppDomain
                        .CurrentDomain
                        .GetAssemblies()
                        .Where(a => a.GetName().Name.StartsWith("RPGCrewPlugin.") 
                                    || a.GetName().Name == "RPGCrewPlugin")
                        .ToList();
                    foreach (var assembly in assemblies)
                        s.Assembly(assembly);

                    // All data providers should be singletons.
                    s.Convention<SingletonConvention<IDataProvider>>();
                });

                _.For<IMultiplayerManagerServer>()
                    .Use((context) => RPGCrewPlugin
                        .Instance
                        .Torch
                        .CurrentSession
                        .Managers
                        .GetManager(typeof(IMultiplayerManagerServer)) as IMultiplayerManagerServer);

                _.For<DefinitionResolver>()
                    .Use(new DefinitionResolver())
                    .Singleton();
            });
        }
    }
}