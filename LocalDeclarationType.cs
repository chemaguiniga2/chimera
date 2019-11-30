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
        public string kind;
        public object value;
        public int pos;
        public bool is_const;

        public LocalDeclarationType(string key, TypeG type, string kind, object value, int pos, bool is_const)
        {
            this.key = key;
            this.type = type;
            this.kind = kind;
            this.value = value;
            this.pos = pos;
            this.is_const = is_const;
        
        }
    }
}