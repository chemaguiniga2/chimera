using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Chimera {

    class Scanner {

        readonly string input;

        static readonly Regex regex = new Regex(
            @"                             
                (?<IntegerLiteral>           \d                 )
              | (?<StringLiteral>           [""].*[""]          )
              | (?<SingleComment>           [/][/].*            )
              | (?<BlockComment>            [/][*](.|\n)*[*][/] )
              | (?<Identifier>                                  )
              | (?<Declaration>             [:]                 )
              | (?<ConstantDeclaration>     [:][=]              )
              | (?<InitBracket>             [[]                 )
              | (?<ClosingBracket>          []]                 )
              | (?<InitParenthesis>         [(]                 )
              | (?<ClosingParenthesis>      [)]                 )
              | (?<EndLine>                 [;]                 )
              | (?<Comma>                   [,]                 )
              | (?<Equal>                   [=]                 )
              | (?<Inequal>                 [<][>]              )
              | (?<LessOrEqual>             [<][=]              )
              | (?<BiggerOrEqual>           [>][=]              )
              | (?<LessThan>                [<]                 )
              | (?<BiggerThan>              [>]                 )
              | (?<Addition>                [+]                 )
              | (?<Substract>               [-]                 )
              | (?<Multiplication>          [*]                 )
            ", 
            RegexOptions.IgnorePatternWhitespace 
                | RegexOptions.Compiled
                | RegexOptions.Multiline
            );
        
        static readonly IDictionary<string, TokenCategory> keywords =
            new Dictionary<string, TokenCategory>() {
                {"const", TokenCategory.CONST},
                {"var", TokenCategory.VAR},
                {"program", TokenCategory.PROGRAM},
                {"end", TokenCategory.END},
                {"integer", TokenCategory.INTEGER},
                {"string", TokenCategory.STRING},
                {"boolean", TokenCategory.BOOLEAN},
                {"list", TokenCategory.LIST},
                {"of", TokenCategory.OF},
                {"procedure", TokenCategory.PROCEDURE},
                {"begin", TokenCategory.BEGIN},
                {"if", TokenCategory.IF},
                {"then", TokenCategory.THEN},
                {"elseif", TokenCategory.ELSEIF},
                {"else", TokenCategory.ELSE},
                {"for", TokenCategory.FOR},
                {"in", TokenCategory.IN},
                {"do", TokenCategory.DO},
                {"return", TokenCategory.RETURN},
                {"exit", TokenCategory.EXIT},
                {"and", TokenCategory.AND},
                {"or", TokenCategory.OR},
                {"xor", TokenCategory.XOR},
                {"div", TokenCategory.DIV},
                {"rem", TokenCategory.REM},
                {"not", TokenCategory.NOT},
                {"loop", TokenCategory.LOOP},
                {"WrInt", TokenCategory.WRINT},
                {"WrStr", TokenCategory.WRSTR},
                {"WrBool", TokenCategory.WRBOOL},
                {"WrLn", TokenCategory.WRLN},
                {"RdInt", TokenCategory.RDINT},
                {"RdStr", TokenCategory.RDSTR},
                {"AtStr", TokenCategory.ATSTR},
                {"LenStr", TokenCategory.LENSTR},
                {"CmpStr", TokenCategory.CMPSTR},
                {"CatStr", TokenCategory.CATSTR},
                {"LenLstInt", TokenCategory.LENLSTINT},
                {"LenLstStr", TokenCategory.LENLSTSTR},
                {"LenLstBool", TokenCategory.LENLSTBOOL},
                {"NewLstInt", TokenCategory.NEWLSTINT},
                {"NewLstStr", TokenCategory.NEWLSTSTR},
                {"NewLstBool", TokenCategory.NEWLSTBOOL},
                {"IntToStr", TokenCategory.INTTOSTR},
                {"StrToInt", TokenCategory.STRTOINT},

            };


    }

}