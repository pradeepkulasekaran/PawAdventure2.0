using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class Extensions 
{

	public static Dictionary<string, object> ToDictionary(this object myObj)
    {
        return myObj.GetType()
            .GetProperties()
            .Select(pi => new { Name = pi.Name, Value = pi.GetValue(myObj, null) })
            .Union( 
                myObj.GetType()
                .GetFields()
                .Select(fi => new { Name = fi.Name, Value = fi.GetValue(myObj) })
             )
            .ToDictionary(ks => ks.Name, vs => vs.Value);
    }
	 
}
