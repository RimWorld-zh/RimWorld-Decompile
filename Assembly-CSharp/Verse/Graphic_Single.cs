using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000DDF RID: 3551
	public class Graphic_Single : Graphic
	{
		// Token: 0x040034CB RID: 13515
		protected Material mat = null;

		// Token: 0x040034CC RID: 13516
		public static readonly string MaskSuffix = "_m";

		// Token: 0x17000CE5 RID: 3301
		// (get) Token: 0x06004F95 RID: 20373 RVA: 0x00295828 File Offset: 0x00293C28
		public override Material MatSingle
		{
			get
			{
				return this.mat;
			}
		}

		// Token: 0x17000CE6 RID: 3302
		// (get) Token: 0x06004F96 RID: 20374 RVA: 0x00295844 File Offset: 0x00293C44
		public override Material MatWest
		{
			get
			{
				return this.mat;
			}
		}

		// Token: 0x17000CE7 RID: 3303
		// (get) Token: 0x06004F97 RID: 20375 RVA: 0x00295860 File Offset: 0x00293C60
		public override Material MatSouth
		{
			get
			{
				return this.mat;
			}
		}

		// Token: 0x17000CE8 RID: 3304
		// (get) Token: 0x06004F98 RID: 20376 RVA: 0x0029587C File Offset: 0x00293C7C
		public override Material MatEast
		{
			get
			{
				return this.mat;
			}
		}

		// Token: 0x17000CE9 RID: 3305
		// (get) Token: 0x06004F99 RID: 20377 RVA: 0x00295898 File Offset: 0x00293C98
		public override Material MatNorth
		{
			get
			{
				return this.mat;
			}
		}

		// Token: 0x17000CEA RID: 3306
		// (get) Token: 0x06004F9A RID: 20378 RVA: 0x002958B4 File Offset: 0x00293CB4
		public override bool ShouldDrawRotated
		{
			get
			{
				return this.data == null || this.data.drawRotated;
			}
		}

		// Token: 0x06004F9B RID: 20379 RVA: 0x002958EC File Offset: 0x00293CEC
		public override void Init(GraphicRequest req)
		{
			this.data = req.graphicData;
			this.path = req.path;
			this.color = req.color;
			this.colorTwo = req.colorTwo;
			this.drawSize = req.drawSize;
			MaterialRequest req2 = default(MaterialRequest);
			req2.mainTex = ContentFinder<Texture2D>.Get(req.path, true);
			req2.shader = req.shader;
			req2.color = this.color;
			req2.colorTwo = this.colorTwo;
			req2.renderQueue = req.renderQueue;
			req2.shaderParameters = req.shaderParameters;
			if (req.shader.SupportsMaskTex())
			{
				req2.maskTex = ContentFinder<Texture2D>.Get(req.path + Graphic_Single.MaskSuffix, false);
			}
			this.mat = MaterialPool.MatFrom(req2);
		}

		// Token: 0x06004F9C RID: 20380 RVA: 0x002959D8 File Offset: 0x00293DD8
		public override Graphic GetColoredVersion(Shader newShader, Color newColor, Color newColorTwo)
		{
			return GraphicDatabase.Get<Graphic_Single>(this.path, newShader, this.drawSize, newColor, newColorTwo, this.data);
		}

		// Token: 0x06004F9D RID: 20381 RVA: 0x00295A08 File Offset: 0x00293E08
		public override Material MatAt(Rot4 rot, Thing thing = null)
		{
			return this.mat;
		}

		// Token: 0x06004F9E RID: 20382 RVA: 0x00295A24 File Offset: 0x00293E24
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"Single(path=",
				this.path,
				", color=",
				this.color,
				", colorTwo=",
				this.colorTwo,
				")"
			});
		}
	}
}
