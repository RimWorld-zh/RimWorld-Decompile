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
					using (IEnumerator<Thing> enumerator = potentialThings.GetEnumerator())
					{
						Thing th;
						while (true)
						{
							if (enumerator.MoveNext())
							{
								th = enumerator.Current;
								if (this.CanLinkTo(th))
									break;
								continue;
							}
							yield break;
						}
						yield return th;
						/*Error: Unable to find new state assignment for yield return*/;
					}
				}
				yield break;
				IL_0133:
				/*Error near IL_0134: Unexpected return in MoveNext()*/;
			}
		}

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
						goto IL_0052;
				}
				result = true;
			}
			goto IL_0076;
			IL_0076:
			return result;
			IL_0052:
			result = false;
			goto IL_0076;
		}

		public static bool CanPotentiallyLinkTo_Static(Thing facility, ThingDef myDef, IntVec3 myPos, Rot4 myRot)
		{
			return (byte)(CompAffectedByFacilities.CanPotentiallyLinkTo_Static(facility.def, facility.Position, facility.Rotation, myDef, myPos, myRot) ? (CompAffectedByFacilities.IsPotentiallyValidFacilityForMe_Static(facility, myDef, myPos, myRot) ? 1 : 0) : 0) != 0;
		}

		public bool CanPotentiallyLinkTo(ThingDef facilityDef, IntVec3 facilityPos, Rot4 facilityRot)
		{
			bool result;
			if (!CompAffectedByFacilities.CanPotentiallyLinkTo_Static(facilityDef, facilityPos, facilityRot, base.parent.def, base.parent.Position, base.parent.Rotation))
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
					result = ((byte)((num + 1 <= compProperties.maxSimultaneous) ? 1 : 0) != 0);
				}
			}
			return result;
		}

		public static bool CanPotentiallyLinkTo_Static(ThingDef facilityDef, IntVec3 facilityPos, Rot4 facilityRot, ThingDef myDef, IntVec3 myPos, Rot4 myRot)
		{
			CompProperties_Facility compProperties = facilityDef.GetCompProperties<CompProperties_Facility>();
			bool result;
			if (compProperties.mustBePlacedAdjacent)
			{
				CellRect rect = GenAdj.OccupiedRect(myPos, myRot, myDef.size);
				CellRect rect2 = GenAdj.OccupiedRect(facilityPos, facilityRot, facilityDef.size);
				if (!GenAdj.AdjacentTo8WayOrInside(rect, rect2))
				{
					result = false;
					goto IL_0135;
				}
			}
			if (compProperties.mustBePlacedAdjacentCardinalToBedHead)
			{
				if (!myDef.IsBed)
				{
					result = false;
					goto IL_0135;
				}
				CellRect other = GenAdj.OccupiedRect(facilityPos, facilityRot, facilityDef.size);
				bool flag = false;
				int sleepingSlotsCount = BedUtility.GetSleepingSlotsCount(myDef.size);
				for (int num = 0; num < sleepingSlotsCount; num++)
				{
					IntVec3 sleepingSlotPos = BedUtility.GetSleepingSlotPos(num, myPos, myRot, myDef.size);
					if (sleepingSlotPos.IsAdjacentToCardinalOrInside(other))
					{
						flag = true;
					}
				}
				if (!flag)
				{
					result = false;
					goto IL_0135;
				}
			}
			if (!compProperties.mustBePlacedAdjacent && !compProperties.mustBePlacedAdjacentCardinalToBedHead)
			{
				Vector3 a = Gen.TrueCenter(myPos, myRot, myDef.size, myDef.Altitude);
				Vector3 b = Gen.TrueCenter(facilityPos, facilityRot, facilityDef.size, facilityDef.Altitude);
				if (Vector3.Distance(a, b) > compProperties.maxDistance)
				{
					result = false;
					goto IL_0135;
				}
			}
			result = true;
			goto IL_0135;
			IL_0135:
			return result;
		}

		public bool IsValidFacilityForMe(Thing facility)
		{
			return (byte)(CompAffectedByFacilities.IsPotentiallyValidFacilityForMe_Static(facility, base.parent.def, base.parent.Position, base.parent.Rotation) ? 1 : 0) != 0;
		}

		private bool IsPotentiallyValidFacilityForMe(ThingDef facilityDef, IntVec3 facilityPos, Rot4 facilityRot)
		{
			bool result;
			if (!CompAffectedByFacilities.IsPotentiallyValidFacilityForMe_Static(facilityDef, facilityPos, facilityRot, base.parent.def, base.parent.Position, base.parent.Rotation, base.parent.Map))
			{
				result = false;
				goto IL_0080;
			}
			CompProperties_Facility compProperties = facilityDef.GetCompProperties<CompProperties_Facility>();
			if (compProperties.canLinkToMedBedsOnly)
			{
				Building_Bed building_Bed = base.parent as Building_Bed;
				if (building_Bed != null && building_Bed.Medical)
				{
					goto IL_0079;
				}
				result = false;
				goto IL_0080;
			}
			goto IL_0079;
			IL_0080:
			return result;
			IL_0079:
			result = true;
			goto IL_0080;
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
							if (GenSight.LineOfSight(start, end, map, startRect, endRect, null))
								goto IL_0086;
						}
					}
				}
				continue;
				IL_0086:
				flag = true;
				break;
			}
			return (byte)(flag ? 1 : 0) != 0;
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
			bool result;
			if (facilityDef != thanThisFacility.def)
			{
				Log.Error("Comparing two different facility defs.");
				result = false;
			}
			else
			{
				Vector3 b = Gen.TrueCenter(facilityPos, facilityRot, facilityDef.size, facilityDef.Altitude);
				Vector3 a = base.parent.TrueCenter();
				float num = Vector3.Distance(a, b);
				float num2 = Vector3.Distance(a, thanThisFacility.TrueCenter());
				if (num != num2)
				{
					result = (num < num2);
				}
				else
				{
					int x = facilityPos.x;
					IntVec3 position = thanThisFacility.Position;
					if (x != position.x)
					{
						int x2 = facilityPos.x;
						IntVec3 position2 = thanThisFacility.Position;
						result = (x2 < position2.x);
					}
					else
					{
						int z = facilityPos.z;
						IntVec3 position3 = thanThisFacility.Position;
						result = (z < position3.z);
					}
				}
			}
			return result;
		}

		public static IEnumerable<Thing> PotentialThingsToLinkTo(ThingDef myDef, IntVec3 myPos, Rot4 myRot, Map map)
		{
			_003CPotentialThingsToLinkTo_003Ec__Iterator1 _003CPotentialThingsToLinkTo_003Ec__Iterator = (_003CPotentialThingsToLinkTo_003Ec__Iterator1)/*Error near IL_0034: stateMachine*/;
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
				orderby Vector3.Distance(myTrueCenter, x.TrueCenter())
				select x).ThenBy((Func<Thing, int>)delegate(Thing x)
				{
					IntVec3 position2 = x.Position;
					return position2.x;
				}).ThenBy((Func<Thing, int>)delegate(Thing x)
				{
					IntVec3 position = x.Position;
					return position.z;
				});
				using (IEnumerator<Thing> enumerator = sortedCandidates.GetEnumerator())
				{
					Thing th;
					while (true)
					{
						if (enumerator.MoveNext())
						{
							th = enumerator.Current;
							if (CompAffectedByFacilities.CanPotentiallyLinkTo_Static(th, myDef, myPos, myRot))
							{
								CompProperties_Facility facilityProps = th.def.GetCompProperties<CompProperties_Facility>();
								if (CompAffectedByFacilities.alreadyReturnedCount.ContainsKey(th.def))
								{
									if (CompAffectedByFacilities.alreadyReturnedCount[th.def] < facilityProps.maxSimultaneous)
										break;
									continue;
								}
								CompAffectedByFacilities.alreadyReturnedCount.Add(th.def, 0);
								break;
							}
							continue;
						}
						yield break;
					}
					Dictionary<ThingDef, int> dictionary;
					ThingDef def;
					(dictionary = CompAffectedByFacilities.alreadyReturnedCount)[def = th.def] = dictionary[def] + 1;
					yield return th;
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			yield break;
			IL_02b9:
			/*Error near IL_02ba: Unexpected return in MoveNext()*/;
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
						if (facilityDef == this.linkedFacilities[j].def && this.IsBetter(thing2.def, thing2.Position, thing2.Rotation, this.linkedFacilities[j]))
						{
							thing2 = this.linkedFacilities[j];
						}
					}
					result = thing2;
				}
			}
			return result;
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
