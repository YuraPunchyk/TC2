using System;
using System.Collections.Generic;
using System.Text;

namespace TCLAB2 {
	class WorkWithText {
		public string FileName { get; set; }

		public WorkWithText (string fileName) {
			FileName = fileName;
		}
		public void SetText (string text) {
			System.IO.File.WriteAllText(FileName, text);
		}

		public string[] GetLines () {
			return System.IO.File.ReadAllLines(FileName);
		}
	}
}