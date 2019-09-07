public class Driver {
	public static void Main(){
		Console.Write("> ");
		var line = Console.ReadLine();
		foreach (var token in new Scanner(line).Start()){
			Console.WriteLine(token);
		}
	}
}