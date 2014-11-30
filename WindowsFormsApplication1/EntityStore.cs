using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml;
using Gibbed.Rebirth.FileFormats;

namespace WindowsFormsApplication1
{
    public class EntityStore
    {
        private static List < StageBinaryFile.Entity > entities = new List < StageBinaryFile.Entity > ();
        public static void clearEntities () { entities.Clear(); }

        public static void LoadEntities ( XmlReader reader )
        {
            while (!reader.EOF)
            {
                Debug.WriteLine(reader.Name);
                if (reader.Name.Contains("entity"))
                {
                    entities.Add(StageBinaryFile.Entity.CreateEntity(reader));
                }
                reader.Read ();
            }
            reader.Close();
            cleanEntities();
        }

        private static void cleanEntities ()
        {
            for ( int i = 0; i < entities.Count; i++ )
            {
                if(entities[i].Type == 65535)
                    entities.RemoveAt(i--);
            }
        }

        public static string[] getNames ()
        {
            string[] ret = new string[entities.Count];
            for ( int i = 0; i < ret.Length; i++ )
            {
                ret [ i ] = entities [ i ].name;
            }
            return ret;
        }

        public static string getName ( int type, int variant, int subtype )
        {
            var firstOrDefault = entities.FirstOrDefault ( entity => matches ( entity, type, variant, subtype ));
            if(firstOrDefault.name != null)
                return (firstOrDefault.name );
            return type + ":" + variant + ":" + subtype;
        }

        private static bool matches ( StageBinaryFile.Entity entity, int type, int variant, int subtype )
        {
            return entity.Type == type && entity.Subtype == subtype && entity.Variant == variant;
        }

        public static StageBinaryFile.Entity findByName ( string selectedValue )
        {
            return entities.FirstOrDefault ( entity => matches ( entity, selectedValue ) );
        }

        private static bool matches ( StageBinaryFile.Entity entity, string selectedValue ) { return entity.name.Equals(selectedValue); }

        public static StageBinaryFile.Entity findByID ( int type, int variant, int subtype )
        {
            return entities.FirstOrDefault ( entity => matches ( entity, type, variant, subtype ) );
        }
    }
}