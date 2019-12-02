/*
Dachely Otero Argote            A01377812
José María Aguíñiga Díaz        A01376669 
José Rodrigo Narváez Berlanga   A01377566
 */

using System;
using System.Collections.Generic;
using System.Collections;
//using System.Collections.ArrayList;

namespace Chimera
{

    class SemanticAnalyzer
    {

        //-----------------------------------------------------------
        /*static readonly IDictionary<TokenCategory, TypeG> typeMapper =
            new Dictionary<TokenCategory, TypeG>() {
                { TokenCategory.INTEGER, TypeG.INTEGER },
                { TokenCategory.STRING, TypeG.STRING },
                { TokenCategory.BOOLEAN, TypeG.BOOLEAN },
                { TokenCategory.BOOLEAN, TypeG.VOID },
                { TokenCategory.LIST, TypeG.LIST },
                { TokenCategory.OF, TypeG.OF }                     
            };
            */


        static readonly IDictionary<TokenCategory, TypeG> typeMapperConstDecl =
        new Dictionary<TokenCategory, TypeG>() {
                { TokenCategory.CONST, TypeG.CONST},
                { TokenCategory.VAR, TypeG.VAR}
        };


        //-----------------------------------------------------------
        public static class CurrentContext
        {
            public static string context { get; set; }
            public static string procedure { get; set; } 
            public static Boolean paramDetect { get; set; }
            public static int cantparam { get; set; } 
            public static int param { get; set; }
            public static ProcedureDeclarationTable current_pdt { get; set; }
            public static LocalDeclarationTable current_ldt { get; set; }
        }

        /*public class ContextSwapper
        {
            private void Get()
            {
                string getValue = CurrentContext.context;
            }
            private void Set(String a)
            {
                CurrentContext.context = "new value";
            }
        }*/

        public GloabalDeclaratonTable GloabalDeclaratonT
        {
            get;
            private set;
        }

        public List<ProcedureDeclarationTable> ListProcedureDeclarationTable
        {
            get;
            private set;
        }


        public ProcedureDeclarationTable ProcedureDeclarationT
        {
            get;
            private set;
        }

        public List<LocalDeclarationTable> ListLocalDeclarationTable
        {
            get;
            private set;
        }

        public LocalDeclarationTable LocalDeclarationT
        {
            get;
            private set;
        }

        public List<ProcedureDeclarationTable> ProcedureDeclarationList
        {
            get;
            private set;
        }

        public List<LocalDeclarationTable> LocalDeclarationList
        {
            get;
            private set;
        }



        /*public SymbolTable GlobalDeclarationTable {
            get;
            private set;
        }

        public SymbolTable LocalDeclarationTable {
            get;
            private set;
        }*/

        //-----------------------------------------------------------
        public SemanticAnalyzer()
        {
            GloabalDeclaratonT = new GloabalDeclaratonTable();
            ListProcedureDeclarationTable =  new List<ProcedureDeclarationTable>();

            ProcedureDeclarationList = new List<ProcedureDeclarationTable>();
            ProcedureDeclarationT = new ProcedureDeclarationTable();

            LocalDeclarationList = new List<LocalDeclarationTable>();
            LocalDeclarationT = new LocalDeclarationTable();
            ListLocalDeclarationTable = new List<LocalDeclarationTable>();
            
            //GlobalDeclarationTable = new SymbolTable("Global Declaration Table");
            //LocalDeclarationTable = new SymbolTable("Local Declaration Table");
        }


