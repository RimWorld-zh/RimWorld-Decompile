using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000520 RID: 1312
	[StaticConstructorOnStartup]
	public static class HostilityResponseModeUtility
	{
		// Token: 0x060017E2 RID: 6114 RVA: 0x000D08C4 File Offset: 0x000CECC4
		public static Texture2D GetIcon(this HostilityResponseMode response)
		{
			Texture2D result;
			switch (response)
			{
			case HostilityResponseMode.Ignore:
				result = HostilityResponseModeUtility.IgnoreIcon;
				break;
			case HostilityResponseMode.Attack:
				result = HostilityResponseModeUtility.AttackIcon;
				break;
			case HostilityResponseMode.Flee:
				result = HostilityResponseModeUtility.FleeIcon;
				break;
			default:
				result = BaseContent.BadTex;
				break;
			}
			return result;
		}

		// Token: 0x060017E3 RID: 6115 RVA: 0x000D0918 File Offset: 0x000CED18
		public static HostilityResponseMode GetNextResponse(Pawn pawn)
		{
			HostilityResponseMode result;
			switch (pawn.playerSettings.hostilityResponse)
			{
			case HostilityResponseMode.Ignore:
				if (pawn.story != null && pawn.story.WorkTagIsDisabled(WorkTags.Violent))
				{
					result = HostilityResponseMode.Flee;
				}
				else
				{
					result = HostilityResponseMode.Attack;
				}
				break;
			case HostilityResponseMode.Attack:
				result = HostilityResponseMode.Flee;
				break;
			case HostilityResponseMode.Flee:
				result = HostilityResponseMode.Ignore;
				break;
			default:
				result = HostilityResponseMode.Ignore;
				break;
			}
			return result;
		}

		// Token: 0x060017E4 RID: 6116 RVA: 0x000D098C File Offset: 0x000CED8C
		public static string GetLabel(this HostilityResponseMode response)
		{
			return ("HostilityResponseMode_" + response).Translate();
		}

		// Token: 0x060017E5 RID: 6117 RVA: 0x000D09B8 File Offset: 0x000CEDB8
		public static void DrawResponseButton(Rect rect, Pawn pawn, bool paintable)
		{
			if (HostilityResponseModeUtility.<>f__mg$cache0 == null)
			{
				HostilityResponseModeUtility.<>f__mg$cache0 = new Func<Pawn, HostilityResponseMode>(HostilityResponseModeUtility.DrawResponseButton_GetResponse);
			}
			Func<Pawn, HostilityResponseMode> getPayload = HostilityResponseModeUtility.<>f__mg$cache0;
			if (HostilityResponseModeUtility.<>f__mg$cache1 == null)
			{
				HostilityResponseModeUtility.<>f__mg$cache1 = new Func<Pawn, IEnumerable<Widgets.DropdownMenuElement<HostilityResponseMode>>>(HostilityResponseModeUtility.DrawResponseButton_GenerateMenu);
			}
			Func<Pawn, IEnumerable<Widgets.DropdownMenuElement<HostilityResponseMode>>> menuGenerator = HostilityResponseModeUtility.<>f__mg$cache1;
			Texture2D icon = pawn.playerSettings.hostilityResponse.GetIcon();
			Widgets.Dropdown<Pawn, HostilityResponseMode>(rect, pawn, getPayload, menuGenerator, null, icon, null, null, delegate
			{
				PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.HostilityResponse, KnowledgeAmount.SpecificInteraction);
			}, paintable);
			UIHighlighter.HighlightOpportunity(rect, "HostilityResponse");
			TooltipHandler.TipRegion(rect, string.Concat(new string[]
			{
				"HostilityReponseTip".Translate(),
				"\n\n",
				"HostilityResponseCurrentMode".Translate(),
				": ",
				pawn.playerSettings.hostilityResponse.GetLabel()
			}));
		}

		// Token: 0x060017E6 RID: 6118 RVA: 0x000D0AA4 File Offset: 0x000CEEA4
		private static HostilityResponseMode DrawResponseButton_GetResponse(Pawn pawn)
		{
			return pawn.playerSettings.hostilityResponse;
		}

		// Token: 0x060017E7 RID: 6119 RVA: 0x000D0AC4 File Offset: 0x000CEEC4
		private static IEnumerable<Widgets.DropdownMenuElement<HostilityResponseMode>> DrawResponseButton_GenerateMenu(Pawn p)
		{
			IEnumerator enumerator = Enum.GetValues(typeof(HostilityResponseMode)).GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					HostilityResponseMode response = (HostilityResponseMode)enumerator.Current;
					yield return new Widgets.DropdownMenuElement<HostilityResponseMode>
					{
						option = new FloatMenuOption(response.GetLabel(), delegate()
						{
							p.playerSettings.hostilityResponse = response;
						}, MenuOptionPriority.Default, null, null, 0f, null, null),
						payload = response
					};
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
			yield break;
		}

		// Token: 0x04000E10 RID: 3600
		private static readonly Texture2D IgnoreIcon = ContentFinder<Texture2D>.Get("UI/Icons/HostilityResponse/Ignore", true);

		// Token: 0x04000E11 RID: 3601
		private static readonly Texture2D AttackIcon = ContentFinder<Texture2D>.Get("UI/Icons/HostilityResponse/Attack", true);

		// Token: 0x04000E12 RID: 3602
		private static readonly Texture2D FleeIcon = ContentFinder<Texture2D>.Get("UI/Icons/HostilityResponse/Flee", true);

		// Token: 0x04000E13 RID: 3603
		[CompilerGenerated]
		private static Func<Pawn, HostilityResponseMode> <>f__mg$cache0;

		// Token: 0x04000E14 RID: 3604
		[CompilerGenerated]
		private static Func<Pawn, IEnumerable<Widgets.DropdownMenuElement<HostilityResponseMode>>> <>f__mg$cache1;
	}
}
