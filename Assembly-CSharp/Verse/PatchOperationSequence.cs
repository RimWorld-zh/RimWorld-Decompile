using System;
using System.Collections.Generic;
using System.Xml;

namespace Verse
{
	// Token: 0x02000CDD RID: 3293
	public class PatchOperationSequence : PatchOperation
	{
		// Token: 0x06004883 RID: 18563 RVA: 0x0026099C File Offset: 0x0025ED9C
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

		// Token: 0x06004884 RID: 18564 RVA: 0x00260A18 File Offset: 0x0025EE18
		public override void Complete(string modIdentifier)
		{
			base.Complete(modIdentifier);
			this.lastFailedOperation = null;
		}

		// Token: 0x06004885 RID: 18565 RVA: 0x00260A2C File Offset: 0x0025EE2C
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

		// Token: 0x0400310B RID: 12555
		private List<PatchOperation> operations;

		// Token: 0x0400310C RID: 12556
		private PatchOperation lastFailedOperation;
	}
}
