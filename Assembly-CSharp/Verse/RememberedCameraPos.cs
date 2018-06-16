using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000AF4 RID: 2804
	public class RememberedCameraPos : IExposable
	{
		// Token: 0x06003E09 RID: 15881 RVA: 0x0020B190 File Offset: 0x00209590
		public RememberedCameraPos(Map map)
		{
			this.rootPos = map.Center.ToVector3Shifted();
			this.rootSize = 24f;
		}

		// Token: 0x06003E0A RID: 15882 RVA: 0x0020B1C4 File Offset: 0x002095C4
		public void ExposeData()
		{
			Scribe_Values.Look<Vector3>(ref this.rootPos, "rootPos", default(Vector3), false);
			Scribe_Values.Look<float>(ref this.rootSize, "rootSize", 0f, false);
		}

		// Token: 0x04002742 RID: 10050
		public Vector3 rootPos;

		// Token: 0x04002743 RID: 10051
		public float rootSize;
	}
}
