/*
Dachely Otero Argote            A01377812
José María Aguíñiga Díaz        A01376669 
José Rodrigo Narváez Berlanga   A01377566
 */

using System;
using System.Collections.Generic;
using System.Collections;

namespace Chimera {

    class SemanticAnalyzer {

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
        public GloabalDeclaratonTable GloabalDeclaratonT {
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

        private void fillDefineProcedure() {
            //Input/Output Operations
            //ProcedureTable["WrInt"] = new List{VOID, true, new SymbolTable("WrInt")};
            // ProcedureTable["WrStr"] = new ArrayList{VOID, true, new SymbolTable("WrStr")};
            // ProcedureTable["WrBool"] = new ArrayList{VOID, true, new SymbolTable("WrBool")};
            // ProcedureTable["WrLn"] = new ArrayList{VOID, true, new SymbolTable("WrLn")};
            // ProcedureTable["RdInt"] = new ArrayList{INTEGER, true};
            // ProcedureTable["RdStr"] = new ArrayList{STRING, true};
            // //String Operations
            // ProcedureTable["AtStr"] = new ArrayList{STRING, true, new SymbolTable("AtStr")};
            // ProcedureTable["LenStr"] = new ArrayList{INTEGER, true, new SymbolTable("LenStr")};
            // ProcedureTable["CmpStr"] = new ArrayList{INTEGER, true, new SymbolTable("CmpStr")};
            // ProcedureTable["CatStr"] = new ArrayList{STRING, true, new SymbolTable("CatStr")};
            // //List Operations
            // ProcedureTable["LenLstInt"] = new ArrayList{INTEGER, true, new SymbolTable("LenLstInt")};
            // ProcedureTable["LenLstStr"] = new ArrayList{INTEGER, true, new SymbolTable("LenLstStr")};
            // ProcedureTable["LenLstBool"] = new ArrayList{INTEGER, true, new SymbolTable("LenLstBool")};
            // ProcedureTable["NewLstInt"] = new ArrayList{LIST, true, new SymbolTable("NewLstInt")};
            // ProcedureTable["NewLstStr"] = new ArrayList{LIST, true, new SymbolTable("NewLstStr")};
            // ProcedureTable["NewLstBool"] = new ArrayList{LIST, true, new SymbolTable("NewLstBool")};
            // //Conversion Operations
            // ProcedureTable["IntToStr"] = new ArrayList{STRING, true, new SymbolTable("IntToStr")};
            // ProcedureTable["StrToInt"] = new ArrayList{INTEGER, true, new SymbolTable("StrToInt")};

        }

        //-----------------------------------------------------------
        public SemanticAnalyzer() {
            GloabalDeclaratonT = new GloabalDeclaratonTable();
            //GlobalDeclarationTable = new SymbolTable("Global Declaration Table");
            //LocalDeclarationTable = new SymbolTable("Local Declaration Table");
        }

        //-----------------------------------------------------------
        public TypeG Visit(Program node) {
            //fillDefineProcedure();
            Visit((dynamic) node[0]);
            //Visit((dynamic) node[1]);
            return TypeG.VOID;
        }

        // Buttercup
        
        //-----------------------------------------------------------
        public TypeG Visit(ConstantDeclarationList node) {
            VisitChildren(node);
            return TypeG.VOID;
        }
        
        //-----------------------------------------------------------
        public TypeG Visit(ConstantDeclaration node) {
            var variableName = node.AnchorToken.Lexeme;
            var varieblaValue = node[0].AnchorToken.Lexeme;
            if (GloabalDeclaratonT.Contains(variableName)) {
                throw new SemanticError(
                    "Duplicated variable: " + variableName,
                    node[0].AnchorToken);

            } else {
                GloabalDeclaratonT[variableName] = 
                    new GlobalDeclarationType(variableName, TypeG.CONST/*typeMapperConstDecl[node.AnchorToken.Category]*/, varieblaValue, true);
                            
                // para tener dos constates
                 GloabalDeclaratonT["otroo"] = new GlobalDeclarationType("es prueba", TypeG.CONST, 5, true);
                     
            }

            return TypeG.VOID;
        }

        void VisitChildren(Node node) {
            foreach (var n in node) {
                Visit((dynamic) n);
            }
        }

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
