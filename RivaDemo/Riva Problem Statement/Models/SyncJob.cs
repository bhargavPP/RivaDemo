namespace IntelSyncLegacy.Models;

public class SyncJob
{
    public int Id { get; set; }
    public CrmUser User { get; set; }
    public string ObjectType { get; set; }
    public string Payload { get; set; }
    public DateTime SyncTime { get; set; }
    public string Status { get; set; }
    public string ErrorMessage { get; set; }
}
