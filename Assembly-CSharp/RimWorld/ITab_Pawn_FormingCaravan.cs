using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200084D RID: 2125
	public class ITab_Pawn_FormingCaravan : ITab
	{
		// Token: 0x06003011 RID: 12305 RVA: 0x001A1C15 File Offset: 0x001A0015
		public ITab_Pawn_FormingCaravan()
		{
			this.size = new Vector2(480f, 450f);
			this.labelKey = "TabFormingCaravan";
		}

		// Token: 0x170007AA RID: 1962
		// (get) Token: 0x06003012 RID: 12306 RVA: 0x001A1C4C File Offset: 0x001A004C
		public override bool IsVisible
		{
			get
			{
				return base.SelPawn.IsFormingCaravan();
			}
		}

		// Token: 0x06003013 RID: 12307 RVA: 0x001A1C6C File Offset: 0x001A006C
		protected override void FillTab()
		{
			this.thingsToSelect.Clear();
			Rect outRect = new Rect(default(Vector2), this.size).ContractedBy(10f);
			outRect.yMin += 20f;
			Rect rect = new Rect(0f, 0f, outRect.width - 16f, Mathf.Max(this.lastDrawnHeight, outRect.height));
			Widgets.BeginScrollView(outRect, ref this.scrollPosition, rect, true);
			float num = 0f;
			string status = ((LordJob_FormAndSendCaravan)base.SelPawn.GetLord().LordJob).Status;
			Widgets.Label(new Rect(0f, num, rect.width, 100f), status);
			num += 22f;
			num += 4f;
			this.DoPeopleAndAnimals(rect, ref num);
			num += 4f;
			this.DoItemsLists(rect, ref num);
			this.lastDrawnHeight = num;
			Widgets.EndScrollView();
			if (this.thingsToSelect.Any<Thing>())
			{
				this.SelectNow(this.thingsToSelect);
				this.thingsToSelect.Clear();
			}
		}

		// Token: 0x06003014 RID: 12308 RVA: 0x001A1D98 File Offset: 0x001A0198
		public override void TabUpdate()
		{
			base.TabUpdate();
			if (base.SelPawn != null && base.SelPawn.GetLord() != null)
			{
				Lord lord = base.SelPawn.GetLord();
				for (int i = 0; i < lord.ownedPawns.Count; i++)
				{
					TargetHighlighter.Highlight(lord.ownedPawns[i], false, false, true);
				}
			}
		}

		// Token: 0x06003015 RID: 12309 RVA: 0x001A1E0C File Offset: 0x001A020C
		private void DoPeopleAndAnimals(Rect inRect, ref float curY)
		{
			Widgets.ListSeparator(ref curY, inRect.width, "CaravanMembers".Translate());
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			int num5 = 0;
			int num6 = 0;
			int num7 = 0;
			Lord lord = base.SelPawn.GetLord();
			for (int i = 0; i < lord.ownedPawns.Count; i++)
			{
				Pawn pawn = lord.ownedPawns[i];
				if (pawn.IsFreeColonist)
				{
					num++;
					if (pawn.InMentalState)
					{
						num2++;
					}
				}
				else if (pawn.IsPrisoner)
				{
					num3++;
					if (pawn.InMentalState)
					{
						num4++;
					}
				}
				else if (pawn.RaceProps.Animal)
				{
					num5++;
					if (pawn.InMentalState)
					{
						num6++;
					}
					if (pawn.RaceProps.packAnimal)
					{
						num7++;
					}
				}
			}
			string pawnsCountLabel = this.GetPawnsCountLabel(num, num2, -1);
			string pawnsCountLabel2 = this.GetPawnsCountLabel(num3, num4, -1);
			string pawnsCountLabel3 = this.GetPawnsCountLabel(num5, num6, num7);
			float y = curY;
			float num8;
			this.DoPeopleAndAnimalsEntry(inRect, Faction.OfPlayer.def.pawnsPlural.CapitalizeFirst(), pawnsCountLabel, ref curY, out num8);
			float y2 = curY;
			float num9;
			this.DoPeopleAndAnimalsEntry(inRect, "CaravanPrisoners".Translate(), pawnsCountLabel2, ref curY, out num9);
			float y3 = curY;
			float num10;
			this.DoPeopleAndAnimalsEntry(inRect, "CaravanAnimals".Translate(), pawnsCountLabel3, ref curY, out num10);
			float width = Mathf.Max(new float[]
			{
				num8,
				num9,
				num10
			}) + 2f;
			Rect rect = new Rect(0f, y, width, 22f);
			if (Mouse.IsOver(rect))
			{
				Widgets.DrawHighlight(rect);
				this.HighlightColonists();
			}
			if (Widgets.ButtonInvisible(rect, false))
			{
				this.SelectColonistsLater();
			}
			Rect rect2 = new Rect(0f, y2, width, 22f);
			if (Mouse.IsOver(rect2))
			{
				Widgets.DrawHighlight(rect2);
				this.HighlightPrisoners();
			}
			if (Widgets.ButtonInvisible(rect2, false))
			{
				this.SelectPrisonersLater();
			}
			Rect rect3 = new Rect(0f, y3, width, 22f);
			if (Mouse.IsOver(rect3))
			{
				Widgets.DrawHighlight(rect3);
				this.HighlightAnimals();
			}
			if (Widgets.ButtonInvisible(rect3, false))
			{
				this.SelectAnimalsLater();
			}
		}

		// Token: 0x06003016 RID: 12310 RVA: 0x001A207C File Offset: 0x001A047C
		private void DoPeopleAndAnimalsEntry(Rect inRect, string leftLabel, string rightLabel, ref float curY, out float drawnWidth)
		{
			Rect rect = new Rect(0f, curY, inRect.width, 100f);
			Widgets.Label(rect, leftLabel);
			rect.xMin += 120f;
			Widgets.Label(rect, rightLabel);
			curY += 22f;
			drawnWidth = 120f + Text.CalcSize(rightLabel).x;
		}

		// Token: 0x06003017 RID: 12311 RVA: 0x001A20EC File Offset: 0x001A04EC
		private void DoItemsLists(Rect inRect, ref float curY)
		{
			LordJob_FormAndSendCaravan lordJob_FormAndSendCaravan = (LordJob_FormAndSendCaravan)base.SelPawn.GetLord().LordJob;
			Rect position = new Rect(0f, curY, (inRect.width - 10f) / 2f, inRect.height);
			float a = 0f;
			GUI.BeginGroup(position);
			Widgets.ListSeparator(ref a, position.width, "ItemsToLoad".Translate());
			bool flag = false;
			for (int i = 0; i < lordJob_FormAndSendCaravan.transferables.Count; i++)
			{
				TransferableOneWay transferableOneWay = lordJob_FormAndSendCaravan.transferables[i];
				if (transferableOneWay.CountToTransfer > 0 && transferableOneWay.HasAnyThing)
				{
					flag = true;
					this.DoThingRow(transferableOneWay.ThingDef, transferableOneWay.CountToTransfer, transferableOneWay.things, position.width, ref a);
				}
			}
			if (!flag)
			{
				Widgets.NoneLabel(ref a, position.width, null);
			}
			GUI.EndGroup();
			Rect position2 = new Rect((inRect.width + 10f) / 2f, curY, (inRect.width - 10f) / 2f, inRect.height);
			float b = 0f;
			GUI.BeginGroup(position2);
			Widgets.ListSeparator(ref b, position2.width, "LoadedItems".Translate());
			bool flag2 = false;
			for (int j = 0; j < lordJob_FormAndSendCaravan.lord.ownedPawns.Count; j++)
			{
				Pawn pawn = lordJob_FormAndSendCaravan.lord.ownedPawns[j];
				if (!pawn.inventory.UnloadEverything)
				{
					for (int k = 0; k < pawn.inventory.innerContainer.Count; k++)
					{
						Thing thing = pawn.inventory.innerContainer[k];
						flag2 = true;
						ITab_Pawn_FormingCaravan.tmpSingleThing.Clear();
						ITab_Pawn_FormingCaravan.tmpSingleThing.Add(thing);
						this.DoThingRow(thing.def, thing.stackCount, ITab_Pawn_FormingCaravan.tmpSingleThing, position2.width, ref b);
						ITab_Pawn_FormingCaravan.tmpSingleThing.Clear();
					}
				}
			}
			if (!flag2)
			{
				Widgets.NoneLabel(ref b, position.width, null);
			}
			GUI.EndGroup();
			curY += Mathf.Max(a, b);
		}

		// Token: 0x06003018 RID: 12312 RVA: 0x001A2348 File Offset: 0x001A0748
		private void SelectColonistsLater()
		{
			Lord lord = base.SelPawn.GetLord();
			ITab_Pawn_FormingCaravan.tmpPawns.Clear();
			for (int i = 0; i < lord.ownedPawns.Count; i++)
			{
				if (lord.ownedPawns[i].IsFreeColonist)
				{
					ITab_Pawn_FormingCaravan.tmpPawns.Add(lord.ownedPawns[i]);
				}
			}
			this.SelectLater(ITab_Pawn_FormingCaravan.tmpPawns);
			ITab_Pawn_FormingCaravan.tmpPawns.Clear();
		}

		// Token: 0x06003019 RID: 12313 RVA: 0x001A23CC File Offset: 0x001A07CC
		private void HighlightColonists()
		{
			Lord lord = base.SelPawn.GetLord();
			for (int i = 0; i < lord.ownedPawns.Count; i++)
			{
				if (lord.ownedPawns[i].IsFreeColonist)
				{
					TargetHighlighter.Highlight(lord.ownedPawns[i], true, true, false);
				}
			}
		}

		// Token: 0x0600301A RID: 12314 RVA: 0x001A2434 File Offset: 0x001A0834
		private void SelectPrisonersLater()
		{
			Lord lord = base.SelPawn.GetLord();
			ITab_Pawn_FormingCaravan.tmpPawns.Clear();
			for (int i = 0; i < lord.ownedPawns.Count; i++)
			{
				if (lord.ownedPawns[i].IsPrisoner)
				{
					ITab_Pawn_FormingCaravan.tmpPawns.Add(lord.ownedPawns[i]);
				}
			}
			this.SelectLater(ITab_Pawn_FormingCaravan.tmpPawns);
			ITab_Pawn_FormingCaravan.tmpPawns.Clear();
		}

		// Token: 0x0600301B RID: 12315 RVA: 0x001A24B8 File Offset: 0x001A08B8
		private void HighlightPrisoners()
		{
			Lord lord = base.SelPawn.GetLord();
			for (int i = 0; i < lord.ownedPawns.Count; i++)
			{
				if (lord.ownedPawns[i].IsPrisoner)
				{
					TargetHighlighter.Highlight(lord.ownedPawns[i], true, true, false);
				}
			}
		}

		// Token: 0x0600301C RID: 12316 RVA: 0x001A2520 File Offset: 0x001A0920
		private void SelectAnimalsLater()
		{
			Lord lord = base.SelPawn.GetLord();
			ITab_Pawn_FormingCaravan.tmpPawns.Clear();
			for (int i = 0; i < lord.ownedPawns.Count; i++)
			{
				if (lord.ownedPawns[i].RaceProps.Animal)
				{
					ITab_Pawn_FormingCaravan.tmpPawns.Add(lord.ownedPawns[i]);
				}
			}
			this.SelectLater(ITab_Pawn_FormingCaravan.tmpPawns);
			ITab_Pawn_FormingCaravan.tmpPawns.Clear();
		}

		// Token: 0x0600301D RID: 12317 RVA: 0x001A25A8 File Offset: 0x001A09A8
		private void HighlightAnimals()
		{
			Lord lord = base.SelPawn.GetLord();
			for (int i = 0; i < lord.ownedPawns.Count; i++)
			{
				if (lord.ownedPawns[i].RaceProps.Animal)
				{
					TargetHighlighter.Highlight(lord.ownedPawns[i], true, true, false);
				}
			}
		}

		// Token: 0x0600301E RID: 12318 RVA: 0x001A2614 File Offset: 0x001A0A14
		private void SelectLater(List<Thing> things)
		{
			this.thingsToSelect.Clear();
			this.thingsToSelect.AddRange(things);
		}

		// Token: 0x0600301F RID: 12319 RVA: 0x001A2630 File Offset: 0x001A0A30
		private void SelectNow(List<Thing> things)
		{
			if (things.Any<Thing>())
			{
				Find.Selector.ClearSelection();
				bool flag = false;
				for (int i = 0; i < things.Count; i++)
				{
					if (things[i].Spawned)
					{
						if (!flag)
						{
							CameraJumper.TryJump(things[i]);
						}
						Find.Selector.Select(things[i], true, true);
						flag = true;
					}
				}
				if (!flag)
				{
					CameraJumper.TryJumpAndSelect(things[0]);
				}
			}
		}

		// Token: 0x06003020 RID: 12320 RVA: 0x001A26CC File Offset: 0x001A0ACC
		private string GetPawnsCountLabel(int count, int countInMentalState, int countPackAnimals)
		{
			string text = count.ToString();
			bool flag = countInMentalState > 0;
			bool flag2 = count > 0 && countPackAnimals >= 0;
			if (flag || flag2)
			{
				text += " (";
				if (flag2)
				{
					text = text + countPackAnimals.ToString() + " " + "PackAnimalsCountLower".Translate();
				}
				if (flag)
				{
					if (flag2)
					{
						text += ", ";
					}
					text = text + countInMentalState.ToString() + " " + "InMentalStateCountLower".Translate();
				}
				text += ")";
			}
			return text;
		}

		// Token: 0x06003021 RID: 12321 RVA: 0x001A2794 File Offset: 0x001A0B94
		private void DoThingRow(ThingDef thingDef, int count, List<Thing> things, float width, ref float curY)
		{
			Rect rect = new Rect(0f, curY, width, 28f);
			if (things.Count == 1)
			{
				Widgets.InfoCardButton(rect.width - 24f, curY, things[0]);
			}
			else
			{
				Widgets.InfoCardButton(rect.width - 24f, curY, thingDef);
			}
			rect.width -= 24f;
			if (Mouse.IsOver(rect))
			{
				GUI.color = ITab_Pawn_FormingCaravan.ThingHighlightColor;
				GUI.DrawTexture(rect, TexUI.HighlightTex);
			}
			if (thingDef.DrawMatSingle != null && thingDef.DrawMatSingle.mainTexture != null)
			{
				Rect rect2 = new Rect(4f, curY, 28f, 28f);
				if (things.Count == 1)
				{
					Widgets.ThingIcon(rect2, things[0], 1f);
				}
				else
				{
					Widgets.ThingIcon(rect2, thingDef);
				}
			}
			Text.Anchor = TextAnchor.MiddleLeft;
			GUI.color = ITab_Pawn_FormingCaravan.ThingLabelColor;
			Rect rect3 = new Rect(36f, curY, rect.width - 36f, rect.height);
			string str;
			if (things.Count == 1)
			{
				str = things[0].LabelCap;
			}
			else
			{
				str = GenLabel.ThingLabel(thingDef, null, count).CapitalizeFirst();
			}
			Text.WordWrap = false;
			Widgets.Label(rect3, str.Truncate(rect3.width, null));
			Text.WordWrap = true;
			Text.Anchor = TextAnchor.UpperLeft;
			TooltipHandler.TipRegion(rect, str);
			if (Widgets.ButtonInvisible(rect, false))
			{
				this.SelectLater(things);
			}
			if (Mouse.IsOver(rect))
			{
				for (int i = 0; i < things.Count; i++)
				{
					TargetHighlighter.Highlight(things[i], true, true, false);
				}
			}
			curY += 28f;
		}

		// Token: 0x040019FE RID: 6654
		private Vector2 scrollPosition;

		// Token: 0x040019FF RID: 6655
		private float lastDrawnHeight;

		// Token: 0x04001A00 RID: 6656
		private List<Thing> thingsToSelect = new List<Thing>();

		// Token: 0x04001A01 RID: 6657
		private static List<Thing> tmpSingleThing = new List<Thing>();

		// Token: 0x04001A02 RID: 6658
		private const float TopPadding = 20f;

		// Token: 0x04001A03 RID: 6659
		private const float StandardLineHeight = 22f;

		// Token: 0x04001A04 RID: 6660
		private const float ExtraSpaceBetweenSections = 4f;

		// Token: 0x04001A05 RID: 6661
		private const float SpaceBetweenItemsLists = 10f;

		// Token: 0x04001A06 RID: 6662
		private const float ThingRowHeight = 28f;

		// Token: 0x04001A07 RID: 6663
		private const float ThingIconSize = 28f;

		// Token: 0x04001A08 RID: 6664
		private const float ThingLeftX = 36f;

		// Token: 0x04001A09 RID: 6665
		private static readonly Color ThingLabelColor = ITab_Pawn_Gear.ThingLabelColor;

		// Token: 0x04001A0A RID: 6666
		private static readonly Color ThingHighlightColor = ITab_Pawn_Gear.HighlightColor;

		// Token: 0x04001A0B RID: 6667
		private static List<Thing> tmpPawns = new List<Thing>();
	}
}
