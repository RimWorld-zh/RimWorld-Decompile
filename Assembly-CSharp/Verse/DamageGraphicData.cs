using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000B15 RID: 2837
	public class DamageGraphicData
	{
		// Token: 0x040027FA RID: 10234
		public bool enabled = true;

		// Token: 0x040027FB RID: 10235
		public Rect rectN;

		// Token: 0x040027FC RID: 10236
		public Rect rectE;

		// Token: 0x040027FD RID: 10237
		public Rect rectS;

		// Token: 0x040027FE RID: 10238
		public Rect rectW;

		// Token: 0x040027FF RID: 10239
		public Rect rect;

		// Token: 0x04002800 RID: 10240
		[NoTranslate]
		public List<string> scratches;

		// Token: 0x04002801 RID: 10241
		[NoTranslate]
		public string cornerTL;

		// Token: 0x04002802 RID: 10242
		[NoTranslate]
		public string cornerTR;

		// Token: 0x04002803 RID: 10243
		[NoTranslate]
		public string cornerBL;

		// Token: 0x04002804 RID: 10244
		[NoTranslate]
		public string cornerBR;

		// Token: 0x04002805 RID: 10245
		[NoTranslate]
		public string edgeLeft;

		// Token: 0x04002806 RID: 10246
		[NoTranslate]
		public string edgeRight;

		// Token: 0x04002807 RID: 10247
		[NoTranslate]
		public string edgeTop;

		// Token: 0x04002808 RID: 10248
		[NoTranslate]
		public string edgeBot;

		// Token: 0x04002809 RID: 10249
		[Unsaved]
		public List<Material> scratchMats;

		// Token: 0x0400280A RID: 10250
		[Unsaved]
		public Material cornerTLMat;

		// Token: 0x0400280B RID: 10251
		[Unsaved]
		public Material cornerTRMat;

		// Token: 0x0400280C RID: 10252
		[Unsaved]
		public Material cornerBLMat;

		// Token: 0x0400280D RID: 10253
		[Unsaved]
		public Material cornerBRMat;

		// Token: 0x0400280E RID: 10254
		[Unsaved]
		public Material edgeLeftMat;

		// Token: 0x0400280F RID: 10255
		[Unsaved]
		public Material edgeRightMat;

		// Token: 0x04002810 RID: 10256
		[Unsaved]
		public Material edgeTopMat;

		// Token: 0x04002811 RID: 10257
		[Unsaved]
		public Material edgeBotMat;

		// Token: 0x06003EA7 RID: 16039 RVA: 0x0020FDA0 File Offset: 0x0020E1A0
		public void ResolveReferencesSpecial()
		{
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				if (this.scratches != null)
				{
					this.scratchMats = new List<Material>();
					for (int i = 0; i < this.scratches.Count; i++)
					{
						this.scratchMats[i] = MaterialPool.MatFrom(this.scratches[i], ShaderDatabase.Transparent);
					}
				}
				if (this.cornerTL != null)
				{
					this.cornerTLMat = MaterialPool.MatFrom(this.cornerTL, ShaderDatabase.Transparent);
				}
				if (this.cornerTR != null)
				{
					this.cornerTRMat = MaterialPool.MatFrom(this.cornerTR, ShaderDatabase.Transparent);
				}
				if (this.cornerBL != null)
				{
					this.cornerBLMat = MaterialPool.MatFrom(this.cornerBL, ShaderDatabase.Transparent);
				}
				if (this.cornerBR != null)
				{
					this.cornerBRMat = MaterialPool.MatFrom(this.cornerBR, ShaderDatabase.Transparent);
				}
				if (this.edgeTop != null)
				{
					this.edgeTopMat = MaterialPool.MatFrom(this.edgeTop, ShaderDatabase.Transparent);
				}
				if (this.edgeBot != null)
				{
					this.edgeBotMat = MaterialPool.MatFrom(this.edgeBot, ShaderDatabase.Transparent);
				}
				if (this.edgeLeft != null)
				{
					this.edgeLeftMat = MaterialPool.MatFrom(this.edgeLeft, ShaderDatabase.Transparent);
				}
				if (this.edgeRight != null)
				{
					this.edgeRightMat = MaterialPool.MatFrom(this.edgeRight, ShaderDatabase.Transparent);
				}
			});
		}
	}
}
