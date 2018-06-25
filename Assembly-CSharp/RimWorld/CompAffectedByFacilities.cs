using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
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

		public CompAffectedByFacilities()
		{
		}

		public List<Thing> LinkedFacilitiesListForReading
		{
			get
			{
				return this.linkedFacilities;
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
					{
						return false;
					}
				}
				result = true;
			}
			return result;
		}

		public static bool CanPotentiallyLinkTo_Static(Thing facility, ThingDef myDef, IntVec3 myPos, Rot4 myRot)
		{
			return CompAffectedByFacilities.CanPotentiallyLinkTo_Static(facility.def, facility.Position, facility.Rotation, myDef, myPos, myRot) && CompAffectedByFacilities.IsPotentiallyValidFacilityForMe_Static(facility, myDef, myPos, myRot);
		}

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

		public bool IsValidFacilityForMe(Thing facility)
		{
			return CompAffectedByFacilities.IsPotentiallyValidFacilityForMe_Static(facility, this.parent.def, this.parent.Position, this.parent.Rotation);
		}

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

		private static bool IsPotentiallyValidFacilityForMe_Static(Thing facility, ThingDef myDef, IntVec3 myPos, Rot4 myRot)
		{
			return CompAffectedByFacilities.IsPotentiallyValidFacilityForMe_Static(facility.def, facility.Position, facility.Rotation, myDef, myPos, myRot, facility.Map);
		}

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
					GenDraw.DrawLineBetween(this.parent.TrueCenter(), this.linkedFacilities[i].TrueCenter());
				}
				else
				{
					GenDraw.DrawLineBetween(this.parent.TrueCenter(), this.linkedFacilities[i].TrueCenter(), CompAffectedByFacilities.InactiveFacilityLineMat);
				}
			}
		}

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

		public static void DrawLinesToPotentialThingsToLinkTo(ThingDef myDef, IntVec3 myPos, Rot4 myRot, Map map)
		{
			Vector3 a = GenThing.TrueCenter(myPos, myRot, myDef.size, myDef.Altitude);
			foreach (Thing t in CompAffectedByFacilities.PotentialThingsToLinkTo(myDef, myPos, myRot, map))
			{
				GenDraw.DrawLineBetween(a, t.TrueCenter());
			}
		}

		public void DrawRedLineToPotentiallySupplantedFacility(ThingDef facilityDef, IntVec3 facilityPos, Rot4 facilityRot)
		{
			Thing potentiallySupplantedFacility = this.GetPotentiallySupplantedFacility(facilityDef, facilityPos, facilityRot);
			if (potentiallySupplantedFacility != null)
			{
				GenDraw.DrawLineBetween(this.parent.TrueCenter(), potentiallySupplantedFacility.TrueCenter(), CompAffectedByFacilities.InactiveFacilityLineMat);
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
			if (this.parent.Spawned)
			{
				foreach (Thing thing in this.ThingsICanLinkTo)
				{
					this.linkedFacilities.Add(thing);
					thing.TryGetComp<CompFacility>().Notify_NewLink(this.parent);
				}
			}
		}

		private void UnlinkAll()
		{
			for (int i = 0; i < this.linkedFacilities.Count; i++)
			{
				this.linkedFacilities[i].TryGetComp<CompFacility>().Notify_LinkRemoved(this.parent);
			}
			this.linkedFacilities.Clear();
		}

		// Note: this type is marked as 'beforefieldinit'.
		static CompAffectedByFacilities()
		{
		}

		[CompilerGenerated]
		private sealed class <>c__Iterator0 : IEnumerable, IEnumerable<Thing>, IEnumerator, IDisposable, IEnumerator<Thing>
		{
			internal IEnumerable<Thing> <potentialThings>__0;

			internal IEnumerator<Thing> $locvar0;

			internal Thing <th>__1;

			internal CompAffectedByFacilities $this;

			internal Thing $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				bool flag = false;
				switch (num)
				{
				case 0u:
					if (!this.parent.Spawned)
					{
						return false;
					}
					potentialThings = CompAffectedByFacilities.PotentialThingsToLinkTo(this.parent.def, this.parent.Position, this.parent.Rotation, this.parent.Map);
					enumerator = potentialThings.GetEnumerator();
					num = 4294967293u;
					break;
				case 1u:
					break;
				default:
					return false;
				}
				try
				{
					switch (num)
					{
					case 1u:
						IL_F9:
						break;
					}
					if (enumerator.MoveNext())
					{
						th = enumerator.Current;
						if (base.CanLinkTo(th))
						{
							this.$current = th;
							if (!this.$disposing)
							{
								this.$PC = 1;
							}
							flag = true;
							return true;
						}
						goto IL_F9;
					}
				}
				finally
				{
					if (!flag)
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
				}
				this.$PC = -1;
				return false;
			}

			Thing IEnumerator<Thing>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			[DebuggerHidden]
			public void Dispose()
			{
				uint num = (uint)this.$PC;
				this.$disposing = true;
				this.$PC = -1;
				switch (num)
				{
				case 1u:
					try
					{
					}
					finally
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
					break;
				}
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<Verse.Thing>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<Thing> IEnumerable<Thing>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				CompAffectedByFacilities.<>c__Iterator0 <>c__Iterator = new CompAffectedByFacilities.<>c__Iterator0();
				<>c__Iterator.$this = this;
				return <>c__Iterator;
			}
		}

		[CompilerGenerated]
		private sealed class <PotentialThingsToLinkTo>c__Iterator1 : IEnumerable, IEnumerable<Thing>, IEnumerator, IDisposable, IEnumerator<Thing>
		{
			internal ThingDef myDef;

			internal CompProperties_AffectedByFacilities <myProps>__0;

			internal IEnumerable<Thing> <candidates>__0;

			internal Map map;

			internal IntVec3 myPos;

			internal Rot4 myRot;

			internal IOrderedEnumerable<Thing> <sortedCandidates>__0;

			internal IEnumerator<Thing> $locvar0;

			internal Thing <th>__1;

			internal CompProperties_Facility <facilityProps>__2;

			internal Thing $current;

			internal bool $disposing;

			internal int $PC;

			private CompAffectedByFacilities.<PotentialThingsToLinkTo>c__Iterator1.<PotentialThingsToLinkTo>c__AnonStorey2 $locvar1;

			private static Func<Thing, int> <>f__am$cache0;

			private static Func<Thing, int> <>f__am$cache1;

			[DebuggerHidden]
			public <PotentialThingsToLinkTo>c__Iterator1()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				bool flag = false;
				switch (num)
				{
				case 0u:
				{
					CompAffectedByFacilities.alreadyReturnedCount.Clear();
					myProps = myDef.GetCompProperties<CompProperties_AffectedByFacilities>();
					if (myProps.linkableFacilities == null)
					{
						return false;
					}
					candidates = Enumerable.Empty<Thing>();
					for (int i = 0; i < myProps.linkableFacilities.Count; i++)
					{
						candidates = candidates.Concat(map.listerThings.ThingsOfDef(myProps.linkableFacilities[i]));
					}
					Vector3 myTrueCenter = GenThing.TrueCenter(myPos, myRot, myDef.size, myDef.Altitude);
					sortedCandidates = from x in candidates
					orderby Vector3.Distance(myTrueCenter, x.TrueCenter()), x.Position.x, x.Position.z
					select x;
					enumerator = sortedCandidates.GetEnumerator();
					num = 4294967293u;
					break;
				}
				case 1u:
					break;
				default:
					return false;
				}
				try
				{
					switch (num)
					{
					}
					while (enumerator.MoveNext())
					{
						th = enumerator.Current;
						if (CompAffectedByFacilities.CanPotentiallyLinkTo_Static(th, myDef, myPos, myRot))
						{
							facilityProps = th.def.GetCompProperties<CompProperties_Facility>();
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
							Dictionary<ThingDef, int> alreadyReturnedCount;
							ThingDef def;
							(alreadyReturnedCount = CompAffectedByFacilities.alreadyReturnedCount)[def = th.def] = alreadyReturnedCount[def] + 1;
							this.$current = th;
							if (!this.$disposing)
							{
								this.$PC = 1;
							}
							flag = true;
							return true;
						}
					}
				}
				finally
				{
					if (!flag)
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
				}
				this.$PC = -1;
				return false;
			}

			Thing IEnumerator<Thing>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			[DebuggerHidden]
			public void Dispose()
			{
				uint num = (uint)this.$PC;
				this.$disposing = true;
				this.$PC = -1;
				switch (num)
				{
				case 1u:
					try
					{
					}
					finally
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
					break;
				}
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<Verse.Thing>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<Thing> IEnumerable<Thing>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				CompAffectedByFacilities.<PotentialThingsToLinkTo>c__Iterator1 <PotentialThingsToLinkTo>c__Iterator = new CompAffectedByFacilities.<PotentialThingsToLinkTo>c__Iterator1();
				<PotentialThingsToLinkTo>c__Iterator.myDef = myDef;
				<PotentialThingsToLinkTo>c__Iterator.map = map;
				<PotentialThingsToLinkTo>c__Iterator.myPos = myPos;
				<PotentialThingsToLinkTo>c__Iterator.myRot = myRot;
				return <PotentialThingsToLinkTo>c__Iterator;
			}

			private static int <>m__0(Thing x)
			{
				return x.Position.x;
			}

			private static int <>m__1(Thing x)
			{
				return x.Position.z;
			}

			private sealed class <PotentialThingsToLinkTo>c__AnonStorey2
			{
				internal Vector3 myTrueCenter;

				internal CompAffectedByFacilities.<PotentialThingsToLinkTo>c__Iterator1 <>f__ref$1;

				public <PotentialThingsToLinkTo>c__AnonStorey2()
				{
				}

				internal float <>m__0(Thing x)
				{
					return Vector3.Distance(this.myTrueCenter, x.TrueCenter());
				}
			}
		}
	}
}
