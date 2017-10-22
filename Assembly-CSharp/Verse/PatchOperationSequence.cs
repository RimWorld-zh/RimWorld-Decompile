using System;
using System.Collections.Generic;
using System.Xml;

namespace Verse
{
	public class PatchOperationSequence : PatchOperation
	{
		private List<PatchOperation> operations;

		protected override bool ApplyWorker(XmlDocument xml)
		{
			List<PatchOperation>.Enumerator enumerator = this.operations.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					PatchOperation current = enumerator.Current;
					if (!current.Apply(xml))
					{
						return false;
					}
				}
			}
			finally
			{
				((IDisposable)(object)enumerator).Dispose();
			}
			return true;
		}
	}
}
