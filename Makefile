chimera.exe: Driver.cs Scanner.cs Token.cs TokenCategory.cs Parser.cs SyntaxError.cs                
	mcs -out:chimera.exe Driver.cs Scanner.cs Token.cs TokenCategory.cs Parser.cs SyntaxError.cs
			
clean:
	rm chimera.exe