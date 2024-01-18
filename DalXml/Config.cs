namespace Dal;

internal static class Config
{
    static string s_data_config_xml = "data-config";
    internal static int NextCoursenextDependencyIdId { get => XMLTools.GetAndIncreaseNextId(s_data_config_xml, "nextDependencyId"); }
    internal static int nextTaskId { get => XMLTools.GetAndIncreaseNextId(s_data_config_xml, "nextTaskId"); }
}


}
