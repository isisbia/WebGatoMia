using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace WebGatoMia.Models
{
    [Table("tb_Reports")]
    public class Report
    {
        public Guid Id { get; set; }
        public string Description { get; set; } = string.Empty; //Descrição da denúncia é armazenada aqui

        public double? Latitude { get; set; }

        public double? Longitude { get; set; }

        //A latidude e longitude estão como opcionais, pois caso a denúncia seja anônima, a pessoa pode não querer colocar esses dados

        public Guid? IdUser { get; set; } //Não é obrigatório caso a denúncia seja anônima

        public User User { get; set; }

        public List<ReportImage> ReportImages { get; set; } = new List<ReportImage>();

        public Report()
        {
            Id = Guid.NewGuid();
        }
    }
}
