using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000BA4 RID: 2980
	public class SubcameraDef : Def
	{
		// Token: 0x170009D8 RID: 2520
		// (get) Token: 0x0600406B RID: 16491 RVA: 0x0021D700 File Offset: 0x0021BB00
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

		// Token: 0x170009D9 RID: 2521
		// (get) Token: 0x0600406C RID: 16492 RVA: 0x0021D738 File Offset: 0x0021BB38
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

		// Token: 0x04002B5A RID: 11098
		[NoTranslate]
		public string layer;

		// Token: 0x04002B5B RID: 11099
		public int depth;

		// Token: 0x04002B5C RID: 11100
		public RenderTextureFormat format;

		// Token: 0x04002B5D RID: 11101
		[Unsaved]
		private int layerCached = -1;
	}
}
