using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	[StaticConstructorOnStartup]
	public class CompAffectedByFacilities : ThingComp
	{
		public const float MaxDistToLinkToFacility = 8f;

		private List<Thing> linkedFacilities = new List<Thing>();

		public static Material InactiveFacilityLineMat = MaterialPool.MatFrom(GenDraw.LineTexPath, ShaderDatabase.Transparent, new Color(1f, 0.5f, 0.5f));

		private static Dictionary<ThingDef, int> alreadyReturnedCount = new Dictionary<ThingDef, int>();

		private List<ThingDef> alreadyUsed = new List<ThingDef>();

		public List<Thing> LinkedFacilitiesListForReading
		{
			get
			{
				return this.linkedFacilities;
			}
		}

		private IEnumerable<Thing> ThingsICanLinkTo
		{
			get
			{
				if (base.parent.Spawned)
				{
					IEnumerable<Thing> potentialThings = CompAffectedByFacilities.PotentialThingsToLinkTo(base.parent.def, base.parent.Position, base.parent.Rotation, base.parent.Map);
					foreach (Thing item in potentialThings)
					{
						if (this.CanLinkTo(item))
						{
							yield return item;
						}
					}
				}
			}
		}

		public bool CanLinkTo(Thing facility)
		{
			if (!this.CanPotentiallyLinkTo(facility.def, facility.Position, facility.Rotation))
			{
				return false;
			}
			if (!this.IsValidFacilityForMe(facility))
			{
				return false;
			}
			for (int i = 0; i < this.linkedFacilities.Count; i++)
			{
				if (this.linkedFacilities[i] == facility)
				{
					return false;
				}
			}
			return true;
		}

		public static bool CanPotentiallyLinkTo_Static(Thing facility, ThingDef myDef, IntVec3 myPos, Rot4 myRot)
		{
			if (!CompAffectedByFacilities.CanPotentiallyLinkTo_Static(facility.def, facility.Position, facility.Rotation, myDef, myPos, myRot))
			{
				return false;
			}
			if (!CompAffectedByFacilities.IsPotentiallyValidFacilityForMe_Static(facility, myDef, myPos, myRot))
			{
				return false;
			}
			return true;
		}

		public bool CanPotentiallyLinkTo(ThingDef facilityDef, IntVec3 facilityPos, Rot4 facilityRot)
		{
			if (!CompAffectedByFacilities.CanPotentiallyLinkTo_Static(facilityDef, facilityPos, facilityRot, base.parent.def, base.parent.Position, base.parent.Rotation))
			{
				return false;
			}
			if (!this.IsPotentiallyValidFacilityForMe(facilityDef, facilityPos, facilityRot))
			{
				return false;
			}
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
				return true;
			}
			CompProperties_Facility compProperties = facilityDef.GetCompProperties<CompProperties_Facility>();
			if (num + 1 > compProperties.maxSimultaneous)
			{
				return false;
			}
			return true;
		}

		public static bool CanPotentiallyLinkTo_Static(ThingDef facilityDef, IntVec3 facilityPos, Rot4 facilityRot, ThingDef myDef, IntVec3 myPos, Rot4 myRot)
		{
			CompProperties_Facility compProperties = facilityDef.GetCompProperties<CompProperties_Facility>();
			bool flag;
			if (compProperties.mustBePlacedAdjacent)
			{
				flag = false;
				foreach (IntVec3 item in GenAdj.CellsOccupiedBy(myPos, myRot, myDef.size))
				{
					IntVec3 current = item;
					foreach (IntVec3 item2 in GenAdj.CellsOccupiedBy(facilityPos, facilityRot, facilityDef.size))
					{
						IntVec3 current2 = item2;
						if (Mathf.Abs(current.x - current2.x) <= 1 && Mathf.Abs(current.z - current2.z) <= 1)
						{
							flag = true;
							goto IL_00cb;
						}
					}
				}
				goto IL_00cb;
			}
			Vector3 a = Gen.TrueCenter(myPos, myRot, myDef.size, myDef.Altitude);
			Vector3 b = Gen.TrueCenter(facilityPos, facilityRot, facilityDef.size, facilityDef.Altitude);
			if (Vector3.Distance(a, b) > 8.0)
			{
				return false;
			}
			goto IL_0119;
			IL_0119:
			return true;
			IL_00cb:
			if (!flag)
			{
				return false;
			}
			goto IL_0119;
		}

		public bool IsValidFacilityForMe(Thing facility)
		{
			if (!CompAffectedByFacilities.IsPotentiallyValidFacilityForMe_Static(facility, base.parent.def, base.parent.Position, base.parent.Rotation))
			{
				return false;
			}
			return true;
		}

		private bool IsPotentiallyValidFacilityForMe(ThingDef facilityDef, IntVec3 facilityPos, Rot4 facilityRot)
		{
			if (!CompAffectedByFacilities.IsPotentiallyValidFacilityForMe_Static(facilityDef, facilityPos, facilityRot, base.parent.def, base.parent.Position, base.parent.Rotation, base.parent.Map))
			{
				return false;
			}
			CompProperties_Facility compProperties = facilityDef.GetCompProperties<CompProperties_Facility>();
			if (compProperties.canLinkToMedBedsOnly)
			{
				Building_Bed building_Bed = base.parent as Building_Bed;
				if (building_Bed != null && building_Bed.Medical)
				{
					goto IL_006c;
				}
				return false;
			}
			goto IL_006c;
			IL_006c:
			return true;
		}

		private static bool IsPotentiallyValidFacilityForMe_Static(Thing facility, ThingDef myDef, IntVec3 myPos, Rot4 myRot)
		{
			return CompAffectedByFacilities.IsPotentiallyValidFacilityForMe_Static(facility.def, facility.Position, facility.Rotation, myDef, myPos, myRot, facility.Map);
		}

		private static bool IsPotentiallyValidFacilityForMe_Static(ThingDef facilityDef, IntVec3 facilityPos, Rot4 facilityRot, ThingDef myDef, IntVec3 myPos, Rot4 myRot, Map map)
		{
			CellRect startRect = GenAdj.OccupiedRect(myPos, myRot, myDef.size);
			CellRect endRect = GenAdj.OccupiedRect(facilityPos, facilityRot, facilityDef.size);
			bool flag = false;
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
							if (GenSight.LineOfSight(start, end, map, startRect, endRect))
								goto IL_0080;
						}
					}
				}
				continue;
				IL_0080:
				flag = true;
				break;
			}
			if (!flag)
			{
				return false;
			}
			return true;
		}

		public void Notify_NewLink(Thing facility)
		{
			for (int i = 0; i < this.linkedFacilities.Count; i++)
			{
				if (this.linkedFacilities[i] == facility)
				{
					Log.Error("Notify_NewLink was called but the link is already here.");
					return;
				}
			}
			Thing potentiallySupplantedFacility = this.GetPotentiallySupplantedFacility(facility.def, facility.Position, facility.Rotation);
			if (potentiallySupplantedFacility != null)
			{
				potentiallySupplantedFacility.TryGetComp<CompFacility>().Notify_LinkRemoved(base.parent);
				this.linkedFacilities.Remove(potentiallySupplantedFacility);
			}
			this.linkedFacilities.Add(facility);
		}

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
			Log.Error("Notify_LinkRemoved was called but there is no such link here.");
		}

		public void Notify_FacilityDespawned()
		{
			this.RelinkAll();
		}

		public void Notify_LOSBlockerSpawnedOrDespawned()
		{
			this.RelinkAll();
		}

		public void Notify_ThingChanged()
		{
			this.RelinkAll();
		}

		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			this.LinkToNearbyFacilities();
		}

		public override void PostDeSpawn(Map map)
		{
			this.UnlinkAll();
		}

		public override void PostDrawExtraSelectionOverlays()
		{
			for (int i = 0; i < this.linkedFacilities.Count; i++)
			{
				if (this.IsFacilityActive(this.linkedFacilities[i]))
				{
					GenDraw.DrawLineBetween(base.parent.TrueCenter(), this.linkedFacilities[i].TrueCenter());
				}
				else
				{
					GenDraw.DrawLineBetween(base.parent.TrueCenter(), this.linkedFacilities[i].TrueCenter(), CompAffectedByFacilities.InactiveFacilityLineMat);
				}
			}
		}

		private bool IsBetter(ThingDef facilityDef, IntVec3 facilityPos, Rot4 facilityRot, Thing thanThisFacility)
		{
			if (facilityDef != thanThisFacility.def)
			{
				Log.Error("Comparing two different facility defs.");
				return false;
			}
			Vector3 b = Gen.TrueCenter(facilityPos, facilityRot, facilityDef.size, facilityDef.Altitude);
			Vector3 a = base.parent.TrueCenter();
			float num = Vector3.Distance(a, b);
			float num2 = Vector3.Distance(a, thanThisFacility.TrueCenter());
			if (num != num2)
			{
				return num < num2;
			}
			int x = facilityPos.x;
			IntVec3 position = thanThisFacility.Position;
			if (x != position.x)
			{
				int x2 = facilityPos.x;
				IntVec3 position2 = thanThisFacility.Position;
				return x2 < position2.x;
			}
			int z = facilityPos.z;
			IntVec3 position3 = thanThisFacility.Position;
			return z < position3.z;
		}

		public static IEnumerable<Thing> PotentialThingsToLinkTo(ThingDef myDef, IntVec3 myPos, Rot4 myRot, Map map)
		{
			CompAffectedByFacilities.alreadyReturnedCount.Clear();
			CompProperties_AffectedByFacilities myProps = myDef.GetCompProperties<CompProperties_AffectedByFacilities>();
			if (myProps.linkableFacilities != null)
			{
				IEnumerable<Thing> candidates = Enumerable.Empty<Thing>();
				for (int i = 0; i < myProps.linkableFacilities.Count; i++)
				{
					candidates = candidates.Concat(map.listerThings.ThingsOfDef(myProps.linkableFacilities[i]));
				}
				Vector3 myTrueCenter = Gen.TrueCenter(myPos, myRot, myDef.size, myDef.Altitude);
				IOrderedEnumerable<Thing> sortedCandidates = (from x in candidates
				orderby Vector3.Distance(((_003CPotentialThingsToLinkTo_003Ec__Iterator163)/*Error near IL_00fe: stateMachine*/)._003CmyTrueCenter_003E__3, x.TrueCenter())
				select x).ThenBy((Func<Thing, int>)delegate(Thing x)
				{
					IntVec3 position2 = x.Position;
					return position2.x;
				}).ThenBy((Func<Thing, int>)delegate(Thing x)
				{
					IntVec3 position = x.Position;
					return position.z;
				});
				foreach (Thing item in sortedCandidates)
				{
					if (CompAffectedByFacilities.CanPotentiallyLinkTo_Static(item, myDef, myPos, myRot))
					{
						CompProperties_Facility facilityProps = item.def.GetCompProperties<CompProperties_Facility>();
						if (CompAffectedByFacilities.alreadyReturnedCount.ContainsKey(item.def))
						{
							if (CompAffectedByFacilities.alreadyReturnedCount[item.def] < facilityProps.maxSimultaneous)
								goto IL_022a;
							continue;
						}
						CompAffectedByFacilities.alreadyReturnedCount.Add(item.def, 0);
						goto IL_022a;
					}
					continue;
					IL_022a:
					Dictionary<ThingDef, int> dictionary;
					Dictionary<ThingDef, int> obj = dictionary = CompAffectedByFacilities.alreadyReturnedCount;
					ThingDef def;
					ThingDef key = def = item.def;
					int num = dictionary[def];
					obj[key] = num + 1;
					yield return item;
				}
			}
		}

		public static void DrawLinesToPotentialThingsToLinkTo(ThingDef myDef, IntVec3 myPos, Rot4 myRot, Map map)
		{
			Vector3 a = Gen.TrueCenter(myPos, myRot, myDef.size, myDef.Altitude);
			foreach (Thing item in CompAffectedByFacilities.PotentialThingsToLinkTo(myDef, myPos, myRot, map))
			{
				GenDraw.DrawLineBetween(a, item.TrueCenter());
			}
		}

		public void DrawRedLineToPotentiallySupplantedFacility(ThingDef facilityDef, IntVec3 facilityPos, Rot4 facilityRot)
		{
			Thing potentiallySupplantedFacility = this.GetPotentiallySupplantedFacility(facilityDef, facilityPos, facilityRot);
			if (potentiallySupplantedFacility != null)
			{
				GenDraw.DrawLineBetween(base.parent.TrueCenter(), potentiallySupplantedFacility.TrueCenter(), CompAffectedByFacilities.InactiveFacilityLineMat);
			}
		}

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
			if (num == 0)
			{
				return null;
			}
			CompProperties_Facility compProperties = facilityDef.GetCompProperties<CompProperties_Facility>();
			if (num + 1 <= compProperties.maxSimultaneous)
			{
				return null;
			}
			Thing thing2 = thing;
			for (int j = 0; j < this.linkedFacilities.Count; j++)
			{
				if (facilityDef == this.linkedFacilities[j].def && this.IsBetter(thing2.def, thing2.Position, thing2.Rotation, this.linkedFacilities[j]))
				{
					thing2 = this.linkedFacilities[j];
				}
			}
			return thing2;
		}

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

		public void GetStatsExplanation(StatDef stat, StringBuilder sb)
		{
			this.alreadyUsed.Clear();
			bool flag = false;
			for (int i = 0; i < this.linkedFacilities.Count; i++)
			{
				bool flag2 = false;
				int num = 0;
				while (num < this.alreadyUsed.Count)
				{
					if (this.alreadyUsed[num] != this.linkedFacilities[i].def)
					{
						num++;
						continue;
					}
					flag2 = true;
					break;
				}
				if (!flag2 && this.IsFacilityActive(this.linkedFacilities[i]))
				{
					CompProperties_Facility compProperties = this.linkedFacilities[i].def.GetCompProperties<CompProperties_Facility>();
					if (compProperties.statOffsets != null)
					{
						float statOffsetFromList = compProperties.statOffsets.GetStatOffsetFromList(stat);
						if (statOffsetFromList != 0.0)
						{
							if (!flag)
							{
								flag = true;
								sb.AppendLine();
								sb.AppendLine("StatsReport_Facilities".Translate() + ":");
							}
							int num2 = 0;
							for (int j = 0; j < this.linkedFacilities.Count; j++)
							{
								if (this.IsFacilityActive(this.linkedFacilities[j]) && this.linkedFacilities[j].def == this.linkedFacilities[i].def)
								{
									num2++;
								}
							}
							statOffsetFromList *= (float)num2;
							sb.Append("    ");
							if (num2 != 1)
							{
								sb.Append(num2.ToString() + "x ");
							}
							sb.AppendLine(this.linkedFacilities[i].LabelCap + ": " + statOffsetFromList.ToStringByStyle(stat.toStringStyle, ToStringNumberSense.Offset));
							this.alreadyUsed.Add(this.linkedFacilities[i].def);
						}
					}
				}
			}
		}

		private void RelinkAll()
		{
			this.LinkToNearbyFacilities();
		}

		public bool IsFacilityActive(Thing facility)
		{
			return facility.TryGetComp<CompFacility>().CanBeActive;
		}

		private void LinkToNearbyFacilities()
		{
			this.UnlinkAll();
			if (base.parent.Spawned)
			{
				foreach (Thing item in this.ThingsICanLinkTo)
				{
					this.linkedFacilities.Add(item);
					item.TryGetComp<CompFacility>().Notify_NewLink(base.parent);
				}
			}
		}

		private void UnlinkAll()
		{
			for (int i = 0; i < this.linkedFacilities.Count; i++)
			{
				this.linkedFacilities[i].TryGetComp<CompFacility>().Notify_LinkRemoved(base.parent);
			}
			this.linkedFacilities.Clear();
		}
	}
}
