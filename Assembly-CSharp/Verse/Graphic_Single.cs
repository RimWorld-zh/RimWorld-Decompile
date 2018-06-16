using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000DE3 RID: 3555
	public class Graphic_Single : Graphic
	{
		// Token: 0x17000CE4 RID: 3300
		// (get) Token: 0x06004F82 RID: 20354 RVA: 0x0029426C File Offset: 0x0029266C
		public override Material MatSingle
		{
			get
			{
				return this.mat;
			}
		}

		// Token: 0x17000CE5 RID: 3301
		// (get) Token: 0x06004F83 RID: 20355 RVA: 0x00294288 File Offset: 0x00292688
		public override Material MatWest
		{
			get
			{
				return this.mat;
			}
		}

		// Token: 0x17000CE6 RID: 3302
		// (get) Token: 0x06004F84 RID: 20356 RVA: 0x002942A4 File Offset: 0x002926A4
		public override Material MatSouth
		{
			get
			{
				return this.mat;
			}
		}

		// Token: 0x17000CE7 RID: 3303
		// (get) Token: 0x06004F85 RID: 20357 RVA: 0x002942C0 File Offset: 0x002926C0
		public override Material MatEast
		{
			get
			{
				return this.mat;
			}
		}

		// Token: 0x17000CE8 RID: 3304
		// (get) Token: 0x06004F86 RID: 20358 RVA: 0x002942DC File Offset: 0x002926DC
		public override Material MatNorth
		{
			get
			{
				return this.mat;
			}
		}

		// Token: 0x17000CE9 RID: 3305
		// (get) Token: 0x06004F87 RID: 20359 RVA: 0x002942F8 File Offset: 0x002926F8
		public override bool ShouldDrawRotated
		{
			get
			{
				return this.data == null || this.data.drawRotated;
			}
		}

		// Token: 0x06004F88 RID: 20360 RVA: 0x00294330 File Offset: 0x00292730
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

		// Token: 0x06004F89 RID: 20361 RVA: 0x0029441C File Offset: 0x0029281C
		public override Graphic GetColoredVersion(Shader newShader, Color newColor, Color newColorTwo)
		{
			return GraphicDatabase.Get<Graphic_Single>(this.path, newShader, this.drawSize, newColor, newColorTwo, this.data);
		}

		// Token: 0x06004F8A RID: 20362 RVA: 0x0029444C File Offset: 0x0029284C
		public override Material MatAt(Rot4 rot, Thing thing = null)
		{
			return this.mat;
		}

		// Token: 0x06004F8B RID: 20363 RVA: 0x00294468 File Offset: 0x00292868
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

		// Token: 0x040034C2 RID: 13506
		protected Material mat = null;

		// Token: 0x040034C3 RID: 13507
		public static readonly string MaskSuffix = "_m";
	}
}
