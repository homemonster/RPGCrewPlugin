using System;
using System.IO;
using System.Linq;
using System.Reflection;
//using RPGCrewPlugin.Data.Types;

namespace RPGCrewPlugin.Resources
{
    /*
    public static class NameGeneratorHelper
    {
        public static string GetIndustryName(IndustryTypeEnum industryType)
        {
            var manifestResource = "";
            switch (industryType)
            {
                case IndustryTypeEnum.Consumer:
                    manifestResource = "RPGCrewPlugin.Resources.Corporations.Consumer.txt";
                    break;
                case IndustryTypeEnum.Military:
                    manifestResource = "RPGCrewPlugin.Resources.Corporations.Military.txt";
                    break;
                case IndustryTypeEnum.Research:
                    manifestResource = "RPGCrewPlugin.Resources.Corporations.Research.txt";
                    break;
                case IndustryTypeEnum.Industrial:
                    manifestResource = "RPGCrewPlugin.Resources.Corporations.Industrial.txt";
                    break;
                default:
                    return $"Testificate {industryType} Station";
            }

            using (Stream stream = Assembly
                .GetAssembly(typeof(RPGCrewPlugin))
                .GetManifestResourceStream(manifestResource)) 
            {
                using (var reader = new StreamReader(stream))
                {
                    var choices = reader.ReadToEnd().Split('\n');
                    return choices.OrderBy(g => Guid.NewGuid()).First();
                }
            }
        }

        public static string GetName()
        {
            using (Stream stream = Assembly
                .GetAssembly(typeof(RPGCrewPlugin))
                .GetManifestResourceStream("RPGCrewPlugin.Resources.Names.txt")) 
            {
                using (var reader = new StreamReader(stream))
                {
                    var choices = reader.ReadToEnd().Split('\n');
                    return choices.OrderBy(g => Guid.NewGuid()).First();
                }
            }
        }
    }*/
}