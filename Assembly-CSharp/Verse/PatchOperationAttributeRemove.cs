using System;
using System.Collections;
using System.Xml;

namespace Verse
{
	public class PatchOperationAttributeRemove : PatchOperationAttribute
	{
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
						xmlNode.Attributes.Remove(xmlNode.Attributes[base.attribute]);
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
