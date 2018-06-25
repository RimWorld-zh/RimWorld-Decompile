using System;

namespace RimWorld
{
	// Token: 0x02000828 RID: 2088
	public struct TransferableCountToTransferStoppingPoint
	{
		// Token: 0x04001944 RID: 6468
		public int threshold;

		// Token: 0x04001945 RID: 6469
		public string leftLabel;

		// Token: 0x04001946 RID: 6470
		public string rightLabel;

		// Token: 0x06002EFE RID: 12030 RVA: 0x00191546 File Offset: 0x0018F946
		public TransferableCountToTransferStoppingPoint(int threshold, string leftLabel, string rightLabel)
		{
			this.threshold = threshold;
			this.leftLabel = leftLabel;
			this.rightLabel = rightLabel;
		}
	}
}
