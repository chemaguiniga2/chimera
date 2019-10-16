/*
Dachely Otero Argote            A01377812
José María Aguíñiga Díaz        A01376669 
José Rodrigo Narváez Berlanga   A01377566
 */
 
enum TokenCategory {
	CONST,  // FOD
	VAR,  // FOD
	PROGRAM,  // FOD
	END,
	INTEGER,
	STRING,
	BOOLEAN,
	LIST, 
	OF,
	PROCEDURE, //(FOS creo que no) FOD
	BEGIN,  //(FOS creo que no)
	IF, //FOS
	THEN,
	ELSEIF,
	ELSE,
	LOOP, //FOS
	FOR, //FOS
	IN,
	DO, //(FOS creo que no)
	RETURN, //FOS
	EXIT,  //FOS
	AND, //FOO
	OR, //FOO
	XOR, //FOO
	DIV, //FOO
	REM, //F00
	NOT, //FOO
	IDENTIFIER, //FOS, FOSE
	INTEGERLITERAL, //FOSE
	STRINGLITERAL, //FOSE
	BOOLEANITERAL, //FOSE
	ENDLINE,
	CONSTANTDECLARATION,
	COMMA,
	DECLARATION,
	INITLIST, // FOSE
	CLOSINGLIST,
	INITPARENTHESIS, // FOSE
	CLOSINGPARENTHESIS,
	INITBRACKET, // FOSE
	CLOSINGBRACKET,
	EQUAL, //FOO
	INEQUAL, //FOO
	LESSTHAN, //FOO
	BIGGERTHAN, //FOO
	LESSOREQUAL, //FOO
	BIGGEROREQUAL, //FOO
	ADDITION, // FOO creo
	SUBSTRACT, // FOO creo
	MULTIPLICATION, // FOO creo
	EOF,
	ILLEGAL_CHAR,
}