
namespace BO;
/// <summary>
/// 
/// </summary>
public class TaskInList
{
    /// <summary>
    /// 
    /// </summary>
    public int Id { get; init; }
    /// <summary>
    /// 
    /// </summary>
    public string? Description { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string? Alias { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public Statuses Status { get; set; }
    //public override string? ToString() => this.ToStringProperty();
}