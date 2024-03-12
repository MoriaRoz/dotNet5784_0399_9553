namespace Dal;

internal static class Config
{
    static string s_data_config_xml = "data-config";
    public static DateTime? ProjectStartDate { get; set; } = null;
    public static int ProjectStatus { get; set; } = 0;
    internal static int NextDependencyId { get => XMLTools.GetAndIncreaseNextId(s_data_config_xml, "NextDependencyId");}
    internal static int NextTaskId { get => XMLTools.GetAndIncreaseNextId(s_data_config_xml, "NextTaskId"); }
}