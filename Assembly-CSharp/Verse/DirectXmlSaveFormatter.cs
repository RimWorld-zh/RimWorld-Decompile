using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Verse
{
	internal static class DirectXmlSaveFormatter
	{
		public static void AddWhitespaceFromRoot(XElement root)
		{
			if (root.Elements().Any())
			{
				List<XElement>.Enumerator enumerator = root.Elements().ToList().GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						XElement current = enumerator.Current;
						XText content = new XText("\n");
						current.AddAfterSelf(content);
					}
				}
				finally
				{
					((IDisposable)(object)enumerator).Dispose();
				}
				root.Elements().First().AddBeforeSelf(new XText("\n"));
				root.Elements().Last().AddAfterSelf(new XText("\n"));
				List<XElement>.Enumerator enumerator2 = root.Elements().ToList().GetEnumerator();
				try
				{
					while (enumerator2.MoveNext())
					{
						XElement current2 = enumerator2.Current;
						DirectXmlSaveFormatter.IndentXml(current2, 1);
					}
				}
				finally
				{
					((IDisposable)(object)enumerator2).Dispose();
				}
			}
		}

		private static void IndentXml(XElement element, int depth)
		{
			element.AddBeforeSelf(new XText(DirectXmlSaveFormatter.IndentString(depth, true)));
			bool startWithNewline = element.NextNode == null;
			element.AddAfterSelf(new XText(DirectXmlSaveFormatter.IndentString(depth - 1, startWithNewline)));
			List<XElement>.Enumerator enumerator = element.Elements().ToList().GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					XElement current = enumerator.Current;
					DirectXmlSaveFormatter.IndentXml(current, depth + 1);
				}
			}
			finally
			{
				((IDisposable)(object)enumerator).Dispose();
			}
		}

		private static string IndentString(int depth, bool startWithNewline)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (startWithNewline)
			{
				stringBuilder.Append("\n");
			}
			for (int num = 0; num < depth; num++)
			{
				stringBuilder.Append("  ");
			}
			return stringBuilder.ToString();
		}
	}
}
