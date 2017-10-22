using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class ITab_Storage : ITab
	{
		private const float TopAreaHeight = 35f;

		private Vector2 scrollPosition = default(Vector2);

		private static readonly Vector2 WinSize = new Vector2(300f, 480f);

		private IStoreSettingsParent SelStoreSettingsParent
		{
			get
			{
				return (IStoreSettingsParent)base.SelObject;
			}
		}

		public override bool IsVisible
		{
			get
			{
				return this.SelStoreSettingsParent.StorageTabVisible;
			}
		}

		public ITab_Storage()
		{
			base.size = ITab_Storage.WinSize;
			base.labelKey = "TabStorage";
			base.tutorTag = "Storage";
		}

		protected override void FillTab()
		{
			IStoreSettingsParent selStoreSettingsParent = this.SelStoreSettingsParent;
			StorageSettings settings = selStoreSettingsParent.GetStoreSettings();
			Vector2 winSize = ITab_Storage.WinSize;
			float x = winSize.x;
			Vector2 winSize2 = ITab_Storage.WinSize;
			Rect position = new Rect(0f, 0f, x, winSize2.y).ContractedBy(10f);
			GUI.BeginGroup(position);
			Text.Font = GameFont.Small;
			Rect rect = new Rect(0f, 0f, 160f, 29f);
			if (Widgets.ButtonText(rect, "Priority".Translate() + ": " + settings.Priority.Label(), true, false, true))
			{
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				foreach (byte value in Enum.GetValues(typeof(StoragePriority)))
				{
					if (value != 0)
					{
						StoragePriority localPr = (StoragePriority)value;
						list.Add(new FloatMenuOption(localPr.Label().CapitalizeFirst(), (Action)delegate
						{
							settings.Priority = localPr;
						}, MenuOptionPriority.Default, null, null, 0f, null, null));
					}
				}
				Find.WindowStack.Add(new FloatMenu(list));
			}
			UIHighlighter.HighlightOpportunity(rect, "StoragePriority");
			ThingFilter parentFilter = null;
			if (selStoreSettingsParent.GetParentStoreSettings() != null)
			{
				parentFilter = selStoreSettingsParent.GetParentStoreSettings().filter;
			}
			Rect rect2 = new Rect(0f, 35f, position.width, (float)(position.height - 35.0));
			ThingFilterUI.DoThingFilterConfigWindow(rect2, ref this.scrollPosition, settings.filter, parentFilter, 8, (IEnumerable<ThingDef>)null, (IEnumerable<SpecialThingFilterDef>)null, (List<ThingDef>)null);
			PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.StorageTab, KnowledgeAmount.FrameDisplayed);
			GUI.EndGroup();
		}
	}
}
