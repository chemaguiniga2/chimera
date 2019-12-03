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
            public static int index { get; set; }
            public static int length { get; set; }
            public static Boolean insideLoop { get; set; }
            public static string procedure { get; set; }
            public static Boolean paramDetect { get; set; }
            public static int cantparam { get; set; } 
            public static int param { get; set; }
            public static ProcedureDeclarationTable current_pdt { get; set; }
            public static LocalDeclarationTable current_ldt { get; set; }
        }


        public GloabalDeclaratonTable GloabalDeclaratonT
        {
            get;
            private set;
        }

        public ProcedureDeclarationTable ProcedureDeclarationT
        {
            get;
            private set;
        }

        public LocalDeclarationTable LocalDeclarationT
        {
            get;
            private set;
        }

        public List<ProcedureDeclarationTable> ListProcedureDeclarationTable
        {
            get;
            private set;
        }


        public List<LocalDeclarationTable> ListLocalDeclarationTable
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
        /*
        private dynamic getValue(var tipo)
        {
            var value = "";
            switch (tipo)
            {
                case TypeG.INTEGER:
                    value = 0;
                    return yeah;
                default:
                    throw new Exception($"Function {name} has no declared call");
            }
            return value;


        }*/

        //-----------------------------------------------------------
        public TypeG Visit(Program node)
        {
            fillDefineProcedure();
            //
            Console.WriteLine(node.ToStringTree());
            CurrentContext.insideLoop = false;
            CurrentContext.context = "GLOBAL";
            CurrentContext.index = 0;
            CurrentContext.length = 0;
            //CurrentContext.procedure = 0;
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
                if (GloabalDeclaratonT.Contains(variableName))
                {
                    throw new SemanticError(
                    "Duplicated variable (" + CurrentContext.context + "): " + variableName,
                    node[0].AnchorToken);
                }
                else
                {
                    GloabalDeclaratonT[variableName] =
                        new GlobalDeclarationType(variableName, TypeG.INTEGER, variableValue, TypeG.CONST);
                }
            }
            else if (CurrentContext.context == "LOCAL")
            {

                if (ListLocalDeclarationTable[CurrentContext.index].Contains(variableName))
                {
                    throw new SemanticError(
                        "Duplicated variable: " + variableName,
                        node[0].AnchorToken);
                } else
                {
                    ListLocalDeclarationTable[CurrentContext.index][variableName] = new LocalDeclarationType(variableName, TypeG.INTEGER, variableValue, -1, TypeG.CONST);
                }

            }

            return TypeG.VOID;
        }

        public TypeG Visit(Identifier node)
        {
            var variableName = node.AnchorToken.Lexeme;
            Console.WriteLine(ListLocalDeclarationTable[CurrentContext.index]);
            if (ListLocalDeclarationTable[CurrentContext.index].Contains(variableName))
            {
                return ListLocalDeclarationTable[CurrentContext.index][variableName].type;
            }

            throw new SemanticError(
                "Undeclared variable: " + variableName,
                node.AnchorToken);
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

            foreach (var n in node)
            {
                TypeG tipo = Visit((dynamic)n);
                foreach (var i in n)
                {

                    var variableName = i.AnchorToken.Lexeme;
                    dynamic variableValue = false;

                    switch (tipo)
                    {
                        case TypeG.BOOLEAN:
                            variableValue = false;
                            break;
                        case TypeG.INTEGER:
                            variableValue = 0;
                            break;
                        case TypeG.STRING:
                            variableValue = "";
                            break;
                        case TypeG.INTEGER_LIST:
                            variableValue = new int[] { 0 };
                            break;
                        case TypeG.BOOLEAN_LIST:
                            variableValue = new bool[] { false };
                            break;
                        case TypeG.STRING_LIST:
                            variableValue = new string[] { "" };
                            break;
                        default:
                            throw new Exception($"Type {tipo} wasn't found");
                    }

                    if (CurrentContext.context == "GLOBAL")
                    {
                        if (GloabalDeclaratonT.Contains(variableName))
                        {
                            throw new SemanticError(
                            "Duplicated variable (" + CurrentContext.context + "): " + variableName,
                            n[0].AnchorToken);
                        }
                        else
                        {
                            GloabalDeclaratonT[variableName] =
                                new GlobalDeclarationType(variableName, TypeG.INTEGER, variableValue, TypeG.VAR);
                        }
                    }
                    else if (CurrentContext.context == "LOCAL")
                    {
                        if (ListLocalDeclarationTable[CurrentContext.index].Contains(variableName))
                        {
                            throw new SemanticError(
                                "Duplicated variable: " + variableName,
                                n[0].AnchorToken);
                        }
                        else
                        {

                            ListLocalDeclarationTable[CurrentContext.index][variableName] = new LocalDeclarationType(variableName, TypeG.INTEGER, variableValue, -1, TypeG.VAR);
                        }
                    }
                }
            }
            //VisitChildren(node);
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

            //SE AGREGA TABLA NUEVA AL HACERSE EL NUEVO PROCEDURE

            LocalDeclarationTable nuevaTabla = new LocalDeclarationTable();
            nuevaTabla.tableID = procedureName;
            ListLocalDeclarationTable.Add(nuevaTabla);


            //CurrentContext.length++;
            if (ProcedureDeclarationT.Contains(procedureName))
            {
                throw new SemanticError(
                "Duplicated procedure: " + procedureName,
                node.AnchorToken);
            } else
            {
                dynamic tipo = TypeG.VOID;
                foreach (var n in node)
                {
                    if (n.ToString().StartsWith("Type"))
                    {
                        tipo = Visit((dynamic)n);
                        Console.WriteLine("Has type: "+ tipo);
                    }
                }

                ProcedureDeclarationT[procedureName] = new ProcedureDeclarationType(procedureName, tipo, false);
                Console.WriteLine("NUEVO PROC" + procedureName);

            }

            /*
            if (ProcedureDeclarationList[CurrentContext.procedure].Contains(procedureName))
            {

            }
            else
            {
                ProcedureDeclarationList[CurrentContext.procedure] =
                    new ProcedureDeclarationType(procedureName, TypeG.VOID, false);
            }

            VisitChildren(node);
            CurrentContext.context = "GLOBAL";
            return TypeG.VOID;
            //CurrentContext.procedure+




            if (ProcedureDeclarationT.Contains(procedureName))
            {
                throw new SemanticError(
                    "Duplicated procedure: " + procedureName,
                    node.AnchorToken);
            }
            else
            {

                ProcedureDeclarationList[CurrentContext.procedure] =
                       new ProcedureDeclarationType(procedureName, TypeG.VOID, false);
                ProcedureDeclarationT.a
            }

            CurrentContext.current_pdt = pdt;
            //ProcedureDeclarationList[CurrentContext.procedure] = pdt;
            VisitChildren(node);
            CurrentContext.context = "GLOBAL";
            return TypeG.VOID;

            if (ProcedureDeclarationList.Contains(pdt))
            {
                Console.WriteLine("Hola");
            } else
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
                */
            VisitChildren(node);
            CurrentContext.context = "GLOBAL";
            CurrentContext.index++;


            return TypeG.VOID;
            //CurrentContext.procedure++;
        }

        public TypeG Visit(ParameterDeclarationList node)
        {

            CurrentContext.paramDetect = true;
            Console.WriteLine("PD");
            TypeG tipo = Visit((dynamic)node[0]);
            foreach (var i in node)
            {

                var variableName = i[0].AnchorToken.Lexeme;
                dynamic variableValue = false;

                Console.WriteLine("ROOT");
                if (CurrentContext.context == "GLOBAL")
                {
                    if (GloabalDeclaratonT.Contains(variableName))
                    {
                        throw new SemanticError(
                        "Duplicated variable (" + CurrentContext.context + "): " + variableName,
                        node[0].AnchorToken);
                    }
                    else
                    {
                        GloabalDeclaratonT[variableName] =
                            new GlobalDeclarationType(variableName, TypeG.INTEGER, variableValue, TypeG.PARAM);
                    }
                }
                else if (CurrentContext.context == "LOCAL")
                {
                    if (ListLocalDeclarationTable[CurrentContext.index].Contains(variableName))
                    {
                        throw new SemanticError(
                            "Duplicated variable: " + variableName,
                            node[0].AnchorToken);
                    }
                    else
                    {
                        Console.WriteLine("GUARDANDO!!!");
                        ListLocalDeclarationTable[CurrentContext.index][variableName] = new LocalDeclarationType(variableName, TypeG.INTEGER, variableValue, -1, TypeG.PARAM);
                        Console.WriteLine("TABLA"+ListLocalDeclarationTable[CurrentContext.index]);


                    }
                }
            }

            //VisitChildren(node);
            return TypeG.VOID;
            
            //return TypeG.VOID;
        }

        public TypeG Visit(ParameterDeclaration node)
        {

            Console.WriteLine("PD");
            TypeG tipo = Visit((dynamic)node);
            foreach (var i in node)
            {

                var variableName = i.AnchorToken.Lexeme;
                dynamic variableValue = false;

                Console.WriteLine("ROOT");
                if (CurrentContext.context == "GLOBAL")
                {
                    if (GloabalDeclaratonT.Contains(variableName))
                    {
                        throw new SemanticError(
                        "Duplicated variable (" + CurrentContext.context + "): " + variableName,
                        node[0].AnchorToken);
                    }
                    else
                    {
                        GloabalDeclaratonT[variableName] =
                            new GlobalDeclarationType(variableName, TypeG.INTEGER, variableValue, TypeG.PARAM);
                    }
                }
                else if (CurrentContext.context == "LOCAL")
                {
                    if (ListLocalDeclarationTable[CurrentContext.index].Contains(variableName))
                    {
                        throw new SemanticError(
                            "Duplicated variable: " + variableName,
                            node[0].AnchorToken);
                    }
                    else
                    {
                        Console.WriteLine("GUARDANDO!!!");
                        ListLocalDeclarationTable[CurrentContext.index][variableName] = new LocalDeclarationType(variableName, TypeG.INTEGER, variableValue, -1, TypeG.PARAM);
                    }
                }
            }

            //VisitChildren(node);
            return TypeG.VOID;
            /*var variableName = i.AnchorToken.Lexeme;
            dynamic variableValue = false;

            if (CurrentContext.context == "GLOBAL")
            {
                if (GloabalDeclaratonT.Contains(variableName))
                {
                    throw new SemanticError(
                    "Duplicated variable (" + CurrentContext.context + "): " + variableName,
                    node[0].AnchorToken);
                }
                else
                {
                    GloabalDeclaratonT[variableName] =
                        new GlobalDeclarationType(variableName, TypeG.INTEGER, variableValue, TypeG.VAR);
                }
            }
            else if (CurrentContext.context == "LOCAL")
            {
                if (ListLocalDeclarationTable[CurrentContext.index].Contains(variableName))
                {
                    throw new SemanticError(
                        "Duplicated variable: " + variableName,
                        node[0].AnchorToken);
                }
                else
                {

                    ListLocalDeclarationTable[CurrentContext.index][variableName] = new LocalDeclarationType(variableName, TypeG.INTEGER, variableValue, -1, TypeG.VAR);
                }
            }
            //Console.WriteLine("FIN");*/
            VisitChildren(node);
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

        /*
        public TypeG Visit(Type node)
        {
            //Console.WriteLine("Rock"+ node.ToStringTree());
            //Console.WriteLine("TIPOOOO");
            VisitChildren(node);
            //Console.WriteLine("FIN");
            return TypeG.VOID;
        }
        */

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
            var name = node.AnchorToken.Lexeme;
            var parametersRequired = 0;
            TypeG typeRequired = TypeG.VOID;
            TypeG typeRequired2 = TypeG.VOID;
            TypeG returnType = TypeG.VOID;
            switch (name)
            {
                case "WrInt":
                    parametersRequired = 1;
                    typeRequired = TypeG.INTEGER;
                    break;
                case "WrStr":
                    parametersRequired = 1;
                    typeRequired = TypeG.STRING;
                    break;
                case "WrLn":
                    break;
                case "RdInt":
                    returnType = TypeG.INTEGER;
                    break;
                case "RdStr":
                    returnType = TypeG.STRING;
                    break;
                case "AtStr":
                    parametersRequired = 2;
                    typeRequired = TypeG.STRING;
                    typeRequired2 = TypeG.INTEGER;
                    returnType = TypeG.STRING;
                    break;
                case "LenStr":
                    parametersRequired = 1;
                    typeRequired = TypeG.STRING;
                    returnType = TypeG.INTEGER;
                    break;
                case "CmpStr":
                    parametersRequired = 2;
                    typeRequired = TypeG.STRING;
                    typeRequired2 = TypeG.STRING;
                    returnType = TypeG.INTEGER;
                    break;
                case "CatStr":
                    parametersRequired = 2;
                    typeRequired = TypeG.STRING;
                    typeRequired2 = TypeG.STRING;
                    returnType = TypeG.STRING;
                    break;

                case "LenLstInt":
                    parametersRequired = 1;
                    typeRequired = TypeG.INTEGER_LIST;
                    returnType = TypeG.INTEGER;
                    break;
                case "LenLstStr":
                    parametersRequired = 1;
                    typeRequired = TypeG.STRING_LIST;
                    returnType = TypeG.INTEGER;
                    break;
                case "LenLstBool":
                    parametersRequired = 1;
                    typeRequired = TypeG.BOOLEAN_LIST;
                    returnType = TypeG.INTEGER;
                    break;

                case "NewLstInt":
                    parametersRequired = 1;
                    typeRequired = TypeG.INTEGER;
                    returnType = TypeG.INTEGER_LIST;
                    break;
                case "NewLstStr":
                    parametersRequired = 1;
                    typeRequired = TypeG.INTEGER;
                    returnType = TypeG.STRING_LIST;
                    break;
                case "NewLstBool":
                    parametersRequired = 1;
                    typeRequired = TypeG.INTEGER;
                    returnType = TypeG.BOOLEAN_LIST;
                    break;
                case "IntToStr":
                    parametersRequired = 1;
                    typeRequired = TypeG.INTEGER;
                    returnType = TypeG.STRING;
                    break;
                case "StrToInt":
                    parametersRequired = 1;
                    typeRequired = TypeG.STRING;
                    returnType = TypeG.INTEGER;
                    break;
                default:
                    throw new Exception($"Function {name} has no declared call");
            }

            //ListLocalDeclarationTable[CurrentContext.index][variableName] = new LocalDeclarationType(variableName, TypeG.INTEGER, variableValue, -1, TypeG.VAR);

            dynamic tipo = Visit((dynamic)node[0]);
            Console.WriteLine("QUE PEDO"+node[0]);
            if (node.getLength() == parametersRequired)
            {
                if (typeRequired == tipo)
                {
                    return returnType;
                }
                else
                {
                    throw new SemanticError($"No type match for Call: "
                        + $"expected {typeRequired} but got {tipo}", node.AnchorToken);
                }
            }
            else
            {
                throw new SemanticError($"Wrong number of params to procedure call: "
                    + $"expected {parametersRequired} but got {node.getLength()}", node.AnchorToken);
            }
            
            
                    /*
                    var _params = procedure.symbols.Where(kv => kv.Value.procType == ProcedureType.PARAM)
                                                .OrderBy(kv => kv.Value.pos)
                                                .ToList();
                    if (node.Count() != _params.Count())
                    {
                        throw new SemanticError($"Wrong number of params to procedure call: "
                            + $"expected {_params.Count()} but got {node.Count()}", node.AnchorToken);
                    }
                    for (int i = 0; i < _params.Count; ++i)
                    {
                        var _node = node[i];
                        var _param = _params[i];
                        Type nodeType = Visit((dynamic)_node);

                        bool typesCompatible;
                        if (nodeType == Type.LIST || _param.Value.type == Type.LIST)
                        {
                            Type otherType = nodeType == Type.LIST ? _param.Value.type : nodeType;
                            var valid = new Type[] { Type.LIST, Type.BOOL_LIST, Type.INT_LIST, Type.STR_LIST };
                            typesCompatible = valid.Contains(otherType);
                        }
                        else
                        {
                            typesCompatible = nodeType == _param.Value.type;
                        }

                        if (!typesCompatible)
                        {
                            throw new SemanticError($"Incompatible types {nodeType} and {_param.Value.type} for parameter {_param.Key}",
                                _node.AnchorToken);
                        }
                    }
                    return procedure.type;
                }
                else
                {
                    throw new SemanticError($"Undeclared procedure: {name}", node.AnchorToken);
                }
            }*/
                    //-------------------
                    return TypeG.VOID;
        }

        public TypeG Visit(SubstractOperator node)
        {
            Console.WriteLine("LLEGUE");
            VisitBinaryOperator("-", node, TypeG.INTEGER);
            return TypeG.INTEGER;
        }

        void VisitBinaryOperator(String op, Node node, TypeG type)
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


        public TypeG Visit(Return node)
        {
            //Console.WriteLine("INICIO");
            //VisitChildren(node);
            //Console.WriteLine("FIN");
            return Visit((dynamic)node[0]);
        }

        public TypeG Visit(RemOperator node)
        {
            VisitBinaryOperator("REM",node, TypeG.INTEGER);
            return TypeG.INTEGER;
        }


        public TypeG Visit(DivOperator node)
        {
            VisitBinaryOperator("DIV", node, TypeG.INTEGER);
            return TypeG.INTEGER;
        }

        public TypeG Visit(Loop node)
        {
            CurrentContext.insideLoop = true;
            //Console.WriteLine("INICIO");
            VisitChildren(node);
            //Console.WriteLine("FIN");
            CurrentContext.insideLoop = false;
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
            VisitBinaryOperator("<", node, TypeG.INTEGER);
            return TypeG.BOOLEAN;
            //return TypeG.VOID;
        }

        public TypeG Visit(LessOrEqualOperator node)
        {
            Console.WriteLine("INICIO");
            VisitBinaryOperator("<=", node, TypeG.INTEGER);
            return TypeG.BOOLEAN;
            //return TypeG.VOID;
        }


        public TypeG Visit(Type node)
        {
            if (node.AnchorToken.Lexeme == "integer")
            {
                return TypeG.INTEGER;
            } else if (node.AnchorToken.Lexeme == "boolean")
            {
                return TypeG.BOOLEAN;
            }
            else if (node.AnchorToken.Lexeme == "string")
            {
                return TypeG.STRING;
            }
            return TypeG.VOID;

        }
        //-----------------------------------------------------------

        public TypeG Visit(ListTypeNode node)
        {
            dynamic listType;
            Console.WriteLine(node.AnchorToken.Category);
            switch (node.AnchorToken.Lexeme)
            {
                case "integer":
                    return TypeG.INTEGER_LIST;
                case "string":
                    return TypeG.STRING_LIST;
                case "boolean":
                    return TypeG.BOOLEAN_LIST;
                default:
                    throw new Exception($"Type {node} has no equivalent list type");
            }

            return listType;
        }


        public TypeG Visit(Exit node)
        {
            if (!CurrentContext.insideLoop)
            {
                throw new SemanticError("Unexpected exit statement", node.AnchorToken);
            }
            return TypeG.VOID;
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
