/*
Dachely Otero Argote            A01377812
Jos� Mar�a Agu��iga D�az        A01376669 
Jos� Rodrigo Narv�ez Berlanga   A01377566
 */

using System;

namespace Chimera
{

    public struct ProcedureDeclarationType
    {

        public string key;
        public TypeG return_type;
        public Boolean is_predef;
        public LocalDeclarationTable dec_table;

        public ProcedureDeclarationType(string key, TypeG return_type, bool is_predef, LocalDeclarationTable dec_table)
        {
            this.key = key;
            this.return_type = return_type;
            this.is_predef = is_predef;
            this.dec_table = dec_table;
        }
    }


}