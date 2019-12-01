/*
Dachely Otero Argote            A01377812
Jos� Mar�a Agu��iga D�az        A01376669 
Jos� Rodrigo Narv�ez Berlanga   A01377566
 */

using System;

namespace Chimera
{

    public struct LocalDeclarationType
    {
        public string key;
        public TypeG type;
        public object value;
        public int pos;
        public TypeG kind;

        public LocalDeclarationType(string key, TypeG type, object value, int pos, TypeG kind)
        {
            this.key = key;
            this.type = type;
            this.value = value;
            this.pos = pos;
            this.kind = kind;

        }
    }
}