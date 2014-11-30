#region

using System;
using System.IO;
using System.Text;
using Gibbed.IO;

#endregion

namespace Gibbed.Rebirth.FileFormats
{
    public class AnimationBinaryFile
    {
        public type1 result;
        public void Serialize ( Stream output ) { throw new NotImplementedException (); }

        public void Deserialize ( Stream input )
        {
            const Endian endian = Endian.Little;
            var unknownType1 = new type1 { unknown1 = new type2[ input.ReadValueU32 ( endian ) ] };     //count
            for ( uint i = 0; i < unknownType1.unknown1.Length; i++ )
            {
                var unknownType2 = new type2
                                   {
                                       unknown1 = input.ReadValueU32(endian),                        //unknown0
                                       unknown2 = ReadString ( input, endian ),                         //unknown1
                                       unknown3 = new type3[ input.ReadValueU32 ( endian ) ]            //unknown2
                                   };

                for ( uint j = 0; j < unknownType2.unknown3.Length; j++ )
                {
                    unknownType2.unknown3[j] = new type3
                    {
                        unknown1 = input.ReadValueU32(endian),                                          //unknown3
                        unknown2 = ReadString(input, endian)                                            //unknown4
                    };
                }

                unknownType2.unknown4 = new type4[ input.ReadValueU32 ( endian ) ];                     //unknown5
                for ( uint j = 0; j < unknownType2.unknown4.Length; j++ )
                {
                    unknownType2.unknown4[j] = new type4    
                    {
                        unknown1 = input.ReadValueU32(endian),                                          //unknown6
                        unknown2 = input.ReadValueU32(endian),                                          //unknown7
                        unknown3 = ReadString(input, endian)                                            //unknown8
                    };
                }

                unknownType2.unknown5 = new type5[ input.ReadValueU32 ( endian ) ];                     //unknown9
                for ( uint j = 0; j < unknownType2.unknown5.Length; j++ )
                {
                    unknownType2.unknown5[j] = new type5
                    {
                        unknown1 = input.ReadValueU32(endian),                                          //unknown10
                        unknown2 = ReadString(input, endian)                                            //unknown11
                    };
                }

                unknownType2.unknown6 = new type6[ input.ReadValueU32 ( endian ) ];                     //unknown12
                for ( uint j = 0; j < unknownType2.unknown6.Length; j++ )
                {
                    unknownType2.unknown6[j] = new type6
                    {
                        unknown1 = input.ReadValueU32(endian),                                          //unknown13
                        unknown2 = ReadString(input, endian)                                            //unknown14
                    };
                }
                unknownType2.unknown7 = ReadString ( input, endian );                                   //unknown15

                unknownType2.unknown8 = new type7[ input.ReadValueU32 ( endian ) ];                     //unknown16
                for ( uint j = 0; j < unknownType2.unknown8.Length; j++ )
                {
                    var unknownType7 = new type7
                                       {
                                           unknown1 = ReadString(input, endian),                                              //unknown17
                                           unknown2 = input.ReadValueU32 ( endian ),                                             //unknown18
                                           unknown3 = input.ReadValueU8 (),                                             //unknown19
                                           unknown4 = new type8[input.ReadValueU32 ( endian )]                                             //unknown20
                                       };
                    for ( uint k = 0; k < unknownType7.unknown4.Length; k++ )
                    {
                        unknownType7.unknown4[k] = new type8
                        {
                            unknown1 = input.ReadValueF32(endian),                                      //unknown21
                            unknown2 = input.ReadValueF32(endian),                                      //unknown22
                            unknown3 = input.ReadValueF32(endian),                                      //unknown23
                            unknown4 = input.ReadValueF32(endian),                                      //unknown24
                            unknown5 = input.ReadValueU32(endian),                                      //unknown25
                            unknown6 = input.ReadValueU8(),                                             //unknown26
                            unknown7 = input.ReadValueF32(endian),                                      //unknown27
                            unknown8 = input.ReadValueF32(endian),                                      //unknown28
                            unknown9 = input.ReadValueF32(endian),                                      //unknown29
                            unknown10 = input.ReadValueF32(endian),                                     //unknown30
                            unknown11 = input.ReadValueU32(endian),                                     //unknown31
                            unknown12 = input.ReadValueU32(endian),                                     //unknown32
                            unknown13 = input.ReadValueU32(endian),                                     //unknown33
                            unknown14 = input.ReadValueU32(endian),                                     //unknown34
                            unknown15 = input.ReadValueU8()                                             //unknown35
                        };
                    }

                    unknownType7.unknown5 = new type9[ input.ReadValueU32 ( endian ) ];                 //unknown36
                    for ( uint k = 0; k < unknownType7.unknown5.Length; k++ )
                    {
                        var unknownType9 = new type9
                                           {
                                               unknown1 = input.ReadValueU32 ( endian ),                //unknown37
                                               unknown2 = input.ReadValueU8 (),                         //unknown38
                                               unknown3 = new type10[ input.ReadValueU32 ( endian ) ]   //unknown39
                                           };

                        for ( uint l = 0; l < unknownType9.unknown3.Length; l++ )
                        {
                            unknownType9.unknown3[l] = new type10
                            {
                                unknown1 = input.ReadValueF32(endian),                                  //unknown40
                                unknown2 = input.ReadValueF32(endian),                                  //unknown41
                                unknown3 = input.ReadValueF32(endian),                                  //unknown42
                                unknown4 = input.ReadValueF32(endian),                                  //unknown43
                                unknown5 = input.ReadValueF32(endian),                                  //unknown44
                                unknown6 = input.ReadValueF32(endian),                                  //unknown45
                                unknown7 = input.ReadValueF32(endian),                                  //unknown46
                                unknown8 = input.ReadValueF32(endian),                                  //unknown47
                                unknown9 = input.ReadValueF32(endian),                                  //unknown48
                                unknown10 = input.ReadValueF32(endian),                                 //unknown49
                                unknown11 = input.ReadValueU32(endian),                                 //unknown50
                                unknown12 = input.ReadValueU8(),                                        //unknown51
                                unknown13 = input.ReadValueF32(endian),                                 //unknown52
                                unknown14 = input.ReadValueF32(endian),                                 //unknown53
                                unknown15 = input.ReadValueF32(endian),                                 //unknown54
                                unknown16 = input.ReadValueF32(endian),                                 //unknown55
                                unknown17 = input.ReadValueF32(endian),                                 //unknown56
                                unknown18 = input.ReadValueF32(endian),                                 //unknown57
                                unknown19 = input.ReadValueF32(endian),                                 //unknown58
                                unknown20 = input.ReadValueF32(endian),                                 //unknown59
                                unknown21 = input.ReadValueU8()                                         //unknown60
                            };
                        }
                        unknownType7.unknown5 [ k ] = unknownType9;
                    }

                    unknownType7.unknown6 = new type11[ input.ReadValueU32 ( endian ) ];                //unknown61
                    for ( uint k = 0; k < unknownType7.unknown6.Length; k++ )
                    {
                        var unknownType11 = new type11
                                            {
                                                unknown1 = input.ReadValueU32 ( endian ),               //unknown62
                                                unknown2 = input.ReadValueU8 (),                        //unknown63
                                                unknown3 = new type8[ input.ReadValueU32 ( endian ) ]  //unknown64 
                                            };

                        for ( uint l = 0; l < unknownType11.unknown3.Length; l++ )
                        {
                            unknownType11.unknown3[l] = new type8
                            {
                                unknown1 = input.ReadValueF32(endian),                                  //unknown21
                                unknown2 = input.ReadValueF32(endian),                                  //unknown22
                                unknown3 = input.ReadValueF32(endian),                                  //unknown23
                                unknown4 = input.ReadValueF32(endian),                                  //unknown24
                                unknown5 = input.ReadValueU32(endian),                                  //unknown25
                                unknown6 = input.ReadValueU8(),                                         //unknown26
                                unknown7 = input.ReadValueF32(endian),                                  //unknown27
                                unknown8 = input.ReadValueF32(endian),                                  //unknown28
                                unknown9 = input.ReadValueF32(endian),                                  //unknown29
                                unknown10 = input.ReadValueF32(endian),                                 //unknown30
                                unknown11 = input.ReadValueU32(endian),                                 //unknown31
                                unknown12 = input.ReadValueU32(endian),                                 //unknown32
                                unknown13 = input.ReadValueU32(endian),                                 //unknown33
                                unknown14 = input.ReadValueU32(endian),                                 //unknown34
                                unknown15 = input.ReadValueU8()                                         //unknown35
                            };
                        }
                        unknownType7.unknown6 [ k ] = unknownType11;
                    }

                    unknownType7.unknown7 = new type13[ input.ReadValueU32 ( endian ) ];                //unknown65
                    for ( uint k = 0; k < unknownType7.unknown7.Length; k++ )
                    {
                        unknownType7.unknown7[k] = new type13
                        {
                            unknown1 = input.ReadValueU32(endian),                                      //unknown66
                            unknown2 = input.ReadValueU32(endian)                                       //unknown67
                        };
                    }
                    unknownType2.unknown8 [ j ] = unknownType7;
                }
                unknownType1.unknown1 [ i ] = unknownType2;
            }
            this.result = unknownType1;
        }

