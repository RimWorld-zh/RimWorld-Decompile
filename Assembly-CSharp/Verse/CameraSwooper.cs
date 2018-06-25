using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000AF1 RID: 2801
	public class CameraSwooper
	{
		// Token: 0x04002737 RID: 10039
		public bool Swooping = false;

		// Token: 0x04002738 RID: 10040
		private bool SwoopingTo = false;

		// Token: 0x04002739 RID: 10041
		private float TimeSinceSwoopStart = 0f;

		// Token: 0x0400273A RID: 10042
		private Vector3 FinalOffset;

		// Token: 0x0400273B RID: 10043
		private float FinalOrthoSizeOffset;

		// Token: 0x0400273C RID: 10044
		private float TotalSwoopTime;

		// Token: 0x0400273D RID: 10045
		private SwoopCallbackMethod SwoopFinishedCallback;

		// Token: 0x06003E06 RID: 15878 RVA: 0x0020B56A File Offset: 0x0020996A
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

		// Token: 0x06003E07 RID: 15879 RVA: 0x0020B5A3 File Offset: 0x002099A3
		public void StartSwoopToRoot(Vector3 FinalOffset, float FinalOrthoSizeOffset, float TotalSwoopTime, SwoopCallbackMethod SwoopFinishedCallback)
		{
			this.StartSwoopFromRoot(FinalOffset, FinalOrthoSizeOffset, TotalSwoopTime, SwoopFinishedCallback);
			this.SwoopingTo = true;
		}

		// Token: 0x06003E08 RID: 15880 RVA: 0x0020B5B8 File Offset: 0x002099B8
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

		// Token: 0x06003E09 RID: 15881 RVA: 0x0020B618 File Offset: 0x00209A18
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
	}
}
