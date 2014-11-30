
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using WindowsFormsApplication1;
using Gibbed.IO;

namespace Gibbed.Rebirth.FileFormats
{
    public class StageBinaryFile
    {
        private readonly List<Room> _Rooms;

        public List<Room> Rooms
        {
            get { return this._Rooms; }
        }

        public String[ ] RoomNames
        {
            get
            {
                var returned = new string[_Rooms.Count];
                for (var i = 0; i < returned.Length; i++)
                {
                    returned[i] = _Rooms[i].FullName;
                }
                return returned;
            }
        }
        public StageBinaryFile()
        {
            this._Rooms = new List<Room>();
        }

        public void Serialize(Stream output)
        {
            const Endian endian = Endian.Little;

            output.WriteValueS32(this._Rooms.Count, endian);
            foreach (var room in this._Rooms)
            {
                output.WriteValueU32(room.Type, endian);
                output.WriteValueU32(room.Variant, endian);
                output.WriteValueU8(room.Difficulty);
                output.WriteString(room.Name, endian);
                output.WriteValueF32(room.Weight, endian);
                output.WriteValueU8(room.Width);
                output.WriteValueU8(room.Height);

                var doorCount = room.Doors == null ? 0 : room.Doors.Length;
                if (doorCount > byte.MaxValue)
                {
                    throw new InvalidOperationException();
                }

                var spawnCount = room.Spawns == null ? 0 : room.Spawns.Length;
                if (spawnCount > ushort.MaxValue)
                {
                    throw new InvalidOperationException();
                }

                output.WriteValueU8((byte)doorCount);
                output.WriteValueU16((ushort)spawnCount, endian);

                if (room.Doors != null)
                {
                    foreach (var door in room.Doors)
                    {
                        output.WriteValueS16(door.X, endian);
                        output.WriteValueS16(door.Y, endian);
                        output.WriteValueB8(door.Exists);
                    }
                }

                if (room.Spawns != null)
                {
                    foreach (var spawn in room.Spawns)
                    {
                        output.WriteValueS16(spawn.X, endian);
                        output.WriteValueS16(spawn.Y, endian);

                        var entityCount = spawn.Entities == null ? 0 : spawn.Entities.Length;
                        if (entityCount > byte.MaxValue)
                        {
                            throw new InvalidOperationException();
                        }

                        output.WriteValueU8((byte)entityCount);

                        if (spawn.Entities != null)
                        {
                            foreach (var entity in spawn.Entities)
                            {
                                output.WriteValueU16(entity.Type, endian);
                                output.WriteValueU16(entity.Variant, endian);
                                output.WriteValueU16(entity.Subtype, endian);
                                output.WriteValueF32(entity.Weight, endian);
                            }
                        }
                    }
                }
            }
        }

        public void Deserialize(Stream input)
        {
            const Endian endian = Endian.Little;

            var roomCount = input.ReadValueU32(endian);

            var rooms = new Room[roomCount];
            for (uint i = 0; i < roomCount; i++)
            {
                var room = new Room();
                room.Type = input.ReadValueU32(endian);
                room.Variant = input.ReadValueU32(endian);
                room.Difficulty = input.ReadValueU8();
                room.Name = input.ReadString(endian);
                room.Weight = input.ReadValueF32(endian);
                room.Width = input.ReadValueU8();
                room.Height = input.ReadValueU8();

                var doorCount = input.ReadValueU8();
                var spawnCount = input.ReadValueU16(endian);

                room.Doors = new Door[doorCount];
                for (int j = 0; j < doorCount; j++)
                {
                    var door = new Door();
                    door.X = input.ReadValueS16(endian);
                    door.Y = input.ReadValueS16(endian);
                    door.Exists = input.ReadValueB8();
                    room.Doors[j] = door;
                }

                room.Spawns = new Spawn[spawnCount];
                for (int j = 0; j < spawnCount; j++)
                {
                    var spawn = new Spawn();
                    spawn.X = input.ReadValueS16(endian);
                    spawn.Y = input.ReadValueS16(endian);

                    var entityCount = input.ReadValueU8();

                    spawn.Entities = new Entity[entityCount];
                    for (int k = 0; k < entityCount; k++)
                    {
                        var entity = new Entity();
                        entity.Type = input.ReadValueU16(endian);
                        entity.Variant = input.ReadValueU16(endian);
                        entity.Subtype = input.ReadValueU16(endian);
                        entity.Weight = input.ReadValueF32(endian);
                        spawn.Entities[k] = entity;
                    }

                    room.Spawns[j] = spawn;
                }

                rooms[i] = room;
            }

            this._Rooms.Clear();
            this._Rooms.AddRange(rooms);
        }

        public struct Room
        {
            public uint Type;
            public uint Variant;
            public byte Difficulty;
            public string Name;
            public float Weight;
            public byte Width;
            public byte Height;
            public Door[] Doors;
            public Spawn[] Spawns;