        private static string ReadString ( Stream input, Endian endian )
        {
            var length = input.ReadValueU16 ( endian );
            var text = input.ReadString ( length, true, Encoding.ASCII );
            return text;
        }

        #region Nested type: type1

        public class type1
        {
            public type2[ ] unknown1;
        }

        #endregion

        #region Nested type: type10

        public class type10
        {
            public float unknown1;
            public float unknown2;
            public float unknown10;
            public uint unknown11;
            public byte unknown12;


            public float unknown13;
            public float unknown14;
            public float unknown15;
            public float unknown16;
            public float unknown17;
            public float unknown18;
            public float unknown19;
            public float unknown20;

            public byte unknown21;
            public float unknown3;
            public float unknown4;
            public float unknown5;
            public float unknown6;
            public float unknown7;
            public float unknown8;
            public float unknown9;
        }

        #endregion

        #region Nested type: type11

        public class type11
        {
            public uint unknown1;
            public byte unknown2;
            public type8[ ] unknown3;
        }

        #endregion

        #region Nested type: type13

        public class type13
        {
            public uint unknown1;
            public uint unknown2;
        }

        #endregion

        #region Nested type: type2

        public class type2
        {
            public uint unknown1;
            public string unknown2;
            public type3[ ] unknown3;
            public type4[ ] unknown4;
            public type5[ ] unknown5;
            public type6[ ] unknown6;
            public string unknown7;
            public type7[ ] unknown8;
        }

