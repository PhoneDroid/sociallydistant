﻿using System.Collections.Generic;
using System.Linq;
using Core;
using Core.DataManagement;
using Core.WorldData.Data;
using GameplaySystems.NonPlayerComputers;
using OS.FileSystems;
using Player;
using UI.Widgets;
using UnityEngine;
using Utility;

namespace GameplaySystems.Hacking
{
	public class HackingSystem : MonoBehaviour
	{
		private readonly Dictionary<ObjectId, CraftedExploitFile> craftedExploits = new Dictionary<ObjectId, CraftedExploitFile>();

		private ExploitAsset[] exploits;
		private PayloadAsset[] payloads;
		private NonPlayerComputerEventListener npcComputers;

		[Header("Dependencies")]
		[SerializeField]
		private PlayerInstanceHolder playerInstance = null!;

		[SerializeField]
		private WorldManagerHolder world = null!;
		
		[Header("Settings")]
		[SerializeField]
		private string exploitsResourcePath = "Exploits";

		[SerializeField]
		private string payloadsResourcePath = "Payloads";

		public IEnumerable<ExploitAsset> Exploits => exploits;
		public IEnumerable<PayloadAsset> Payloads => payloads;

		private void Awake()
		{
			this.AssertAllFieldsAreSerialized(typeof(HackingSystem));
			
			UnityHelpers.MustFindObjectOfType(out npcComputers);
		}

		private void Start()
		{
			exploits = Resources.LoadAll<ExploitAsset>(exploitsResourcePath);
			payloads = Resources.LoadAll<PayloadAsset>(payloadsResourcePath);

			world.Value.Callbacks.AddCreateCallback<WorldCraftedExploitData>(OnCraftedExploitCreated);
			world.Value.Callbacks.AddModifyCallback<WorldCraftedExploitData>(OnCraftedExploitModified);
			world.Value.Callbacks.AddDeleteCallback<WorldCraftedExploitData>(OnCraftedExploitDeleted);
		}

		private void OnCraftedExploitDeleted(WorldCraftedExploitData subject)
		{
			if (!craftedExploits.TryGetValue(subject.InstanceId, out CraftedExploitFile file))
				return;
			
			IFileOverrider overrider = GetFileOverrider(subject.Computer);
			string[] path = PathUtility.Split(subject.FilePath);

			overrider.RemoveFile(path, file);
			craftedExploits.Remove(subject.InstanceId);
		}

		private void OnCraftedExploitModified(WorldCraftedExploitData subjectprevious, WorldCraftedExploitData subjectnew)
		{
		}

		private void OnCraftedExploitCreated(WorldCraftedExploitData subject)
		{
			IFileOverrider overrider = GetFileOverrider(subject.Computer);
			string directoryName = PathUtility.GetDirectoryName(subject.FilePath);
			string[] directory = PathUtility.Split(directoryName);

			if (!craftedExploits.TryGetValue(subject.InstanceId, out CraftedExploitFile file))
			{
				file = new CraftedExploitFile();
				craftedExploits.Add(subject.InstanceId, file);
			}

			overrider.AddFile(directory, file);
			UpdateInstance(subject, file);
		}

		private void UpdateInstance(WorldCraftedExploitData subject, CraftedExploitFile file)
		{
			string filename = PathUtility.GetFileName(subject.FilePath);

			file.Name = filename;
			file.Exploit = Exploits.First(x => x.Name == subject.Exploit);
			file.Payload = Payloads.FirstOrDefault(x => x.Name == subject.Exploit);
		}

		private IFileOverrider GetFileOverrider(ObjectId computer)
		{
			if (computer == ObjectId.Invalid)
				return playerInstance.Value.FileOverrider;
			else
				return npcComputers.GetNpcFileOverrider(computer);
		}
	}
}