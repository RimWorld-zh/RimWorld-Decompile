using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000AF2 RID: 2802
	public class RememberedCameraPos : IExposable
	{
		// Token: 0x0400273E RID: 10046
		public Vector3 rootPos;

		// Token: 0x0400273F RID: 10047
		public float rootSize;

		// Token: 0x06003E0A RID: 15882 RVA: 0x0020B6B4 File Offset: 0x00209AB4
		public RememberedCameraPos(Map map)
		{
			this.rootPos = map.Center.ToVector3Shifted();
			this.rootSize = 24f;
		}

		// Token: 0x06003E0B RID: 15883 RVA: 0x0020B6E8 File Offset: 0x00209AE8
		public void ExposeData()
		{
			Scribe_Values.Look<Vector3>(ref this.rootPos, "rootPos", default(Vector3), false);
			Scribe_Values.Look<float>(ref this.rootSize, "rootSize", 0f, false);
		}
	}
}
