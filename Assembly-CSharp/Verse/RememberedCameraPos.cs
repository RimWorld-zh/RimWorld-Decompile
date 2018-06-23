using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000AF0 RID: 2800
	public class RememberedCameraPos : IExposable
	{
		// Token: 0x0400273D RID: 10045
		public Vector3 rootPos;

		// Token: 0x0400273E RID: 10046
		public float rootSize;

		// Token: 0x06003E06 RID: 15878 RVA: 0x0020B588 File Offset: 0x00209988
		public RememberedCameraPos(Map map)
		{
			this.rootPos = map.Center.ToVector3Shifted();
			this.rootSize = 24f;
		}

		// Token: 0x06003E07 RID: 15879 RVA: 0x0020B5BC File Offset: 0x002099BC
		public void ExposeData()
		{
			Scribe_Values.Look<Vector3>(ref this.rootPos, "rootPos", default(Vector3), false);
			Scribe_Values.Look<float>(ref this.rootSize, "rootSize", 0f, false);
		}
	}
}
