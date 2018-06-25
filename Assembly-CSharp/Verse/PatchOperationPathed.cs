using System;

namespace Verse
{
	public abstract class PatchOperationPathed : PatchOperation
	{
		protected string xpath;

		protected PatchOperationPathed()
		{
		}

		public override string ToString()
		{
			return string.Format("{0}({1})", base.ToString(), this.xpath);
		}
	}
}
