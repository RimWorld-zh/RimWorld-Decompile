using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000EC4 RID: 3780
	public static class DialogDatabase
	{
		// Token: 0x0600596E RID: 22894 RVA: 0x002DD718 File Offset: 0x002DBB18
		static DialogDatabase()
		{
			DialogDatabase.LoadAllDialog();
		}

		// Token: 0x0600596F RID: 22895 RVA: 0x002DD734 File Offset: 0x002DBB34
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

		// Token: 0x06005970 RID: 22896 RVA: 0x002DD86C File Offset: 0x002DBC6C
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

		// Token: 0x06005971 RID: 22897 RVA: 0x002DD90C File Offset: 0x002DBD0C
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

		// Token: 0x04003BB8 RID: 15288
		private static List<DiaNodeMold> Nodes = new List<DiaNodeMold>();

		// Token: 0x04003BB9 RID: 15289
		private static List<DiaNodeList> NodeLists = new List<DiaNodeList>();
	}
}
