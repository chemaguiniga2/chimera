using System;
using System.Collections.Generic;

namespace Chimera {

    class Parser {
        
        static readonly ISet<TokenCategory> firstOfDeclaration =
            new HashSet<TokenCategory>() {
                TokenCategory.CONST,
                TokenCategory.VAR,
            };

        static readonly ISet<TokenCategory> firstOfStatement =
            new HashSet<TokenCategory>() {
                TokenCategory.IDENTIFIER,
                TokenCategory.PROCEDURE,
                //TokenCategory.PRINT,
                TokenCategory.IF,
                TokenCategory.FOR,
                //TokenCategory.WHILE,
                TokenCategory.LOOP,
                TokenCategory.BEGIN,
                TokenCategory.RETURN, 
                TokenCategory.DO
            };

        static readonly ISet<TokenCategory> firstOfOperator =
            new HashSet<TokenCategory>() {
                TokenCategory.AND,
                TokenCategory.OR,
                TokenCategory.XOR,
                TokenCategory.DIV,
                TokenCategory.REM,
                TokenCategory.NOT,
                TokenCategory.EQUAL,
                TokenCategory.INEQUAL,
                TokenCategory.LESSTHAN,
                TokenCategory.BIGGERTHAN,
                TokenCategory.LESSOREQUAL,
                TokenCategory.BIGGEROREQUAL,
                TokenCategory.ADDITION,
                TokenCategory.SUBSTRACT,
                TokenCategory.MULTIPLICATION,
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

        public void Program() {
            while(firstOfDeclaration.Contains(CurrentToken)) {
                Declaration();
            }
            while(firstOfStatement.Contains(CurrentToken)) {
                Statement();
            }

            Expect(TokenCategory.EOF);
        }

        public void Declaration() {
            Type();
            Expect(TokenCategory.IDENTIFIER);
        }

        public void Statement() {

            //considerar aqui el else if

            switch (CurrentToken) {

            case TokenCategory.IDENTIFIER:
                Assignment();
                break;

            case TokenCategory.PROCEDURE:
                // Procedure function
                Procedure();
                break;

            case TokenCategory.IF:
                If();
                break;
            
            case TokenCategory.FOR:
                // for function
                // If();
                // break;

            case TokenCategory.LOOP:
                // loop function
                // If();
                // break;

            case TokenCategory.BEGIN:
                // BEGIN function
                // If();
                // break;

            case TokenCategory.RETURN:
                // return function
                // If();
                // break;
            
            case TokenCategory.DO:
                // do function
                // If();
                // break;

            default:
                throw new SyntaxError(firstOfStatement, 
                                      tokenStream.Current);
            }
        }

        public void Type() {
            switch (CurrentToken) {

            case TokenCategory.CONST:
                Expect(TokenCategory.CONST);
                break;

            case TokenCategory.VAR:
                Expect(TokenCategory.VAR);
                break;

            default:
                throw new SyntaxError(firstOfDeclaration, 
                                      tokenStream.Current);
            }
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