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
                TokenCategory.LESS,
                TokenCategory.PLUS,
                TokenCategory.MUL
            };

        static readonly ISet<TokenCategory> firstOfSimpleExpression =
            new HashSet<TokenCategory>() {
                TokenCategory.IDENTIFIER,
                TokenCategory.INT_LITERAL,
                TokenCategory.TRUE,
                TokenCategory.FALSE,
                TokenCategory.PARENTHESIS_OPEN,
                TokenCategory.NEG
            };
                
        IEnumerator<Token> tokenStream;

        public Parser(IEnumerator<Token> tokenStream) {
            this.tokenStream = tokenStream;
            this.tokenStream.MoveNext();
        }

        public TokenCategory CurrentToken {
            get { return tokenStream.Current.Category; }
        }

		// queda parecido, solo hay que hacer un cambio
        public Token Expect(TokenCategory category) {
            if (CurrentToken == category) {
                Token current = tokenStream.Current;
                tokenStream.MoveNext();
                return current;
            } else {
				// Hay que cambiar (category, tokenStream.Current) y enviarle nuestra info
                throw new SyntaxError(category, tokenStream.Current);                
            }
        }

        public void Program() {            

            while (firstOfDeclaration.Contains(CurrentToken)) {
                Declaration();
            }

            while (firstOfStatement.Contains(CurrentToken)) {
                Statement();
            }

            Expect(TokenCategory.EOF);
        }

        public void Declaration() {
            Type();
            Expect(TokenCategory.IDENTIFIER);
        }

        public void Statement() {

            switch (CurrentToken) {

            case TokenCategory.IDENTIFIER:
                Assignment();
                break;

            case TokenCategory.PRINT:
                Print();
                break;

            case TokenCategory.IF:
                If();
                break;

            default:
                throw new SyntaxError(firstOfStatement, 
                                      tokenStream.Current);
            }
        }

        public void Type() {
            switch (CurrentToken) {

            case TokenCategory.INT:
                Expect(TokenCategory.INT);
                break;

            case TokenCategory.BOOL:
                Expect(TokenCategory.BOOL);
                break;

            default:
                throw new SyntaxError(firstOfDeclaration, 
                                      tokenStream.Current);
            }
        }

        public void Assignment() {
            Expect(TokenCategory.IDENTIFIER);
            Expect(TokenCategory.ASSIGN);
            Expression();
        }

        public void Print() {
            Expect(TokenCategory.PRINT);
            Expression();
        }

        public void If() {
            Expect(TokenCategory.IF);
            Expression();
            Expect(TokenCategory.THEN);
            while (firstOfStatement.Contains(CurrentToken)) {
                Statement();
            }
            Expect(TokenCategory.END);
        }

        public void Expression() {
            SimpleExpression();
            while (firstOfOperator.Contains(CurrentToken)) {
                Operator();
                SimpleExpression();
            }
        }

        public void SimpleExpression() {

            switch (CurrentToken) {

            case TokenCategory.IDENTIFIER:
                Expect(TokenCategory.IDENTIFIER);
                break;

            case TokenCategory.INT_LITERAL:
                Expect(TokenCategory.INT_LITERAL);
                break;

            case TokenCategory.TRUE:
                Expect(TokenCategory.TRUE);
                break;

            case TokenCategory.FALSE:
                Expect(TokenCategory.FALSE);
                break;

            case TokenCategory.PARENTHESIS_OPEN:
                Expect(TokenCategory.PARENTHESIS_OPEN);
                Expression();
                Expect(TokenCategory.PARENTHESIS_CLOSE);
                break;

            case TokenCategory.NEG:
                Expect(TokenCategory.NEG);
                SimpleExpression();
                break;

            default:
                throw new SyntaxError(firstOfSimpleExpression, 
                                      tokenStream.Current);
            }
        }

        public void Operator() {

            switch (CurrentToken) {

            case TokenCategory.AND:
                Expect(TokenCategory.AND);
                break;

            case TokenCategory.LESS:
                Expect(TokenCategory.LESS);
                break;

            case  TokenCategory.PLUS:
                Expect(TokenCategory.PLUS);
                break;

            case TokenCategory.MUL:
                Expect(TokenCategory.MUL);
                break;

            default:
                throw new SyntaxError(firstOfOperator, 
                                      tokenStream.Current);
            }
        }
    }
}