        #endregion

        #region Nested type: type3

        public class type3
        {
            public uint unknown1;
            public string unknown2;
        }

        #endregion

        #region Nested type: type4

        public class type4
        {
            public uint unknown1;
            public uint unknown2;
            public string unknown3;
        }

        #endregion

        #region Nested type: type5

        public class type5
        {
            public uint unknown1;
            public string unknown2;
        }

        #endregion

        #region Nested type: type6

        public class type6
        {
            public uint unknown1;
            public string unknown2;
        }

        #endregion

        #region Nested type: type7

        public class type7
        {
            public string unknown1;
            public uint unknown2;
            public byte unknown3;
            public type8[ ] unknown4;
            public type9[ ] unknown5;
            public type11[ ] unknown6;
            public type13[ ] unknown7;
        }

        #endregion

        #region Nested type: type8

        public class type8
        {
            public float unknown1;
            public float unknown10;
            public uint unknown11;
            public uint unknown12;
            public uint unknown13;
            public uint unknown14;

            public byte unknown15;
            public float unknown2;
            public float unknown3;
            public float unknown4;
            public uint unknown5;
            public byte unknown6;


            public float unknown7;
            public float unknown8;
            public float unknown9;
        }

        #endregion

        #region Nested type: type9

        public class type9
        {
            public uint unknown1;
            public byte unknown2;
            public type10[ ] unknown3;
        }

        #endregion
    }
}
