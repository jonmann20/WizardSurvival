using UnityEngine;
using System.Collections;

namespace MGSpawn.Spawn
{
	public enum SpawnModes
	{
		/// <summary>
		/// This will continually spawn units until the limit, then spawn whenever a unit dies.
		/// Use this mode with a trigger that disables the spawner.
		/// </summary>
		Continuous,
		Once,
		Wave
	}
	public enum SpawnMemberType
	{

		Skeleton_Basic = 0,
		Skeleton_Spear,
		Skeleton_Mage,
		Ice_Golem,
		Daemon
	}
}
