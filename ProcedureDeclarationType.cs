/*
Dachely Otero Argote            A01377812
José María Aguíñiga Díaz        A01376669 
José Rodrigo Narváez Berlanga   A01377566
 */

using System;

namespace Chimera
{

    public struct ProcedureDeclarationType
    {

        public string key;
        public TypeG return_type;
        public Boolean is_predef;

        public ProcedureDeclarationType(string key, TypeG return_type, bool is_predef)
        {
            this.key = key;
            this.return_type = return_type;
            this.is_predef = is_predef;
        }
    }


}