namespace Entity.Dtos
{
    public class NodeDto
    {
        public string ConnectionId { get; set; }
        public string? IpAddress { get; set; }
        public DateTime ConnectedAt { get; set; }
        public DateTime LastActivity { get; set; }
    }
}
