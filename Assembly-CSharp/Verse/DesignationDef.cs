using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000B36 RID: 2870
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

		// Token: 0x06003F28 RID: 16168 RVA: 0x002143CE File Offset: 0x002127CE
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
