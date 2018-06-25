using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000CF1 RID: 3313
	public class PawnGraphicSet
	{
		// Token: 0x0400316C RID: 12652
		public Pawn pawn;

		// Token: 0x0400316D RID: 12653
		public Graphic nakedGraphic = null;

		// Token: 0x0400316E RID: 12654
		public Graphic rottingGraphic = null;

		// Token: 0x0400316F RID: 12655
		public Graphic dessicatedGraphic = null;

		// Token: 0x04003170 RID: 12656
		public Graphic packGraphic = null;

		// Token: 0x04003171 RID: 12657
		public DamageFlasher flasher;

		// Token: 0x04003172 RID: 12658
		public Graphic headGraphic = null;

		// Token: 0x04003173 RID: 12659
		public Graphic desiccatedHeadGraphic = null;

		// Token: 0x04003174 RID: 12660
		public Graphic skullGraphic = null;

		// Token: 0x04003175 RID: 12661
		public Graphic headStumpGraphic = null;

		// Token: 0x04003176 RID: 12662
		public Graphic desiccatedHeadStumpGraphic = null;

		// Token: 0x04003177 RID: 12663
		public Graphic hairGraphic = null;

		// Token: 0x04003178 RID: 12664
		public List<ApparelGraphicRecord> apparelGraphics = new List<ApparelGraphicRecord>();

		// Token: 0x04003179 RID: 12665
		private List<Material> cachedMatsBodyBase = new List<Material>();

		// Token: 0x0400317A RID: 12666
		private int cachedMatsBodyBaseHash = -1;

		// Token: 0x0400317B RID: 12667
		public static readonly Color RottingColor = new Color(0.34f, 0.32f, 0.3f);

		// Token: 0x060048E7 RID: 18663 RVA: 0x002642FC File Offset: 0x002626FC
		public PawnGraphicSet(Pawn pawn)
		{
			this.pawn = pawn;
			this.flasher = new DamageFlasher(pawn);
		}

		// Token: 0x17000B83 RID: 2947
		// (get) Token: 0x060048E8 RID: 18664 RVA: 0x00264388 File Offset: 0x00262788
		public bool AllResolved
		{
			get
			{
				return this.nakedGraphic != null;
			}
		}

		// Token: 0x060048E9 RID: 18665 RVA: 0x002643AC File Offset: 0x002627AC
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
		// (get) Token: 0x060048EA RID: 18666 RVA: 0x0026450C File Offset: 0x0026290C
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

		// Token: 0x060048EB RID: 18667 RVA: 0x00264590 File Offset: 0x00262990
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

		// Token: 0x060048EC RID: 18668 RVA: 0x00264648 File Offset: 0x00262A48
		public Material HairMatAt(Rot4 facing)
		{
			Material baseMat = this.hairGraphic.MatAt(facing, null);
			return this.flasher.GetDamagedMat(baseMat);
		}

		// Token: 0x060048ED RID: 18669 RVA: 0x00264677 File Offset: 0x00262A77
		public void ClearCache()
		{
			this.cachedMatsBodyBaseHash = -1;
		}

		// Token: 0x060048EE RID: 18670 RVA: 0x00264684 File Offset: 0x00262A84
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

		// Token: 0x060048EF RID: 18671 RVA: 0x002648C4 File Offset: 0x00262CC4
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
	}
}
