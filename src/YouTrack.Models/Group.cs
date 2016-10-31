namespace YouTrack.Models
{
    public class Group
    {
        public string Name { get; set; }
        public bool AutoJoin { get; set; }

        public string Description { get; set; }

        public string RolesUrl { get; set; }
    }
}