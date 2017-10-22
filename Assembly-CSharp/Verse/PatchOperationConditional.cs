using System.Xml;

namespace Verse
{
	public class PatchOperationConditional : PatchOperationPathed
	{
		private PatchOperation match;

		private PatchOperation nomatch;

		protected override bool ApplyWorker(XmlDocument xml)
		{
			bool result;
			if (xml.SelectSingleNode(base.xpath) != null)
			{
				if (this.match != null)
				{
					result = this.match.Apply(xml);
					goto IL_005c;
				}
			}
			else if (this.nomatch != null)
			{
				result = this.nomatch.Apply(xml);
				goto IL_005c;
			}
			result = false;
			goto IL_005c;
			IL_005c:
			return result;
		}
	}
}
