using System;
using System.Collections.Generic;

namespace Chimera {

    class Parser {
        
        static readonly ISet<TokenCategory> firstOfDeclaration =
            new HashSet<TokenCategory>() {
                TokenCategory.CONST,
                TokenCategory.VAR,
                TokenCategory.PROCEDURE, //--> creo que hay que agregar esto
                TokenCategory.PROGRAM,
            };

        static readonly ISet<TokenCategory> firstOfStatement =
            new HashSet<TokenCategory>() {
                TokenCategory.IDENTIFIER,
                //TokenCategory.PROCEDURE, --> creo que no
                //TokenCategory.PRINT, --> Ya estaba comentado
                TokenCategory.IF,
                TokenCategory.FOR,
                //TokenCategory.WHILE, --> Ya estaba comentado
                TokenCategory.LOOP,
                //TokenCategory.BEGIN, --> creo que no
                TokenCategory.RETURN, 
                // TokenCategory.DO  --> creo que no
                TokenCategory.EXIT,  // creo que hay que agregar esto
            };

        static readonly ISet<TokenCategory> firstOfOperator =
            new HashSet<TokenCategory>() {
                TokenCategory.AND,
                TokenCategory.OR,
                TokenCategory.XOR,
                TokenCategory.MULTIPLICATION, // Hayq que agregarlo
                TokenCategory.DIV,
                TokenCategory.REM,
                //TokenCategory.NOT, // Creo que no hay que ponerlo
                TokenCategory.EQUAL,
                TokenCategory.INEQUAL,
                TokenCategory.LESSTHAN,
                TokenCategory.BIGGERTHAN,
                TokenCategory.LESSOREQUAL,
                TokenCategory.BIGGEROREQUAL,
                TokenCategory.ADDITION, // Hay que agregarlo
                TokenCategory.SUBSTRACT, // Hay que agregarlo
            };
        
        static readonly ISet<TokenCategory> firstOfSimpleExpression =
            new HashSet<TokenCategory>() {
                TokenCategory.INITPARENTHESIS,
                TokenCategory.IDENTIFIER,
                TokenCategory.INTEGERLITERAL,
                TokenCategory.STRINGLITERAL,
                TokenCategory.BOOLEANITERAL,
                TokenCategory.INITLIST,
                TokenCategory.INITBRACKET,
                TokenCategory.NOT,
                TokenCategory.SUBSTRACT,
            };
                
        IEnumerator<Token> tokenStream;

        public Parser(IEnumerator<Token> tokenStream) {
            this.tokenStream = tokenStream;
            this.tokenStream.MoveNext();
        }

        public TokenCategory CurrentToken {
            get { return tokenStream.Current.Category; }
        }

        public Token Expect(TokenCategory category) {
            if (CurrentToken == category) {
                Token current = tokenStream.Current;
                tokenStream.MoveNext();
                return current;
            } else {
                throw new SyntaxError(category, tokenStream.Current);                
            }
        }

        //dache
        public void Program() {
            if (CurrentToken == TokenCategory.CONST) {
                do{
                    ConstantDeclaration();
                }while(CurrentToken == TokenCategory.IDENTIFIER);
            }

            if (CurrentToken == TokenCategory.VAR) {
                do{
                    VariableDeclaration();
                }while(CurrentToken == TokenCategory.IDENTIFIER);
            }
                   
            while(CurrentToken == TokenCategory.PROCEDURE){
                ProcedureDeclaration();
            }

            Expect(TokenCategory.PROGRAM);
            while(firstOfStatement.Contains(CurrentToken)){
                Statement();
            }
            Expect(TokenCategory.END);
            Expect(TokenCategory.ENDLINE);                         

            Expect(TokenCategory.EOF);
        }

        public void ConstantDeclaration() {
            Expect(TokenCategory.IDENTIFIER);
            Expect(TokenCategory.CONSTANTDECLARATION);
            Literal();
            Expect(TokenCategory.ENDLINE);
        }

