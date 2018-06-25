using System;
using System.Collections;
using System.Xml;

namespace Verse
{
	public class PatchOperationAttributeSet : PatchOperationAttribute
	{
		protected string value;

		public PatchOperationAttributeSet()
		{
		}

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
