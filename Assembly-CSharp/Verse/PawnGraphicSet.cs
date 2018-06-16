using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000CF2 RID: 3314
	public class PawnGraphicSet
	{
		// Token: 0x060048D5 RID: 18645 RVA: 0x00262B50 File Offset: 0x00260F50
		public PawnGraphicSet(Pawn pawn)
		{
			this.pawn = pawn;
			this.flasher = new DamageFlasher(pawn);
		}

		// Token: 0x17000B83 RID: 2947
		// (get) Token: 0x060048D6 RID: 18646 RVA: 0x00262BDC File Offset: 0x00260FDC
		public bool AllResolved
		{
			get
			{
				return this.nakedGraphic != null;
			}
		}

		// Token: 0x060048D7 RID: 18647 RVA: 0x00262C00 File Offset: 0x00261000
		public List<Material> MatsBodyBaseAt(Rot4 facing, RotDrawMode bodyCondition = RotDrawMode.Fresh)
		{
			int num = facing.AsInt + 1000 * (int)bodyCondition;
			if (num != this.cachedMatsBodyBaseHash)
			{
				this.cachedMatsBodyBase.Clear();
				this.cachedMatsBodyBaseHash = num;
				if (bodyCondition == RotDrawMode.Fresh)
				{
					this.cachedMatsBodyBase.Add(this.nakedGraphic.MatAt(facing, null));
				}
				else if (bodyCondition == RotDrawMode.Rotting || this.dessicatedGraphic == null)
				{
					this.cachedMatsBodyBase.Add(this.rottingGraphic.MatAt(facing, null));
				}
				else if (bodyCondition == RotDrawMode.Dessicated)
				{
					this.cachedMatsBodyBase.Add(this.dessicatedGraphic.MatAt(facing, null));
				}
				for (int i = 0; i < this.apparelGraphics.Count; i++)
				{
					if (this.apparelGraphics[i].sourceApparel.def.apparel.LastLayer != ApparelLayerDefOf.Shell && this.apparelGraphics[i].sourceApparel.def.apparel.LastLayer != ApparelLayerDefOf.Overhead)
					{
						this.cachedMatsBodyBase.Add(this.apparelGraphics[i].graphic.MatAt(facing, null));
					}
				}
			}
			return this.cachedMatsBodyBase;
		}

		// Token: 0x17000B84 RID: 2948
		// (get) Token: 0x060048D8 RID: 18648 RVA: 0x00262D60 File Offset: 0x00261160
		public GraphicMeshSet HairMeshSet
		{
			get
			{
				GraphicMeshSet result;
				if (this.pawn.story.crownType == CrownType.Average)
				{
					result = MeshPool.humanlikeHairSetAverage;
				}
				else if (this.pawn.story.crownType == CrownType.Narrow)
				{
					result = MeshPool.humanlikeHairSetNarrow;
				}
				else
				{
					Log.Error("Unknown crown type: " + this.pawn.story.crownType, false);
					result = MeshPool.humanlikeHairSetAverage;
				}
				return result;
			}
		}

		// Token: 0x060048D9 RID: 18649 RVA: 0x00262DE4 File Offset: 0x002611E4
		public Material HeadMatAt(Rot4 facing, RotDrawMode bodyCondition = RotDrawMode.Fresh, bool stump = false)
		{
			Material material = null;
			if (bodyCondition == RotDrawMode.Fresh)
			{
				if (stump)
				{
					material = this.headStumpGraphic.MatAt(facing, null);
				}
				else
				{
					material = this.headGraphic.MatAt(facing, null);
				}
			}
			else if (bodyCondition == RotDrawMode.Rotting)
			{
				if (stump)
				{
					material = this.desiccatedHeadStumpGraphic.MatAt(facing, null);
				}
				else
				{
					material = this.desiccatedHeadGraphic.MatAt(facing, null);
				}
			}
			else if (bodyCondition == RotDrawMode.Dessicated && !stump)
			{
				material = this.skullGraphic.MatAt(facing, null);
			}
			if (material != null)
			{
				material = this.flasher.GetDamagedMat(material);
			}
			return material;
		}

		// Token: 0x060048DA RID: 18650 RVA: 0x00262E9C File Offset: 0x0026129C
		public Material HairMatAt(Rot4 facing)
		{
			Material baseMat = this.hairGraphic.MatAt(facing, null);
			return this.flasher.GetDamagedMat(baseMat);
		}

		// Token: 0x060048DB RID: 18651 RVA: 0x00262ECB File Offset: 0x002612CB
		public void ClearCache()
		{
			this.cachedMatsBodyBaseHash = -1;
		}

		// Token: 0x060048DC RID: 18652 RVA: 0x00262ED8 File Offset: 0x002612D8
		public void ResolveAllGraphics()
		{
			this.ClearCache();
			if (this.pawn.RaceProps.Humanlike)
			{
				this.nakedGraphic = GraphicGetter_NakedHumanlike.GetNakedBodyGraphic(this.pawn.story.bodyType, ShaderDatabase.CutoutSkin, this.pawn.story.SkinColor);
				this.rottingGraphic = GraphicGetter_NakedHumanlike.GetNakedBodyGraphic(this.pawn.story.bodyType, ShaderDatabase.CutoutSkin, PawnGraphicSet.RottingColor);
				this.dessicatedGraphic = GraphicDatabase.Get<Graphic_Multi>("Things/Pawn/Humanlike/HumanoidDessicated", ShaderDatabase.Cutout);
				this.headGraphic = GraphicDatabaseHeadRecords.GetHeadNamed(this.pawn.story.HeadGraphicPath, this.pawn.story.SkinColor);
				this.desiccatedHeadGraphic = GraphicDatabaseHeadRecords.GetHeadNamed(this.pawn.story.HeadGraphicPath, PawnGraphicSet.RottingColor);
				this.skullGraphic = GraphicDatabaseHeadRecords.GetSkull();
				this.headStumpGraphic = GraphicDatabaseHeadRecords.GetStump(this.pawn.story.SkinColor);
				this.desiccatedHeadStumpGraphic = GraphicDatabaseHeadRecords.GetStump(PawnGraphicSet.RottingColor);
				this.hairGraphic = GraphicDatabase.Get<Graphic_Multi>(this.pawn.story.hairDef.texPath, ShaderDatabase.Cutout, Vector2.one, this.pawn.story.hairColor);
				this.ResolveApparelGraphics();
			}
			else
			{
				PawnKindLifeStage curKindLifeStage = this.pawn.ageTracker.CurKindLifeStage;
				if (this.pawn.gender != Gender.Female || curKindLifeStage.femaleGraphicData == null)
				{
					this.nakedGraphic = curKindLifeStage.bodyGraphicData.Graphic;
				}
				else
				{
					this.nakedGraphic = curKindLifeStage.femaleGraphicData.Graphic;
				}
				this.rottingGraphic = this.nakedGraphic.GetColoredVersion(ShaderDatabase.CutoutSkin, PawnGraphicSet.RottingColor, PawnGraphicSet.RottingColor);
				if (this.pawn.RaceProps.packAnimal)
				{
					this.packGraphic = GraphicDatabase.Get<Graphic_Multi>(this.nakedGraphic.path + "Pack", ShaderDatabase.Cutout, this.nakedGraphic.drawSize, Color.white);
				}
				if (curKindLifeStage.dessicatedBodyGraphicData != null)
				{
					this.dessicatedGraphic = curKindLifeStage.dessicatedBodyGraphicData.GraphicColoredFor(this.pawn);
				}
			}
		}

		// Token: 0x060048DD RID: 18653 RVA: 0x00263118 File Offset: 0x00261518
		public void ResolveApparelGraphics()
		{
			this.ClearCache();
			this.apparelGraphics.Clear();
			foreach (Apparel apparel in this.pawn.apparel.WornApparel)
			{
				ApparelGraphicRecord item;
				if (ApparelGraphicRecordGetter.TryGetGraphicApparel(apparel, this.pawn.story.bodyType, out item))
				{
					this.apparelGraphics.Add(item);
				}
			}
		}

		// Token: 0x0400315C RID: 12636
		public Pawn pawn;

		// Token: 0x0400315D RID: 12637
		public Graphic nakedGraphic = null;

		// Token: 0x0400315E RID: 12638
		public Graphic rottingGraphic = null;

		// Token: 0x0400315F RID: 12639
		public Graphic dessicatedGraphic = null;

		// Token: 0x04003160 RID: 12640
		public Graphic packGraphic = null;

		// Token: 0x04003161 RID: 12641
		public DamageFlasher flasher;

		// Token: 0x04003162 RID: 12642
		public Graphic headGraphic = null;

		// Token: 0x04003163 RID: 12643
		public Graphic desiccatedHeadGraphic = null;

		// Token: 0x04003164 RID: 12644
		public Graphic skullGraphic = null;

		// Token: 0x04003165 RID: 12645
		public Graphic headStumpGraphic = null;

		// Token: 0x04003166 RID: 12646
		public Graphic desiccatedHeadStumpGraphic = null;

		// Token: 0x04003167 RID: 12647
		public Graphic hairGraphic = null;

		// Token: 0x04003168 RID: 12648
		public List<ApparelGraphicRecord> apparelGraphics = new List<ApparelGraphicRecord>();

		// Token: 0x04003169 RID: 12649
		private List<Material> cachedMatsBodyBase = new List<Material>();

		// Token: 0x0400316A RID: 12650
		private int cachedMatsBodyBaseHash = -1;

		// Token: 0x0400316B RID: 12651
		public static readonly Color RottingColor = new Color(0.34f, 0.32f, 0.3f);
	}
}
