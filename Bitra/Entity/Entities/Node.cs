namespace Entitiy.Entites
{
    public class Node
    {
        public string ConnectionId { get; set; }
        public string? IpAddress { get; set; }
        public DateTime ConnectedAt { get; set; }
        public DateTime LastActivity { get; set; }
    }
}
