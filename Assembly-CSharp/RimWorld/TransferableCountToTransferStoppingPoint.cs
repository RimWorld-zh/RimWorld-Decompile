using System;

namespace RimWorld
{
	// Token: 0x0200082A RID: 2090
	public struct TransferableCountToTransferStoppingPoint
	{
		// Token: 0x06002F02 RID: 12034 RVA: 0x00190FAE File Offset: 0x0018F3AE
		public TransferableCountToTransferStoppingPoint(int threshold, string leftLabel, string rightLabel)
		{
			this.threshold = threshold;
			this.leftLabel = leftLabel;
			this.rightLabel = rightLabel;
		}

		// Token: 0x04001942 RID: 6466
		public int threshold;

		// Token: 0x04001943 RID: 6467
		public string leftLabel;

		// Token: 0x04001944 RID: 6468
		public string rightLabel;
	}
}
