using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000AF4 RID: 2804
	public class RememberedCameraPos : IExposable
	{
		// Token: 0x06003E0B RID: 15883 RVA: 0x0020B264 File Offset: 0x00209664
		public RememberedCameraPos(Map map)
		{
			this.rootPos = map.Center.ToVector3Shifted();
			this.rootSize = 24f;
		}

		// Token: 0x06003E0C RID: 15884 RVA: 0x0020B298 File Offset: 0x00209698
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
