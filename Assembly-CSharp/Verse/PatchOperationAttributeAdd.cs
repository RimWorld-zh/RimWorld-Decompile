using System;
using System.Collections;
using System.Xml;

namespace Verse
{
	// Token: 0x02000CDB RID: 3291
	public class PatchOperationAttributeAdd : PatchOperationAttribute
	{
		// Token: 0x0600487F RID: 18559 RVA: 0x00260758 File Offset: 0x0025EB58
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

		// Token: 0x0400310B RID: 12555
		protected string value;
	}
}