            public String FindEntityNames ( string ButtonName )
            {
                return this.FindEntities(ButtonName).Aggregate("", (current, entity) => current + (EntityStore.getName(entity.Type, entity.Variant, entity.Subtype) + ", ")).Trim().Trim(',');
            }
            public Entity[] FindEntities ( string ButtonName )
            {
                return FindEntities ( int.Parse(ButtonName.Split ( ',' ) [ 0 ]), int.Parse(ButtonName.Split ( ',' ) [ 1 ]) );
            }
            public Spawn FindSpawn(int x, int y)
            {
                foreach (var entities in from spawn in this.Spawns where spawn.X == x && spawn.Y == y select spawn)
                {
                    return entities;
                }
                return new Spawn(){X = -1, Y = -1};
            }
            public Entity[ ] FindEntities ( int x, int y )
            {
                foreach ( var entities in from spawn in this.Spawns where spawn.X == x && spawn.Y == y select spawn.Entities ) {
                    return entities;
                }
                return new Entity[ ] { };
            }

            public String FullName
            {
                get { return Type + " - " + Name + ": " + Variant; }
            }
            public void AddSpawn(Spawn toAdd)
            {
                var backup = Spawns;
                Spawns = new Spawn[backup.Length + 1];
                for (var i = 0; i < backup.Length; i++)
                {
                    this.Spawns[i] = backup[i];
                }
                Spawns[Spawns.Length - 1] = toAdd;
            }
            public void RemoveSpawnAt(int index)
            {
                var backup = Spawns;
                Spawns = new Spawn[backup.Length - 1];
                for (var i = 0; i < index; i++)
                {
                    this.Spawns[i] = backup[i];
                }
                for (var i = index; i < Spawns.Length; i++)
                {
                    this.Spawns[i] = backup[i + 1];
                }
            }
            public void AddDoor(Door toAdd)
            {
                var backup = Doors;
                Doors = new Door[backup.Length + 1];
                for (var i = 0; i < backup.Length; i++)
                {
                    this.Doors[i] = backup[i];
                }
                Doors[Doors.Length - 1] = toAdd;
            }
            public void RemoveDoorAt(int index)
            {
                var backup = Doors;
                Doors = new Door[backup.Length - 1];
                for (var i = 0; i < index; i++)
                {
                    this.Doors[i] = backup[i];
                }
                for (var i = index; i < Spawns.Length; i++)
                {
                    this.Doors[i] = backup[i + 1];
                }
            }
        }

        public struct Door
        {
            public short X;
            public short Y;
            public bool Exists;
        }

        public struct Spawn
        {
            public short X;
            public short Y;
            public Entity[] Entities;

            public string[ ] Names
            {
                get
                {
                    string[] entries = new string[Entities.Length];
                    for ( int i = 0; i < Entities.Length; i++ )
                        entries [ i ] = EntityStore.getName (
                                                             Entities [ i ].Type,
                                                             Entities [ i ].Variant,
                                                             Entities [ i ].Subtype );
                    return entries;
                }
            }

            public void AddEntity ( Entity toAdd )
            {
                if(Entities == null)
                    Entities = new Entity[0];
                Entity[ ] backup = Entities;
                Entities = new Entity[ backup.Length + 1 ];
                for ( int i = 0; i < backup.Length; i++ )
                {
                    this.Entities[i] = backup[i];
                }
                Entities [ Entities.Length - 1 ] = toAdd;
            }
            public void RemoveAt(int index)
            {

                if ( Entities == null ) return;
                Entity[] backup = Entities;
                Entities = new Entity[backup.Length - 1];
                for (int i = 0; i < index; i++)
                {
                    this.Entities[i] = backup[i];
                }
                for (int i = index; i < Entities.Length; i++)
                {
                    this.Entities[i] = backup[i+1];
                }
            }
        }

        public struct Entity
        {
            public string name;
            public ushort Type;
            public ushort Variant;
            public ushort Subtype;
            public float Weight;

            public static Entity CreateEntity(XmlReader fromLine)
            {

                int type = -1;
                int variant = 0;
                int subtype = 0;
                double weight = 1;
                string name = "";
                string temp = fromLine.GetAttribute("type");
                if (temp != null)
                    type = int.Parse(temp);
                else
                {
                    temp = fromLine.GetAttribute("id");
                    if (temp != null)
                        type = int.Parse(temp);
                }
                temp = fromLine.GetAttribute("name");
                if (temp != null)
                    name = temp;
                temp = fromLine.GetAttribute("variant");
                if (temp != null)
                    variant = int.Parse(temp);
                temp = fromLine.GetAttribute("subtype");
                if (temp != null)
                    subtype = int.Parse(temp);
                temp = fromLine.GetAttribute("weight");
                if (temp != null)
                    weight = double.Parse(temp);
                if (type != -1)
                    return new Entity()
                           {
                               name = name,
                               Subtype = (ushort) subtype,
                               Variant = (ushort) variant,
                               Type = (ushort) type
                           };
                return new Entity ()
                       {
                           name = type + ", " + variant + ", " + subtype,

                           Subtype = (ushort) subtype,
                           Variant = (ushort) variant,
                           Type = (ushort) type
                       };
            }
        }
    }
}