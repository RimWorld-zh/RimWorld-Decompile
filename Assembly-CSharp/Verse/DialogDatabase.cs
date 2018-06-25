using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000EC7 RID: 3783
	public static class DialogDatabase
	{
		// Token: 0x04003BC0 RID: 15296
		private static List<DiaNodeMold> Nodes = new List<DiaNodeMold>();

		// Token: 0x04003BC1 RID: 15297
		private static List<DiaNodeList> NodeLists = new List<DiaNodeList>();

		// Token: 0x06005971 RID: 22897 RVA: 0x002DDA24 File Offset: 0x002DBE24
		static DialogDatabase()
		{
			DialogDatabase.LoadAllDialog();
		}

		// Token: 0x06005972 RID: 22898 RVA: 0x002DDA40 File Offset: 0x002DBE40
		private static void LoadAllDialog()
		{
			DialogDatabase.Nodes.Clear();
			UnityEngine.Object[] array = Resources.LoadAll("Dialog", typeof(TextAsset));
			foreach (UnityEngine.Object @object in array)
			{
				TextAsset ass = @object as TextAsset;
				if (@object.name == "BaseEncounters" || @object.name == "GeneratedDialogs")
				{
					LayerLoader.LoadFileIntoList(ass, DialogDatabase.Nodes, DialogDatabase.NodeLists, DiaNodeType.BaseEncounters);
				}
				if (@object.name == "InsanityBattles")
				{
					LayerLoader.LoadFileIntoList(ass, DialogDatabase.Nodes, DialogDatabase.NodeLists, DiaNodeType.InsanityBattles);
				}
				if (@object.name == "SpecialEncounters")
				{
					LayerLoader.LoadFileIntoList(ass, DialogDatabase.Nodes, DialogDatabase.NodeLists, DiaNodeType.Special);
				}
			}
			foreach (DiaNodeMold diaNodeMold in DialogDatabase.Nodes)
			{
				diaNodeMold.PostLoad();
			}
			LayerLoader.MarkNonRootNodes(DialogDatabase.Nodes);
		}

		// Token: 0x06005973 RID: 22899 RVA: 0x002DDB78 File Offset: 0x002DBF78
		public static DiaNodeMold GetRandomEncounterRootNode(DiaNodeType NType)
		{
			List<DiaNodeMold> list = new List<DiaNodeMold>();
			foreach (DiaNodeMold diaNodeMold in DialogDatabase.Nodes)
			{
				if (diaNodeMold.isRoot && (!diaNodeMold.unique || !diaNodeMold.used) && diaNodeMold.nodeType == NType)
				{
					list.Add(diaNodeMold);
				}
			}
			return list.RandomElement<DiaNodeMold>();
		}

		// Token: 0x06005974 RID: 22900 RVA: 0x002DDC18 File Offset: 0x002DC018
		public static DiaNodeMold GetNodeNamed(string NodeName)
		{
			foreach (DiaNodeMold diaNodeMold in DialogDatabase.Nodes)
			{
				if (diaNodeMold.name == NodeName)
				{
					return diaNodeMold;
				}
			}
			foreach (DiaNodeList diaNodeList in DialogDatabase.NodeLists)
			{
				if (diaNodeList.Name == NodeName)
				{
					return diaNodeList.RandomNodeFromList();
				}
			}
			Log.Error("Did not find node named '" + NodeName + "'.", false);
			return null;
		}
	}
}
