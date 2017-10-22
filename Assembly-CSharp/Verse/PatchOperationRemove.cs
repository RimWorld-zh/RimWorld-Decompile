using System;
using System.Collections;
using System.Xml;

namespace Verse
{
	public class PatchOperationRemove : PatchOperationPathed
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
					result = true;
					XmlNode xmlNode = current as XmlNode;
					xmlNode.ParentNode.RemoveChild(xmlNode);
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
