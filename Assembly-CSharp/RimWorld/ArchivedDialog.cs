using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020002F6 RID: 758
	public class ArchivedDialog : IArchivable, IExposable
	{
		// Token: 0x0400083B RID: 2107
		public string text;

		// Token: 0x0400083C RID: 2108
		public string title;

		// Token: 0x0400083D RID: 2109
		public Faction relatedFaction;

		// Token: 0x0400083E RID: 2110
		public int createdTick;

		// Token: 0x06000C93 RID: 3219 RVA: 0x0006F64E File Offset: 0x0006DA4E
		public ArchivedDialog()
		{
		}

		// Token: 0x06000C94 RID: 3220 RVA: 0x0006F657 File Offset: 0x0006DA57
		public ArchivedDialog(string text, string title = null, Faction relatedFaction = null)
		{
			this.text = text;
			this.title = title;
			this.relatedFaction = relatedFaction;
			this.createdTick = GenTicks.TicksGame;
		}

		// Token: 0x170001DE RID: 478
		// (get) Token: 0x06000C95 RID: 3221 RVA: 0x0006F680 File Offset: 0x0006DA80
		Texture IArchivable.ArchivedIcon
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170001DF RID: 479
		// (get) Token: 0x06000C96 RID: 3222 RVA: 0x0006F698 File Offset: 0x0006DA98
		Color IArchivable.ArchivedIconColor
		{
			get
			{
				return Color.white;
			}
		}

		// Token: 0x170001E0 RID: 480
		// (get) Token: 0x06000C97 RID: 3223 RVA: 0x0006F6B4 File Offset: 0x0006DAB4
		string IArchivable.ArchivedLabel
		{
			get
			{
				return this.text.Flatten();
			}
		}

		// Token: 0x170001E1 RID: 481
		// (get) Token: 0x06000C98 RID: 3224 RVA: 0x0006F6D4 File Offset: 0x0006DAD4
		string IArchivable.ArchivedTooltip
		{
			get
			{
				return this.text;
			}
		}

		// Token: 0x170001E2 RID: 482
		// (get) Token: 0x06000C99 RID: 3225 RVA: 0x0006F6F0 File Offset: 0x0006DAF0
		int IArchivable.CreatedTicksGame
		{
			get
			{
				return this.createdTick;
			}
		}

		// Token: 0x170001E3 RID: 483
		// (get) Token: 0x06000C9A RID: 3226 RVA: 0x0006F70C File Offset: 0x0006DB0C
		bool IArchivable.CanCullArchivedNow
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170001E4 RID: 484
		// (get) Token: 0x06000C9B RID: 3227 RVA: 0x0006F724 File Offset: 0x0006DB24
		LookTargets IArchivable.LookTargets
		{
			get
			{
				return null;
			}
		}

		// Token: 0x06000C9C RID: 3228 RVA: 0x0006F73C File Offset: 0x0006DB3C
		void IArchivable.OpenArchived()
		{
			DiaNode diaNode = new DiaNode(this.text);
			DiaOption diaOption = new DiaOption("OK".Translate());
			diaOption.resolveTree = true;
			diaNode.options.Add(diaOption);
			WindowStack windowStack = Find.WindowStack;
			DiaNode nodeRoot = diaNode;
			Faction faction = this.relatedFaction;
			string text = this.title;
			windowStack.Add(new Dialog_NodeTreeWithFactionInfo(nodeRoot, faction, false, false, text));
		}

		// Token: 0x06000C9D RID: 3229 RVA: 0x0006F7A0 File Offset: 0x0006DBA0
		public void ExposeData()
		{
			Scribe_Values.Look<string>(ref this.text, "text", null, false);
			Scribe_Values.Look<string>(ref this.title, "title", null, false);
			Scribe_References.Look<Faction>(ref this.relatedFaction, "relatedFaction", false);
			Scribe_Values.Look<int>(ref this.createdTick, "createdTick", 0, false);
		}
	}
}
