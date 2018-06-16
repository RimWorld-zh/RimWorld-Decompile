using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000B38 RID: 2872
	public class DesignationDef : Def
	{
		// Token: 0x06003F24 RID: 16164 RVA: 0x00213C06 File Offset: 0x00212006
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				this.iconMat = MaterialPool.MatFrom(this.texturePath, ShaderDatabase.MetaOverlay);
			});
		}

		// Token: 0x0400293E RID: 10558
		[NoTranslate]
		public string texturePath;

		// Token: 0x0400293F RID: 10559
		public TargetType targetType;

		// Token: 0x04002940 RID: 10560
		public bool removeIfBuildingDespawned = false;

		// Token: 0x04002941 RID: 10561
		public bool designateCancelable = true;

		// Token: 0x04002942 RID: 10562
		[Unsaved]
		public Material iconMat;
	}
}
