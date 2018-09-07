using System;
using System.Xml;

namespace Verse
{
	public class PatchOperationTest : PatchOperationPathed
	{
		public PatchOperationTest()
		{
		}

		protected override bool ApplyWorker(XmlDocument xml)
		{
			return xml.SelectSingleNode(this.xpath) != null;
		}
	}
}
