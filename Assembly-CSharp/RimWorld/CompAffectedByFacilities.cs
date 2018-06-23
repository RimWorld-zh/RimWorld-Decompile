using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020006FD RID: 1789
	[StaticConstructorOnStartup]
	public class CompAffectedByFacilities : ThingComp
	{
		// Token: 0x040015B0 RID: 5552
		private List<Thing> linkedFacilities = new List<Thing>();

		// Token: 0x040015B1 RID: 5553
		public static Material InactiveFacilityLineMat = MaterialPool.MatFrom(GenDraw.LineTexPath, ShaderDatabase.Transparent, new Color(1f, 0.5f, 0.5f));

		// Token: 0x040015B2 RID: 5554
		private static Dictionary<ThingDef, int> alreadyReturnedCount = new Dictionary<ThingDef, int>();

		// Token: 0x040015B3 RID: 5555
		private List<ThingDef> alreadyUsed = new List<ThingDef>();

		// Token: 0x170005DC RID: 1500
		// (get) Token: 0x0600270E RID: 9998 RVA: 0x00150558 File Offset: 0x0014E958
		public List<Thing> LinkedFacilitiesListForReading
		{
			get
			{
				return this.linkedFacilities;
			}
		}

		// Token: 0x0600270F RID: 9999 RVA: 0x00150574 File Offset: 0x0014E974
		public bool CanLinkTo(Thing facility)
		{
			bool result;
			if (!this.CanPotentiallyLinkTo(facility.def, facility.Position, facility.Rotation))
			{
				result = false;
			}
			else if (!this.IsValidFacilityForMe(facility))
			{
				result = false;
			}
			else
			{
				for (int i = 0; i < this.linkedFacilities.Count; i++)
				{
					if (this.linkedFacilities[i] == facility)
					{
						return false;
					}
				}
				result = true;
			}
			return result;
		}

		// Token: 0x06002710 RID: 10000 RVA: 0x001505F8 File Offset: 0x0014E9F8
		public static bool CanPotentiallyLinkTo_Static(Thing facility, ThingDef myDef, IntVec3 myPos, Rot4 myRot)
		{
			return CompAffectedByFacilities.CanPotentiallyLinkTo_Static(facility.def, facility.Position, facility.Rotation, myDef, myPos, myRot) && CompAffectedByFacilities.IsPotentiallyValidFacilityForMe_Static(facility, myDef, myPos, myRot);
		}

		// Token: 0x06002711 RID: 10001 RVA: 0x0015064C File Offset: 0x0014EA4C
		public bool CanPotentiallyLinkTo(ThingDef facilityDef, IntVec3 facilityPos, Rot4 facilityRot)
		{
			bool result;
			if (!CompAffectedByFacilities.CanPotentiallyLinkTo_Static(facilityDef, facilityPos, facilityRot, this.parent.def, this.parent.Position, this.parent.Rotation))
			{
				result = false;
			}
			else if (!this.IsPotentiallyValidFacilityForMe(facilityDef, facilityPos, facilityRot))
			{
				result = false;
			}
			else
			{
				int num = 0;
				bool flag = false;
				for (int i = 0; i < this.linkedFacilities.Count; i++)
				{
					if (this.linkedFacilities[i].def == facilityDef)
					{
						num++;
						if (this.IsBetter(facilityDef, facilityPos, facilityRot, this.linkedFacilities[i]))
						{
							flag = true;
							break;
						}
					}
				}
				if (flag)
				{
					result = true;
				}
				else
				{
					CompProperties_Facility compProperties = facilityDef.GetCompProperties<CompProperties_Facility>();
					result = (num + 1 <= compProperties.maxSimultaneous);
				}
			}
			return result;
		}

		// Token: 0x06002712 RID: 10002 RVA: 0x00150738 File Offset: 0x0014EB38
		public static bool CanPotentiallyLinkTo_Static(ThingDef facilityDef, IntVec3 facilityPos, Rot4 facilityRot, ThingDef myDef, IntVec3 myPos, Rot4 myRot)
		{
			CompProperties_Facility compProperties = facilityDef.GetCompProperties<CompProperties_Facility>();
			if (compProperties.mustBePlacedAdjacent)
			{
				CellRect rect = GenAdj.OccupiedRect(myPos, myRot, myDef.size);
				CellRect rect2 = GenAdj.OccupiedRect(facilityPos, facilityRot, facilityDef.size);
				if (!GenAdj.AdjacentTo8WayOrInside(rect, rect2))
				{
					return false;
				}
			}
			if (compProperties.mustBePlacedAdjacentCardinalToBedHead)
			{
				if (!myDef.IsBed)
				{
					return false;
				}
				CellRect other = GenAdj.OccupiedRect(facilityPos, facilityRot, facilityDef.size);
				bool flag = false;
				int sleepingSlotsCount = BedUtility.GetSleepingSlotsCount(myDef.size);
				for (int i = 0; i < sleepingSlotsCount; i++)
				{
					IntVec3 sleepingSlotPos = BedUtility.GetSleepingSlotPos(i, myPos, myRot, myDef.size);
					if (sleepingSlotPos.IsAdjacentToCardinalOrInside(other))
					{
						flag = true;
					}
				}
				if (!flag)
				{
					return false;
				}
			}
			if (!compProperties.mustBePlacedAdjacent && !compProperties.mustBePlacedAdjacentCardinalToBedHead)
			{
				Vector3 a = GenThing.TrueCenter(myPos, myRot, myDef.size, myDef.Altitude);
				Vector3 b = GenThing.TrueCenter(facilityPos, facilityRot, facilityDef.size, facilityDef.Altitude);
				if (Vector3.Distance(a, b) > compProperties.maxDistance)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06002713 RID: 10003 RVA: 0x0015087C File Offset: 0x0014EC7C
		public bool IsValidFacilityForMe(Thing facility)
		{
			return CompAffectedByFacilities.IsPotentiallyValidFacilityForMe_Static(facility, this.parent.def, this.parent.Position, this.parent.Rotation);
		}

		// Token: 0x06002714 RID: 10004 RVA: 0x001508C8 File Offset: 0x0014ECC8
		private bool IsPotentiallyValidFacilityForMe(ThingDef facilityDef, IntVec3 facilityPos, Rot4 facilityRot)
		{
			bool result;
			if (!CompAffectedByFacilities.IsPotentiallyValidFacilityForMe_Static(facilityDef, facilityPos, facilityRot, this.parent.def, this.parent.Position, this.parent.Rotation, this.parent.Map))
			{
				result = false;
			}
			else
			{
				CompProperties_Facility compProperties = facilityDef.GetCompProperties<CompProperties_Facility>();
				if (compProperties.canLinkToMedBedsOnly)
				{
					Building_Bed building_Bed = this.parent as Building_Bed;
					if (building_Bed == null || !building_Bed.Medical)
					{
						return false;
					}
				}
				result = true;
			}
			return result;
		}

		// Token: 0x06002715 RID: 10005 RVA: 0x00150958 File Offset: 0x0014ED58
		private static bool IsPotentiallyValidFacilityForMe_Static(Thing facility, ThingDef myDef, IntVec3 myPos, Rot4 myRot)
		{
			return CompAffectedByFacilities.IsPotentiallyValidFacilityForMe_Static(facility.def, facility.Position, facility.Rotation, myDef, myPos, myRot, facility.Map);
		}

		// Token: 0x06002716 RID: 10006 RVA: 0x00150990 File Offset: 0x0014ED90
		private static bool IsPotentiallyValidFacilityForMe_Static(ThingDef facilityDef, IntVec3 facilityPos, Rot4 facilityRot, ThingDef myDef, IntVec3 myPos, Rot4 myRot, Map map)
		{
			CellRect startRect = GenAdj.OccupiedRect(myPos, myRot, myDef.size);
			CellRect endRect = GenAdj.OccupiedRect(facilityPos, facilityRot, facilityDef.size);
			bool result = false;
			for (int i = startRect.minZ; i <= startRect.maxZ; i++)
			{
				for (int j = startRect.minX; j <= startRect.maxX; j++)
				{
					for (int k = endRect.minZ; k <= endRect.maxZ; k++)
					{
						for (int l = endRect.minX; l <= endRect.maxX; l++)
						{
							IntVec3 start = new IntVec3(j, 0, i);
							IntVec3 end = new IntVec3(l, 0, k);
							if (GenSight.LineOfSight(start, end, map, startRect, endRect, null))
							{
								return true;
							}
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06002717 RID: 10007 RVA: 0x00150A94 File Offset: 0x0014EE94
		public void Notify_NewLink(Thing facility)
		{
			for (int i = 0; i < this.linkedFacilities.Count; i++)
			{
				if (this.linkedFacilities[i] == facility)
				{
					Log.Error("Notify_NewLink was called but the link is already here.", false);
					return;
				}
			}
			Thing potentiallySupplantedFacility = this.GetPotentiallySupplantedFacility(facility.def, facility.Position, facility.Rotation);
			if (potentiallySupplantedFacility != null)
			{
				potentiallySupplantedFacility.TryGetComp<CompFacility>().Notify_LinkRemoved(this.parent);
				this.linkedFacilities.Remove(potentiallySupplantedFacility);
			}
			this.linkedFacilities.Add(facility);
		}

		// Token: 0x06002718 RID: 10008 RVA: 0x00150B30 File Offset: 0x0014EF30
		public void Notify_LinkRemoved(Thing thing)
		{
			for (int i = 0; i < this.linkedFacilities.Count; i++)
			{
				if (this.linkedFacilities[i] == thing)
				{
					this.linkedFacilities.RemoveAt(i);
					return;
				}
			}
			Log.Error("Notify_LinkRemoved was called but there is no such link here.", false);
		}

		// Token: 0x06002719 RID: 10009 RVA: 0x00150B8B File Offset: 0x0014EF8B
		public void Notify_FacilityDespawned()
		{
			this.RelinkAll();
		}

		// Token: 0x0600271A RID: 10010 RVA: 0x00150B94 File Offset: 0x0014EF94
		public void Notify_LOSBlockerSpawnedOrDespawned()
		{
			this.RelinkAll();
		}

		// Token: 0x0600271B RID: 10011 RVA: 0x00150B9D File Offset: 0x0014EF9D
		public void Notify_ThingChanged()
		{
			this.RelinkAll();
		}

		// Token: 0x0600271C RID: 10012 RVA: 0x00150BA6 File Offset: 0x0014EFA6
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			this.LinkToNearbyFacilities();
		}

		// Token: 0x0600271D RID: 10013 RVA: 0x00150BAF File Offset: 0x0014EFAF
		public override void PostDeSpawn(Map map)
		{
			this.UnlinkAll();
		}

		// Token: 0x0600271E RID: 10014 RVA: 0x00150BB8 File Offset: 0x0014EFB8
		public override void PostDrawExtraSelectionOverlays()
		{
			for (int i = 0; i < this.linkedFacilities.Count; i++)
			{
				if (this.IsFacilityActive(this.linkedFacilities[i]))
				{
					GenDraw.DrawLineBetween(this.parent.TrueCenter(), this.linkedFacilities[i].TrueCenter());
				}
				else
				{
					GenDraw.DrawLineBetween(this.parent.TrueCenter(), this.linkedFacilities[i].TrueCenter(), CompAffectedByFacilities.InactiveFacilityLineMat);
				}
			}
		}

		// Token: 0x0600271F RID: 10015 RVA: 0x00150C48 File Offset: 0x0014F048
		private bool IsBetter(ThingDef facilityDef, IntVec3 facilityPos, Rot4 facilityRot, Thing thanThisFacility)
		{
			bool result;
			if (facilityDef != thanThisFacility.def)
			{
				Log.Error("Comparing two different facility defs.", false);
				result = false;
			}
			else
			{
				Vector3 b = GenThing.TrueCenter(facilityPos, facilityRot, facilityDef.size, facilityDef.Altitude);
				Vector3 a = this.parent.TrueCenter();
				float num = Vector3.Distance(a, b);
				float num2 = Vector3.Distance(a, thanThisFacility.TrueCenter());
				if (num != num2)
				{
					result = (num < num2);
				}
				else if (facilityPos.x != thanThisFacility.Position.x)
				{
					result = (facilityPos.x < thanThisFacility.Position.x);
				}
				else
				{
					result = (facilityPos.z < thanThisFacility.Position.z);
				}
			}
			return result;
		}

		// Token: 0x170005DD RID: 1501
		// (get) Token: 0x06002720 RID: 10016 RVA: 0x00150D1C File Offset: 0x0014F11C
		private IEnumerable<Thing> ThingsICanLinkTo
		{
			get
			{
				if (!this.parent.Spawned)
				{
					yield break;
				}
				IEnumerable<Thing> potentialThings = CompAffectedByFacilities.PotentialThingsToLinkTo(this.parent.def, this.parent.Position, this.parent.Rotation, this.parent.Map);
				foreach (Thing th in potentialThings)
				{
					if (this.CanLinkTo(th))
					{
						yield return th;
					}
				}
				yield break;
			}
		}

		// Token: 0x06002721 RID: 10017 RVA: 0x00150D48 File Offset: 0x0014F148
		public static IEnumerable<Thing> PotentialThingsToLinkTo(ThingDef myDef, IntVec3 myPos, Rot4 myRot, Map map)
		{
			CompAffectedByFacilities.alreadyReturnedCount.Clear();
			CompProperties_AffectedByFacilities myProps = myDef.GetCompProperties<CompProperties_AffectedByFacilities>();
			if (myProps.linkableFacilities == null)
			{
				yield break;
			}
			IEnumerable<Thing> candidates = Enumerable.Empty<Thing>();
			for (int i = 0; i < myProps.linkableFacilities.Count; i++)
			{
				candidates = candidates.Concat(map.listerThings.ThingsOfDef(myProps.linkableFacilities[i]));
			}
			Vector3 myTrueCenter = GenThing.TrueCenter(myPos, myRot, myDef.size, myDef.Altitude);
			IOrderedEnumerable<Thing> sortedCandidates = from x in candidates
			orderby Vector3.Distance(myTrueCenter, x.TrueCenter()), x.Position.x, x.Position.z
			select x;
			foreach (Thing th in sortedCandidates)
			{
				if (CompAffectedByFacilities.CanPotentiallyLinkTo_Static(th, myDef, myPos, myRot))
				{
					CompProperties_Facility facilityProps = th.def.GetCompProperties<CompProperties_Facility>();
					if (CompAffectedByFacilities.alreadyReturnedCount.ContainsKey(th.def))
					{
						if (CompAffectedByFacilities.alreadyReturnedCount[th.def] >= facilityProps.maxSimultaneous)
						{
							continue;
						}
					}
					else
					{
						CompAffectedByFacilities.alreadyReturnedCount.Add(th.def, 0);
					}
					Dictionary<ThingDef, int> dictionary;
					ThingDef def;
					(dictionary = CompAffectedByFacilities.alreadyReturnedCount)[def = th.def] = dictionary[def] + 1;
					yield return th;
				}
			}
			yield break;
		}

		// Token: 0x06002722 RID: 10018 RVA: 0x00150D88 File Offset: 0x0014F188
		public static void DrawLinesToPotentialThingsToLinkTo(ThingDef myDef, IntVec3 myPos, Rot4 myRot, Map map)
		{
			Vector3 a = GenThing.TrueCenter(myPos, myRot, myDef.size, myDef.Altitude);
			foreach (Thing t in CompAffectedByFacilities.PotentialThingsToLinkTo(myDef, myPos, myRot, map))
			{
				GenDraw.DrawLineBetween(a, t.TrueCenter());
			}
		}

		// Token: 0x06002723 RID: 10019 RVA: 0x00150E04 File Offset: 0x0014F204
		public void DrawRedLineToPotentiallySupplantedFacility(ThingDef facilityDef, IntVec3 facilityPos, Rot4 facilityRot)
		{
			Thing potentiallySupplantedFacility = this.GetPotentiallySupplantedFacility(facilityDef, facilityPos, facilityRot);
			if (potentiallySupplantedFacility != null)
			{
				GenDraw.DrawLineBetween(this.parent.TrueCenter(), potentiallySupplantedFacility.TrueCenter(), CompAffectedByFacilities.InactiveFacilityLineMat);
			}
		}

		// Token: 0x06002724 RID: 10020 RVA: 0x00150E40 File Offset: 0x0014F240
		private Thing GetPotentiallySupplantedFacility(ThingDef facilityDef, IntVec3 facilityPos, Rot4 facilityRot)
		{
			Thing thing = null;
			int num = 0;
			for (int i = 0; i < this.linkedFacilities.Count; i++)
			{
				if (this.linkedFacilities[i].def == facilityDef)
				{
					if (thing == null)
					{
						thing = this.linkedFacilities[i];
					}
					num++;
				}
			}
			Thing result;
			if (num == 0)
			{
				result = null;
			}
			else
			{
				CompProperties_Facility compProperties = facilityDef.GetCompProperties<CompProperties_Facility>();
				if (num + 1 <= compProperties.maxSimultaneous)
				{
					result = null;
				}
				else
				{
					Thing thing2 = thing;
					for (int j = 0; j < this.linkedFacilities.Count; j++)
					{
						if (facilityDef == this.linkedFacilities[j].def)
						{
							if (this.IsBetter(thing2.def, thing2.Position, thing2.Rotation, this.linkedFacilities[j]))
							{
								thing2 = this.linkedFacilities[j];
							}
						}
					}
					result = thing2;
				}
			}
			return result;
		}

		// Token: 0x06002725 RID: 10021 RVA: 0x00150F54 File Offset: 0x0014F354
		public float GetStatOffset(StatDef stat)
		{
			float num = 0f;
			for (int i = 0; i < this.linkedFacilities.Count; i++)
			{
				if (this.IsFacilityActive(this.linkedFacilities[i]))
				{
					CompProperties_Facility compProperties = this.linkedFacilities[i].def.GetCompProperties<CompProperties_Facility>();
					if (compProperties.statOffsets != null)
					{
						num += compProperties.statOffsets.GetStatOffsetFromList(stat);
					}
				}
			}
			return num;
		}

		// Token: 0x06002726 RID: 10022 RVA: 0x00150FE0 File Offset: 0x0014F3E0
		public void GetStatsExplanation(StatDef stat, StringBuilder sb)
		{
			this.alreadyUsed.Clear();
			bool flag = false;
			for (int i = 0; i < this.linkedFacilities.Count; i++)
			{
				bool flag2 = false;
				for (int j = 0; j < this.alreadyUsed.Count; j++)
				{
					if (this.alreadyUsed[j] == this.linkedFacilities[i].def)
					{
						flag2 = true;
						break;
					}
				}
				if (!flag2)
				{
					if (this.IsFacilityActive(this.linkedFacilities[i]))
					{
						CompProperties_Facility compProperties = this.linkedFacilities[i].def.GetCompProperties<CompProperties_Facility>();
						if (compProperties.statOffsets != null)
						{
							float num = compProperties.statOffsets.GetStatOffsetFromList(stat);
							if (num != 0f)
							{
								if (!flag)
								{
									flag = true;
									sb.AppendLine();
									sb.AppendLine("StatsReport_Facilities".Translate() + ":");
								}
								int num2 = 0;
								for (int k = 0; k < this.linkedFacilities.Count; k++)
								{
									if (this.IsFacilityActive(this.linkedFacilities[k]) && this.linkedFacilities[k].def == this.linkedFacilities[i].def)
									{
										num2++;
									}
								}
								num *= (float)num2;
								sb.Append("    ");
								if (num2 != 1)
								{
									sb.Append(num2.ToString() + "x ");
								}
								sb.AppendLine(this.linkedFacilities[i].LabelCap + ": " + num.ToStringByStyle(stat.toStringStyle, ToStringNumberSense.Offset));
								this.alreadyUsed.Add(this.linkedFacilities[i].def);
							}
						}
					}
				}
			}
		}

		// Token: 0x06002727 RID: 10023 RVA: 0x001511F0 File Offset: 0x0014F5F0
		private void RelinkAll()
		{
			this.LinkToNearbyFacilities();
		}

		// Token: 0x06002728 RID: 10024 RVA: 0x001511FC File Offset: 0x0014F5FC
		public bool IsFacilityActive(Thing facility)
		{
			return facility.TryGetComp<CompFacility>().CanBeActive;
		}

		// Token: 0x06002729 RID: 10025 RVA: 0x0015121C File Offset: 0x0014F61C
		private void LinkToNearbyFacilities()
		{
			this.UnlinkAll();
			if (this.parent.Spawned)
			{
				foreach (Thing thing in this.ThingsICanLinkTo)
				{
					this.linkedFacilities.Add(thing);
					thing.TryGetComp<CompFacility>().Notify_NewLink(this.parent);
				}
			}
		}

		// Token: 0x0600272A RID: 10026 RVA: 0x001512A8 File Offset: 0x0014F6A8
		private void UnlinkAll()
		{
			for (int i = 0; i < this.linkedFacilities.Count; i++)
			{
				this.linkedFacilities[i].TryGetComp<CompFacility>().Notify_LinkRemoved(this.parent);
			}
			this.linkedFacilities.Clear();
		}
	}
}
