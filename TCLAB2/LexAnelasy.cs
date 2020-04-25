using System;

namespace TCLAB2 {
	class LexAnelasy {
		public string[] Lines { get; set; }
		public int Pos { get; set; }
		public char Lexema { get; set; }
		public string LexemText { get; set; } = "";
		public int Bail { get; set; }
		public string Inden { get; set; }

		public LexAnelasy ( string[] lines ) {
			Lines = lines;
		}
		public const string Operation = "+*";
		public const string Math = "=()";
		public const string IndenConst = "id4op35";
		public void GetToken ( string line ) {
			Lexema = line[Pos++];
			if(Operation.Contains(Lexema)) {
				LexemText += $"{{{Lexema}, Operation}}\n";
			} else if(Math.Contains(Lexema)) {
				LexemText += $"{{{Lexema}, Math}}\n";
			} else if (!IndenConst.Contains(Lexema)){
				LexemText += $"{{{Lexema}, NotDefined}}\n";
			}
		}

		public void Analyse () {
			bool ok;
			WorkWithText workWithText = new WorkWithText("OutPut.txt");
			WorkWithText workWithLexem = new WorkWithText("Lexem");
			string text = "";
			for(int i = 0; i < Lines.Length; i++) {
				Pos = 0;
				Bail = 0;
				try {
					ok = Start(Lines[i]);
					if(ok && Bail == 1) {
						text += Lines[i] + "\n";
					} else {
						Console.WriteLine($"Lexical exception in {i} row, {Pos} position");
					}
				} catch(Exception ex) {
					Console.WriteLine($"Lexical exception in {i} row, {Pos} position. Body{ex.Message}");
				}

			}
			if(!String.IsNullOrWhiteSpace(text)) {
				workWithText.SetText(text);
			}
			workWithLexem.SetText(LexemText);
		}

		public bool Start ( string line ) {
			return S(line);
		}

		public bool S ( string line ) {
			string Repeat = "repeat";
			string Until = "until";
			GetToken(line);
			if(Lexema != 'r') {
				if(P(line)) {
					LexemText += $"{{{Inden}, Inden}}\n";
					if(Lexema == '=') {
						return E(line);
					} else {
						return false;
					}
				} else {
					return false;
				}
			} else {
				int i = 1;
				while(true) {
					if(Lexema != Repeat[i++]) {
						return false;
					}
					if(i == Repeat.Length) {
						LexemText += $"{{{Repeat}, Word}}\n";
						bool ok = S(line);
						i = 1;
						if(ok) {
							while(true) {
								if(Lexema != Until[i++]) {
									return false;
								}
								if(i == Until.Length) {
									LexemText += $"{{{Until}, Word}}\n";
									return E(line);
								}
								GetToken(line);
							}
						} else {
							return false;
						}
					}
					GetToken(line);
				}
			}
		}

		public bool P ( string line ) {
			string Ind1consanent = "id";
			string Ind1number = "4";
			string Ind2consanet = "op";
			string Ind2number = "35";
			if(Ind1consanent.Contains(Lexema)) {
				return ChechInden(line, Ind1consanent, Ind1number);
			} else if(Ind2consanet.Contains(Lexema)) {
				return ChechInden(line, Ind2consanet, Ind2number);
			} else {
				return false;
			}

		}

		public bool E ( string line ) {
			bool ok = false;
			if(ADD(line)) {
				if(Lexema == '+') {
					ok = E(line);
				} else {
					ok = true;
				}
			}
			return ok;
		}

		public bool ADD ( string line ) {
			bool ok = false;
			if(MULT(line)) {
				if(Lexema == '*') {
					GetToken(line);
					ok = ADD(line);
				} else {
					ok = true;
				}
			}
			return ok;
		}

		public bool MULT ( string line ) {
			string consts = "+*=";
			bool ok = false;
			if(Lexema != '(') {
				if(consts.Contains(Lexema)) {
					GetToken(line);
				}
				ok = P(line);
				if(ok) {
					LexemText += $"{{{Inden}, Inden}}\n";
				}
			} else {
				Bail++;
				GetToken(line);
				ok = E(line);
				if(ok && Lexema == ')') {
					if(Pos < line.Length) {
						Bail--;
						GetToken(line);
					}
					return true;
				}
			}
			return ok;
		}
		public bool ChechInden ( string line, string consanents, string numbers ) {
			string consts = "+*=)";
			bool check = false;
			Inden = "";
			while(true) {
				if(consanents.Contains(Lexema)) {
					check = true;
					Inden += Lexema.ToString();
				} else if(check) {
					while(numbers.Contains(Lexema)) {
						Inden += Lexema.ToString();
						if(Pos == line.Length) {
							Bail = 1;
							return true;
						}
						GetToken(line);
					}
					if(consts.Contains(Lexema) || Pos > line.Length) {
						return true;
					} else {
						return false;
					}
				} else {
					return false;
				}
				GetToken(line);
			}

		}
	}
}