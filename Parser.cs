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
            };
    }
}