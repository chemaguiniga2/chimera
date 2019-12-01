/*
Dachely Otero Argote            A01377812
José María Aguíñiga Díaz        A01376669 
José Rodrigo Narváez Berlanga   A01377566
 */

using System;
using System.Text;
using System.Collections.Generic;
using System.Collections;

namespace Chimera
{
    public class ProcedureDeclarationTable : IEnumerable<KeyValuePair<string, ProcedureDeclarationType>>
    {
        //public class SymbolTable: IEnumerable<KeyValuePair<string, TypeG>> { 

        IDictionary<string, ProcedureDeclarationType> data = new SortedDictionary<string, ProcedureDeclarationType>();
        // IDictionary<string, TypeG> data = new SortedDictionary<string, TypeG>();


        public ProcedureDeclarationTable()
        {
        }

        //-----------------------------------------------------------
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("Procedure Declaration Table " + "\n");
            sb.Append("====================\n");
            foreach (var entry in data)
            {
                sb.Append(String.Format("[{0} {1} {2}]\n",
                                        entry.Key,
                                        entry.Value.return_type,
                                        entry.Value.is_predef

                                        //Add(entry.Value)
                                        ));
            }
            sb.Append("====================\n");
            return sb.ToString();
        }

        /*public string Add(ArrayList values) {
            var sb = new StringBuilder();
            foreach (var v in values) {
                
            }
           return sb.ToString();

        }*/

        //-----------------------------------------------------------
        //public TypeG this[string key] {
        public ProcedureDeclarationType this[string key]
        {
            get
            {
                return data[key];
            }
            set
            {
                data[key] = value;
            }
        }

        //-----------------------------------------------------------
        public bool Contains(string key)
        {
            return data.ContainsKey(key);
        }

        //-----------------------------------------------------------
        //public IEnumerator<KeyValuePair<string, TypeG>> GetEnumerator() {
        public IEnumerator<KeyValuePair<string, ProcedureDeclarationType>> GetEnumerator()
        {
            return data.GetEnumerator();
        }

        //-----------------------------------------------------------
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            Console.WriteLine("Es un errorrrrrrrrr-------------------------------------------------------------------------");
            throw new NotImplementedException();

        }
    }
}