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
        public bool is_const;

        public LocalDeclarationType(string key, TypeG type, object value, bool is_const)
        {
            this.key = key;
            this.type = type;
            this.value = value;
            this.is_const = is_const;
        }
    }


}