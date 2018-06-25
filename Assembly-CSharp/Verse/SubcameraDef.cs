using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000BA3 RID: 2979
	public class SubcameraDef : Def
	{
		// Token: 0x04002B66 RID: 11110
		[NoTranslate]
		public string layer;

		// Token: 0x04002B67 RID: 11111
		public int depth;

		// Token: 0x04002B68 RID: 11112
		public RenderTextureFormat format;

		// Token: 0x04002B69 RID: 11113
		[Unsaved]
		private int layerCached = -1;

		// Token: 0x170009D9 RID: 2521
		// (get) Token: 0x06004070 RID: 16496 RVA: 0x0021E158 File Offset: 0x0021C558
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

		// Token: 0x170009DA RID: 2522
		// (get) Token: 0x06004071 RID: 16497 RVA: 0x0021E190 File Offset: 0x0021C590
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
