using System.Collections.Generic;
using System.Xml;

namespace Verse
{
	public class PatchOperationSequence : PatchOperation
	{
		private List<PatchOperation> operations;

		protected override bool ApplyWorker(XmlDocument xml)
		{
			foreach (PatchOperation operation in this.operations)
			{
				if (!operation.Apply(xml))
				{
					return false;
				}
			}
			return true;
		}
	}
}
