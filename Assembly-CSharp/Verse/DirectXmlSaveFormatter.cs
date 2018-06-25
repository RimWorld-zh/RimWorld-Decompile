using System;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Verse
{
	internal static class DirectXmlSaveFormatter
	{
		public static void AddWhitespaceFromRoot(XElement root)
		{
			if (root.Elements().Any<XElement>())
			{
				foreach (XElement xelement in root.Elements().ToList<XElement>())
				{
					XText content = new XText("\n");
					xelement.AddAfterSelf(content);
				}
				root.Elements().First<XElement>().AddBeforeSelf(new XText("\n"));
				root.Elements().Last<XElement>().AddAfterSelf(new XText("\n"));
				foreach (XElement element in root.Elements().ToList<XElement>())
				{
					DirectXmlSaveFormatter.IndentXml(element, 1);
				}
			}
		}

		private static void IndentXml(XElement element, int depth)
		{
			element.AddBeforeSelf(new XText(DirectXmlSaveFormatter.IndentString(depth, true)));
			bool startWithNewline = element.NextNode == null;
			element.AddAfterSelf(new XText(DirectXmlSaveFormatter.IndentString(depth - 1, startWithNewline)));
			foreach (XElement element2 in element.Elements().ToList<XElement>())
			{
				DirectXmlSaveFormatter.IndentXml(element2, depth + 1);
			}
		}

		private static string IndentString(int depth, bool startWithNewline)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (startWithNewline)
			{
				stringBuilder.Append("\n");
			}
			for (int i = 0; i < depth; i++)
			{
				stringBuilder.Append("  ");
			}
			return stringBuilder.ToString();
		}
	}
}
