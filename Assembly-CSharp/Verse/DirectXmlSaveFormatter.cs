using System;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Verse
{
	// Token: 0x02000D85 RID: 3461
	internal static class DirectXmlSaveFormatter
	{
		// Token: 0x06004D5B RID: 19803 RVA: 0x00284888 File Offset: 0x00282C88
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

		// Token: 0x06004D5C RID: 19804 RVA: 0x00284998 File Offset: 0x00282D98
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

		// Token: 0x06004D5D RID: 19805 RVA: 0x00284A30 File Offset: 0x00282E30
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
