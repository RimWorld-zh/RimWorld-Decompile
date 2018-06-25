using System;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Verse
{
	// Token: 0x02000D84 RID: 3460
	internal static class DirectXmlSaveFormatter
	{
		// Token: 0x06004D72 RID: 19826 RVA: 0x00286224 File Offset: 0x00284624
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

		// Token: 0x06004D73 RID: 19827 RVA: 0x00286334 File Offset: 0x00284734
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

		// Token: 0x06004D74 RID: 19828 RVA: 0x002863CC File Offset: 0x002847CC
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
