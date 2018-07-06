using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SickscoreGames.HUDNavigationSystem; // MANDATORY !!

public class ExampleCallbackScript : MonoBehaviour
{
	#region Example Methods
	public void OnElementAppeared (NavigationElementType type)
	{
//		Debug.LogFormat ("{0} element appeared.", type);
	}


	public void OnElementDisappeared (NavigationElementType type)
	{
//		Debug.LogFormat ("{0} element disappeared.", type);
	}


	public void OnElementEnterRadius (NavigationElementType type)
	{
//		Debug.LogFormat ("{0} element entered radius.", type);
	}


	public void OnElementLeaveRadius (NavigationElementType type)
	{
//		Debug.LogFormat ("{0} element left radius.", type);
	}
	#endregion
}
