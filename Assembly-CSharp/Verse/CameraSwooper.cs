using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000AF3 RID: 2803
	public class CameraSwooper
	{
		// Token: 0x06003E05 RID: 15877 RVA: 0x0020B046 File Offset: 0x00209446
		public void StartSwoopFromRoot(Vector3 FinalOffset, float FinalOrthoSizeOffset, float TotalSwoopTime, SwoopCallbackMethod SwoopFinishedCallback)
		{
			this.Swooping = true;
			this.TimeSinceSwoopStart = 0f;
			this.FinalOffset = FinalOffset;
			this.FinalOrthoSizeOffset = FinalOrthoSizeOffset;
			this.TotalSwoopTime = TotalSwoopTime;
			this.SwoopFinishedCallback = SwoopFinishedCallback;
			this.SwoopingTo = false;
		}

		// Token: 0x06003E06 RID: 15878 RVA: 0x0020B07F File Offset: 0x0020947F
		public void StartSwoopToRoot(Vector3 FinalOffset, float FinalOrthoSizeOffset, float TotalSwoopTime, SwoopCallbackMethod SwoopFinishedCallback)
		{
			this.StartSwoopFromRoot(FinalOffset, FinalOrthoSizeOffset, TotalSwoopTime, SwoopFinishedCallback);
			this.SwoopingTo = true;
		}

		// Token: 0x06003E07 RID: 15879 RVA: 0x0020B094 File Offset: 0x00209494
		public void Update()
		{
			if (this.Swooping)
			{
				this.TimeSinceSwoopStart += Time.deltaTime;
				if (this.TimeSinceSwoopStart >= this.TotalSwoopTime)
				{
					this.Swooping = false;
					if (this.SwoopFinishedCallback != null)
					{
						this.SwoopFinishedCallback();
					}
				}
			}
		}

		// Token: 0x06003E08 RID: 15880 RVA: 0x0020B0F4 File Offset: 0x002094F4
		public void OffsetCameraFrom(GameObject camObj, Vector3 basePos, float baseSize)
		{
			float num = this.TimeSinceSwoopStart / this.TotalSwoopTime;
			if (!this.Swooping)
			{
				num = 0f;
			}
			else
			{
				num = this.TimeSinceSwoopStart / this.TotalSwoopTime;
				if (this.SwoopingTo)
				{
					num = 1f - num;
				}
				num = (float)Math.Pow((double)num, 1.7000000476837158);
			}
			camObj.transform.position = basePos + this.FinalOffset * num;
			Find.Camera.orthographicSize = baseSize + this.FinalOrthoSizeOffset * num;
		}

		// Token: 0x0400273B RID: 10043
		public bool Swooping = false;

		// Token: 0x0400273C RID: 10044
		private bool SwoopingTo = false;

		// Token: 0x0400273D RID: 10045
		private float TimeSinceSwoopStart = 0f;

		// Token: 0x0400273E RID: 10046
		private Vector3 FinalOffset;

		// Token: 0x0400273F RID: 10047
		private float FinalOrthoSizeOffset;

		// Token: 0x04002740 RID: 10048
		private float TotalSwoopTime;

		// Token: 0x04002741 RID: 10049
		private SwoopCallbackMethod SwoopFinishedCallback;
	}
}
