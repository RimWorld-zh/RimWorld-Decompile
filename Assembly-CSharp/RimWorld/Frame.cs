using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using Verse.AI.Group;
using Verse.Sound;

namespace RimWorld
{
	[StaticConstructorOnStartup]
	public class Frame : Building, IThingHolder, IConstructible
	{
		public ThingOwner resourceContainer;

		public float workDone;

		private Material cachedCornerMat;

		private Material cachedTileMat;

		protected const float UnderfieldOverdrawFactor = 1.15f;

		protected const float CenterOverdrawFactor = 0.5f;

		private const int LongConstructionProjectThreshold = 10000;

		private static readonly Material UnderfieldMat = MaterialPool.MatFrom("Things/Building/BuildingFrame/Underfield", ShaderDatabase.Transparent);

		private static readonly Texture2D CornerTex = ContentFinder<Texture2D>.Get("Things/Building/BuildingFrame/Corner", true);

		private static readonly Texture2D TileTex = ContentFinder<Texture2D>.Get("Things/Building/BuildingFrame/Tile", true);

		private List<ThingCountClass> cachedMaterialsNeeded = new List<ThingCountClass>();

		public float WorkToMake
		{
			get
			{
				return base.def.entityDefToBuild.GetStatValueAbstract(StatDefOf.WorkToBuild, base.Stuff);
			}
		}

		public float WorkLeft
		{
			get
			{
				return this.WorkToMake - this.workDone;
			}
		}

		public float PercentComplete
		{
			get
			{
				return this.workDone / this.WorkToMake;
			}
		}

		public override string Label
		{
			get
			{
				string text = base.def.entityDefToBuild.label + "FrameLabelExtra".Translate();
				if (base.Stuff != null)
				{
					return base.Stuff.label + " " + text;
				}
				return text;
			}
		}

		public override Color DrawColor
		{
			get
			{
				if (!base.def.MadeFromStuff)
				{
					List<ThingCountClass> costList = base.def.entityDefToBuild.costList;
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
					return new Color(0.6f, 0.6f, 0.6f);
				}
				return base.DrawColor;
			}
		}

		public EffecterDef ConstructionEffect
		{
			get
			{
				if (base.Stuff != null && base.Stuff.stuffProps.constructEffect != null)
				{
					return base.Stuff.stuffProps.constructEffect;
				}
				if (base.def.entityDefToBuild.constructEffect != null)
				{
					return base.def.entityDefToBuild.constructEffect;
				}
				return EffecterDefOf.ConstructMetal;
			}
		}

		private Material CornerMat
		{
			get
			{
				if ((Object)this.cachedCornerMat == (Object)null)
				{
					this.cachedCornerMat = MaterialPool.MatFrom(Frame.CornerTex, ShaderDatabase.Cutout, this.DrawColor);
				}
				return this.cachedCornerMat;
			}
		}

		private Material TileMat
		{
			get
			{
				if ((Object)this.cachedTileMat == (Object)null)
				{
					this.cachedTileMat = MaterialPool.MatFrom(Frame.TileTex, ShaderDatabase.Cutout, this.DrawColor);
				}
				return this.cachedTileMat;
			}
		}

		public Frame()
		{
			this.resourceContainer = new ThingOwner<Thing>(this, false, LookMode.Deep);
		}