        private void fillDefineProcedure()
        {
            /*ProcedureDeclarationT[procedureName] =
                        new ProcedureDeclarationType(procedureName, TypeG.VOID, false);*/

            //Input/Output Operations
            ProcedureDeclarationT["WrInt"] = new ProcedureDeclarationType("WrInt", TypeG.VOID, true);
            ProcedureDeclarationT["WrStr"] = new ProcedureDeclarationType("WrStr", TypeG.VOID, true);
            ProcedureDeclarationT["WrBool"] = new ProcedureDeclarationType("WrBool", TypeG.VOID, true);
            ProcedureDeclarationT["WrLn"] = new ProcedureDeclarationType("WrLn", TypeG.VOID, true);
            ProcedureDeclarationT["RdInt"] = new ProcedureDeclarationType("RdInt", TypeG.INTEGER, true);
            ProcedureDeclarationT["RdStr"] = new ProcedureDeclarationType("RdStr", TypeG.STRING, true);
            //String Operations
            ProcedureDeclarationT["AtStr"] = new ProcedureDeclarationType("AtStr", TypeG.STRING, true);
            ProcedureDeclarationT["LenStr"] = new ProcedureDeclarationType("LenStr", TypeG.INTEGER, true);
            ProcedureDeclarationT["CmpStr"] = new ProcedureDeclarationType("CmpStr", TypeG.INTEGER, true);
            ProcedureDeclarationT["CatStr"] = new ProcedureDeclarationType("CatStr", TypeG.STRING, true);
            //List Operations
            ProcedureDeclarationT["LenLstInt"] = new ProcedureDeclarationType("LenLstInt", TypeG.INTEGER, true);
            ProcedureDeclarationT["LenLstStr"] = new ProcedureDeclarationType("LenLstStr", TypeG.INTEGER, true);
            ProcedureDeclarationT["LenLstBool"] = new ProcedureDeclarationType("LenLstBool", TypeG.INTEGER, true);
            ProcedureDeclarationT["NewLstInt"] = new ProcedureDeclarationType("NewLstInt", TypeG.LIST, true);
            ProcedureDeclarationT["NewLstStr"] = new ProcedureDeclarationType("NewLstStr", TypeG.LIST, true);
            ProcedureDeclarationT["NewLstBool"] = new ProcedureDeclarationType("NewLstBool", TypeG.LIST, true);
            //Conversion Operations
            ProcedureDeclarationT["IntToStr"] = new ProcedureDeclarationType("IntToStr", TypeG.STRING, true);
            ProcedureDeclarationT["StrToInt"] = new ProcedureDeclarationType("StrToInt", TypeG.INTEGER, true);
            

        }

        //-----------------------------------------------------------
        public TypeG Visit(Program node)
        {
            fillDefineProcedure();
            //
            Console.WriteLine(node.ToStringTree());
            CurrentContext.context = "GLOBAL";
            CurrentContext.paramDetect = false;
            //
            //Console.WriteLine(node[0]);
            VisitChildren(node);
            /*isit((dynamic)node[0]);
            //
            //Console.WriteLine(node[1]);
            Visit((dynamic)node[1]);
            //
            //Console.WriteLine("VISITANDO.." + node[2]);
            Visit((dynamic)node[2]);
            */
            return TypeG.VOID;
        }

        public TypeG Visit(ConstantDeclarationList node)
        {
            VisitChildren(node);
            return TypeG.VOID;
        }

        public TypeG Visit(ConstantDeclaration node)
        {
            var variableName = node.AnchorToken.Lexeme;
            var variableValue = node[0].AnchorToken.Lexeme;
            if (CurrentContext.context == "GLOBAL")
            {
                if(GloabalDeclaratonT.Contains(variableName)){
                    throw new SemanticError(
                    "Duplicated variable ("+CurrentContext.context+"): " + variableName,
                    node[0].AnchorToken);
                }
                else{
                    GloabalDeclaratonT[variableName] =
                        new GlobalDeclarationType(variableName, TypeG.INTEGER, variableValue, TypeG.CONST);
                }                
            }
            else if (CurrentContext.context == "LOCAL")
            {
                if(ListLocalDeclarationTable.Count > 0){
                        
                    if (ListLocalDeclarationTable[ListLocalDeclarationTable.Count - 1].Contains(variableName) && ListLocalDeclarationTable[ListLocalDeclarationTable.Count - 1].tableID == CurrentContext.procedure)
                    {
                            throw new SemanticError(
                                "Duplicated variable: " + variableName,
                                node[0].AnchorToken);
                    }
                    else
                    {   
                        if(ListLocalDeclarationTable[ListLocalDeclarationTable.Count - 1].tableID == CurrentContext.procedure){
                                LocalDeclarationTable d = ListLocalDeclarationTable[ListLocalDeclarationTable.Count - 1];
                                d[variableName] = 
                                    new LocalDeclarationType(variableName, TypeG.INTEGER, variableValue, -1, TypeG.CONST);                               
                                //ListLocalDeclarationTable.Add(d);
                        }
                        else{                                
                                LocalDeclarationTable d = new LocalDeclarationTable();
                                    d[variableName] = 
                                        new LocalDeclarationType(variableName, TypeG.INTEGER, variableValue, -1, TypeG.CONST);                              
                                    ListLocalDeclarationTable.Add(d);
                                ListLocalDeclarationTable[ListLocalDeclarationTable.Count - 1].tableID = CurrentContext.procedure;
                        }
                            
                        }
                    }
                    else{
                        LocalDeclarationTable d = new LocalDeclarationTable();
                            d[variableName] = 
                                new LocalDeclarationType(variableName, TypeG.INTEGER, variableValue, -1, TypeG.CONST);                              
                            ListLocalDeclarationTable.Add(d);
                        ListLocalDeclarationTable[0].tableID = CurrentContext.procedure;

                    }
                    /*LocalDeclarationT[variableName] =
                        new LocalDeclarationType(variableName, TypeG.INTEGER, variableValue, 3, TypeG.CONST, CurrentContext.procedure);
                    */
                }
            
            return TypeG.VOID;
        }

