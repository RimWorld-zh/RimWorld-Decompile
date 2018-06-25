using System;
using System.Collections;
using System.Xml;

namespace Verse
{
	// Token: 0x02000CDA RID: 3290
	public class PatchOperationAttributeAdd : PatchOperationAttribute
	{
		// Token: 0x0400311B RID: 12571
		protected string value;

		// Token: 0x06004891 RID: 18577 RVA: 0x00261F04 File Offset: 0x00260304
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
