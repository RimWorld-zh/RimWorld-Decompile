using System;

namespace RimWorld
{
	// Token: 0x02000826 RID: 2086
	public struct TransferableCountToTransferStoppingPoint
	{
		// Token: 0x06002EFB RID: 12027 RVA: 0x0019118E File Offset: 0x0018F58E
		public TransferableCountToTransferStoppingPoint(int threshold, string leftLabel, string rightLabel)
		{
			this.threshold = threshold;
			this.leftLabel = leftLabel;
			this.rightLabel = rightLabel;
		}

		// Token: 0x04001940 RID: 6464
		public int threshold;

		// Token: 0x04001941 RID: 6465
		public string leftLabel;

		// Token: 0x04001942 RID: 6466
		public string rightLabel;
	}
}
