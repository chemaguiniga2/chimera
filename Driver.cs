/*
Dachely Otero Argote            A01377812
José María Aguíñiga Díaz        A01376669 
José Rodrigo Narváez Berlanga   A01377566
 */

using System;
using System.IO;
using System.Text;

namespace Chimera
{

    public class Driver
    {

        const string VERSION = "0.4";

        //-----------------------------------------------------------
        static readonly string[] ReleaseIncludes = {
            "Lexical analysis",
            "Syntactic analysis",
            "AST construction",
            "Semantic analysis"
        };


        void PrintAppHeader()
        {
            Console.WriteLine("Chimera compiler, version " + VERSION);
            Console.WriteLine("Copyright \u00A9 2019 by DHL, ITESM CEM.");
            Console.WriteLine("This program was made for Compiler Design class");
            Console.WriteLine("This program has absolutely no warranty.");
        }

        void PrintReleaseIncludes()
        {
            Console.WriteLine("Included in this release:");
            foreach (var phase in ReleaseIncludes)
            {
                Console.WriteLine("   * " + phase);
            }
        }

        void Run(string[] args)
        {

            PrintAppHeader();
            Console.WriteLine();
            PrintReleaseIncludes();
            Console.WriteLine();

            if (args.Length != 1)
            {
                Console.Error.WriteLine(
                    "Please specify the name of the input file.");
                Environment.Exit(1);
            }
            //Comentar de aqui hasta
            /*try
            {
                var inputPath = args[0];
                var input = File.ReadAllText(inputPath);

                Console.WriteLine(String.Format(
                    "===== Tokens from: \"{0}\" =====", inputPath)
                );
                var count = 1;
                foreach (var tok in new Scanner(input).Start())
                {
                    Console.WriteLine(String.Format("[{0}] {1}",
                                                    count++, tok)
                    );
                }

            }
            catch (FileNotFoundException e)
            {
                Console.Error.WriteLine(e.Message);
                Environment.Exit(1);
            }

            try
            {
                var inputPath = args[0];
                var input = File.ReadAllText(inputPath);
                var parser = new Parser(new Scanner(input).Start().GetEnumerator());
                parser.Program();
                Console.WriteLine("Syntax OK.");

            }
            catch (Exception e)
            {

                if (e is FileNotFoundException || e is SyntaxError)
                {
                    Console.Error.WriteLine(e.Message);
                    Environment.Exit(1);
                }

                throw;
            }*/
            //Aca
            try
            {
                var inputPath = args[0];
                var input = File.ReadAllText(inputPath);
                var parser = new Parser(new Scanner(input).Start().GetEnumerator());
                var program = parser.Program();
                //Console.Write(program.ToStringTree());
                Console.WriteLine("Syntax OK.");

                var semantic = new SemanticAnalyzer();
                semantic.Visit((dynamic) program);

                Console.WriteLine("Semantics OK.");
                Console.WriteLine();
                //solucion profe
                Console.WriteLine("Global Declaration Table");
                Console.WriteLine("============");
                foreach (var entry in semantic.GloabalDeclaratonT) {
                    Console.WriteLine(entry);  
                                                         
                }
                Console.WriteLine();
                // mi solución
                Console.WriteLine(semantic.GloabalDeclaratonT);

            }
            catch (Exception e)
            {
                if (e is FileNotFoundException || e is SyntaxError || e is SemanticError)
                {
                    Console.Error.WriteLine(e.Message);
                    Environment.Exit(1);
                }
                throw;
            }


        }

        //-----------------------------------------------------------
        public static void Main(string[] args)
        {
            new Driver().Run(args);
        }
    }
}






/*
public class Driver {

    public static void Main(){
		Console.Write("> ");
		var line = Console.ReadLine();
		foreach (var token in new Scanner(line).Start()){
			Console.WriteLine(token);
		}
	}
}
*/