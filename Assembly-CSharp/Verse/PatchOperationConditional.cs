using System.Xml;

namespace Verse
{
	public class PatchOperationConditional : PatchOperationPathed
	{
		private PatchOperation match;

		private PatchOperation nomatch;

		protected override bool ApplyWorker(XmlDocument xml)
		{
			if (xml.SelectSingleNode(base.xpath) != null)
			{
				if (this.match != null)
				{
					return this.match.Apply(xml);
				}
			}
			else if (this.nomatch != null)
			{
				return this.nomatch.Apply(xml);
			}
			return false;
		}
	}
}
