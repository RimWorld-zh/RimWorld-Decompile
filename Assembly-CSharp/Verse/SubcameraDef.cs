using UnityEngine;

namespace Verse
{
	public class SubcameraDef : Def
	{
		public string layer;

		public int depth;

		public RenderTextureFormat format;

		private int layerCached = -1;

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
				else
				{
					if ((this.format == RenderTextureFormat.R8 || this.format == RenderTextureFormat.RG16) && SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGB32))
					{
						result = RenderTextureFormat.ARGB32;
						goto IL_00f9;
					}
					if ((this.format == RenderTextureFormat.R8 || this.format == RenderTextureFormat.RHalf || this.format == RenderTextureFormat.RFloat) && SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.RGFloat))
					{
						result = RenderTextureFormat.RGFloat;
						goto IL_00f9;
					}
					if ((this.format == RenderTextureFormat.R8 || this.format == RenderTextureFormat.RHalf || this.format == RenderTextureFormat.RFloat || this.format == RenderTextureFormat.RGFloat) && SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGBFloat))
					{
						result = RenderTextureFormat.ARGBFloat;
						goto IL_00f9;
					}
					result = this.format;
				}
				goto IL_00f9;
				IL_00f9:
				return result;
			}
		}
	}
}
