using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020002F4 RID: 756
	public class ArchivedDialog : IArchivable, IExposable
	{
		// Token: 0x06000C90 RID: 3216 RVA: 0x0006F442 File Offset: 0x0006D842
		public ArchivedDialog()
		{
		}

		// Token: 0x06000C91 RID: 3217 RVA: 0x0006F44B File Offset: 0x0006D84B
		public ArchivedDialog(string text, string title = null, Faction relatedFaction = null)
		{
			this.text = text;
			this.title = title;
			this.relatedFaction = relatedFaction;
			this.createdTick = GenTicks.TicksGame;
		}

		// Token: 0x170001DE RID: 478
		// (get) Token: 0x06000C92 RID: 3218 RVA: 0x0006F474 File Offset: 0x0006D874
		Texture IArchivable.ArchivedIcon
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170001DF RID: 479
		// (get) Token: 0x06000C93 RID: 3219 RVA: 0x0006F48C File Offset: 0x0006D88C
		Color IArchivable.ArchivedIconColor
		{
			get
			{
				return Color.white;
			}
		}

		// Token: 0x170001E0 RID: 480
		// (get) Token: 0x06000C94 RID: 3220 RVA: 0x0006F4A8 File Offset: 0x0006D8A8
		string IArchivable.ArchivedLabel
		{
			get
			{
				return this.text.Flatten();
			}
		}

		// Token: 0x170001E1 RID: 481
		// (get) Token: 0x06000C95 RID: 3221 RVA: 0x0006F4C8 File Offset: 0x0006D8C8
		string IArchivable.ArchivedTooltip
		{
			get
			{
				return this.text;
			}
		}

		// Token: 0x170001E2 RID: 482
		// (get) Token: 0x06000C96 RID: 3222 RVA: 0x0006F4E4 File Offset: 0x0006D8E4
		int IArchivable.CreatedTicksGame
		{
			get
			{
				return this.createdTick;
			}
		}

		// Token: 0x170001E3 RID: 483
		// (get) Token: 0x06000C97 RID: 3223 RVA: 0x0006F500 File Offset: 0x0006D900
		bool IArchivable.CanCullArchivedNow
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170001E4 RID: 484
		// (get) Token: 0x06000C98 RID: 3224 RVA: 0x0006F518 File Offset: 0x0006D918
		LookTargets IArchivable.LookTargets
		{
			get
			{
				return null;
			}
		}

		// Token: 0x06000C99 RID: 3225 RVA: 0x0006F530 File Offset: 0x0006D930
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

		// Token: 0x06000C9A RID: 3226 RVA: 0x0006F594 File Offset: 0x0006D994
		public void ExposeData()
		{
			Scribe_Values.Look<string>(ref this.text, "text", null, false);
			Scribe_Values.Look<string>(ref this.title, "title", null, false);
			Scribe_References.Look<Faction>(ref this.relatedFaction, "relatedFaction", false);
			Scribe_Values.Look<int>(ref this.createdTick, "createdTick", 0, false);
		}

		// Token: 0x04000836 RID: 2102
		public string text;

		// Token: 0x04000837 RID: 2103
		public string title;

		// Token: 0x04000838 RID: 2104
		public Faction relatedFaction;

		// Token: 0x04000839 RID: 2105
		public int createdTick;
	}
}
