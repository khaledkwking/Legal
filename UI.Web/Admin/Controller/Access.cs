using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Diagnostics;

/// <summary>
/// this object to represent page access object
/// </summary>
/// <remarks></remarks>
///
namespace UI.Web.Admin.Controller
{
public class Access
{

	#region "Private Members"
	private bool _show;
	private bool _add;
	private bool _edit;
	private bool _delete;
		#endregion
	private bool _date;

	#region "Public Properties"
	public bool Show {
		get { return _show; }
	}
	public bool Add {
		get { return _add; }
	}
	public bool Edit {
		get { return _edit; }
	}
	public bool Delete {
		get { return _delete; }
	}
	public bool DateControl {
		get { return _date; }
	}
	#endregion

	public Access(string per)
	{
            if (per.IndexOf(",") == -1)
            {
                throw new Exception("Invalid Permissions String: " + per);
            }
            else
            {
                string[] data = per.Split(',');
                _show = getBool(data[0]);
                _add = getBool(data[1]);
                _edit = getBool(data[2]);
                _delete = getBool(data[3]);
                _date = getBool(data[4]);
            }
	}

    public bool getBool(object ch)
    {
        if (object.ReferenceEquals(ch, DBNull.Value))
        {
            return false;
        }
        else
        {
            if (ch.ToString().Equals("0"))
            {
                return Convert.ToBoolean("false");
            }
            else
            { return Convert.ToBoolean("true"); }

        }
    }

}
}