using System;
using System.Collections;
using System.Xml;

namespace Verse
{
	// Token: 0x02000CD7 RID: 3287
	public class PatchOperationAttributeAdd : PatchOperationAttribute
	{
		// Token: 0x04003114 RID: 12564
		protected string value;

		// Token: 0x0600488E RID: 18574 RVA: 0x00261B48 File Offset: 0x0025FF48
		protected override bool ApplyWorker(XmlDocument xml)
		{
			bool result = false;
			IEnumerator enumerator = xml.SelectNodes(this.xpath).GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					XmlNode xmlNode = obj as XmlNode;
					if (xmlNode.Attributes[this.attribute] == null)
					{
						XmlAttribute xmlAttribute = xmlNode.OwnerDocument.CreateAttribute(this.attribute);
						xmlAttribute.Value = this.value;
						xmlNode.Attributes.Append(xmlAttribute);
						result = true;
					}
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
			return result;
		}
	}
}
