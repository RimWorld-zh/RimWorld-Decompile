using System;
using System.Collections;
using System.Xml;

namespace Verse
{
	// Token: 0x02000CDD RID: 3293
	public class PatchOperationAttributeSet : PatchOperationAttribute
	{
		// Token: 0x06004883 RID: 18563 RVA: 0x002608D8 File Offset: 0x0025ECD8
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
					if (xmlNode.Attributes[this.attribute] != null)
					{
						xmlNode.Attributes[this.attribute].Value = this.value;
					}
					else
					{
						XmlAttribute xmlAttribute = xmlNode.OwnerDocument.CreateAttribute(this.attribute);
						xmlAttribute.Value = this.value;
						xmlNode.Attributes.Append(xmlAttribute);
					}
					result = true;
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

		// Token: 0x0400310C RID: 12556
		protected string value;
	}
}
