using System;
using UnityEngine;

namespace RimWorld.Planet
{
	// Token: 0x02000571 RID: 1393
	public abstract class WorldFeatureTextMesh
	{
		// Token: 0x170003C0 RID: 960
		// (get) Token: 0x06001A6F RID: 6767
		public abstract bool Active { get; }

		// Token: 0x170003C1 RID: 961
		// (get) Token: 0x06001A70 RID: 6768
		public abstract Vector3 Position { get; }

		// Token: 0x170003C2 RID: 962
		// (get) Token: 0x06001A71 RID: 6769
		// (set) Token: 0x06001A72 RID: 6770
		public abstract Color Color { get; set; }

		// Token: 0x170003C3 RID: 963
		// (get) Token: 0x06001A73 RID: 6771
		// (set) Token: 0x06001A74 RID: 6772
		public abstract string Text { get; set; }

		// Token: 0x170003C4 RID: 964
		// (set) Token: 0x06001A75 RID: 6773
		public abstract float Size { set; }

		// Token: 0x170003C5 RID: 965
		// (get) Token: 0x06001A76 RID: 6774
		// (set) Token: 0x06001A77 RID: 6775
		public abstract Quaternion Rotation { get; set; }

		// Token: 0x170003C6 RID: 966
		// (get) Token: 0x06001A78 RID: 6776
		// (set) Token: 0x06001A79 RID: 6777
		public abstract Vector3 LocalPosition { get; set; }

		// Token: 0x06001A7A RID: 6778
		public abstract void SetActive(bool active);

		// Token: 0x06001A7B RID: 6779
		public abstract void Destroy();

		// Token: 0x06001A7C RID: 6780
		public abstract void Init();

		// Token: 0x06001A7D RID: 6781
		public abstract void WrapAroundPlanetSurface();
	}
}
