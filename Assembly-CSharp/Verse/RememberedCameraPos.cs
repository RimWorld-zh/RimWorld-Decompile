using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000AF3 RID: 2803
	public class RememberedCameraPos : IExposable
	{
		// Token: 0x04002745 RID: 10053
		public Vector3 rootPos;

		// Token: 0x04002746 RID: 10054
		public float rootSize;

		// Token: 0x06003E0A RID: 15882 RVA: 0x0020B994 File Offset: 0x00209D94
		public RememberedCameraPos(Map map)
		{
			this.rootPos = map.Center.ToVector3Shifted();
			this.rootSize = 24f;
		}

		// Token: 0x06003E0B RID: 15883 RVA: 0x0020B9C8 File Offset: 0x00209DC8
		public void ExposeData()
		{
			Scribe_Values.Look<Vector3>(ref this.rootPos, "rootPos", default(Vector3), false);
			Scribe_Values.Look<float>(ref this.rootSize, "rootSize", 0f, false);
		}
	}
}