        public TypeG Visit(Identifier node)
        {
            VisitChildren(node);
            /*
            var variableName = node.AnchorToken.Lexeme;
            
            if (ProcedureDeclarationList[CurrentContext.current_pdt].Contains(variableName))
            {
                return ProcedureDeclarationList[CurrentContext.current_pdt];
            }

            throw new SemanticError(
                "Undeclared variable: " + variableName,
                node.AnchorToken);


            //var variableName = node.AnchorToken.Lexeme;
            if (ProcedureDeclarationT.Contains(variableName))
            {
                Console.WriteLine("TE ENCONTRE PREDEFINIDO");
                if (variableName == "LenStr")
                {
                    Console.WriteLine("Es un LenStr");
                }

                return TypeG.INTEGER;
                //PredefinedProcedure(variableName);
                //Visit(StandardProcedure node);
            }
            if (LocalDeclarationT.Contains(variableName))
            {
                Console.WriteLine(LocalDeclarationT[variableName]);
                return TypeG.INTEGER;
            }
                
            dynamic variableValue = node.AnchorToken.Lexeme;
            try
            {
                variableValue = node[0].AnchorToken.Lexeme;
            }
            catch (ArgumentOutOfRangeException e)
            {
                return TypeG.VOID;
            }
            TypeG type = TypeG.VAR;
            TypeG kind = TypeG.VAR;
            var is_const = true;
            if (CurrentContext.paramDetect == true)
            {
                Console.WriteLine("CAMBIEEEEE A PARAM!!");
                kind = TypeG.PARAM;
            }


            if (node[0].AnchorToken.Lexeme == "integer")
            {
                variableValue = 0;
                is_const = false;
                type = TypeG.INTEGER;
            }
            else if (node[0].AnchorToken.Lexeme == "string")
            {
                variableValue = "";
                is_const = false;
                type = TypeG.STRING;
            }
            else if (node[0].AnchorToken.Lexeme == "boolean")
            {
                variableValue = false;
                is_const = false;
                type = TypeG.BOOLEAN;
            }
            else if (node[0].AnchorToken.Lexeme == "list")
            {
                variableValue = "{}";
                is_const = false;
                type = TypeG.LIST;
            }
            //
            //Console.WriteLine("VAR VALUE: " + variableValue);

            if (CurrentContext.context == "GLOBAL")
            {
                if(GloabalDeclaratonT.Contains(variableName)) {
                    throw new SemanticError(
                    "Duplicated variable ("+CurrentContext.context+"): " + variableName,
                    node[0].AnchorToken);
                }                
                else
                {
                    GloabalDeclaratonT[variableName] =
                        new GlobalDeclarationType(variableName, type, variableValue, kind);
                }
            }            
            
            else if (CurrentContext.context == "LOCAL")
            {
                    if(ListLocalDeclarationTable.Count > 0){
                        if (ListLocalDeclarationTable[ListLocalDeclarationTable.Count - 1].Contains(variableName) && 
                                ListLocalDeclarationTable[ListLocalDeclarationTable.Count - 1].tableID == CurrentContext.procedure)
                        {
                            throw new SemanticError(
                                "Duplicated variable2: " + variableName,
                                node[0].AnchorToken);
                        }
                        else
                        {   
                            if(ListLocalDeclarationTable[ListLocalDeclarationTable.Count - 1].tableID == CurrentContext.procedure){
                                LocalDeclarationTable d = ListLocalDeclarationTable[ListLocalDeclarationTable.Count - 1];
                                if(CurrentContext.cantparam > 0){
                                        d[variableName] = 
                                        new LocalDeclarationType(variableName, type, variableValue, CurrentContext.param , kind); 
                                        CurrentContext.cantparam --;
                                        CurrentContext.param ++;      

                                    }
                                    else{
                                        d[variableName] = 
                                        new LocalDeclarationType(variableName, type, variableValue, -1, kind); 
                                    }
                            }
                            else{                                
                                LocalDeclarationTable d = new LocalDeclarationTable();
                                    if(CurrentContext.cantparam > 0){
                                        d[variableName] = 
                                        new LocalDeclarationType(variableName, type, variableValue, CurrentContext.param , kind); 
                                        CurrentContext.cantparam --;
                                        CurrentContext.param ++;      

                                    }
                                    else{
                                        d[variableName] = 
                                        new LocalDeclarationType(variableName, type, variableValue, -1, kind); 
                                    }                             
                                    ListLocalDeclarationTable.Add(d);
                                ListLocalDeclarationTable[ListLocalDeclarationTable.Count - 1].tableID = CurrentContext.procedure;
                            }
                            
                        }
                    }
                    else{
                        LocalDeclarationTable d = new LocalDeclarationTable();
                            if(CurrentContext.cantparam > 0){
                                        d[variableName] = 
                                        new LocalDeclarationType(variableName, type, variableValue, CurrentContext.param , kind); 
                                        CurrentContext.cantparam --;
                                        CurrentContext.param ++;      

                                    }
                                    else{
                                        d[variableName] = 
                                        new LocalDeclarationType(variableName, type, variableValue, -1, kind); 
                                    }                              
                            ListLocalDeclarationTable.Add(d);
                        ListLocalDeclarationTable[0].tableID = CurrentContext.procedure;

                    }
                    
                    //
                    LocalDeclarationT[variableName] =
                        new LocalDeclarationType(variableName, type, variableValue, 1, TypeG.VAR, CurrentContext.procedure);
                      
            }
              */  
            return TypeG.VOID;
        }


