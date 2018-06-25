using System;

namespace RimWorld
{
	// Token: 0x02000828 RID: 2088
	public struct TransferableCountToTransferStoppingPoint
	{
		// Token: 0x04001940 RID: 6464
		public int threshold;

		// Token: 0x04001941 RID: 6465
		public string leftLabel;

		// Token: 0x04001942 RID: 6466
		public string rightLabel;

		// Token: 0x06002EFF RID: 12031 RVA: 0x001912DE File Offset: 0x0018F6DE
		public TransferableCountToTransferStoppingPoint(int threshold, string leftLabel, string rightLabel)
		{
			this.threshold = threshold;
			this.leftLabel = leftLabel;
			this.rightLabel = rightLabel;
		}
	}
}