        public void Literal(){
            switch(CurrentToken){
                case TokenCategory.INITLIST:
                    List();
                    break;
                case TokenCategory.INTEGERLITERAL:
                    SimpleLiteral();
                    break;
                case TokenCategory.STRINGLITERAL:
                    SimpleLiteral();
                    break;
                case TokenCategory.BOOLEANITERAL:
                    SimpleLiteral();
                    break;
                default:
                    throw new SyntaxError(firstOfSimpleExpression, 
                                        tokenStream.Current);            
            }
        }

        public void SimpleLiteral(){
            switch (CurrentToken) {
                case TokenCategory.INTEGERLITERAL:
                    Expect(TokenCategory.INTEGERLITERAL);
                    break;
                case TokenCategory.STRINGLITERAL:
                    Expect(TokenCategory.STRINGLITERAL);
                    break;
                case TokenCategory.BOOLEANITERAL:
                    Expect(TokenCategory.BOOLEANITERAL);
                    break;
                default:
                    throw new SyntaxError(firstOfSimpleExpression, 
                                        tokenStream.Current);
            }
        }

        public void List(){
            Expect(TokenCategory.INITLIST);
            if(CurrentToken == TokenCategory.INTEGERLITERAL | CurrentToken == TokenCategory.STRINGLITERAL | CurrentToken == TokenCategory.BOOLEANITERAL){
                SimpleExpression();
                while(CurrentToken == TokenCategory.COMMA){
                    Expect(TokenCategory.COMMA);
                    SimpleLiteral();
                }
            }            
            Expect(TokenCategory.ENDLINE);
        }

        public void VariableDeclaration(){
            Expect(TokenCategory.IDENTIFIER);
            while(CurrentToken == TokenCategory.COMMA){
                Expect(TokenCategory.COMMA);
                Expect(TokenCategory.IDENTIFIER);
            }
            Expect(TokenCategory.DECLARATION);
            Type();
            Expect(TokenCategory.ENDLINE);
        }

        public void Type(){
            switch(CurrentToken){
                case TokenCategory.LIST:
                    ListType();
                    break;
                case TokenCategory.INTEGER:
                    SimpleType();
                    break;
                case TokenCategory.STRING:
                    SimpleType();
                    break;
                case TokenCategory.BOOLEAN:
                    SimpleType();
                    break;
                default:
                // No estoy segura de qué clase mandar
                    throw new SyntaxError(firstOfSimpleExpression, 
                                        tokenStream.Current);            
            }
        }

        public void SimpleType(){
            switch (CurrentToken) {
                case TokenCategory.INTEGER:
                    Expect(TokenCategory.INTEGER);
                    break;
                case TokenCategory.STRING:
                    Expect(TokenCategory.STRING);
                    break;
                case TokenCategory.BOOLEAN:
                    Expect(TokenCategory.BOOLEAN);
                    break;
                default:
                // No estoy segura de que clase mandar
                    throw new SyntaxError(firstOfSimpleExpression, 
                                        tokenStream.Current);
            }
        }

        public void ListType(){
            Expect(TokenCategory.LIST);
            Expect(TokenCategory.OF);
            SimpleType();
        }

        public void Statement() {
            switch (CurrentToken) {
                case TokenCategory.INTEGER:
                    AssignmentCallStatement();
                    break;
                case TokenCategory.IF:
                    IfStatement();
                    break;
                case TokenCategory.LOOP:
                    LoopStatement();
                    break;
                case TokenCategory.FOR:
                    ForStatement();
                    break;
                case TokenCategory.RETURN:
                    ReturnStatement();
                    break;
                case TokenCategory.EXIT:
                    EXITStatement();
                    break;
                default:
                    throw new SyntaxError(firstOfStatement, 
                                        tokenStream.Current);
            }            
        }

