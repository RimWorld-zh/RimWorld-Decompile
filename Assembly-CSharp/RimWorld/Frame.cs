using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using Verse.AI.Group;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x0200067A RID: 1658
	[StaticConstructorOnStartup]
	public class Frame : Building, IThingHolder, IConstructible
	{
		// Token: 0x04001397 RID: 5015
		public ThingOwner resourceContainer;

		// Token: 0x04001398 RID: 5016
		public float workDone;

		// Token: 0x04001399 RID: 5017
		private Material cachedCornerMat;

		// Token: 0x0400139A RID: 5018
		private Material cachedTileMat;

		// Token: 0x0400139B RID: 5019
		protected const float UnderfieldOverdrawFactor = 1.15f;

		// Token: 0x0400139C RID: 5020
		protected const float CenterOverdrawFactor = 0.5f;

		// Token: 0x0400139D RID: 5021
		private const int LongConstructionProjectThreshold = 10000;

		// Token: 0x0400139E RID: 5022
		private static readonly Material UnderfieldMat = MaterialPool.MatFrom("Things/Building/BuildingFrame/Underfield", ShaderDatabase.Transparent);

		// Token: 0x0400139F RID: 5023
		private static readonly Texture2D CornerTex = ContentFinder<Texture2D>.Get("Things/Building/BuildingFrame/Corner", true);

		// Token: 0x040013A0 RID: 5024
		private static readonly Texture2D TileTex = ContentFinder<Texture2D>.Get("Things/Building/BuildingFrame/Tile", true);

		// Token: 0x040013A1 RID: 5025
		[TweakValue("Pathfinding", 0f, 1000f)]
		public static ushort AvoidUnderConstructionPathFindCost = 800;

		// Token: 0x040013A2 RID: 5026
		private List<ThingDefCountClass> cachedMaterialsNeeded = new List<ThingDefCountClass>();

		// Token: 0x060022D6 RID: 8918 RVA: 0x0012C18A File Offset: 0x0012A58A
		public Frame()
		{
			this.resourceContainer = new ThingOwner<Thing>(this, false, LookMode.Deep);
		}

		// Token: 0x1700051B RID: 1307
		// (get) Token: 0x060022D7 RID: 8919 RVA: 0x0012C1AC File Offset: 0x0012A5AC
		public float WorkToMake
		{
			get
			{
				return this.def.entityDefToBuild.GetStatValueAbstract(StatDefOf.WorkToBuild, base.Stuff);
			}
		}

		// Token: 0x1700051C RID: 1308
		// (get) Token: 0x060022D8 RID: 8920 RVA: 0x0012C1DC File Offset: 0x0012A5DC
		public float WorkLeft
		{
			get
			{
				return this.WorkToMake - this.workDone;
			}
		}

		// Token: 0x1700051D RID: 1309
		// (get) Token: 0x060022D9 RID: 8921 RVA: 0x0012C200 File Offset: 0x0012A600
		public float PercentComplete
		{
			get
			{
				return this.workDone / this.WorkToMake;
			}
		}

		// Token: 0x1700051E RID: 1310
		// (get) Token: 0x060022DA RID: 8922 RVA: 0x0012C224 File Offset: 0x0012A624
		public override string Label
		{
			get
			{
				return this.LabelEntityToBuild + "FrameLabelExtra".Translate();
			}
		}

		// Token: 0x1700051F RID: 1311
		// (get) Token: 0x060022DB RID: 8923 RVA: 0x0012C250 File Offset: 0x0012A650
		public string LabelEntityToBuild
		{
			get
			{
				string label = this.def.entityDefToBuild.label;
				string result;
				if (base.Stuff != null)
				{
					result = base.Stuff.label + " " + label;
				}
				else
				{
					result = label;
				}
				return result;
			}
		}

		// Token: 0x17000520 RID: 1312
		// (get) Token: 0x060022DC RID: 8924 RVA: 0x0012C2A0 File Offset: 0x0012A6A0
		public override Color DrawColor
		{
			get
			{
				Color result;
				if (!this.def.MadeFromStuff)
				{
					List<ThingDefCountClass> costList = this.def.entityDefToBuild.costList;
					if (costList != null)
					{
						for (int i = 0; i < costList.Count; i++)
						{
							ThingDef thingDef = costList[i].thingDef;
							if (thingDef.IsStuff && thingDef.stuffProps.color != Color.white)
							{
								return thingDef.stuffProps.color;
							}
						}
					}
					result = new Color(0.6f, 0.6f, 0.6f);
				}
				else
				{
					result = base.DrawColor;
				}
				return result;
			}
		}

		// Token: 0x17000521 RID: 1313
		// (get) Token: 0x060022DD RID: 8925 RVA: 0x0012C35C File Offset: 0x0012A75C
		public EffecterDef ConstructionEffect
		{
			get
			{
				EffecterDef result;
				if (base.Stuff != null && base.Stuff.stuffProps.constructEffect != null)
				{
					result = base.Stuff.stuffProps.constructEffect;
				}
				else if (this.def.entityDefToBuild.constructEffect != null)
				{
					result = this.def.entityDefToBuild.constructEffect;
				}
				else
				{
					result = EffecterDefOf.ConstructMetal;
				}
				return result;
			}
		}

		// Token: 0x17000522 RID: 1314
		// (get) Token: 0x060022DE RID: 8926 RVA: 0x0012C3D8 File Offset: 0x0012A7D8
		private Material CornerMat
		{
			get
			{
				if (this.cachedCornerMat == null)
				{
					this.cachedCornerMat = MaterialPool.MatFrom(Frame.CornerTex, ShaderDatabase.Cutout, this.DrawColor);
				}
				return this.cachedCornerMat;
			}
		}

		// Token: 0x17000523 RID: 1315
		// (get) Token: 0x060022DF RID: 8927 RVA: 0x0012C424 File Offset: 0x0012A824
		private Material TileMat
		{
			get
			{
				if (this.cachedTileMat == null)
				{
					this.cachedTileMat = MaterialPool.MatFrom(Frame.TileTex, ShaderDatabase.Cutout, this.DrawColor);
				}
				return this.cachedTileMat;
			}
		}

		// Token: 0x060022E0 RID: 8928 RVA: 0x0012C470 File Offset: 0x0012A870
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.resourceContainer;
		}

		// Token: 0x060022E1 RID: 8929 RVA: 0x0012C48B File Offset: 0x0012A88B
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x060022E2 RID: 8930 RVA: 0x0012C49A File Offset: 0x0012A89A
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.workDone, "workDone", 0f, false);
			Scribe_Deep.Look<ThingOwner>(ref this.resourceContainer, "resourceContainer", new object[]
			{
				this
			});
		}

		// Token: 0x060022E3 RID: 8931 RVA: 0x0012C4D4 File Offset: 0x0012A8D4
		public ThingDef UIStuff()
		{
			return base.Stuff;
		}

		// Token: 0x060022E4 RID: 8932 RVA: 0x0012C4F0 File Offset: 0x0012A8F0
		public List<ThingDefCountClass> MaterialsNeeded()
		{
			this.cachedMaterialsNeeded.Clear();
			List<ThingDefCountClass> list = this.def.entityDefToBuild.CostListAdjusted(base.Stuff, true);
			for (int i = 0; i < list.Count; i++)
			{
				ThingDefCountClass thingDefCountClass = list[i];
				int num = this.resourceContainer.TotalStackCountOfDef(thingDefCountClass.thingDef);
				int num2 = thingDefCountClass.count - num;
				if (num2 > 0)
				{
					this.cachedMaterialsNeeded.Add(new ThingDefCountClass(thingDefCountClass.thingDef, num2));
				}
			}
			return this.cachedMaterialsNeeded;
		}

		// Token: 0x060022E5 RID: 8933 RVA: 0x0012C590 File Offset: 0x0012A990
		public void CompleteConstruction(Pawn worker)
		{
			this.resourceContainer.ClearAndDestroyContents(DestroyMode.Vanish);
			Map map = base.Map;
			this.Destroy(DestroyMode.Vanish);
			if (this.GetStatValue(StatDefOf.WorkToBuild, true) > 150f && this.def.entityDefToBuild is ThingDef && ((ThingDef)this.def.entityDefToBuild).category == ThingCategory.Building)
			{
				SoundDefOf.Building_Complete.PlayOneShot(new TargetInfo(base.Position, map, false));
			}
			ThingDef thingDef = this.def.entityDefToBuild as ThingDef;
			Thing thing = null;
			if (thingDef != null)
			{
				thing = ThingMaker.MakeThing(thingDef, base.Stuff);
				thing.SetFactionDirect(base.Faction);
				CompQuality compQuality = thing.TryGetComp<CompQuality>();
				if (compQuality != null)
				{
					QualityCategory q = QualityUtility.GenerateQualityCreatedByPawn(worker, SkillDefOf.Construction);
					compQuality.SetQuality(q, ArtGenerationContext.Colony);
					QualityUtility.SendCraftNotification(thing, worker);
				}
				CompArt compArt = thing.TryGetComp<CompArt>();
				if (compArt != null)
				{
					if (compQuality == null)
					{
						compArt.InitializeArt(ArtGenerationContext.Colony);
					}
					compArt.JustCreatedBy(worker);
				}
				thing.HitPoints = Mathf.CeilToInt((float)this.HitPoints / (float)base.MaxHitPoints * (float)thing.MaxHitPoints);
				GenSpawn.Spawn(thing, base.Position, map, base.Rotation, WipeMode.FullRefund, false);
			}
			else
			{
				map.terrainGrid.SetTerrain(base.Position, (TerrainDef)this.def.entityDefToBuild);
				FilthMaker.RemoveAllFilth(base.Position, map);
			}
			worker.records.Increment(RecordDefOf.ThingsConstructed);
			if (thing != null && thing.GetStatValue(StatDefOf.WorkToBuild, true) >= 10000f)
			{
				TaleRecorder.RecordTale(TaleDefOf.CompletedLongConstructionProject, new object[]
				{
					worker,
					thing.def
				});
			}
		}

		// Token: 0x060022E6 RID: 8934 RVA: 0x0012C758 File Offset: 0x0012AB58
		public void FailConstruction(Pawn worker)
		{
			Map map = base.Map;
			this.Destroy(DestroyMode.FailConstruction);
			Blueprint_Build blueprint_Build = null;
			if (this.def.entityDefToBuild.blueprintDef != null)
			{
				blueprint_Build = (Blueprint_Build)ThingMaker.MakeThing(this.def.entityDefToBuild.blueprintDef, null);
				blueprint_Build.stuffToUse = base.Stuff;
				blueprint_Build.SetFactionDirect(base.Faction);
				GenSpawn.Spawn(blueprint_Build, base.Position, map, base.Rotation, WipeMode.FullRefund, false);
			}
			Lord lord = worker.GetLord();
			if (lord != null)
			{
				lord.Notify_ConstructionFailed(worker, this, blueprint_Build);
			}
			MoteMaker.ThrowText(this.DrawPos, map, "TextMote_ConstructionFail".Translate(), 6f);
			if (base.Faction == Faction.OfPlayer && this.WorkToMake > 1400f)
			{
				Messages.Message("MessageConstructionFailed".Translate(new object[]
				{
					this.LabelEntityToBuild,
					worker.LabelShort
				}), new TargetInfo(base.Position, map, false), MessageTypeDefOf.NegativeEvent, true);
			}
		}

		// Token: 0x060022E7 RID: 8935 RVA: 0x0012C86C File Offset: 0x0012AC6C
		public override void Draw()
		{
			Vector2 vector = new Vector2((float)this.def.size.x, (float)this.def.size.z);
			vector.x *= 1.15f;
			vector.y *= 1.15f;
			Vector3 s = new Vector3(vector.x, 1f, vector.y);
			Matrix4x4 matrix = default(Matrix4x4);
			matrix.SetTRS(this.DrawPos, base.Rotation.AsQuat, s);
			Graphics.DrawMesh(MeshPool.plane10, matrix, Frame.UnderfieldMat, 0);
			int num = 4;
			for (int i = 0; i < num; i++)
			{
				float num2 = (float)Mathf.Min(base.RotatedSize.x, base.RotatedSize.z);
				float num3 = num2 * 0.38f;
				IntVec3 intVec = default(IntVec3);
				if (i == 0)
				{
					intVec = new IntVec3(-1, 0, -1);
				}
				else if (i == 1)
				{
					intVec = new IntVec3(-1, 0, 1);
				}
				else if (i == 2)
				{
					intVec = new IntVec3(1, 0, 1);
				}
				else if (i == 3)
				{
					intVec = new IntVec3(1, 0, -1);
				}
				Vector3 b = default(Vector3);
				b.x = (float)intVec.x * ((float)base.RotatedSize.x / 2f - num3 / 2f);
				b.z = (float)intVec.z * ((float)base.RotatedSize.z / 2f - num3 / 2f);
				Vector3 s2 = new Vector3(num3, 1f, num3);
				Matrix4x4 matrix2 = default(Matrix4x4);
				Vector3 pos = this.DrawPos + Vector3.up * 0.03f + b;
				Rot4 rot = new Rot4(i);
				matrix2.SetTRS(pos, rot.AsQuat, s2);
				Graphics.DrawMesh(MeshPool.plane10, matrix2, this.CornerMat, 0);
			}
			float num4 = this.PercentComplete / 1f;
			int num5 = Mathf.CeilToInt(num4 * (float)base.RotatedSize.x * (float)base.RotatedSize.z * 4f);
			IntVec2 intVec2 = base.RotatedSize * 2;
			for (int j = 0; j < num5; j++)
			{
				IntVec2 intVec3 = default(IntVec2);
				intVec3.z = j / intVec2.x;
				intVec3.x = j - intVec3.z * intVec2.x;
				Vector3 a = new Vector3((float)intVec3.x * 0.5f, 0f, (float)intVec3.z * 0.5f) + this.DrawPos;
				a.x -= (float)base.RotatedSize.x * 0.5f - 0.25f;
				a.z -= (float)base.RotatedSize.z * 0.5f - 0.25f;
				Vector3 s3 = new Vector3(0.5f, 1f, 0.5f);
				Matrix4x4 matrix3 = default(Matrix4x4);
				matrix3.SetTRS(a + Vector3.up * 0.02f, Quaternion.identity, s3);
				Graphics.DrawMesh(MeshPool.plane10, matrix3, this.TileMat, 0);
			}
			base.Comps_PostDraw();
		}

		// Token: 0x060022E8 RID: 8936 RVA: 0x0012CC0C File Offset: 0x0012B00C
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo c in this.<GetGizmos>__BaseCallProxy0())
			{
				yield return c;
			}
			Command buildCopy = BuildCopyCommandUtility.BuildCopyCommand(this.def.entityDefToBuild, base.Stuff);
			if (buildCopy != null)
			{
				yield return buildCopy;
			}
			if (base.Faction == Faction.OfPlayer)
			{
				foreach (Command facility in BuildFacilityCommandUtility.BuildFacilityCommands(this.def.entityDefToBuild))
				{
					yield return facility;
				}
			}
			yield break;
		}

		// Token: 0x060022E9 RID: 8937 RVA: 0x0012CC38 File Offset: 0x0012B038
		public override string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(base.GetInspectString());
			stringBuilder.AppendLine("ContainedResources".Translate() + ":");
			List<ThingDefCountClass> list = this.def.entityDefToBuild.CostListAdjusted(base.Stuff, true);
			for (int i = 0; i < list.Count; i++)
			{
				ThingDefCountClass need = list[i];
				int num = need.count;
				foreach (ThingDefCountClass thingDefCountClass in from needed in this.MaterialsNeeded()
				where needed.thingDef == need.thingDef
				select needed)
				{
					num -= thingDefCountClass.count;
				}
				stringBuilder.AppendLine(string.Concat(new object[]
				{
					need.thingDef.LabelCap,
					": ",
					num,
					" / ",
					need.count
				}));
			}
			stringBuilder.Append("WorkLeft".Translate() + ": " + this.WorkLeft.ToStringWorkAmount());
			return stringBuilder.ToString();
		}

		// Token: 0x060022EA RID: 8938 RVA: 0x0012CDB8 File Offset: 0x0012B1B8
		public override ushort PathFindCostFor(Pawn p)
		{
			ushort result;
			if (base.Faction == null)
			{
				result = 0;
			}
			else if (this.def.entityDefToBuild is TerrainDef)
			{
				result = 0;
			}
			else if (p.Faction == base.Faction || p.HostFaction == base.Faction)
			{
				result = Frame.AvoidUnderConstructionPathFindCost;
			}
			else
			{
				result = 0;
			}
			return result;
		}
	}
}
