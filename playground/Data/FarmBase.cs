using System;

namespace playground.Data;

public class FarmBase
{
    public int Id { get; set; } = default;
    public FarmBase Parent { get; set; } = null;
    public string Name { get; set; } = "Default Name";
    public string ParentId { get; set; }
    public bool HasSubFolder { get; set; }
    public string FolderName { get; set; }
    public bool Expanded { get; set; }
}