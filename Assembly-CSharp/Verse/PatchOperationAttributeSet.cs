using System;
using System.Collections;
using System.Xml;

namespace Verse
{
	// Token: 0x02000CDC RID: 3292
	public class PatchOperationAttributeSet : PatchOperationAttribute
	{
		// Token: 0x0400311C RID: 12572
		protected string value;

		// Token: 0x06004895 RID: 18581 RVA: 0x00262084 File Offset: 0x00260484
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
	}
}
