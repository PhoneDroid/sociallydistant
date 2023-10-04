﻿#nullable enable
using Core;
using Core.WorldData.Data;
using Social;
using UnityEngine.Analytics;

namespace GameplaySystems.Social
{
	public class Profile : IProfile
	{
		private readonly SocialService socialService;
		private WorldProfileData data;

		public Profile(SocialService socialService)
		{
			this.socialService = socialService;
		}

		internal void SetData(WorldProfileData newData)
		{
			this.data = newData;
		}

		/// <inheritdoc />
		public ObjectId ProfileId => data.InstanceId;

		/// <inheritdoc />
		public Gender Gender => data.Gender;

		/// <inheritdoc />
		public string Bio => data.SocialBio;

		/// <inheritdoc />
		public bool IsPrivate => data.IsSocialPrivate;
	}
}