using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000DE1 RID: 3553
	public class Graphic_Single : Graphic
	{
		// Token: 0x040034CB RID: 13515
		protected Material mat = null;

		// Token: 0x040034CC RID: 13516
		public static readonly string MaskSuffix = "_m";

		// Token: 0x17000CE4 RID: 3300
		// (get) Token: 0x06004F99 RID: 20377 RVA: 0x00295954 File Offset: 0x00293D54
		public override Material MatSingle
		{
			get
			{
				return this.mat;
			}
		}

		// Token: 0x17000CE5 RID: 3301
		// (get) Token: 0x06004F9A RID: 20378 RVA: 0x00295970 File Offset: 0x00293D70
		public override Material MatWest
		{
			get
			{
				return this.mat;
			}
		}

		// Token: 0x17000CE6 RID: 3302
		// (get) Token: 0x06004F9B RID: 20379 RVA: 0x0029598C File Offset: 0x00293D8C
		public override Material MatSouth
		{
			get
			{
				return this.mat;
			}
		}

		// Token: 0x17000CE7 RID: 3303
		// (get) Token: 0x06004F9C RID: 20380 RVA: 0x002959A8 File Offset: 0x00293DA8
		public override Material MatEast
		{
			get
			{
				return this.mat;
			}
		}

		// Token: 0x17000CE8 RID: 3304
		// (get) Token: 0x06004F9D RID: 20381 RVA: 0x002959C4 File Offset: 0x00293DC4
		public override Material MatNorth
		{
			get
			{
				return this.mat;
			}
		}

		// Token: 0x17000CE9 RID: 3305
		// (get) Token: 0x06004F9E RID: 20382 RVA: 0x002959E0 File Offset: 0x00293DE0
		public override bool ShouldDrawRotated
		{
			get
			{
				return this.data == null || this.data.drawRotated;
			}
		}

		// Token: 0x06004F9F RID: 20383 RVA: 0x00295A18 File Offset: 0x00293E18
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

		// Token: 0x06004FA0 RID: 20384 RVA: 0x00295B04 File Offset: 0x00293F04
		public override Graphic GetColoredVersion(Shader newShader, Color newColor, Color newColorTwo)
		{
			return GraphicDatabase.Get<Graphic_Single>(this.path, newShader, this.drawSize, newColor, newColorTwo, this.data);
		}

		// Token: 0x06004FA1 RID: 20385 RVA: 0x00295B34 File Offset: 0x00293F34
		public override Material MatAt(Rot4 rot, Thing thing = null)
		{
			return this.mat;
		}

		// Token: 0x06004FA2 RID: 20386 RVA: 0x00295B50 File Offset: 0x00293F50
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
