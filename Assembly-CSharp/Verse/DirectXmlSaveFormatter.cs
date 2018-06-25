using System;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Verse
{
	// Token: 0x02000D83 RID: 3459
	internal static class DirectXmlSaveFormatter
	{
		// Token: 0x06004D72 RID: 19826 RVA: 0x00285F44 File Offset: 0x00284344
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

		// Token: 0x06004D73 RID: 19827 RVA: 0x00286054 File Offset: 0x00284454
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

		// Token: 0x06004D74 RID: 19828 RVA: 0x002860EC File Offset: 0x002844EC
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
