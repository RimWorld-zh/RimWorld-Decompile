using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020002E5 RID: 741
	public class TimeAssignmentDef : Def
	{
		// Token: 0x170001D4 RID: 468
		// (get) Token: 0x06000C3D RID: 3133 RVA: 0x0006C9F0 File Offset: 0x0006ADF0
		public Texture2D ColorTexture
		{
			get
			{
				if (this.colorTextureInt == null)
				{
					this.colorTextureInt = SolidColorMaterials.NewSolidColorTexture(this.color);
				}
				return this.colorTextureInt;
			}
		}

		// Token: 0x040007C9 RID: 1993
		public Color color;

		// Token: 0x040007CA RID: 1994
		public bool allowRest = true;

		// Token: 0x040007CB RID: 1995
		public bool allowJoy = true;

		// Token: 0x040007CC RID: 1996
		private Texture2D colorTextureInt;
	}
}
