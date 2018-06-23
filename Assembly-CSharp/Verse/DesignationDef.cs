using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000B34 RID: 2868
	public class DesignationDef : Def
	{
		// Token: 0x0400293B RID: 10555
		[NoTranslate]
		public string texturePath;

		// Token: 0x0400293C RID: 10556
		public TargetType targetType;

		// Token: 0x0400293D RID: 10557
		public bool removeIfBuildingDespawned = false;

		// Token: 0x0400293E RID: 10558
		public bool designateCancelable = true;

		// Token: 0x0400293F RID: 10559
		[Unsaved]
		public Material iconMat;

		// Token: 0x06003F25 RID: 16165 RVA: 0x002142F2 File Offset: 0x002126F2
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				this.iconMat = MaterialPool.MatFrom(this.texturePath, ShaderDatabase.MetaOverlay);
			});
		}
	}
}
