using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000B37 RID: 2871
	public class DesignationDef : Def
	{
		// Token: 0x04002942 RID: 10562
		[NoTranslate]
		public string texturePath;

		// Token: 0x04002943 RID: 10563
		public TargetType targetType;

		// Token: 0x04002944 RID: 10564
		public bool removeIfBuildingDespawned = false;

		// Token: 0x04002945 RID: 10565
		public bool designateCancelable = true;

		// Token: 0x04002946 RID: 10566
		[Unsaved]
		public Material iconMat;

		// Token: 0x06003F28 RID: 16168 RVA: 0x002146AE File Offset: 0x00212AAE
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
