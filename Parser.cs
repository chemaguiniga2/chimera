/*
Dachely Otero Argote            A01377812
José María Aguíñiga Díaz        A01376669 
José Rodrigo Narváez Berlanga   A01377566
 */

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
        public Node Program() {
            var statList = new StatementList();
            var procDecList = new ProcedureDeclarationList();
            var consDecList = new ConstantDeclarationList();
            var varDecList = new VariableDeclarationList();

            if (CurrentToken == TokenCategory.CONST) {
                do{
                    consDecList.Add(ConstantDeclaration());
                }while(CurrentToken == TokenCategory.IDENTIFIER);
            }

            if (CurrentToken == TokenCategory.VAR) {
                do{
                    varDecList.Add(VariableDeclaration());
                }while(CurrentToken == TokenCategory.IDENTIFIER);
            }
                   
            while(CurrentToken == TokenCategory.PROCEDURE){
                procDecList.Add(ProcedureDeclaration());
            }

            Expect(TokenCategory.PROGRAM);
            while(firstOfStatement.Contains(CurrentToken)){
                statList.Add(Statement());
            }
            Expect(TokenCategory.END);
            Expect(TokenCategory.ENDLINE); 
            Expect(TokenCategory.EOF);

            return new Program(){
                consDecList,
                varDecList,
                procDecList,
                statList
            };
        }

        public Node ConstantDeclaration() {
            Expect(TokenCategory.CONST);
            var idToken = Expect(TokenCategory.IDENTIFIER);
            Expect(TokenCategory.CONSTANTDECLARATION);
            var lit = Literal();
            Expect(TokenCategory.ENDLINE);
            var result = new ConstantDeclaration() { lit };
            result.AnchorToken = idToken;
            return result;
        }

        public Node VariableDeclaration()
        {
            Expect(TokenCategory.VAR);
            var idToken = Expect(TokenCategory.IDENTIFIER);
            var idList = IdentifierList();
            while (CurrentToken == TokenCategory.COMMA)
            {
                idList.Add(TokenCategory.IDENTIFIER);
            }

            Expect(TokenCategory.CONSTANT);
            var type = Type();
            Expect(TokenCategory.ENDLINE);
            var result = new VariableDeclaration() { idToken, idList, type };
            result.AnchorToken = idToken;
            return result;
        }

        public Node Literal()
        {
            switch (CurrentToken)
            {
                case TokenCategory.INITLIST:
                    return List();
                case TokenCategory.INTEGERLITERAL:
                    return SimpleLiteral();
                case TokenCategory.STRINGLITERAL:
                    return SimpleLiteral();
                case TokenCategory.BOOLEANITERAL:
                    return SimpleLiteral();
                default:
                    throw new SyntaxError(firstOfSimpleExpression,
                                        tokenStream.Current);
            }
        }

        public Node SimpleLiteral()
        {
            switch (CurrentToken)
            {
                case TokenCategory.INTEGERLITERAL:

                    return List();
                    return new IntegerLiteral()
                    {
                        AnchorToken = Expect(TokenCategory.INTEGERLITERAL)
                    };
                case TokenCategory.STRINGLITERAL:
                    return new StringLiteral()
                    {
                        AnchorToken = Expect(TokenCategory.STRINGLITERAL)
                    };
                case TokenCategory.BOOLEANITERAL:
                    return new BooleanLiteral()
                    {
                        AnchorToken = Expect(TokenCategory.BOOLEANITERAL)
                    };
                default:
                    throw new SyntaxError(firstOfSimpleExpression,
                                        tokenStream.Current);
            }
        }

        public Node Type()
        {
            switch (CurrentToken)
            {
                case TokenCategory.LIST:
                    return ListType();
                case TokenCategory.STRINGLITERAL:
                    return SimpleType();
                case TokenCategory.BOOLEANITERAL:
                    return SimpleType();
                case TokenCategory.INTEGERLITERAL:
                    return SimpleType();
                default:
                    throw new SyntaxError(firstOfSimpleExpression,
                                        tokenStream.Current);
            }
        }

        public Node ListType()
        {
            Expect(TokenCategory.LIST);
            Expect(TokenCategory.OF);
            switch (CurrentToken)
            {
                case TokenCategory.STRINGLITERAL:
                    return SimpleType();
                case TokenCategory.BOOLEANITERAL:
                    return SimpleType();
                case TokenCategory.INTEGERLITERAL:
                    return SimpleType();
                default:
                    throw new SyntaxError(firstOfSimpleExpression,
                                        tokenStream.Current);
            }
        }

        public Node ProcedureDeclaration()
        {
            var procToken = Expect(TokenCategory.PROCEDURE);
            var inden = Expect(TokenCategory.IDENTIFIER);
            var parDecList = ParameterDeclarationList();
            var type;
            var consDecList = ConstantDeclarationList();
            var varDecList = VariableDeclarationList();
            var statement = StatementList();

            Expect(TokenCategory.INITPARENTHESIS);
            while (CurrentToken == TokenCategory.IDENTIFIER)
            {
                parDecList.Add(ParameterDeclaration());
            }
            Expect(TokenCategory.CLOSINGPARENTHESIS);
            if (CurrentToken == TokenCategory.DECLARATION)
            {
                Expect(TokenCategory.DECLARATION);
                type = Type();
            }
            Expect(TokenCategory.ENDLINE); // hay que agregar esto, dache
            if (CurrentToken == TokenCategory.CONST)
            {
                do
                {
                    consDecList.Add(ConstantDeclaration());
                } while (CurrentToken == TokenCategory.IDENTIFIER);
            }

            if (CurrentToken == TokenCategory.VAR)
            {
                do
                {
                    varDecList.Add(VariableDeclaration());
                } while (CurrentToken == TokenCategory.IDENTIFIER);
            }

            Expect(TokenCategory.BEGIN);
            while (firstOfStatement.Contains(CurrentToken))
            {
                statement.Add(Statement());
            }
            Expect(TokenCategory.END);
            Expect(TokenCategory.ENDLINE);
            var result = new ProcedureDeclaration() { inden, parDecList, type, consDecList, varDecList, statement };
            result.AnchorToken = procToken;
            return result;
        }


        public Node ParameterDeclaration()
        {
            Expect(TokenCategory.PARAM);
            var paramToken = Expect(TokenCategory.IDENTIFIER);
            var paramList = IdentifierList();
            while (CurrentToken == TokenCategory.COMMA)
            {
                paramList.Add(TokenCategory.IDENTIFIER);
            }

            Expect(TokenCategory.CONSTANT);
            var type = Type();
            Expect(TokenCategory.ENDLINE);
            var result = new ParameterDeclaration() { paramToken, paramList, type };
            result.AnchorToken = idToken;
            return result;
        }
        
        

        public Node List()
        {
            Expect(TokenCategory.INITLIST);
            var simpleLitList = SimpleLiteralList();
            simpleLitList.add(SimpleLiteral());
            while (CurrentToken == TokenCategory.COMMA)
            {
                Expect(TokenCategory.COMMA);
                simpleLitList.add(SimpleLiteral());
            }
            Expect(TokenCategory.CLOSINGLIST);
            var result = new List() { simpleLitList };
            result.AnchorToken = procToken;
            return result;
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

        public Node Statement() {
            switch (CurrentToken) {
                case TokenCategory.IDENTIFIER:
                    return AssignmentCallStatement();
                case TokenCategory.IF:
                    return If();
                case TokenCategory.LOOP:
                    return Loop();
                case TokenCategory.FOR:
                    return For();
                case TokenCategory.RETURN:
                    return Return();
                case TokenCategory.EXIT:
                    return Exit();
                default:
                    throw new SyntaxError(firstOfStatement, 
                                        tokenStream.Current);
            }            
        }

        public Node AssignmentCallStatement(){
            var identif = Expect(TokenCategory.IDENTIFIER);
            var expressionL = ExpressionList();
            //Console.WriteLine(CurrentToken);
            if(CurrentToken == TokenCategory.INITBRACKET || CurrentToken == TokenCategory.CONSTANTDECLARATION ){
                if(CurrentToken == TokenCategory.INITBRACKET){
                    Expect(TokenCategory.INITBRACKET);
                    expressionL.Add(Expression());
                    Expect(TokenCategory.CLOSINGBRACKET);
                }
                Expect(TokenCategory.CONSTANTDECLARATION);
                expressionL.Add(Expression());
                Expect(TokenCategory.ENDLINE);
            }
            else if(CurrentToken == TokenCategory.INITPARENTHESIS){
                Expect(TokenCategory.INITPARENTHESIS);
                if(firstOfSimpleExpression.Contains(CurrentToken)){
                    expressionL.Add(Expression());
                    while(CurrentToken == TokenCategory.COMMA){
                        Expect(TokenCategory.COMMA);
                        expressionL.Add(Expression());
                    }                
                }
                Expect(TokenCategory.CLOSINGPARENTHESIS);
                Expect(TokenCategory.ENDLINE);
            }

            var result = new AssignmentCallStatement( expressionL);
            result.AnchorToken = identif;
            return result;
        }


        public Node If() {
            var ifToken = Expect(TokenCategory.IF);
            var exprList = new ExpressionList();
            exprList.Add(Expression());
            Expect(TokenCategory.THEN);
            var stmtList = new StatementList();
            while (firstOfStatement.Contains(CurrentToken)) {
                stmtList.Add(Statement());
            }
            if(CurrentToken == TokenCategory.ELSEIF) {
                while (CurrentToken == TokenCategory.ELSEIF) {
                    Expect(TokenCategory.ELSEIF);
                    exprList.Add(Expression());
                    Expect(TokenCategory.THEN);
                    while (firstOfStatement.Contains(CurrentToken)) {
                        stmtList.Add(Statement());
                    }
                }
            }
            if(CurrentToken == TokenCategory.ELSE) {
                Expect(TokenCategory.ELSE);
                while (firstOfStatement.Contains(CurrentToken)) {
                    stmtList.Add(Statement());

                }
            }
            Expect(TokenCategory.END);
            Expect(TokenCategory.ENDLINE);
            var result = new If() { exprList, stmtList };
            result.AnchorToken = ifToken;
            return result;
        }

        public Node Loop() {
            var idToken = Expect(TokenCategory.LOOP);
            var stmtList = new StatementList();
            while(firstOfStatement.Contains(CurrentToken)) {
                stmtList.Add(Statement());
            }
            Expect(TokenCategory.END);
            Expect(TokenCategory.ENDLINE);
            var result = new LoaderOptimization() { stmtList };
            result.AnchorToken = idToken;
            return result;
        }
        public Node For()
        {
            var idToken = Expect(TokenCategory.FOR);
            var identifier = Expect(TokenCategory.IDENTIFIER);
            Expect(TokenCategory.IN);
            var expr = Expression();
            Expect(TokenCategory.DO);
            var stmtList =  new StatementList();
            while (firstOfStatement.Contains(CurrentToken))
            {
                stmtList.Add(Statement());
            }
            Expect(TokenCategory.END);
            Expect(TokenCategory.ENDLINE);
            var result = new FormatException() { identifier, expr, stmtList };
            result.AnchorToken = idToken;
            return result;
        }

        public Node Return()
        {
            var result = new Return() {};
            result.AnchorToken = Expect(TokenCategory.RETURN);
            if (firstOfSimpleExpression.Contains(CurrentToken))
            {
                result.Add(Expression());
            }
            Expect(TokenCategory.ENDLINE);
            return result;
        }

        public Node Exit()
        {
            var result = new Exit() {};
            result.AnchorToken = Expect(TokenCategory.EXIT);
            Expect(TokenCategory.ENDLINE);
            return result;
        }

        public Node Expression(){
            return LogicExpression();
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
            while(CurrentToken == TokenCategory.EQUAL | CurrentToken == TokenCategory.INEQUAL | CurrentToken == TokenCategory.LESSOREQUAL | CurrentToken == TokenCategory.BIGGEROREQUAL | CurrentToken == TokenCategory.LESSTHAN | CurrentToken == TokenCategory.BIGGERTHAN) {
                RelationalOperator();
                SumExpression();
            }
        }

        public Node LogicOperator(){
            switch(CurrentToken) {
                case TokenCategory.AND:
                    return new AndOperator() {
                        AnchorToken = Expect(TokenCategory.AND)
                    };
                case TokenCategory.OR:
                    return new OrOperator() {
                        AnchorToken = Expect(TokenCategory.OR)
                    };
                case TokenCategory.XOR:
                    return new XorOperator() {
                        AnchorToken = Expect(TokenCategory.XOR)
                    };
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
                    return new EqualOperator() {
                        AnchorToken = Expect(TokenCategory.EQUAL)
                    };
                case TokenCategory.INEQUAL:
                    return new InequalOperator() {
                        AnchorToken = Expect(TokenCategory.INEQUAL)
                    };
                case TokenCategory.LESSTHAN:
                    return new LessThanOperator() {
                        AnchorToken = Expect(TokenCategory.LESSTHAN)
                    };
                case TokenCategory.BIGGERTHAN:
                    return new BiggerThanOperator() {
                        AnchorToken = Expect(TokenCategory.BIGGERTHAN)
                    };
                case TokenCategory.LESSOREQUAL:
                    return new LessOrEqualOperator() {
                        AnchorToken = Expect(TokenCategory.LESSOREQUAL)
                    };
                case TokenCategory.BIGGEROREQUAL:
                    return new BiggerOrEqualOperator() {
                        AnchorToken = Expect(TokenCategory.BIGGEROREQUAL)
                    };
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
                Expect(TokenCategory.INITPARENTHESIS);
                Expression();
                Expect(TokenCategory.CLOSINGPARENTHESIS);
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
            if (CurrentToken == TokenCategory.INITBRACKET)
            {
                Expect(TokenCategory.INITBRACKET);
                Expression();
                Expect(TokenCategory.CLOSINGBRACKET);
            }
        }

        public void Call(){
            Expect(TokenCategory.INITPARENTHESIS);
            //if(CurrentToken == TokenCategory.NOT | CurrentToken == TokenCategory.SUBSTRACT | CurrentToken == TokenCategory.INTEGERLITERAL | CurrentToken == TokenCategory.STRINGLITERAL | CurrentToken == TokenCategory.BOOLEANITERAL | CurrentToken == TokenCategory.IDENTIFIER){
            if(firstOfSimpleExpression.Contains(CurrentToken)){ 
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