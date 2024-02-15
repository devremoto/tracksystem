namespace Models;

public class TrackRequest
{
    public string? Referrer { get; set; }
    public string? UserAgent { get; set; }
    public string? IpAddress { get; set; }
}