        void Visit(SimpleType node)
        {
            VisitChildren(node);
            //
            /*Console.WriteLine("FOR EACH " + node.getLength());
            foreach (var n in node)
            {
                //
                Console.WriteLine("TIPO" + n);
                Visit((dynamic)n);
            }
            */
            //Console.WriteLine("FIN DEL CICLO");
            //return TypeG.VOID;
        }


        void VisitChildren(Node node)
        {
            //
            //Console.WriteLine("FOR EACH " + node.getLength());
            foreach (var n in node)
            {
                //
                Console.WriteLine("TIPO" + n);
                Visit((dynamic)n);
            }
            //
            //Console.WriteLine("FIN DEL CICLO");
        }


        public TypeG Visit(VariableDeclarationList node)
        {
            VisitChildren(node);
            return TypeG.VOID;
        }


        public TypeG Visit(ProcedureDeclarationList node)
        {
            VisitChildren(node);
            return TypeG.VOID;
        }

        public TypeG Visit(ProcedureDeclaration node)
        {
            
            CurrentContext.context = "LOCAL";


            var procedureName = node.AnchorToken.Lexeme;
            CurrentContext.procedure = procedureName;

            foreach (var n in node)
            {
                if (n.ToString().StartsWith("Type"))
                {
                    Console.WriteLine("Has type: "+ n.AnchorToken.Lexeme);
                }
            }

            
            ProcedureDeclarationTable pdt = new ProcedureDeclarationTable();
            CurrentContext.current_pdt = pdt;
            ProcedureDeclarationList.Add(pdt);
            VisitChildren(node);
            CurrentContext.context = "GLOBAL";
            return TypeG.VOID;

            /*
            else
            {
                LocalDeclarationTable d = new LocalDeclarationTable();
                if (CurrentContext.cantparam > 0)
                {
                    d[variableName] =
                    new LocalDeclarationType(variableName, type, variableValue, CurrentContext.param, kind);
                    CurrentContext.cantparam--;
                    CurrentContext.param++;

                }
                else
                {
                    d[variableName] =
                    new LocalDeclarationType(variableName, type, variableValue, -1, kind);
                }
                ListLocalDeclarationTable.Add(d);
                ListLocalDeclarationTable[0].tableID = CurrentContext.procedure;

            }

            if (ProcedureDeclarationList[CurrentContext.procedure].Contains(procedureName))
                {
                    throw new SemanticError(
                        "Duplicated procedure: " + procedureName,
                        node.AnchorToken);
                }
                else
                {
                    ProcedureDeclarationList[CurrentContext.procedure] =
                        new ProcedureDeclarationType(procedureName, TypeG.VOID, false);
                }

            VisitChildren(node);
            CurrentContext.context = "GLOBAL";
            return TypeG.VOID;*/
        }

