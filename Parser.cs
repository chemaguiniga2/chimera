/*
Dachely Otero Argote            A01377812
José María Aguíñiga Díaz        A01376669 
José Rodrigo Narváez Berlanga   A01377566
 */

using System;
using System.Collections.Generic;

namespace Chimera
{

    class Parser
    {

        static readonly ISet<TokenCategory> firstOfDeclaration =
            new HashSet<TokenCategory>() {
                TokenCategory.CONST,
                TokenCategory.VAR,
                TokenCategory.PROCEDURE, //--> creo que hay que agregar esto
                TokenCategory.PROGRAM,
            };

        static readonly ISet<TokenCategory> simpleTypes =
            new HashSet<TokenCategory>() {
                TokenCategory.INTEGER,
                TokenCategory.STRING,
                TokenCategory.BOOLEAN
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

        public Parser(IEnumerator<Token> tokenStream)
        {
            this.tokenStream = tokenStream;
            this.tokenStream.MoveNext();
        }

        public TokenCategory CurrentToken
        {
            get { return tokenStream.Current.Category; }
        }

        public Token Expect(TokenCategory category)
        {
            if (CurrentToken == category)
            {
                Token current = tokenStream.Current;
                tokenStream.MoveNext();
                return current;
            }
            else
            {
                throw new SyntaxError(category, tokenStream.Current);
            }
        }

        //dache
        public Node Program()
        {
            var program = new Program();
            var statList = new StatementList();
            var procDecList = new ProcedureDeclarationList();
            var consDecList = new ConstantDeclarationList();
            //var consDecList2 = VariableDeclaration();
            //var varDecList = new VariableDeclarationList();

            if (CurrentToken == TokenCategory.CONST)
            {
                consDecList.AnchorToken = Expect(TokenCategory.CONST);
                var i = 0;
                do
                { 
                    consDecList.Add(ConstantDeclaration());
                    Console.WriteLine(consDecList[i]);
                    i++;
                } while (CurrentToken == TokenCategory.IDENTIFIER);
                program.Add(consDecList);
            }

            if (CurrentToken == TokenCategory.VAR)
            {
                var varDecList = VariableDeclaration();
                program.Add(varDecList);
            }

            while (CurrentToken == TokenCategory.PROCEDURE)
            {
                procDecList.Add(ProcedureDeclaration());
                
            }
            if (procDecList.getLength()>0)
            {
                program.Add(procDecList);
            }


            Expect(TokenCategory.PROGRAM);
            while (firstOfStatement.Contains(CurrentToken))
            {
                statList.Add(Statement());
            }

            if (statList.getLength() > 0)
            {
                program.Add(statList);
            }
            Expect(TokenCategory.END);
            Expect(TokenCategory.ENDLINE);
            Expect(TokenCategory.EOF);

            return program;
        }

        public Node ConstantDeclaration()
        {
            var idToken = Expect(TokenCategory.IDENTIFIER);
            Expect(TokenCategory.CONSTANTDECLARATION);
            var lit = Literal();
            Expect(TokenCategory.ENDLINE);

            var constDeclar = new ConstantDeclaration() { lit };
            constDeclar.AnchorToken = idToken;

            return constDeclar;
        }

        public Node VariableDeclaration()
        {
            var i = 0;
            var declarationList = new VariableDeclarationList();
            declarationList.AnchorToken = Expect(TokenCategory.VAR);
            do
            {
                var tempList = new VariableDeclarationList();
                var firstIdentifier = new Identifier()
                {
                    AnchorToken = Expect(TokenCategory.IDENTIFIER)
                };
                tempList.Add(firstIdentifier);

                //declarationList.Add(firstIdentifier);
                while (CurrentToken == TokenCategory.COMMA)
                {
                    Expect(TokenCategory.COMMA);
                    tempList.Add(new Identifier()
                    {
                        AnchorToken = Expect(TokenCategory.IDENTIFIER)

                    });
                }
                Expect(TokenCategory.DECLARATION);
                var tipo = Type();
            //declarationList.Add(Type());
                foreach (var node in tempList)
                {
                    declarationList.Add(tipo);
                    
                }

                foreach (var nodo in tempList)
                {
                    declarationList[i].Add(nodo);
                    i++;
                }
                Expect(TokenCategory.ENDLINE);

            } while (CurrentToken == TokenCategory.IDENTIFIER);
            return declarationList;
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

                    //return List();
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
                case TokenCategory.STRING:
                    return new Type()
                    {
                        AnchorToken = SimpleType()
                    };
                case TokenCategory.BOOLEAN:
                    return new Type()
                    {
                        AnchorToken = SimpleType()
                    };
                case TokenCategory.INTEGER:
                    return new Type()
                    {
                        AnchorToken = SimpleType()
                    };
                default:
                    Console.WriteLine("POS NOMAS NO");
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

                case TokenCategory.INTEGER:
                    return new ListTypeNode()
                    {
                        AnchorToken = Expect(TokenCategory.INTEGER)
                    };
             
                case TokenCategory.STRING:
                    return new ListTypeNode()
                    {
                        AnchorToken = Expect(TokenCategory.STRING)
                    };
                case TokenCategory.BOOLEAN:
                    return new ListTypeNode()
                    {
                        AnchorToken = Expect(TokenCategory.BOOLEAN)
                    };
                default:
                    throw new SyntaxError(firstOfSimpleExpression,
                                        tokenStream.Current);
            }/*
            return new ListType()
            {
                AnchorToken = ListType()
            };*/
        }

