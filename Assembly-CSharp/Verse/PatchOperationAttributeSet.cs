using System;
using System.Collections;
using System.Xml;

namespace Verse
{
	public class PatchOperationAttributeSet : PatchOperationAttribute
	{
		protected string value;

		protected override bool ApplyWorker(XmlDocument xml)
		{
			bool result = false;
			IEnumerator enumerator = xml.SelectNodes(base.xpath).GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object current = enumerator.Current;
					XmlNode xmlNode = current as XmlNode;
					if (xmlNode.Attributes[base.attribute] != null)
					{
						xmlNode.Attributes[base.attribute].Value = this.value;
					}
					else
					{
						XmlAttribute xmlAttribute = xmlNode.OwnerDocument.CreateAttribute(base.attribute);
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