        public TypeG Visit(ParameterDeclarationList node)
        {

            CurrentContext.paramDetect = true;
            /*
            Console.WriteLine("***************** CurrentContext.cantparam " + CurrentContext.cantparam);
            Console.WriteLine("***************** node " + node);
            Console.WriteLine("***************** node tam" + node.getLength());
            */
            var totalParam = 0;
            if(node.getLength() > 0){
                Console.WriteLine("***************** node[0] tam" + node[0].getLength());
                foreach (var child in node)
                {
                    totalParam += child.getLength();
                }
            }
            
            CurrentContext.cantparam = totalParam;
            CurrentContext.param = 0;
            Console.WriteLine("////////////////// CurrentContext.cantparam " + CurrentContext.cantparam);
            VisitChildren(node);
            CurrentContext.paramDetect = false;
            return TypeG.VOID;
        }

        public TypeG Visit(ParameterDeclaration node)
        {
            //Console.WriteLine("Rock"+ node.ToStringTree());
            //Console.WriteLine("ARBOL\n"+node.ToStringTree()+"FIN");
            VisitChildren(node);
            //Console.WriteLine("FIN");
            return TypeG.VOID;
        }

        public TypeG Visit(StatementList node)
        {
            //Console.WriteLine("Rock"+ node.ToStringTree());
            VisitChildren(node);
            //Console.WriteLine("FIN");
            return TypeG.VOID;
        }

        public TypeG Visit(CallStatement node)
        {
            //Console.WriteLine("Rock"+ node.ToStringTree());
            VisitChildren(node);
            //Console.WriteLine("FIN");
            return TypeG.VOID;
        }

        public TypeG Visit(Type node)
        {
            //Console.WriteLine("Rock"+ node.ToStringTree());
            //Console.WriteLine("TIPOOOO");
            VisitChildren(node);
            //Console.WriteLine("FIN");
            return TypeG.VOID;
        }


        /* ---------- Pending -------- */
        public TypeG Visit(Expression node)
        {
            //Console.WriteLine("Rock"+ node.ToStringTree());
            //Console.WriteLine("TIPOOOO");
            VisitChildren(node);
            //Console.WriteLine("FIN");
            return TypeG.VOID;
        }
        /* ---------- Pending -------- */
        public TypeG Visit(ExpressionList node)
        {
            //Console.WriteLine("Rock"+ node.ToStringTree());
            //Console.WriteLine("TIPOOOO");
            VisitChildren(node);
            //Console.WriteLine("FIN");
            return TypeG.VOID;
        }

        /* ---------- Pending -------- */
        public TypeG Visit(AssignmentStatement node)
        {
            //Console.WriteLine("Rock"+ node.ToStringTree());
            //Console.WriteLine("TIPOOOO");


            VisitChildren(node);
            //Console.WriteLine("FIN");
            return TypeG.VOID;
        }


