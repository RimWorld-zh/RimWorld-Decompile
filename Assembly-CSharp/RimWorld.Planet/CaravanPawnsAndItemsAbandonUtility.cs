using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld.Planet
{
	public static class CaravanPawnsAndItemsAbandonUtility
	{
		private const float DeathChanceForPawnAbandonedToDie = 0.8f;

		private static List<Hediff> tmpHediffs = new List<Hediff>();

		public static void TryAbandonViaInterface(Thing t, Caravan caravan)
		{
			Pawn p = t as Pawn;
			if (p != null)
			{
				if (!caravan.PawnsListForReading.Any((Predicate<Pawn>)((Pawn x) => x != p && caravan.IsOwner(x))))
				{
					Messages.Message("MessageCantAbandonLastColonist".Translate(), (WorldObject)caravan, MessageSound.RejectInput);
				}
				else
				{
					Dialog_MessageBox window = Dialog_MessageBox.CreateConfirmation(CaravanPawnsAndItemsAbandonUtility.GetAbandonPawnDialogText(p, caravan), (Action)delegate()
					{
						bool flag = CaravanPawnsAndItemsAbandonUtility.WouldBeLeftToDie(p, caravan);
						PawnDiedOrDownedThoughtsUtility.TryGiveThoughts(p, default(DamageInfo?), (PawnDiedOrDownedThoughtsKind)((!flag) ? 1 : 2));
						CaravanInventoryUtility.MoveAllInventoryToSomeoneElse(p, caravan.PawnsListForReading, null);
						caravan.RemovePawn(p);
						if (flag)
						{
							if (Rand.Value < 0.800000011920929)
							{
								p.Destroy(DestroyMode.Vanish);
							}
							else
							{
								CaravanPawnsAndItemsAbandonUtility.HealIfPossible(p);
							}
						}
						Find.WorldPawns.DiscardIfUnimportant(p);
					}, true, (string)null);
					Find.WindowStack.Add(window);
				}
			}
			else
			{
				Dialog_MessageBox window2 = Dialog_MessageBox.CreateConfirmation("ConfirmAbandonItemDialog".Translate(t.Label), (Action)delegate()
				{
					Pawn ownerOf = CaravanInventoryUtility.GetOwnerOf(caravan, t);
					if (ownerOf == null)
					{
						Log.Error("Could not find owner of " + t);
					}
					else
					{
						ownerOf.inventory.innerContainer.Remove(t);
						t.Destroy(DestroyMode.Vanish);
						caravan.RecacheImmobilizedNow();
						caravan.RecacheDaysWorthOfFood();
					}
				}, true, (string)null);
				Find.WindowStack.Add(window2);
			}
		}

		public static void TryAbandonSpecificCountViaInterface(Thing t, Caravan caravan)
		{
			Find.WindowStack.Add(new Dialog_Slider("AbandonSliderText".Translate(t.LabelNoCount), 1, t.stackCount, (Action<int>)delegate(int x)
			{
				Pawn ownerOf = CaravanInventoryUtility.GetOwnerOf(caravan, t);
				if (ownerOf == null)
				{
					Log.Error("Could not find owner of " + t);
				}
				else
				{
					if (x == t.stackCount)
					{
						ownerOf.inventory.innerContainer.Remove(t);
						t.Destroy(DestroyMode.Vanish);
					}
					else
					{
						t.SplitOff(x).Destroy(DestroyMode.Vanish);
					}
					caravan.RecacheImmobilizedNow();
					caravan.RecacheDaysWorthOfFood();
				}
			}, -2147483648));
		}

		public static string GetAbandonButtonTooltip(Thing t, Caravan caravan, bool abandonSpecificCount)
		{
			StringBuilder stringBuilder = new StringBuilder();
			Pawn pawn = t as Pawn;
			if (pawn != null)
			{
				stringBuilder.Append("AbandonTip".Translate());
				if (CaravanPawnsAndItemsAbandonUtility.WouldBeLeftToDie(pawn, caravan))
				{
					stringBuilder.AppendLine();
					stringBuilder.AppendLine();
					stringBuilder.Append("AbandonTipWillDie".Translate(pawn.LabelShort).CapitalizeFirst());
				}
			}
			else
			{
				if (t.stackCount == 1)
				{
					stringBuilder.AppendLine("AbandonTip".Translate());
				}
				else if (abandonSpecificCount)
				{
					stringBuilder.AppendLine("AbandonSpecificCountTip".Translate());
				}
				else
				{
					stringBuilder.AppendLine("AbandonAllTip".Translate());
				}
				stringBuilder.AppendLine();
				stringBuilder.Append("AbandonItemTipExtraText".Translate());
			}
			return stringBuilder.ToString();
		}

		private static void HealIfPossible(Pawn p)
		{
			CaravanPawnsAndItemsAbandonUtility.tmpHediffs.Clear();
			CaravanPawnsAndItemsAbandonUtility.tmpHediffs.AddRange(p.health.hediffSet.hediffs);
			for (int i = 0; i < CaravanPawnsAndItemsAbandonUtility.tmpHediffs.Count; i++)
			{
				Hediff_Injury hediff_Injury = CaravanPawnsAndItemsAbandonUtility.tmpHediffs[i] as Hediff_Injury;
				if (hediff_Injury != null && !hediff_Injury.IsOld())
				{
					p.health.RemoveHediff(hediff_Injury);
				}
				else
				{
					ImmunityRecord immunityRecord = p.health.immunity.GetImmunityRecord(CaravanPawnsAndItemsAbandonUtility.tmpHediffs[i].def);
					if (immunityRecord != null)
					{
						immunityRecord.immunity = 1f;
					}
				}
			}
		}

		private static string GetAbandonPawnDialogText(Pawn abandonedPawn, Caravan caravan)
		{
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = CaravanPawnsAndItemsAbandonUtility.WouldBeLeftToDie(abandonedPawn, caravan);
			stringBuilder.Append("ConfirmAbandonPawnDialog".Translate(abandonedPawn.Label));
			if (flag)
			{
				stringBuilder.AppendLine();
				stringBuilder.AppendLine();
				stringBuilder.Append("ConfirmAbandonPawnDialog_LeftToDie".Translate(abandonedPawn.LabelShort).CapitalizeFirst());
			}
			List<ThingWithComps> list = (abandonedPawn.equipment == null) ? null : abandonedPawn.equipment.AllEquipmentListForReading;
			List<Apparel> list2 = (abandonedPawn.apparel == null) ? null : abandonedPawn.apparel.WornApparel;
			if (!list.NullOrEmpty() || !list2.NullOrEmpty())
			{
				stringBuilder.AppendLine();
				stringBuilder.AppendLine();
				stringBuilder.Append("ConfirmAbandonPawnDialog_EquipmentAndApparel".Translate(abandonedPawn.LabelShort).CapitalizeFirst().AdjustedFor(abandonedPawn));
				stringBuilder.AppendLine();
				if (list != null)
				{
					for (int i = 0; i < list.Count; i++)
					{
						stringBuilder.AppendLine();
						stringBuilder.Append("  - " + list[i].LabelCap);
					}
				}
				if (list2 != null)
				{
					for (int j = 0; j < list2.Count; j++)
					{
						stringBuilder.AppendLine();
						stringBuilder.Append("  - " + list2[j].LabelCap);
					}
				}
			}
			PawnDiedOrDownedThoughtsUtility.BuildMoodThoughtsListString(abandonedPawn, default(DamageInfo?), (PawnDiedOrDownedThoughtsKind)((!flag) ? 1 : 2), stringBuilder, "\n\n" + "ConfirmAbandonPawnDialog_IndividualThoughts".Translate(abandonedPawn.LabelShort), "\n\n" + "ConfirmAbandonPawnDialog_AllColonistsThoughts".Translate());
			return stringBuilder.ToString();
		}

		private static bool WouldBeLeftToDie(Pawn p, Caravan caravan)
		{
			if (p.Downed)
			{
				return true;
			}
			if (p.health.hediffSet.BleedRateTotal > 0.40000000596046448)
			{
				return true;
			}
			float f = GenTemperature.AverageTemperatureAtTileForTwelfth(caravan.Tile, GenLocalDate.Twelfth(caravan.Tile));
			if (!p.ComfortableTemperatureRange().Includes(f))
			{
				return true;
			}
			List<Hediff> hediffs = p.health.hediffSet.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				HediffStage curStage = hediffs[i].CurStage;
				if (curStage != null && curStage.lifeThreatening)
				{
					return true;
				}
			}
			return false;
		}
	}
}