        public void AssignmentCallStatement(){
            Expect(TokenCategory.IDENTIFIER);
            if(CurrentToken == TokenCategory.INITBRACKET | CurrentToken == TokenCategory.CONSTANTDECLARATION ){
                AssignmentStatement();
            }
            else if(CurrentToken == TokenCategory.INITPARENTHESIS){
                CallStatement();
            }
        }

        public void AssignmentStatement(){
            if(CurrentToken == TokenCategory.INITBRACKET){
                Expect(TokenCategory.INITBRACKET);
                Expression();
                Expect(TokenCategory.CLOSINGBRACKET);
                Expect(TokenCategory.ConstantDeclaration);
                Expression();
                Expect(TokenCategory.ENDLINE);
            }
        }

        public void CallStatement(){
            Expect(TokenCategory.INITPARENTHESIS);
            if(CurrentToken == TokenCategory.NOT | CurrentToken == TokenCategory.SUBSTRACT | CurrentToken == TokenCategory.INTEGERLITERAL | CurrentToken == TokenCategory.STRINGLITERAL | CurrentToken == TokenCategory.BOOLEANITERAL){
                Expression();
                while(CurrentToken == TokenCategory.COMMA){
                    Expect(TokenCategory.COMMA);
                    Expression();
                }                
            }
            Expect(TokenCategory.CLOSINGPARENTHESIS);
            Expect(TokenCategory.ENDLINE);
        }


        public void Assignment() {
            Expect(TokenCategory.IDENTIFIER);
            //Expression(); revisar como verificar 
            Expect(TokenCategory.CONSTANTDECLARATION);
            Expression(); //Seguir revisando expression()
            Expect(TokenCategory.ENDLINE);
        }

        public void Print() {
            // Expect(TokenCategory.PRINT);
            // Expression();
        }

        public void If() {
            Expect(TokenCategory.IF);
            // Expression();
            Expect(TokenCategory.THEN);
            // while (firstOfStatement.Contains(CurrentToken)) {
            //     Statement();
            // }
            // Expect(TokenCategory.END);
        }

        public void Expression() {
            SimpleExpression();
            //ver si descomentar lo siguiente
            while (firstOfOperator.Contains(CurrentToken)) {
                // Operator();
                SimpleExpression();
            }
        }

        public void SimpleExpression() {

            switch (CurrentToken) {

            case TokenCategory.IDENTIFIER:
                Expect(TokenCategory.IDENTIFIER);
                break;

            case TokenCategory.INITBRACKET:
                Expect(TokenCategory.INITBRACKET);
                Expression();
                Expect(TokenCategory.CLOSINGBRACKET);
                break;

            case TokenCategory.INTEGERLITERAL:
                Expect(TokenCategory.INTEGERLITERAL);
                break;

            case TokenCategory.TRUE:
                Expect(TokenCategory.TRUE);
                break;

            case TokenCategory.FALSE:
                Expect(TokenCategory.FALSE);
                break;

            // case TokenCategory.NEG:
            //     Expect(TokenCategory.NEG);
            //     SimpleExpression();
            //     break;

            default:
                throw new SyntaxError(firstOfSimpleExpression, 
                                      tokenStream.Current);
            }
        }

        public void Procedure() {
            Expect(TokenCategory.PROCEDURE);
            Expect(TokenCategory.IDENTIFIER);
        }

        public void Operator() {

            // switch (CurrentToken) {

            // case TokenCategory.AND:
            //     Expect(TokenCategory.AND);
            //     break;

            // case TokenCategory.LESS:
            //     Expect(TokenCategory.LESS);
            //     break;

            // case  TokenCategory.PLUS:
            //     Expect(TokenCategory.PLUS);
            //     break;

            // case TokenCategory.MUL:
            //     Expect(TokenCategory.MUL);
            //     break;

            // default:
            //     throw new SyntaxError(firstOfOperator, 
            //                           tokenStream.Current);
            // }
        }

        

    }
}