		public ThingOwner GetDirectlyHeldThings()
		{
			return this.resourceContainer;
		}

		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.workDone, "workDone", 0f, false);
			Scribe_Deep.Look<ThingOwner>(ref this.resourceContainer, "resourceContainer", new object[1]
			{
				this
			});
		}

		public ThingDef UIStuff()
		{
			return base.Stuff;
		}

		public List<ThingCountClass> MaterialsNeeded()
		{
			this.cachedMaterialsNeeded.Clear();
			List<ThingCountClass> list = base.def.entityDefToBuild.CostListAdjusted(base.Stuff, true);
			for (int i = 0; i < list.Count; i++)
			{
				ThingCountClass thingCountClass = list[i];
				int num = this.resourceContainer.TotalStackCountOfDef(thingCountClass.thingDef);
				int num2 = thingCountClass.count - num;
				if (num2 > 0)
				{
					this.cachedMaterialsNeeded.Add(new ThingCountClass(thingCountClass.thingDef, num2));
				}
			}
			return this.cachedMaterialsNeeded;
		}

		public void CompleteConstruction(Pawn worker)
		{
			this.resourceContainer.ClearAndDestroyContents(DestroyMode.Vanish);
			Map map = base.Map;
			this.Destroy(DestroyMode.Vanish);
			if (this.GetStatValue(StatDefOf.WorkToBuild, true) > 150.0 && base.def.entityDefToBuild is ThingDef && ((ThingDef)base.def.entityDefToBuild).category == ThingCategory.Building)
			{
				SoundDefOf.BuildingComplete.PlayOneShot(new TargetInfo(base.Position, map, false));
			}
			ThingDef thingDef = base.def.entityDefToBuild as ThingDef;
			Thing thing = null;
			if (thingDef != null)
			{
				thing = ThingMaker.MakeThing(thingDef, base.Stuff);
				thing.SetFactionDirect(base.Faction);
				CompQuality compQuality = thing.TryGetComp<CompQuality>();
				if (compQuality != null)
				{
					int level = worker.skills.GetSkill(SkillDefOf.Construction).Level;
					compQuality.SetQuality(QualityUtility.RandomCreationQuality(level), ArtGenerationContext.Colony);
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
				GenSpawn.Spawn(thing, base.Position, map, base.Rotation, false);
			}
			else
			{
				map.terrainGrid.SetTerrain(base.Position, (TerrainDef)base.def.entityDefToBuild);
				FilthMaker.RemoveAllFilth(base.Position, map);
			}
			if (thingDef != null && (thingDef.passability == Traversability.Impassable || thingDef.Fillage == FillCategory.Full) && (thing == null || !(thing is Building_Door)))
			{
				foreach (IntVec3 item in GenAdj.CellsOccupiedBy(base.Position, base.Rotation, base.def.Size))
				{
					foreach (Thing item2 in map.thingGrid.ThingsAt(item).ToList())
					{
						if (item2 is Plant)
						{
							item2.Destroy(DestroyMode.KillFinalize);
						}
						else if (item2.def.category == ThingCategory.Item || item2 is Pawn)
						{
							GenPlace.TryMoveThing(item2, item2.Position, item2.Map);
						}
					}
				}
			}
			worker.records.Increment(RecordDefOf.ThingsConstructed);
			if (thing != null && thing.GetStatValue(StatDefOf.WorkToBuild, true) >= 10000.0)
			{
				TaleRecorder.RecordTale(TaleDefOf.CompletedLongConstructionProject, worker, thing.def);
			}
		}

		public void FailConstruction(Pawn worker)
		{
			Map map = base.Map;
			this.Destroy(DestroyMode.FailConstruction);
			Blueprint_Build blueprint_Build = null;
			if (base.def.entityDefToBuild.blueprintDef != null)
			{
				blueprint_Build = (Blueprint_Build)ThingMaker.MakeThing(base.def.entityDefToBuild.blueprintDef, null);
				blueprint_Build.stuffToUse = base.Stuff;
				blueprint_Build.SetFactionDirect(base.Faction);
				GenSpawn.Spawn(blueprint_Build, base.Position, map, base.Rotation, false);
			}
			Lord lord = worker.GetLord();
			if (lord != null)
			{
				lord.Notify_ConstructionFailed(worker, this, blueprint_Build);
			}
			MoteMaker.ThrowText(this.DrawPos, map, "TextMote_ConstructionFail".Translate(), 6f);
			if (base.Faction == Faction.OfPlayer && this.WorkToMake > 1400.0)
			{
				Messages.Message("MessageConstructionFailed".Translate(this.Label, worker.LabelShort), new TargetInfo(base.Position, map, false), MessageTypeDefOf.NegativeEvent);
			}
		}

		public override void Draw()
		{
			Vector2 vector = new Vector2((float)base.def.size.x, (float)base.def.size.z);
			vector.x *= 1.15f;
			vector.y *= 1.15f;
			Vector3 s = new Vector3(vector.x, 1f, vector.y);
			Matrix4x4 matrix = default(Matrix4x4);
			matrix.SetTRS(this.DrawPos, base.Rotation.AsQuat, s);
			Graphics.DrawMesh(MeshPool.plane10, matrix, Frame.UnderfieldMat, 0);
			int num = 4;
			for (int i = 0; i < num; i++)
			{
				IntVec2 rotatedSize = base.RotatedSize;
				int x = rotatedSize.x;
				IntVec2 rotatedSize2 = base.RotatedSize;
				float num2 = (float)Mathf.Min(x, rotatedSize2.z);
				float num3 = (float)(num2 * 0.37999999523162842);
				IntVec3 intVec = default(IntVec3);
				switch (i)
				{
				case 0:
					intVec = new IntVec3(-1, 0, -1);
					break;
				case 1:
					intVec = new IntVec3(-1, 0, 1);
					break;
				case 2:
					intVec = new IntVec3(1, 0, 1);
					break;
				case 3:
					intVec = new IntVec3(1, 0, -1);
					break;
				}
				Vector3 b = default(Vector3);
				float num4 = (float)intVec.x;
				IntVec2 rotatedSize3 = base.RotatedSize;
				b.x = (float)(num4 * ((float)rotatedSize3.x / 2.0 - num3 / 2.0));
				float num5 = (float)intVec.z;
				IntVec2 rotatedSize4 = base.RotatedSize;
				b.z = (float)(num5 * ((float)rotatedSize4.z / 2.0 - num3 / 2.0));
				Vector3 s2 = new Vector3(num3, 1f, num3);
				Matrix4x4 matrix2 = default(Matrix4x4);
				matrix2.SetTRS(this.DrawPos + Vector3.up * 0.03f + b, new Rot4(i).AsQuat, s2);
				Graphics.DrawMesh(MeshPool.plane10, matrix2, this.CornerMat, 0);
			}
			float num6 = (float)(this.PercentComplete / 1.0);
			float num7 = num6;
			IntVec2 rotatedSize5 = base.RotatedSize;
			float num8 = num7 * (float)rotatedSize5.x;
			IntVec2 rotatedSize6 = base.RotatedSize;
			int num9 = Mathf.CeilToInt((float)(num8 * (float)rotatedSize6.z * 4.0));
			IntVec2 intVec2 = base.RotatedSize * 2;
			for (int j = 0; j < num9; j++)
			{
				IntVec2 intVec3 = default(IntVec2);
				intVec3.z = j / intVec2.x;
				intVec3.x = j - intVec3.z * intVec2.x;
				Vector3 a = new Vector3((float)((float)intVec3.x * 0.5), 0f, (float)((float)intVec3.z * 0.5)) + this.DrawPos;
				float x2 = a.x;
				IntVec2 rotatedSize7 = base.RotatedSize;
				a.x = (float)(x2 - ((float)rotatedSize7.x * 0.5 - 0.25));
				float z = a.z;
				IntVec2 rotatedSize8 = base.RotatedSize;
				a.z = (float)(z - ((float)rotatedSize8.z * 0.5 - 0.25));
				Vector3 s3 = new Vector3(0.5f, 1f, 0.5f);
				Matrix4x4 matrix3 = default(Matrix4x4);
				matrix3.SetTRS(a + Vector3.up * 0.02f, Quaternion.identity, s3);
				Graphics.DrawMesh(MeshPool.plane10, matrix3, this.TileMat, 0);
			}
			base.Comps_PostDraw();
		}

		public override IEnumerable<Gizmo> GetGizmos()
		{
			using (IEnumerator<Gizmo> enumerator = base.GetGizmos().GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					Gizmo c = enumerator.Current;
					yield return c;
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			Command buildCopy = BuildCopyCommandUtility.BuildCopyCommand(base.def.entityDefToBuild, base.Stuff);
			if (buildCopy == null)
				yield break;
			yield return (Gizmo)buildCopy;
			/*Error: Unable to find new state assignment for yield return*/;
			IL_010e:
			/*Error near IL_010f: Unexpected return in MoveNext()*/;
		}

		public override string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(base.GetInspectString());
			stringBuilder.AppendLine("ContainedResources".Translate() + ":");
			List<ThingCountClass> list = base.def.entityDefToBuild.CostListAdjusted(base.Stuff, true);
			for (int i = 0; i < list.Count; i++)
			{
				ThingCountClass need = list[i];
				int num = need.count;
				foreach (ThingCountClass item in from needed in this.MaterialsNeeded()
				where needed.thingDef == need.thingDef
				select needed)
				{
					num -= item.count;
				}
				stringBuilder.AppendLine(need.thingDef.LabelCap + ": " + num + " / " + need.count);
			}
			stringBuilder.Append("WorkLeft".Translate() + ": " + this.WorkLeft.ToStringWorkAmount());
			return stringBuilder.ToString();
		}
	}
}
