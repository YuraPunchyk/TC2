using System;

namespace TCLAB2 {
	class Program {
		static void Main ( string[] args ) {
			WorkWithText workWithText = new WorkWithText("input.txt");
			LexAnelasy lexAnelasy = new LexAnelasy(workWithText.GetLines());
			lexAnelasy.Analyse();
			Console.ReadLine();
		}
	}
}