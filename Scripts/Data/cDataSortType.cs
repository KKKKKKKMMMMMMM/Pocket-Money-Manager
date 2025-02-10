using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KMUtils.Data.SortType
{
    public enum SortType
    {
        Date,
        Calender,
    }

    public enum DateType
    {
        Today,
        Month1,
        Month3,
        Month6,
        All,
    }

    public enum InOutType
    {
        Total,
        Input,
        Output,
    }

    public enum OrderType
    {
        Date,
        Money,
    }
}