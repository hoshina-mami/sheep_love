using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class rollController {


	public bool doAnim (System.Action anim)
	{
		
		anim.Invoke ();

		return true;
	}

}
