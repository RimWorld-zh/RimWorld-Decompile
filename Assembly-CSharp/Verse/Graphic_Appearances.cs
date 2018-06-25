using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000DD5 RID: 3541
	public class Graphic_Appearances : Graphic
	{
		// Token: 0x040034B7 RID: 13495
		protected Graphic[] subGraphics;

		// Token: 0x17000CD2 RID: 3282
		// (get) Token: 0x06004F4B RID: 20299 RVA: 0x002952E8 File Offset: 0x002936E8
		public override Material MatSingle
		{
			get
			{
				return this.subGraphics[(int)StuffAppearanceDefOf.Smooth.index].MatSingle;
			}
		}

		// Token: 0x06004F4C RID: 20300 RVA: 0x00295314 File Offset: 0x00293714
		public override void Init(GraphicRequest req)
		{
			this.data = req.graphicData;
			this.path = req.path;
			this.color = req.color;
			this.drawSize = req.drawSize;
			List<StuffAppearanceDef> allDefsListForReading = DefDatabase<StuffAppearanceDef>.AllDefsListForReading;
			this.subGraphics = new Graphic[allDefsListForReading.Count];
			for (int i = 0; i < this.subGraphics.Length; i++)
			{
				StuffAppearanceDef stuffAppearance = allDefsListForReading[i];
				string text = req.path;
				if (!stuffAppearance.pathPrefix.NullOrEmpty())
				{
					text = stuffAppearance.pathPrefix + "/" + text.Split(new char[]
					{
						'/'
					}).Last<string>();
				}
				Texture2D texture2D = (from x in ContentFinder<Texture2D>.GetAllInFolder(text)
				where x.name.EndsWith(stuffAppearance.defName)
				select x).FirstOrDefault<Texture2D>();
				if (texture2D != null)
				{
					this.subGraphics[i] = GraphicDatabase.Get<Graphic_Single>(text + "/" + texture2D.name, req.shader, this.drawSize, this.color);
				}
			}
			for (int j = 0; j < this.subGraphics.Length; j++)
			{
				if (this.subGraphics[j] == null)
				{
					this.subGraphics[j] = this.subGraphics[(int)StuffAppearanceDefOf.Smooth.index];
				}
			}
		}

		// Token: 0x06004F4D RID: 20301 RVA: 0x00295488 File Offset: 0x00293888
		public override Graphic GetColoredVersion(Shader newShader, Color newColor, Color newColorTwo)
		{
			if (newColorTwo != Color.white)
			{
				Log.ErrorOnce("Cannot use Graphic_Appearances.GetColoredVersion with a non-white colorTwo.", 9910251, false);
			}
			return GraphicDatabase.Get<Graphic_Appearances>(this.path, newShader, this.drawSize, newColor, Color.white, this.data);
		}

		// Token: 0x06004F4E RID: 20302 RVA: 0x002954DC File Offset: 0x002938DC
		public override Material MatSingleFor(Thing thing)
		{
			StuffAppearanceDef stuffAppearanceDef = StuffAppearanceDefOf.Smooth;
			if (thing != null && thing.Stuff != null && thing.Stuff.stuffProps.appearance != null)
			{
				stuffAppearanceDef = thing.Stuff.stuffProps.appearance;
			}
			Graphic graphic = this.subGraphics[(int)stuffAppearanceDef.index];
			return graphic.MatSingleFor(thing);
		}

		// Token: 0x06004F4F RID: 20303 RVA: 0x00295544 File Offset: 0x00293944
		public override void DrawWorker(Vector3 loc, Rot4 rot, ThingDef thingDef, Thing thing, float extraRotation)
		{
			StuffAppearanceDef stuffAppearanceDef = StuffAppearanceDefOf.Smooth;
			if (thing != null && thing.Stuff != null && thing.Stuff.stuffProps.appearance != null)
			{
				stuffAppearanceDef = thing.Stuff.stuffProps.appearance;
			}
			Graphic graphic = this.subGraphics[(int)stuffAppearanceDef.index];
			graphic.DrawWorker(loc, rot, thingDef, thing, extraRotation);
		}

		// Token: 0x06004F50 RID: 20304 RVA: 0x002955B0 File Offset: 0x002939B0
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"Appearance(path=",
				this.path,
				", color=",
				this.color,
				", colorTwo=unsupported)"
			});
		}
	}
}
