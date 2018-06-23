using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000BA0 RID: 2976
	public class SubcameraDef : Def
	{
		// Token: 0x04002B5F RID: 11103
		[NoTranslate]
		public string layer;

		// Token: 0x04002B60 RID: 11104
		public int depth;

		// Token: 0x04002B61 RID: 11105
		public RenderTextureFormat format;

		// Token: 0x04002B62 RID: 11106
		[Unsaved]
		private int layerCached = -1;

		// Token: 0x170009DA RID: 2522
		// (get) Token: 0x0600406D RID: 16493 RVA: 0x0021DD9C File Offset: 0x0021C19C
		public int LayerId
		{
			get
			{
				if (this.layerCached == -1)
				{
					this.layerCached = LayerMask.NameToLayer(this.layer);
				}
				return this.layerCached;
			}
		}

		// Token: 0x170009DB RID: 2523
		// (get) Token: 0x0600406E RID: 16494 RVA: 0x0021DDD4 File Offset: 0x0021C1D4
		public RenderTextureFormat BestFormat
		{
			get
			{
				RenderTextureFormat result;
				if (SystemInfo.SupportsRenderTextureFormat(this.format))
				{
					result = this.format;
				}
				else if (this.format == RenderTextureFormat.R8 && SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.RG16))
				{
					result = RenderTextureFormat.RG16;
				}
				else if ((this.format == RenderTextureFormat.R8 || this.format == RenderTextureFormat.RG16) && SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGB32))
				{
					result = RenderTextureFormat.ARGB32;
				}
				else if ((this.format == RenderTextureFormat.R8 || this.format == RenderTextureFormat.RHalf || this.format == RenderTextureFormat.RFloat) && SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.RGFloat))
				{
					result = RenderTextureFormat.RGFloat;
				}
				else if ((this.format == RenderTextureFormat.R8 || this.format == RenderTextureFormat.RHalf || this.format == RenderTextureFormat.RFloat || this.format == RenderTextureFormat.RGFloat) && SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGBFloat))
				{
					result = RenderTextureFormat.ARGBFloat;
				}
				else
				{
					result = this.format;
				}
				return result;
			}
		}
	}
}
