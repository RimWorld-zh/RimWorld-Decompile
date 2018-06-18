using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000B73 RID: 2931
	public class ScatterableDef : Def
	{
		// Token: 0x06003FF0 RID: 16368 RVA: 0x0021AD54 File Offset: 0x00219154
		public override void PostLoad()
		{
			base.PostLoad();
			if (this.defName == "UnnamedDef")
			{
				this.defName = "Scatterable_" + this.texturePath;
			}
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				this.mat = MaterialPool.MatFrom(this.texturePath, ShaderDatabase.Transparent);
			});
		}

		// Token: 0x04002AD0 RID: 10960
		[NoTranslate]
		public string texturePath;

		// Token: 0x04002AD1 RID: 10961
		public float minSize;

		// Token: 0x04002AD2 RID: 10962
		public float maxSize;

		// Token: 0x04002AD3 RID: 10963
		public float selectionWeight = 100f;

		// Token: 0x04002AD4 RID: 10964
		[NoTranslate]
		public string scatterType = "";

		// Token: 0x04002AD5 RID: 10965
		public Material mat;
	}
}
