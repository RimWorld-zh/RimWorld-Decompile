using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000511 RID: 1297
	public class Pawn_ApparelTracker : IThingHolder, IExposable
	{
		// Token: 0x0600174F RID: 5967 RVA: 0x000CC66B File Offset: 0x000CAA6B
		public Pawn_ApparelTracker(Pawn pawn)
		{
			this.pawn = pawn;
			this.wornApparel = new ThingOwner<Apparel>(this);
		}

		// Token: 0x17000336 RID: 822
		// (get) Token: 0x06001750 RID: 5968 RVA: 0x000CC690 File Offset: 0x000CAA90
		public IThingHolder ParentHolder
		{
			get
			{
				return this.pawn;
			}
		}

		// Token: 0x17000337 RID: 823
		// (get) Token: 0x06001751 RID: 5969 RVA: 0x000CC6AC File Offset: 0x000CAAAC
		public List<Apparel> WornApparel
		{
			get
			{
				return this.wornApparel.InnerListForReading;
			}
		}

		// Token: 0x17000338 RID: 824
		// (get) Token: 0x06001752 RID: 5970 RVA: 0x000CC6CC File Offset: 0x000CAACC
		public int WornApparelCount
		{
			get
			{
				return this.wornApparel.Count;
			}
		}

		// Token: 0x17000339 RID: 825
		// (get) Token: 0x06001753 RID: 5971 RVA: 0x000CC6EC File Offset: 0x000CAAEC
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

		// Token: 0x06001754 RID: 5972 RVA: 0x000CC7F0 File Offset: 0x000CABF0
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

		// Token: 0x06001755 RID: 5973 RVA: 0x000CC830 File Offset: 0x000CAC30
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

		// Token: 0x06001756 RID: 5974 RVA: 0x000CC8A8 File Offset: 0x000CACA8
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

		// Token: 0x06001757 RID: 5975 RVA: 0x000CC930 File Offset: 0x000CAD30
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

		// Token: 0x06001758 RID: 5976 RVA: 0x000CC9F4 File Offset: 0x000CADF4
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

		// Token: 0x06001759 RID: 5977 RVA: 0x000CCA5C File Offset: 0x000CAE5C
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

		// Token: 0x0600175A RID: 5978 RVA: 0x000CCBE7 File Offset: 0x000CAFE7
		public void Remove(Apparel ap)
		{
			this.wornApparel.Remove(ap);
		}

		// Token: 0x0600175B RID: 5979 RVA: 0x000CCBF8 File Offset: 0x000CAFF8
		public bool TryDrop(Apparel ap)
		{
			Apparel apparel;
			return this.TryDrop(ap, out apparel);
		}

		// Token: 0x0600175C RID: 5980 RVA: 0x000CCC18 File Offset: 0x000CB018
		public bool TryDrop(Apparel ap, out Apparel resultingAp)
		{
			return this.TryDrop(ap, out resultingAp, this.pawn.PositionHeld, true);
		}

		// Token: 0x0600175D RID: 5981 RVA: 0x000CCC44 File Offset: 0x000CB044
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

		// Token: 0x0600175E RID: 5982 RVA: 0x000CCC94 File Offset: 0x000CB094
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

		// Token: 0x0600175F RID: 5983 RVA: 0x000CCD13 File Offset: 0x000CB113
		public void DestroyAll(DestroyMode mode = DestroyMode.Vanish)
		{
			this.wornApparel.ClearAndDestroyContents(mode);
		}

		// Token: 0x06001760 RID: 5984 RVA: 0x000CCD24 File Offset: 0x000CB124
		public bool Contains(Thing apparel)
		{
			return this.wornApparel.Contains(apparel);
		}

		// Token: 0x06001761 RID: 5985 RVA: 0x000CCD48 File Offset: 0x000CB148
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

		// Token: 0x06001762 RID: 5986 RVA: 0x000CCE48 File Offset: 0x000CB248
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

		// Token: 0x06001763 RID: 5987 RVA: 0x000CCEDA File Offset: 0x000CB2DA
		private void SortWornApparelIntoDrawOrder()
		{
			this.wornApparel.InnerListForReading.Sort((Apparel a, Apparel b) => a.def.apparel.LastLayer.drawOrder.CompareTo(b.def.apparel.LastLayer.drawOrder));
		}

		// Token: 0x06001764 RID: 5988 RVA: 0x000CCF0C File Offset: 0x000CB30C
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

		// Token: 0x06001765 RID: 5989 RVA: 0x000CCFCC File Offset: 0x000CB3CC
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

		// Token: 0x06001766 RID: 5990 RVA: 0x000CD05C File Offset: 0x000CB45C
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

		// Token: 0x06001767 RID: 5991 RVA: 0x000CD0E8 File Offset: 0x000CB4E8
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

		// Token: 0x06001768 RID: 5992 RVA: 0x000CD112 File Offset: 0x000CB512
		private void ApparelChanged()
		{
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				this.pawn.Drawer.renderer.graphics.ResolveApparelGraphics();
				PortraitsCache.SetDirty(this.pawn);
			});
		}

		// Token: 0x06001769 RID: 5993 RVA: 0x000CD126 File Offset: 0x000CB526
		public void Notify_ApparelAdded(Apparel apparel)
		{
			this.SortWornApparelIntoDrawOrder();
			this.ApparelChanged();
		}

		// Token: 0x0600176A RID: 5994 RVA: 0x000CD138 File Offset: 0x000CB538
		public void Notify_ApparelRemoved(Apparel apparel)
		{
			this.ApparelChanged();
			if (this.pawn.outfits != null && this.pawn.outfits.forcedHandler != null)
			{
				this.pawn.outfits.forcedHandler.SetForced(apparel, false);
			}
		}

		// Token: 0x0600176B RID: 5995 RVA: 0x000CD188 File Offset: 0x000CB588
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.wornApparel;
		}

		// Token: 0x0600176C RID: 5996 RVA: 0x000CD1A3 File Offset: 0x000CB5A3
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x04000DD3 RID: 3539
		public Pawn pawn;

		// Token: 0x04000DD4 RID: 3540
		private ThingOwner<Apparel> wornApparel;

		// Token: 0x04000DD5 RID: 3541
		private int lastApparelWearoutTick = -1;

		// Token: 0x04000DD6 RID: 3542
		private const int RecordWalkedNakedTaleIntervalTicks = 60000;

		// Token: 0x04000DD7 RID: 3543
		private static List<Apparel> tmpApparelList = new List<Apparel>();

		// Token: 0x04000DD8 RID: 3544
		private static List<Apparel> tmpApparel = new List<Apparel>();
	}
}
