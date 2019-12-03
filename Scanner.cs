/*
Dachely Otero Argote            A01377812
José María Aguíñiga Díaz        A01376669 
José Rodrigo Narváez Berlanga   A01377566
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Chimera {

	class Scanner {

		readonly string input;

		static readonly Regex regex = new Regex(
			@"                             
                
                (?<LineComment>           	[/][/].*            		)
              | (?<BlockComment>            [/][*](.|\n)*?[*][/]		)
              | (?<Identifier>              [a-zA-Z][_a-zA-Z\d]*		)
			  | (?<IntegerLiteral>           \d+                 		)
              | (?<StringLiteral>           [""]([^""]|[""][""])*[""] 	)
			  | (?<BooleanLiteral>          (true|false) 				)
              | (?<EndLine>                 [;]                 )
              | (?<ConstantDeclaration>     [:][=]              )
              | (?<Comma>                   [,]                 )
              | (?<Declaration>             [:]                 )
              | (?<InitList>         		[{]                 )
              | (?<ClosingList>      		[}]                 )
              | (?<InitParenthesis>         [(]                 )
              | (?<ClosingParenthesis>      [)]                 )
              | (?<InitBracket>             [[]                 )
              | (?<ClosingBracket>          []]                 )
              | (?<Equal>                   [=]                 )
              | (?<Inequal>                 [<][>]              )
              | (?<LessOrEqual>             [<][=]              )
              | (?<BiggerOrEqual>           [>][=]              )
			  | (?<LessThan>                [<]                 )
              | (?<BiggerThan>              [>]                 )
              | (?<Addition>                [+]                 )
              | (?<Substract>               [-]                 )
              | (?<Multiplication>          [*]                 )
			  | (?<Newline>    				\n        			)
			  | (?<WhiteSpace> 				\s        			)     
              | (?<Other>      				.         			) 
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
			{"loop", TokenCategory.LOOP},
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
			{"IntegerLiteral", TokenCategory.INTEGERLITERAL},
			{"StringLiteral", TokenCategory.STRINGLITERAL},
			{"BooleanLiteral", TokenCategory.BOOLEANITERAL},

		};

		static readonly IDictionary<string, TokenCategory> nonKeywords =
			new Dictionary<string, TokenCategory>() {
			{"EndLine", TokenCategory.ENDLINE},
			{"ConstantDeclaration", TokenCategory.CONSTANTDECLARATION},
			{"Comma", TokenCategory.COMMA},
			{"Declaration", TokenCategory.DECLARATION},
			{"InitList", TokenCategory.INITLIST},
			{"ClosingList", TokenCategory.CLOSINGLIST},
			{"InitParenthesis", TokenCategory.INITPARENTHESIS},
			{"ClosingParenthesis", TokenCategory.CLOSINGPARENTHESIS},
			{"InitBracket", TokenCategory.INITBRACKET},
			{"ClosingBracket", TokenCategory.CLOSINGBRACKET},
			{"Equal", TokenCategory.EQUAL},
			{"Inequal", TokenCategory.INEQUAL},
			{"LessThan", TokenCategory.LESSTHAN},
			{"BiggerThan", TokenCategory.BIGGERTHAN},
			{"LessOrEqual", TokenCategory.LESSOREQUAL},
			{"BiggerOrEqual", TokenCategory.BIGGEROREQUAL},
			{"Addition", TokenCategory.ADDITION},
			{"Substract", TokenCategory.SUBSTRACT},
			{"Multiplication", TokenCategory.MULTIPLICATION}
		};

		public Scanner(string input) {
			this.input = input;
		}

		public IEnumerable<Token> Start() {

			var row = 1;
			var columnStart = 0;

			Func<Match, TokenCategory, Token> newTok = (m, tc) =>
				new Token(m.Value, tc, row, m.Index - columnStart + 1);

			foreach (Match m in regex.Matches(input)) {

				if (m.Groups["Newline"].Success) {

					// Found a new line.
					row++;
					columnStart = m.Index + m.Length;

				} else if (m.Groups["WhiteSpace"].Success 
					|| m.Groups["LineComment"].Success) {

					// Skip white space and comments.

				} else if (m.Groups["BlockComment"].Success) {

					var cantAdd = m.Value.Split('\n').Length - 1;
					row += cantAdd;

				} 
				else if (m.Groups["Identifier"].Success) {

					if (keywords.ContainsKey(m.Value)) {

						// Matched string is a Buttercup keyword.
						yield return newTok(m, keywords[m.Value]);                                               

					} else { 

						// Otherwise it's just a plain identifier.
						yield return newTok(m, TokenCategory.IDENTIFIER);
					}

				} else if (m.Groups["Other"].Success) {

					// Found an illegal character.
					yield return newTok(m, TokenCategory.ILLEGAL_CHAR);

				} else {

					// Match must be one of the non keywords.
					foreach (var name in nonKeywords.Keys) {
						if (m.Groups[name].Success) {
							yield return newTok(m, nonKeywords[name]);
							break;
						}
					}
				}
			}

			yield return new Token(null, 
				TokenCategory.EOF, 
				row, 
				input.Length - columnStart + 1);
		}
	}

}
