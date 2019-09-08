chimera.exe: Driver.cs Scanner.cs Token.cs TokenCategory.cs               
	mcs -out:chimera.exe Driver.cs Scanner.cs Token.cs TokenCategory.cs
			
clean:
	rm chimera.e