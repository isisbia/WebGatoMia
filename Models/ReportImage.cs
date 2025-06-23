using System.ComponentModel.DataAnnotations.Schema;

namespace WebGatoMia.Models
{
    [Table("tb_ReportImages")]
    public class ReportImage
    {
        public Guid Id { get; set; }

        public string FileName { get; set; } = string.Empty;

        public string FileType { get; set; } = string.Empty;

        public string FileUrl { get; set; } = string.Empty;

        public Guid ReportId { get; set; }

        public Report Report { get; set; } = null!;

        public ReportImage()
        {
            Id = Guid.NewGuid();
        }

    }
}
