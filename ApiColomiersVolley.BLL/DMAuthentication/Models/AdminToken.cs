namespace ApiColomiersVolley.BLL.DMAuthentication.Models
{
    public class AdminToken
    {
        public string id_token { get; set; }
        public int IdAdmin { get; set; }
        public string? refresh_token { get; set; }
        public DateTime? expire_in { get; set; }
    }
}