        public Node ProcedureDeclaration()
        {

            var procedure = new ProcedureDeclaration();
            Expect(TokenCategory.PROCEDURE);
            procedure.AnchorToken = Expect(TokenCategory.IDENTIFIER);

            var consDecList = new ConstantDeclarationList();
            var statement = new StatementList();

            Expect(TokenCategory.INITPARENTHESIS);

            if (CurrentToken == TokenCategory.IDENTIFIER)
            {
                var parDecList = ParameterDeclaration();
                procedure.Add(parDecList);
            }

            Expect(TokenCategory.CLOSINGPARENTHESIS);
            if (CurrentToken == TokenCategory.DECLARATION)
            {
                Expect(TokenCategory.DECLARATION);
                var type = Type();
                procedure.Add(type);
            }

            Expect(TokenCategory.ENDLINE); 
            if (CurrentToken == TokenCategory.CONST)
            {
                consDecList.AnchorToken = Expect(TokenCategory.CONST);
                var i = 0;
                do
                {
                    consDecList.Add(ConstantDeclaration());
                    Console.WriteLine(consDecList[i]);
                    i++;
                } while (CurrentToken == TokenCategory.IDENTIFIER);
                procedure.Add(consDecList);
            }

            if (CurrentToken == TokenCategory.VAR)
            {
                var varDecList = VariableDeclaration();
                procedure.Add(varDecList);
            }

            var tempStatement = Expect(TokenCategory.BEGIN);
            while (firstOfStatement.Contains(CurrentToken))
            {
                statement.Add(Statement());
            }
            if (statement.getLength() > 0)
            {
                statement.AnchorToken = tempStatement;
                procedure.Add(statement);
            }

            Expect(TokenCategory.END);
            Expect(TokenCategory.ENDLINE);

            return procedure;
        }


        public Node ParameterDeclaration()
        {
            var paramList = new ParameterDeclarationList();
            while (CurrentToken == TokenCategory.IDENTIFIER)
            {
                Node idToken = new Identifier()
                {
                    AnchorToken = Expect(TokenCategory.IDENTIFIER)

                };

                paramList.Add(idToken);

                while (CurrentToken == TokenCategory.COMMA)
                {
                    Expect(TokenCategory.COMMA);
                    paramList.Add(new Identifier()
                    {
                        AnchorToken = Expect(TokenCategory.IDENTIFIER)

                    });
                }
                Expect(TokenCategory.DECLARATION);
                var tipo = Type();
                //declarationList.Add(Type());
                foreach (var node in paramList)
                {
                    if (node.getLength()==0) //Only if type hasn't been assigned
                    {
                        node.Add(tipo);
                    }
                }

                Expect(TokenCategory.ENDLINE);
            }

            return paramList;
        }

