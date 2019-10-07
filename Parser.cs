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
            Expect(TokenCategory.CONST);
            Expect(TokenCategory.IDENTIFIER);
            Expect(TokenCategory.CONSTANTDECLARATION);
            Literal();
            Expect(TokenCategory.ENDLINE);
        }

        public void ParameterDeclaration()
        {
            Expect(TokenCategory.IDENTIFIER);
            while(CurrentToken == TokenCategory.COMMA)
            {
                Expect(TokenCategory.IDENTIFIER);
            }
            Expect(TokenCategory.DECLARATION);
            Type();
            Expect(TokenCategory.ENDLINE);
        }

        public void ProcedureDeclaration()
        {
            Expect(TokenCategory.PROCEDURE);
            Expect(TokenCategory.IDENTIFIER);
            Expect(TokenCategory.INITPARENTHESIS);
            while(CurrentToken == TokenCategory.IDENTIFIER)
            {
                ParameterDeclaration();
            }
            Expect(TokenCategory.CLOSINGPARENTHESIS);
            if(CurrentToken == TokenCategory.DECLARATION)
            {
                Type();
            }
            Expect(TokenCategory.ENDLINE); // hay que agregar esto, dache
            if (CurrentToken == TokenCategory.CONST)
            {
                do
                {
                    ConstantDeclaration();
                } while (CurrentToken == TokenCategory.IDENTIFIER);
            }

            if (CurrentToken == TokenCategory.VAR)
            {
                do
                {
                    VariableDeclaration();
                } while (CurrentToken == TokenCategory.IDENTIFIER);
            }

            Expect(TokenCategory.BEGIN);
            while (firstOfStatement.Contains(CurrentToken))
            {
                Statement();
            }
            Expect(TokenCategory.END);
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
                SimpleLiteral();
                while(CurrentToken == TokenCategory.COMMA){
                    Expect(TokenCategory.COMMA);
                    SimpleLiteral();
                }
            }            
            Expect(TokenCategory.ENDLINE);
        }

        public void VariableDeclaration(){
            Expect(TokenCategory.VAR);
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
                case TokenCategory.IDENTIFIER:
                    AssignmentCallStatement();
                    break;
                case TokenCategory.IF:
                    If();
                    break;
                case TokenCategory.LOOP:
                    Loop();
                    break;
                case TokenCategory.FOR:
                    For();
                    break;
                case TokenCategory.RETURN:
                    Return();
                    break;
                case TokenCategory.EXIT:
                    Exit();
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
                Expect(TokenCategory.CONSTANTDECLARATION);
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

        public void If() {
            Expect(TokenCategory.IF);
            Expression();
            Expect(TokenCategory.THEN);
            while (firstOfStatement.Contains(CurrentToken)) {
                Statement();
            }
            if (CurrentToken == TokenCategory.ELSEIF) {
                Expression();
                Expect(TokenCategory.THEN);
                Statement();
            } else if (CurrentToken == TokenCategory.ELSE) {
                Statement();
            }
            Expect(TokenCategory.END);
            Expect(TokenCategory.ENDLINE);
        }

        public void Loop() {
            Expect(TokenCategory.LOOP);
            while(firstOfStatement.Contains(CurrentToken)) {
                Statement();
            }
            Expect(TokenCategory.END);
            Expect(TokenCategory.ENDLINE);
        }
        public void For()
        {
            Expect(TokenCategory.FOR);
            Expect(TokenCategory.IDENTIFIER);
            Expect(TokenCategory.IN);
            Expression();
            Expect(TokenCategory.DO);
            while (firstOfStatement.Contains(CurrentToken))
            {
                Statement();
            }
            Expect(TokenCategory.END);
            Expect(TokenCategory.ENDLINE);
        }

        public void Return()
        {
            Expect(TokenCategory.RETURN);
            if (firstOfSimpleExpression.Contains(CurrentToken))
            {
                Expression();
            }
            Expect(TokenCategory.ENDLINE);
        }

        public void Exit()
        {
            Expect(TokenCategory.EXIT);
            Expect(TokenCategory.ENDLINE);
        }

        public void Expression(){
            LogicExpression();
        }

        public void LogicExpression(){
            RelationalExpression();
            //while(CurrentToken == TokenCategory.AND | CurrentToken == TokenCategory.OR | CurrentToken == TokenCategory.XOR){
            while(firstOfOperator.Contains(CurrentToken)){
                LogicOperator();
                RelationalExpression();
            }            
        }

        public void RelationalExpression(){
            SumExpression();
            while(CurrentToken == TokenCategory.EQUAL | CurrentToken == TokenCategory.INEQUAL | CurrentToken == TokenCategory.LESSTHAN | CurrentToken == TokenCategory.BIGGERTHAN | CurrentToken == TokenCategory.LESSOREQUAL | CurrentToken == TokenCategory.BIGGEROREQUAL) {
                RelationalOperator();
                SumExpression();
            }
        }

        public void LogicOperator(){
            switch(CurrentToken) {
                case TokenCategory.AND:
                    Expect(TokenCategory.AND);
                    break;
                case TokenCategory.OR:
                    Expect(TokenCategory.OR);
                    break;
                case TokenCategory.XOR:
                    Expect(TokenCategory.XOR);
                    break;
                default:
                    throw new SyntaxError(firstOfStatement, 
                                        tokenStream.Current);
            }
        }

        public void SumExpression() {
            MulExpression();
            while(CurrentToken == TokenCategory.ADDITION | CurrentToken == TokenCategory.SUBSTRACT) {
                SumOperator();
                MulExpression();
            }

        }

        public void RelationalOperator() {
            switch(CurrentToken) {
                case TokenCategory.EQUAL:
                    Expect(TokenCategory.EQUAL);
                    break;
                case TokenCategory.INEQUAL:
                    Expect(TokenCategory.INEQUAL);
                    break;
                case TokenCategory.LESSTHAN:
                    Expect(TokenCategory.LESSTHAN);
                    break;
                case TokenCategory.BIGGERTHAN:
                    Expect(TokenCategory.BIGGERTHAN);
                    break;
                case TokenCategory.LESSOREQUAL:
                    Expect(TokenCategory.LESSOREQUAL);
                    break;
                case TokenCategory.BIGGEROREQUAL:
                    Expect(TokenCategory.BIGGEROREQUAL);
                    break;
                default:
                    throw new SyntaxError(firstOfStatement, 
                                        tokenStream.Current);
            }

        }

        public void MulExpression() {
            UnaryExpression();
            while(CurrentToken == TokenCategory.MULTIPLICATION | CurrentToken == TokenCategory.DIV | CurrentToken == TokenCategory.REM) {
                MulOperator();
                UnaryExpression();
            }
        }


        public void SumOperator() {
            switch(CurrentToken) {
                case TokenCategory.ADDITION:
                    Expect(TokenCategory.ADDITION);
                    break;
                case TokenCategory.SUBSTRACT:
                    Expect(TokenCategory.SUBSTRACT);
                    break;
                default:
                    throw new SyntaxError(firstOfStatement, 
                                        tokenStream.Current);
            }
        }

        public void UnaryExpression() {
            switch(CurrentToken) {
                case TokenCategory.NOT:
                    Expect(TokenCategory.NOT);
                    UnaryExpression();
                    break;
                case TokenCategory.SUBSTRACT:
                    Expect(TokenCategory.SUBSTRACT);
                    UnaryExpression();
                    break;
                default:
                    SimpleExpression();
                    break;
            }
        }

        public void MulOperator() {
            switch(CurrentToken) {
                case TokenCategory.MULTIPLICATION:
                    Expect(TokenCategory.MULTIPLICATION);
                    break;
                case TokenCategory.DIV:
                    Expect(TokenCategory.DIV);
                    break;
                case TokenCategory.REM:
                    Expect(TokenCategory.REM);
                    break;
                default:
                    throw new SyntaxError(firstOfStatement, 
                                        tokenStream.Current);
            }
        }
        public void SimpleExpression() {
            switch (CurrentToken) { 
            case TokenCategory.INITPARENTHESIS:
                Expression();
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
            case TokenCategory.INITLIST:
                List();
                break;
            case TokenCategory.IDENTIFIER:
                Expect(TokenCategory.IDENTIFIER);
                if(CurrentToken == TokenCategory.INITPARENTHESIS){
                    Call();
                }
                break;
            default:
                throw new SyntaxError(firstOfSimpleExpression, 
                    tokenStream.Current);
            }
//<<<<<<< HEAD

            if (CurrentToken == TokenCategory.INITBRACKET)
//=======
            if (CurrentToken == TokenCategory.INITBRACKET)
//>>>>>>> 20d8ca772040af63f787ef44ef401d54daa5e786
            {
                Expression();
                Expect(TokenCategory.CLOSINGBRACKET);
            }
//<<<<<<< HEAD
//=======
                
//>>>>>>> 20d8ca772040af63f787ef44ef401d54daa5e786
        }

        public void Call(){
            Expect(TokenCategory.INITPARENTHESIS);
            if(CurrentToken == TokenCategory.NOT | CurrentToken == TokenCategory.SUBSTRACT | CurrentToken == TokenCategory.INTEGERLITERAL | CurrentToken == TokenCategory.STRINGLITERAL | CurrentToken == TokenCategory.BOOLEANITERAL){
                Expression();
                while(CurrentToken == TokenCategory.COMMA){
                    Expect(TokenCategory.COMMA);
                    Expression();
                }                
            }
            Expect(TokenCategory.CLOSINGPARENTHESIS);            
        }


    }
}