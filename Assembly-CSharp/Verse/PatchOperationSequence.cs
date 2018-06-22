using System;
using System.Collections.Generic;
using System.Xml;

namespace Verse
{
	// Token: 0x02000CDA RID: 3290
	public class PatchOperationSequence : PatchOperation
	{
		// Token: 0x06004894 RID: 18580 RVA: 0x00261DB4 File Offset: 0x002601B4
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

		// Token: 0x06004895 RID: 18581 RVA: 0x00261E30 File Offset: 0x00260230
		public override void Complete(string modIdentifier)
		{
			base.Complete(modIdentifier);
			this.lastFailedOperation = null;
		}

		// Token: 0x06004896 RID: 18582 RVA: 0x00261E44 File Offset: 0x00260244
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

		// Token: 0x04003116 RID: 12566
		private List<PatchOperation> operations;

		// Token: 0x04003117 RID: 12567
		private PatchOperation lastFailedOperation;
	}
}
