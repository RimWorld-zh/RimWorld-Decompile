using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000AEF RID: 2799
	public class CameraSwooper
	{
		// Token: 0x06003E02 RID: 15874 RVA: 0x0020B43E File Offset: 0x0020983E
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

		// Token: 0x06003E03 RID: 15875 RVA: 0x0020B477 File Offset: 0x00209877
		public void StartSwoopToRoot(Vector3 FinalOffset, float FinalOrthoSizeOffset, float TotalSwoopTime, SwoopCallbackMethod SwoopFinishedCallback)
		{
			this.StartSwoopFromRoot(FinalOffset, FinalOrthoSizeOffset, TotalSwoopTime, SwoopFinishedCallback);
			this.SwoopingTo = true;
		}

		// Token: 0x06003E04 RID: 15876 RVA: 0x0020B48C File Offset: 0x0020988C
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

		// Token: 0x06003E05 RID: 15877 RVA: 0x0020B4EC File Offset: 0x002098EC
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

		// Token: 0x04002736 RID: 10038
		public bool Swooping = false;

		// Token: 0x04002737 RID: 10039
		private bool SwoopingTo = false;

		// Token: 0x04002738 RID: 10040
		private float TimeSinceSwoopStart = 0f;

		// Token: 0x04002739 RID: 10041
		private Vector3 FinalOffset;

		// Token: 0x0400273A RID: 10042
		private float FinalOrthoSizeOffset;

		// Token: 0x0400273B RID: 10043
		private float TotalSwoopTime;

		// Token: 0x0400273C RID: 10044
		private SwoopCallbackMethod SwoopFinishedCallback;
	}
}
