namespace Hooome.Domain;

public class CompanyImage
{
    public Guid Id { get; set; }
    public string FileName { get; set; } = null!;
    public string FilePath { get; set; } = null!;
    public long FileSize { get; set; }
    public bool IsMain { get; set; }

    public Guid CompanyId { get; set; }

    public Company Company { get; set; } = null!;
}