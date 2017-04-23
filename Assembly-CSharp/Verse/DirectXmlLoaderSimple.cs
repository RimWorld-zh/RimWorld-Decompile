using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Verse
{
	public static class DirectXmlLoaderSimple
	{
		[DebuggerHidden]
		public static IEnumerable<KeyValuePair<string, string>> ValuesFromXmlFile(FileInfo file)
		{
			DirectXmlLoaderSimple.<ValuesFromXmlFile>c__Iterator220 <ValuesFromXmlFile>c__Iterator = new DirectXmlLoaderSimple.<ValuesFromXmlFile>c__Iterator220();
			<ValuesFromXmlFile>c__Iterator.file = file;
			<ValuesFromXmlFile>c__Iterator.<$>file = file;
			DirectXmlLoaderSimple.<ValuesFromXmlFile>c__Iterator220 expr_15 = <ValuesFromXmlFile>c__Iterator;
			expr_15.$PC = -2;
			return expr_15;
		}
	}
}
