using System;
using System.Collections.Generic;
using System.Xml;

namespace Verse
{
	// Token: 0x02000CDC RID: 3292
	public class PatchOperationSequence : PatchOperation
	{
		// Token: 0x04003116 RID: 12566
		private List<PatchOperation> operations;

		// Token: 0x04003117 RID: 12567
		private PatchOperation lastFailedOperation;

		// Token: 0x06004897 RID: 18583 RVA: 0x00261E90 File Offset: 0x00260290
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

		// Token: 0x06004898 RID: 18584 RVA: 0x00261F0C File Offset: 0x0026030C
		public override void Complete(string modIdentifier)
		{
			base.Complete(modIdentifier);
			this.lastFailedOperation = null;
		}

		// Token: 0x06004899 RID: 18585 RVA: 0x00261F20 File Offset: 0x00260320
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
