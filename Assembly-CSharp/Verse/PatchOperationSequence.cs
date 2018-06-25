using System;
using System.Collections.Generic;
using System.Xml;

namespace Verse
{
	// Token: 0x02000CDD RID: 3293
	public class PatchOperationSequence : PatchOperation
	{
		// Token: 0x0400311D RID: 12573
		private List<PatchOperation> operations;

		// Token: 0x0400311E RID: 12574
		private PatchOperation lastFailedOperation;

		// Token: 0x06004897 RID: 18583 RVA: 0x00262170 File Offset: 0x00260570
		protected override bool ApplyWorker(XmlDocument xml)
		{
			foreach (PatchOperation patchOperation in this.operations)
			{
				if (!patchOperation.Apply(xml))
				{
					this.lastFailedOperation = patchOperation;
					return false;
				}
			}
			return true;
		}

		// Token: 0x06004898 RID: 18584 RVA: 0x002621EC File Offset: 0x002605EC
		public override void Complete(string modIdentifier)
		{
			base.Complete(modIdentifier);
			this.lastFailedOperation = null;
		}

		// Token: 0x06004899 RID: 18585 RVA: 0x00262200 File Offset: 0x00260600
		public override string ToString()
		{
			int num = (this.operations == null) ? 0 : this.operations.Count;
			string text = string.Format("{0}(count={1}", base.ToString(), num);
			if (this.lastFailedOperation != null)
			{
				text = text + ", lastFailedOperation=" + this.lastFailedOperation;
			}
			return text + ")";
		}
	}
}