        public Node List()
        {
            var initList = Expect(TokenCategory.INITLIST);
            var simpleLitList = new SimpleLiteralList();
            var litToken = new SimpleLiteral();
            if (CurrentToken == TokenCategory.INTEGERLITERAL | CurrentToken == TokenCategory.STRINGLITERAL | CurrentToken == TokenCategory.BOOLEANITERAL)
            {
                litToken.Add(SimpleLiteral());
                while (CurrentToken == TokenCategory.COMMA)
                {
                    Expect(TokenCategory.COMMA);
                    simpleLitList.Add(SimpleLiteral());
                }
            }
            Expect(TokenCategory.CLOSINGLIST);
            var result = new List() { litToken, simpleLitList };
            result.AnchorToken = initList;
            return result;
        }



        public Token SimpleType()
        {
            switch (CurrentToken)
            {
                case TokenCategory.INTEGER:
                    return Expect(TokenCategory.INTEGER);
                case TokenCategory.STRING:
                    return Expect(TokenCategory.STRING);
                case TokenCategory.BOOLEAN:
                    return Expect(TokenCategory.BOOLEAN);
                default:
                    throw new SyntaxError(firstOfSimpleExpression,
                                        tokenStream.Current);
            }
        }

        public Node Statement()
        {
            switch (CurrentToken)
            {
                case TokenCategory.IDENTIFIER:
                    Console.WriteLine("AQUI CACA");
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


        public Node AssignmentCallStatement()
        {
            var identif = Expect(TokenCategory.IDENTIFIER);
            var assignment = new AssignmentStatement();
            var call = new CallStatement();

            Console.WriteLine("IDENTIFFFF" + identif);
            Console.WriteLine("CT: "+CurrentToken);
            //var alter = new Expression();

            //call-statement
            if (CurrentToken == TokenCategory.INITPARENTHESIS)
            {
                Expect(TokenCategory.INITPARENTHESIS);
                if (firstOfSimpleExpression.Contains(CurrentToken))
                {
                    call.Add(Expression());
                    while (CurrentToken == TokenCategory.COMMA)
                    {
                        Expect(TokenCategory.COMMA);
                        call.Add(Expression());
                    }
                }
                //call.Add(
                Expect(TokenCategory.CLOSINGPARENTHESIS);
                Expect(TokenCategory.ENDLINE);

                var result = new List() { call };
                call.AnchorToken = identif;
                return call;
                //call.AnchorToken = identif;
                //return call;
            }else if (CurrentToken == TokenCategory.INITBRACKET || CurrentToken == TokenCategory.CONSTANTDECLARATION)
            {
                if (CurrentToken == TokenCategory.INITBRACKET)
                {
                    Expect(TokenCategory.INITBRACKET);
                    var expression = Expression();
                    Expect(TokenCategory.CLOSINGBRACKET);
                    assignment.Add(expression);
                }
                Expect(TokenCategory.CONSTANTDECLARATION);
                var expression2 = Expression();
                Expect(TokenCategory.ENDLINE);
                assignment.Add(expression2);
                assignment.AnchorToken = identif;
                return assignment;
            }
            /************** ATENCION ************/
            /************** ATENCION ************/
            /************** ATENCION ************/

            else
            {
                throw new SyntaxError(firstOfStatement,
                    tokenStream.Current);
            }

            return call;

        }


        public Node If()
        {
            var ifToken = Expect(TokenCategory.IF);
            var nodeIf = new If()
            {
                AnchorToken = ifToken
            };

            nodeIf.Add(Expression());
            Expect(TokenCategory.THEN);
            var stmtList = new StatementList();
            while (firstOfStatement.Contains(CurrentToken))
            {
                stmtList.Add(Statement());
            }

            nodeIf.Add(stmtList);

            while (CurrentToken == TokenCategory.ELSEIF)
            {
                var nodeElseIf = new ElseIf()
                {
                    AnchorToken = Expect(TokenCategory.ELSEIF)
                };

                nodeElseIf.Add(Expression());
                Expect(TokenCategory.THEN);

                var stmtListElseIf = new StatementList();
                while (firstOfStatement.Contains(CurrentToken))
                {
                    stmtListElseIf.Add(Statement());
                }

                nodeElseIf.Add(stmtListElseIf);
                nodeIf.Add(nodeElseIf);

            }

            while (CurrentToken == TokenCategory.ELSE)
            {
                var nodeElse = new Else()
                {
                    AnchorToken = Expect(TokenCategory.ELSE)
                };

                var stmtListElse = new StatementList();
                while (firstOfStatement.Contains(CurrentToken))
                {
                    stmtListElse.Add(Statement());
                }

                nodeElse.Add(stmtListElse);
                nodeIf.Add(nodeElse);
            }

            Expect(TokenCategory.END);
            Expect(TokenCategory.ENDLINE);

            return nodeIf;


            // var ifToken = Expect(TokenCategory.IF);
            // var exprList = new ExpressionList();
            // exprList.Add(Expression());
            // var thenNode = new Then() {
            //     AnchorToken = Expect(TokenCategory.THEN)
            // };
            // var stmtList = new StatementList();
            // while (firstOfStatement.Contains(CurrentToken)) {
            //     stmtList.Add(Statement());
            // }
            // if(CurrentToken == TokenCategory.ELSEIF) {
            //     while (CurrentToken == TokenCategory.ELSEIF) {
            //         var elseIf = new ElseIf() {
            //             AnchorToken = Expect(TokenCategory.ELSEIF)
            //         };
            //         exprList.Add(Expression());
            //         Console.WriteLine("Revisa then");
            //         var thenNodetwo = new Then() {
            //             AnchorToken = Expect(TokenCategory.THEN)
            //         };
            //         while (firstOfStatement.Contains(CurrentToken)) {
            //             stmtList.Add(Statement());
            //         }
            //     }
            // }
            // if(CurrentToken == TokenCategory.ELSE) {
            //     Expect(TokenCategory.ELSE);
            //     while (firstOfStatement.Contains(CurrentToken)) {
            //         stmtList.Add(Statement());

            //     }
            // }
            // Expect(TokenCategory.END);
            // Expect(TokenCategory.ENDLINE);
            // var result = new If() { exprList, stmtList };
            // result.AnchorToken = ifToken;
            // return result;
        }

        public Node Loop()
        {
            var idToken = Expect(TokenCategory.LOOP);
            var stmtList = new StatementList();
            while (firstOfStatement.Contains(CurrentToken))
            {
                stmtList.Add(Statement());
            }
            Expect(TokenCategory.END);
            Expect(TokenCategory.ENDLINE);
            var result = new Loop() { stmtList };
            result.AnchorToken = idToken;
            return result;
        }
        public Node For()
        {
            var idToken = Expect(TokenCategory.FOR);
            var identifier = new Identifier()
            {
                AnchorToken = Expect(TokenCategory.IDENTIFIER)
            };
            Expect(TokenCategory.IN);
            var expr = Expression();
            Expect(TokenCategory.DO);
            var stmtList = new StatementList();
            while (firstOfStatement.Contains(CurrentToken))
            {
                stmtList.Add(Statement());
            }
            Expect(TokenCategory.END);
            Expect(TokenCategory.ENDLINE);
            var result = new For() { identifier, expr, stmtList };
            result.AnchorToken = idToken;
            return result;
            //Si nos estamos refiriendo a FormatException?
        }

        public Node Return()
        {
            var result = new Return() { };
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
            var result = new Exit() { };
            result.AnchorToken = Expect(TokenCategory.EXIT);
            Expect(TokenCategory.ENDLINE);
            return result;
        }

        public Node Expression()
        {
            return LogicExpression();
        }

        public Node LogicExpression()
        {
            var logExpr1 = RelationalExpression();
            while (firstOfOperator.Contains(CurrentToken))
            {
                var logExpr2 = LogicOperator();
                logExpr2.Add(logExpr1);
                logExpr2.Add(RelationalExpression());
                logExpr1 = logExpr2;
            }
            return logExpr1;
        }

        public Node RelationalExpression()
        {
            var relExpr = SumExpression();
            while (CurrentToken == TokenCategory.EQUAL | CurrentToken == TokenCategory.INEQUAL | CurrentToken == TokenCategory.LESSOREQUAL | CurrentToken == TokenCategory.BIGGEROREQUAL | CurrentToken == TokenCategory.LESSTHAN | CurrentToken == TokenCategory.BIGGERTHAN)
            {
                var relExpr2 = RelationalOperator();
                relExpr2.Add(relExpr);
                relExpr2.Add(SumExpression());
                relExpr = relExpr2;
            }
            return relExpr;
        }

        public Node LogicOperator()
        {
            switch (CurrentToken)
            {
                case TokenCategory.AND:
                    return new AndOperator()
                    {
                        AnchorToken = Expect(TokenCategory.AND)
                    };
                case TokenCategory.OR:
                    return new OrOperator()
                    {
                        AnchorToken = Expect(TokenCategory.OR)
                    };
                case TokenCategory.XOR:
                    return new XorOperator()
                    {
                        AnchorToken = Expect(TokenCategory.XOR)
                    };
                default:
                    throw new SyntaxError(firstOfStatement,
                                        tokenStream.Current);
            }
        }

        public Node SumExpression()
        {
            var sumExpr1 = MulExpression();
            while (CurrentToken == TokenCategory.ADDITION | CurrentToken == TokenCategory.SUBSTRACT)
            {
                var sumExpr2 = SumOperator();
                sumExpr2.Add(sumExpr1);
                sumExpr2.Add(MulExpression());
                sumExpr1 = sumExpr2;
            }
            return sumExpr1;

        }

        public Node RelationalOperator()
        {
            switch (CurrentToken)
            {
                case TokenCategory.EQUAL:
                    return new EqualOperator()
                    {
                        AnchorToken = Expect(TokenCategory.EQUAL)
                    };
                case TokenCategory.INEQUAL:
                    return new InequalOperator()
                    {
                        AnchorToken = Expect(TokenCategory.INEQUAL)
                    };
                case TokenCategory.LESSTHAN:
                    return new LessThanOperator()
                    {
                        AnchorToken = Expect(TokenCategory.LESSTHAN)
                    };
                case TokenCategory.BIGGERTHAN:
                    return new BiggerThanOperator()
                    {
                        AnchorToken = Expect(TokenCategory.BIGGERTHAN)
                    };
                case TokenCategory.LESSOREQUAL:
                    return new LessOrEqualOperator()
                    {
                        AnchorToken = Expect(TokenCategory.LESSOREQUAL)
                    };
                case TokenCategory.BIGGEROREQUAL:
                    return new BiggerOrEqualOperator()
                    {
                        AnchorToken = Expect(TokenCategory.BIGGEROREQUAL)
                    };
                default:
                    throw new SyntaxError(firstOfStatement,
                                        tokenStream.Current);
            }

        }

        public Node MulExpression()
        {
            var mulExpr1 = UnaryExpression();
            while (CurrentToken == TokenCategory.MULTIPLICATION | CurrentToken == TokenCategory.DIV | CurrentToken == TokenCategory.REM)
            {
                var mulExpr2 = MulOperator();
                mulExpr2.Add(mulExpr1);
                mulExpr2.Add(UnaryExpression());
                mulExpr1 = mulExpr2;
            }
            return mulExpr1;
        }


        public Node SumOperator()
        {
            switch (CurrentToken)
            {
                case TokenCategory.ADDITION:
                    return new AdditionOperator()
                    {
                        AnchorToken = Expect(TokenCategory.ADDITION)
                    };
                case TokenCategory.SUBSTRACT:
                    return new SubstractOperator()
                    {
                        AnchorToken = Expect(TokenCategory.SUBSTRACT)
                    };
                default:
                    throw new SyntaxError(firstOfStatement,
                                        tokenStream.Current);
            }
        }

        public Node UnaryExpression()
        {
            switch (CurrentToken)
            {
                case TokenCategory.NOT:
                    Expect(TokenCategory.NOT);
                    return UnaryExpression();
                case TokenCategory.SUBSTRACT:
                    Expect(TokenCategory.SUBSTRACT);
                    return UnaryExpression();
                default:
                    return SimpleExpression();
            }
        }

        public Node MulOperator()
        {
            switch (CurrentToken)
            {
                case TokenCategory.MULTIPLICATION:
                    return new MultiplicationOperator()
                    {
                        AnchorToken = Expect(TokenCategory.MULTIPLICATION)
                    };
                case TokenCategory.DIV:
                    return new DivOperator()
                    {
                        AnchorToken = Expect(TokenCategory.DIV)
                    };
                case TokenCategory.REM:
                    return new RemOperator()
                    {
                        AnchorToken = Expect(TokenCategory.REM)
                    };
                default:
                    throw new SyntaxError(firstOfStatement,
                                        tokenStream.Current);
            }
        }
        public Node SimpleExpression()
        {
            switch (CurrentToken)
            {
                case TokenCategory.INITPARENTHESIS:
                    Expect(TokenCategory.INITPARENTHESIS);
                    var result = Expression();
                    Expect(TokenCategory.CLOSINGPARENTHESIS);
                    if (CurrentToken == TokenCategory.INITBRACKET)
                    {
                        Expect(TokenCategory.INITBRACKET);
                        result.Add(Expression());
                        Expect(TokenCategory.CLOSINGBRACKET);
                    }
                    return result;
                case TokenCategory.INTEGERLITERAL:
                    return SimpleLiteral();
                case TokenCategory.STRINGLITERAL:
                    return SimpleLiteral();
                case TokenCategory.BOOLEANITERAL:
                    return SimpleLiteral();
                case TokenCategory.INITLIST:
                    return List();
                case TokenCategory.IDENTIFIER:
                    var resultIden = new Identifier() { };
                    // Console.WriteLine("Es aqui");
                    resultIden.AnchorToken = Expect(TokenCategory.IDENTIFIER);

                    if (CurrentToken == TokenCategory.INITPARENTHESIS)
                    {
                        resultIden.Add(Call());
                    }
                    if (CurrentToken == TokenCategory.INITBRACKET)
                    {
                        Expect(TokenCategory.INITBRACKET);
                        resultIden.Add(Expression());
                        Expect(TokenCategory.CLOSINGBRACKET);
                    }
                    return resultIden;
                // Node resultNode = null;
                // resultNode.AnchorToken = Expect(TokenCategory.IDENTIFIER);
                // if(CurrentToken == TokenCategory.INITPARENTHESIS){
                //     resultNode.Add(Call());
                // }
                // return resultNode;
                default:
                    throw new SyntaxError(firstOfSimpleExpression,
                        tokenStream.Current);
            }
            //Porque este if no entra dentro del switch
            /*if (CurrentToken == TokenCategory.INITBRACKET)
            {
                Expect(TokenCategory.INITBRACKET);
                Expression();
                Expect(TokenCategory.CLOSINGBRACKET);
            }*/
        }

        public Node Call()
        {
            Expect(TokenCategory.INITPARENTHESIS);
            //if(CurrentToken == TokenCategory.NOT | CurrentToken == TokenCategory.SUBSTRACT | CurrentToken == TokenCategory.INTEGERLITERAL | CurrentToken == TokenCategory.STRINGLITERAL | CurrentToken == TokenCategory.BOOLEANITERAL | CurrentToken == TokenCategory.IDENTIFIER){
            var result = new CallNode() { };
            if (firstOfSimpleExpression.Contains(CurrentToken))
            {
                result.Add(Expression());
                while (CurrentToken == TokenCategory.COMMA)
                {
                    Expect(TokenCategory.COMMA);
                    result.Add(Expression());
                }
            }
            Expect(TokenCategory.CLOSINGPARENTHESIS);
            return result;
        }


    }
}