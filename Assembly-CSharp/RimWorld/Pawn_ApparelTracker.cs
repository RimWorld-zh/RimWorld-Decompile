using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200050F RID: 1295
	public class Pawn_ApparelTracker : IThingHolder, IExposable
	{
		// Token: 0x04000DD0 RID: 3536
		public Pawn pawn;

		// Token: 0x04000DD1 RID: 3537
		private ThingOwner<Apparel> wornApparel;

		// Token: 0x04000DD2 RID: 3538
		private int lastApparelWearoutTick = -1;

		// Token: 0x04000DD3 RID: 3539
		private const int RecordWalkedNakedTaleIntervalTicks = 60000;

		// Token: 0x04000DD4 RID: 3540
		private static List<Apparel> tmpApparelList = new List<Apparel>();

		// Token: 0x04000DD5 RID: 3541
		private static List<Apparel> tmpApparel = new List<Apparel>();

		// Token: 0x0600174A RID: 5962 RVA: 0x000CC7B3 File Offset: 0x000CABB3
		public Pawn_ApparelTracker(Pawn pawn)
		{
			this.pawn = pawn;
			this.wornApparel = new ThingOwner<Apparel>(this);
		}

		// Token: 0x17000336 RID: 822
		// (get) Token: 0x0600174B RID: 5963 RVA: 0x000CC7D8 File Offset: 0x000CABD8
		public IThingHolder ParentHolder
		{
			get
			{
				return this.pawn;
			}
		}

		// Token: 0x17000337 RID: 823
		// (get) Token: 0x0600174C RID: 5964 RVA: 0x000CC7F4 File Offset: 0x000CABF4
		public List<Apparel> WornApparel
		{
			get
			{
				return this.wornApparel.InnerListForReading;
			}
		}

		// Token: 0x17000338 RID: 824
		// (get) Token: 0x0600174D RID: 5965 RVA: 0x000CC814 File Offset: 0x000CAC14
		public int WornApparelCount
		{
			get
			{
				return this.wornApparel.Count;
			}
		}

		// Token: 0x17000339 RID: 825
		// (get) Token: 0x0600174E RID: 5966 RVA: 0x000CC834 File Offset: 0x000CAC34
		public bool PsychologicallyNude
		{
			get
			{
				bool result;
				if (this.pawn.gender == Gender.None)
				{
					result = false;
				}
				else
				{
					bool flag;
					bool flag2;
					this.HasBasicApparel(out flag, out flag2);
					if (!flag)
					{
						bool flag3 = false;
						foreach (BodyPartRecord bodyPartRecord in this.pawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null))
						{
							if (bodyPartRecord.IsInGroup(BodyPartGroupDefOf.Legs))
							{
								flag3 = true;
								break;
							}
						}
						if (!flag3)
						{
							flag = true;
						}
					}
					if (this.pawn.gender == Gender.Male)
					{
						result = !flag;
					}
					else
					{
						result = (this.pawn.gender == Gender.Female && (!flag || !flag2));
					}
				}
				return result;
			}
		}

		// Token: 0x0600174F RID: 5967 RVA: 0x000CC938 File Offset: 0x000CAD38
		public void ExposeData()
		{
			Scribe_Deep.Look<ThingOwner<Apparel>>(ref this.wornApparel, "wornApparel", new object[]
			{
				this
			});
			Scribe_Values.Look<int>(ref this.lastApparelWearoutTick, "lastApparelWearoutTick", 0, false);
			if (Scribe.mode == LoadSaveMode.ResolvingCrossRefs)
			{
				this.SortWornApparelIntoDrawOrder();
			}
		}

		// Token: 0x06001750 RID: 5968 RVA: 0x000CC978 File Offset: 0x000CAD78
		public void ApparelTrackerTickRare()
		{
			int ticksGame = Find.TickManager.TicksGame;
			if (this.lastApparelWearoutTick < 0)
			{
				this.lastApparelWearoutTick = ticksGame;
			}
			if (ticksGame - this.lastApparelWearoutTick >= 60000)
			{
				for (int i = 0; i < this.wornApparel.Count; i++)
				{
					this.TakeWearoutDamageForDay(this.wornApparel[i]);
				}
				this.lastApparelWearoutTick = ticksGame;
			}
		}

		// Token: 0x06001751 RID: 5969 RVA: 0x000CC9F0 File Offset: 0x000CADF0
		public void ApparelTrackerTick()
		{
			this.wornApparel.ThingOwnerTick(true);
			if (this.pawn.IsColonist && this.pawn.Spawned && !this.pawn.Dead && this.pawn.IsHashIntervalTick(60000) && this.PsychologicallyNude)
			{
				TaleRecorder.RecordTale(TaleDefOf.WalkedNaked, new object[]
				{
					this.pawn
				});
			}
		}

		// Token: 0x06001752 RID: 5970 RVA: 0x000CCA78 File Offset: 0x000CAE78
		private void TakeWearoutDamageForDay(Thing ap)
		{
			int num = GenMath.RoundRandom(ap.def.apparel.wearPerDay);
			if (num > 0)
			{
				ap.TakeDamage(new DamageInfo(DamageDefOf.Deterioration, (float)num, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null));
			}
			if (ap.Destroyed && PawnUtility.ShouldSendNotificationAbout(this.pawn) && !this.pawn.Dead)
			{
				string text = "MessageWornApparelDeterioratedAway".Translate(new object[]
				{
					GenLabel.ThingLabel(ap.def, ap.Stuff, 1),
					this.pawn
				});
				text = text.CapitalizeFirst();
				Messages.Message(text, this.pawn, MessageTypeDefOf.NegativeEvent, true);
			}
		}

		// Token: 0x06001753 RID: 5971 RVA: 0x000CCB3C File Offset: 0x000CAF3C
		public bool CanWearWithoutDroppingAnything(ThingDef apDef)
		{
			for (int i = 0; i < this.wornApparel.Count; i++)
			{
				if (!ApparelUtility.CanWearTogether(apDef, this.wornApparel[i].def, this.pawn.RaceProps.body))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06001754 RID: 5972 RVA: 0x000CCBA4 File Offset: 0x000CAFA4
		public void Wear(Apparel newApparel, bool dropReplacedApparel = true)
		{
			if (newApparel.Spawned)
			{
				newApparel.DeSpawn(DestroyMode.Vanish);
			}
			if (!ApparelUtility.HasPartsToWear(this.pawn, newApparel.def))
			{
				Log.Warning(string.Concat(new object[]
				{
					this.pawn,
					" tried to wear ",
					newApparel,
					" but he has no body parts required to wear it."
				}), false);
			}
			else
			{
				for (int i = this.wornApparel.Count - 1; i >= 0; i--)
				{
					Apparel apparel = this.wornApparel[i];
					if (!ApparelUtility.CanWearTogether(newApparel.def, apparel.def, this.pawn.RaceProps.body))
					{
						if (dropReplacedApparel)
						{
							bool forbid = this.pawn.Faction != null && this.pawn.Faction.HostileTo(Faction.OfPlayer);
							Apparel apparel2;
							if (!this.TryDrop(apparel, out apparel2, this.pawn.PositionHeld, forbid))
							{
								Log.Error(this.pawn + " could not drop " + apparel, false);
								return;
							}
						}
						else
						{
							this.Remove(apparel);
						}
					}
				}
				if (newApparel.Wearer != null)
				{
					Log.Warning(string.Concat(new object[]
					{
						this.pawn,
						" is trying to wear ",
						newApparel,
						" but this apparel already has a wearer (",
						newApparel.Wearer,
						"). This may or may not cause bugs."
					}), false);
				}
				this.wornApparel.TryAdd(newApparel, false);
			}
		}

		// Token: 0x06001755 RID: 5973 RVA: 0x000CCD2F File Offset: 0x000CB12F
		public void Remove(Apparel ap)
		{
			this.wornApparel.Remove(ap);
		}

		// Token: 0x06001756 RID: 5974 RVA: 0x000CCD40 File Offset: 0x000CB140
		public bool TryDrop(Apparel ap)
		{
			Apparel apparel;
			return this.TryDrop(ap, out apparel);
		}

		// Token: 0x06001757 RID: 5975 RVA: 0x000CCD60 File Offset: 0x000CB160
		public bool TryDrop(Apparel ap, out Apparel resultingAp)
		{
			return this.TryDrop(ap, out resultingAp, this.pawn.PositionHeld, true);
		}

		// Token: 0x06001758 RID: 5976 RVA: 0x000CCD8C File Offset: 0x000CB18C
		public bool TryDrop(Apparel ap, out Apparel resultingAp, IntVec3 pos, bool forbid = true)
		{
			bool result;
			if (this.wornApparel.TryDrop(ap, pos, this.pawn.MapHeld, ThingPlaceMode.Near, out resultingAp, null, null))
			{
				if (resultingAp != null)
				{
					resultingAp.SetForbidden(forbid, false);
				}
				result = true;
			}
			else
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06001759 RID: 5977 RVA: 0x000CCDDC File Offset: 0x000CB1DC
		public void DropAll(IntVec3 pos, bool forbid = true)
		{
			Pawn_ApparelTracker.tmpApparelList.Clear();
			for (int i = 0; i < this.wornApparel.Count; i++)
			{
				Pawn_ApparelTracker.tmpApparelList.Add(this.wornApparel[i]);
			}
			for (int j = 0; j < Pawn_ApparelTracker.tmpApparelList.Count; j++)
			{
				Apparel apparel;
				this.TryDrop(Pawn_ApparelTracker.tmpApparelList[j], out apparel, pos, forbid);
			}
		}

		// Token: 0x0600175A RID: 5978 RVA: 0x000CCE5B File Offset: 0x000CB25B
		public void DestroyAll(DestroyMode mode = DestroyMode.Vanish)
		{
			this.wornApparel.ClearAndDestroyContents(mode);
		}

		// Token: 0x0600175B RID: 5979 RVA: 0x000CCE6C File Offset: 0x000CB26C
		public bool Contains(Thing apparel)
		{
			return this.wornApparel.Contains(apparel);
		}

		// Token: 0x0600175C RID: 5980 RVA: 0x000CCE90 File Offset: 0x000CB290
		public void Notify_PawnKilled(DamageInfo? dinfo)
		{
			if (dinfo != null && dinfo.Value.Def.externalViolence)
			{
				for (int i = 0; i < this.wornApparel.Count; i++)
				{
					if (this.wornApparel[i].def.useHitPoints)
					{
						int num = Mathf.RoundToInt((float)this.wornApparel[i].HitPoints * Rand.Range(0.15f, 0.4f));
						this.wornApparel[i].TakeDamage(new DamageInfo(dinfo.Value.Def, (float)num, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null));
					}
				}
			}
			for (int j = 0; j < this.wornApparel.Count; j++)
			{
				this.wornApparel[j].Notify_PawnKilled();
			}
		}

		// Token: 0x0600175D RID: 5981 RVA: 0x000CCF90 File Offset: 0x000CB390
		public void Notify_LostBodyPart()
		{
			Pawn_ApparelTracker.tmpApparel.Clear();
			for (int i = 0; i < this.wornApparel.Count; i++)
			{
				Pawn_ApparelTracker.tmpApparel.Add(this.wornApparel[i]);
			}
			for (int j = 0; j < Pawn_ApparelTracker.tmpApparel.Count; j++)
			{
				Apparel apparel = Pawn_ApparelTracker.tmpApparel[j];
				if (!ApparelUtility.HasPartsToWear(this.pawn, apparel.def))
				{
					this.Remove(apparel);
				}
			}
		}

		// Token: 0x0600175E RID: 5982 RVA: 0x000CD022 File Offset: 0x000CB422
		private void SortWornApparelIntoDrawOrder()
		{
			this.wornApparel.InnerListForReading.Sort((Apparel a, Apparel b) => a.def.apparel.LastLayer.drawOrder.CompareTo(b.def.apparel.LastLayer.drawOrder));
		}

		// Token: 0x0600175F RID: 5983 RVA: 0x000CD054 File Offset: 0x000CB454
		public void HasBasicApparel(out bool hasPants, out bool hasShirt)
		{
			hasShirt = false;
			hasPants = false;
			for (int i = 0; i < this.wornApparel.Count; i++)
			{
				Apparel apparel = this.wornApparel[i];
				for (int j = 0; j < apparel.def.apparel.bodyPartGroups.Count; j++)
				{
					if (apparel.def.apparel.bodyPartGroups[j] == BodyPartGroupDefOf.Torso)
					{
						hasShirt = true;
					}
					if (apparel.def.apparel.bodyPartGroups[j] == BodyPartGroupDefOf.Legs)
					{
						hasPants = true;
					}
					if (hasShirt && hasPants)
					{
						return;
					}
				}
			}
		}

		// Token: 0x06001760 RID: 5984 RVA: 0x000CD114 File Offset: 0x000CB514
		public Apparel FirstApparelOnBodyPartGroup(BodyPartGroupDef g)
		{
			for (int i = 0; i < this.wornApparel.Count; i++)
			{
				Apparel apparel = this.wornApparel[i];
				for (int j = 0; j < apparel.def.apparel.bodyPartGroups.Count; j++)
				{
					if (apparel.def.apparel.bodyPartGroups[j] == BodyPartGroupDefOf.Torso)
					{
						return apparel;
					}
				}
			}
			return null;
		}

		// Token: 0x06001761 RID: 5985 RVA: 0x000CD1A4 File Offset: 0x000CB5A4
		public bool BodyPartGroupIsCovered(BodyPartGroupDef bp)
		{
			for (int i = 0; i < this.wornApparel.Count; i++)
			{
				Apparel apparel = this.wornApparel[i];
				for (int j = 0; j < apparel.def.apparel.bodyPartGroups.Count; j++)
				{
					if (apparel.def.apparel.bodyPartGroups[j] == bp)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06001762 RID: 5986 RVA: 0x000CD230 File Offset: 0x000CB630
		public IEnumerable<Gizmo> GetGizmos()
		{
			for (int i = 0; i < this.wornApparel.Count; i++)
			{
				foreach (Gizmo g in this.wornApparel[i].GetWornGizmos())
				{
					yield return g;
				}
			}
			yield break;
		}

		// Token: 0x06001763 RID: 5987 RVA: 0x000CD25A File Offset: 0x000CB65A
		private void ApparelChanged()
		{
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				this.pawn.Drawer.renderer.graphics.ResolveApparelGraphics();
				PortraitsCache.SetDirty(this.pawn);
			});
		}

		// Token: 0x06001764 RID: 5988 RVA: 0x000CD26E File Offset: 0x000CB66E
		public void Notify_ApparelAdded(Apparel apparel)
		{
			this.SortWornApparelIntoDrawOrder();
			this.ApparelChanged();
		}

		// Token: 0x06001765 RID: 5989 RVA: 0x000CD280 File Offset: 0x000CB680
		public void Notify_ApparelRemoved(Apparel apparel)
		{
			this.ApparelChanged();
			if (this.pawn.outfits != null && this.pawn.outfits.forcedHandler != null)
			{
				this.pawn.outfits.forcedHandler.SetForced(apparel, false);
			}
		}

		// Token: 0x06001766 RID: 5990 RVA: 0x000CD2D0 File Offset: 0x000CB6D0
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.wornApparel;
		}

		// Token: 0x06001767 RID: 5991 RVA: 0x000CD2EB File Offset: 0x000CB6EB
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}
	}
}
