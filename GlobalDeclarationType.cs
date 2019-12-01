/*
Dachely Otero Argote            A01377812
José María Aguíñiga Díaz        A01376669 
José Rodrigo Narváez Berlanga   A01377566
 */

using System;

namespace Chimera {

    public struct GlobalDeclarationType {

        public  string key;
        public  TypeG type;
        public  object value;
        public  TypeG kind;

        public GlobalDeclarationType (string key, TypeG type, object value,TypeG kind){
            this.key = key;
            this.type = type;
            this.value = value;
            this.kind = kind;
        }
    }

    
}