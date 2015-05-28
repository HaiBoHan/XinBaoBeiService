using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.CompilerServices;


public class TableTypeParameter
{
    //public string ParameterName { get; set; }

    private string parameterName;

    public string ParameterName
    {
        get { return parameterName; }
        set { parameterName = value; }
    }
	

    //public object ParameterValue { get; set; }

    private object parameterValue;

    public object ParameterValue
    {
        get { return parameterValue; }
        set { parameterValue = value; }
    }


    public TableTypeParameter()
    {
    }

    public TableTypeParameter(string name, object value)
    {
        ParameterName = name;
        ParameterValue = value;
    }
}
