using System;
using System.Collections.Generic;
using System.Xml;

namespace Verse
{
	// Token: 0x02000CDE RID: 3294
	public class PatchOperationSequence : PatchOperation
	{
		// Token: 0x06004885 RID: 18565 RVA: 0x002609C4 File Offset: 0x0025EDC4
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

		// Token: 0x06004886 RID: 18566 RVA: 0x00260A40 File Offset: 0x0025EE40
		public override void Complete(string modIdentifier)
		{
			base.Complete(modIdentifier);
			this.lastFailedOperation = null;
		}

		// Token: 0x06004887 RID: 18567 RVA: 0x00260A54 File Offset: 0x0025EE54
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

		// Token: 0x0400310D RID: 12557
		private List<PatchOperation> operations;

		// Token: 0x0400310E RID: 12558
		private PatchOperation lastFailedOperation;
	}
}
