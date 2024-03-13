using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO;

public class TaskGantt
{
    public static DateTime StartTime { set; get; }
    public static DateTime EndTime { set; get; }
    public int TaskId { set; get; }
    public string? TaskAlias { set; get; }
    public DateTime TaskStart { set; get; }
    public DateTime TaskEnd { set; get; }
    public int Duration { set; get; }
    public int TimeFromStart { set; get; }
    public int TimeToEnd { set; get; }
}