        /* ---------- Pending -------- */
        public TypeG Visit(AdditionOperator node)
        {
            //Console.WriteLine("Rock"+ node.ToStringTree());
            //Console.WriteLine("TIPOOOO");
            VisitChildren(node);
            //Console.WriteLine("FIN");
            return TypeG.VOID;
        }

        /* ---------- Pending -------- 
        public TypeG Visit(IntegerLiteral node)
        {
            //Console.WriteLine("Rock"+ node.ToStringTree());
            //Console.WriteLine("TIPOOOO");
            VisitChildren(node);
            //Console.WriteLine("FIN");
            return TypeG.VOID;
        }

        */


        public TypeG Visit(IdentifierList node)
        {
            //Console.WriteLine("Rock"+ node.ToStringTree());
            VisitChildren(node);
            //Console.WriteLine("FIN");
            return TypeG.VOID;
        }

        public TypeG Visit(CallNode node)
        {
            Console.WriteLine("INICIO");
            VisitChildren(node);
            Console.WriteLine("FIN");
            return TypeG.VOID;
        }

        public TypeG Visit(SubstractOperator node)
        {
            Console.WriteLine("LLEGUE");
            VisitBinaryOperator('-', node, TypeG.INTEGER);
            return TypeG.INTEGER;
        }

        void VisitBinaryOperator(char op, Node node, TypeG type)
        {

            Console.WriteLine("CA " + node.ToStringTree());
            Console.WriteLine("F" + node[0]);
            Console.WriteLine("U" + node[1]);
            if (Visit((dynamic)node[0]) != type ||
                Visit((dynamic)node[1]) != type)
            {
                throw new SemanticError(
                    String.Format(
                        "Operator {0} requires two operands of type {1}",
                        op,
                        type),
                    node.AnchorToken);
            }
        }

        public TypeG Visit(IntegerLiteral node)
        {

            var intStr = node.AnchorToken.Lexeme;

            try
            {
                Convert.ToInt32(intStr);

            }
            catch (OverflowException)
            {
                throw new SemanticError(
                    "Integer literal too large: " + intStr,
                    node.AnchorToken);
            }

            return TypeG.INTEGER;
        }

        public TypeG Visit(StringLiteral node)
        {
            //Console.WriteLine("INICIO");
            VisitChildren(node);
            //Console.WriteLine("FIN");
            return TypeG.STRING;
        }



        public TypeG Visit(Loop node)
        {
            //Console.WriteLine("INICIO");
            VisitChildren(node);
            //Console.WriteLine("FIN");
            return TypeG.VOID;
        }
        public TypeG Visit(For node)
        {
            Console.WriteLine("FOR: " + node.ToStringTree());
            TypeG varType = Visit((dynamic)node[0]);
            TypeG listType = Visit((dynamic)node[1]);
            dynamic varListType;
            switch (varType)
            {
                case TypeG.INTEGER:
                    varListType = TypeG.INTEGER_LIST;
                    break;
                case TypeG.STRING:
                    varListType = TypeG.STRING_LIST;
                    break;
                case TypeG.BOOLEAN:
                    varListType = TypeG.BOOLEAN_LIST;
                    break;
                default:
                    throw new Exception($"Type {varType} has no equivalent list type");
            }

            if (varListType != listType)
            {
                throw new SemanticError($"Incompatible types {varType} and {listType}",
                    node[0].AnchorToken);
            }

            string key = node.AnchorToken.Lexeme;

            ProcedureDeclarationT[CurrentContext.procedure] = new ProcedureDeclarationType(key, TypeG.VOID, false);

            var lastInLoopOrFor = CurrentContext.paramDetect;
            //insideLoop = true;

            Visit((dynamic)node[2]);

            //insideLoop = lastInLoopOrFor;
            return TypeG.VOID;
        }



