using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BaseNKatmanProject.Core.Entities
{
    [Table("Logs")]
    public class SerilogLog
    {
        [Key]
        public int Id { get; set; }

        public string? Message { get; set; }
        public string? MessageTemplate { get; set; }
        public string? Level { get; set; }
        public DateTimeOffset TimeStamp { get; set; }
        public string? Exception { get; set; }
        public string? Properties { get; set; }
        public byte[]? LogEvent { get; set; }

        public string? IpAddress { get; set; }
        public string? City { get; set; }
        public string? UserName { get; set; }

        public Guid? UserId { get; set; }
        public string? RequestPath { get; set; }
        public string? HttpMethod { get; set; }
        public int? StatusCode { get; set; }
        public string? MachineName { get; set; }
        public string? UserAgent { get; set; }
        public string? CorrelationId { get; set; }
    }
}
