chimera.exe: Driver.cs Scanner.cs Token.cs TokenCategory.cs Parser.cs SyntaxError.cs                
	mcs -out:chimera.exe Driver.cs Scanner.cs Token.cs TokenCategory.cs Parser.cs SyntaxError.cs Node.cs SpecificNodes.cs SemanticAnalyzer.cs GloabalDeclaratonTable.cs SemanticError.cs TypeG.cs GlobalDeclarationType.cs ProcedureDeclarationTable.cs ProcedureDeclarationType.cs LocalDeclarationTable.cs LocalDeclarationType.cs 
clean:
	rm chimera.exe