        public TypeG Visit(If node)
        {
            Console.WriteLine("IF TREE: "+node.ToStringTree());
            if (Visit((dynamic)node[0]) != TypeG.BOOLEAN)
            {
                throw new SemanticError(
                    "Expecting type " + TypeG.BOOLEAN
                    + " in conditional statement",
                    node.AnchorToken);
            }
            VisitChildren(node[1]);
            return TypeG.VOID;
        }

        public TypeG Visit(BiggerThanOperator node)
        {
            Console.WriteLine("INICIO");
            VisitBinaryOperator('<', node, TypeG.INTEGER);
            return TypeG.BOOLEAN;
            //return TypeG.VOID;
        }



        //-----------------------------------------------------------

        /*
        //-----------------------------------------------------------
        public Type Visit(StatementList node) {
            VisitChildren(node);
            return Type.VOID;
        }

        //-----------------------------------------------------------
        public Type Visit(Assignment node) {

            var variableName = node.AnchorToken.Lexeme;

            if (Table.Contains(variableName)) {

                var expectedType = Table[variableName];

                if (expectedType != Visit((dynamic) node[0])) {
                    throw new SemanticError(
                        "Expecting type " + expectedType 
                        + " in assignment statement",
                        node.AnchorToken);
                }

            } else {
                throw new SemanticError(
                    "Undeclared variable: " + variableName,
                    node.AnchorToken);
            }

            return Type.VOID;
        }

        //-----------------------------------------------------------
        public Type Visit(Print node) {
            node.ExpressionType = Visit((dynamic) node[0]);
            return Type.VOID;
        }

        //-----------------------------------------------------------
        public Type Visit(If node) {
            if (Visit((dynamic) node[0]) != Type.BOOL) {
                throw new SemanticError(
                    "Expecting type " + Type.BOOL 
                    + " in conditional statement",                   
                    node.AnchorToken);
            }
            VisitChildren(node[1]);
            return Type.VOID;
        }

        //-----------------------------------------------------------
        public Type Visit(Identifier node) {

            var variableName = node.AnchorToken.Lexeme;

            if (Table.Contains(variableName)) {
                return Table[variableName];
            }

            throw new SemanticError(
                "Undeclared variable: " + variableName,
                node.AnchorToken);
        }

        //-----------------------------------------------------------
        public Type Visit(IntLiteral node) {

            var intStr = node.AnchorToken.Lexeme;

            try {
                Convert.ToInt32(intStr);

            } catch (OverflowException) {
                throw new SemanticError(
                    "Integer literal too large: " + intStr, 
                    node.AnchorToken);
            }

            return Type.INT;
        }

        //-----------------------------------------------------------
        public Type Visit(True node) {
            return Type.BOOL;
        }

        //-----------------------------------------------------------
        public Type Visit(False node) {
            return Type.BOOL;
        }

        //-----------------------------------------------------------
        public Type Visit(Neg node) {          
            if (Visit((dynamic) node[0]) != Type.INT) {
                throw new SemanticError(
                    "Operator - requires an operand of type " + Type.INT,
                    node.AnchorToken);
            }
            return Type.INT;
        }

        //-----------------------------------------------------------
        public Type Visit(And node) {
            VisitBinaryOperator('&', node, Type.BOOL);
            return Type.BOOL;
        }

        //-----------------------------------------------------------
        public Type Visit(Less node) {
            VisitBinaryOperator('<', node, Type.INT);
            return Type.BOOL;
        }

        //-----------------------------------------------------------
        public Type Visit(Plus node) {
            VisitBinaryOperator('+', node, Type.INT);
            return Type.INT;
        }

        //-----------------------------------------------------------
        public Type Visit(Mul node) {
            VisitBinaryOperator('*', node, Type.INT);
            return Type.INT;
        }

        //-----------------------------------------------------------
        void VisitChildren(Node node) {
            foreach (var n in node) {
                Visit((dynamic) n);
            }
        }

        //-----------------------------------------------------------
        void VisitBinaryOperator(char op, Node node, Type type) {
            if (Visit((dynamic) node[0]) != type || 
                Visit((dynamic) node[1]) != type) {
                throw new SemanticError(
                    String.Format(
                        "Operator {0} requires two operands of type {1}",
                        op, 
                        type),
                    node.AnchorToken);
            }
        }*/
    }
}
