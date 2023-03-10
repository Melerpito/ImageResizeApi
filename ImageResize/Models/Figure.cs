using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ImageResize.Models
{
    public class Figure
    {
        public int Id { get; set; }                 //Codice univoco rappresentante l'immagine
        [Required]
        public string? Name { get; set; }           //Nome assegnato all'immagine dall'utente
        public string? ImageUrl { get; set; }       //Path dell'immagine

        [Required]
        [NotMapped]
        public IFormFile? realFigure { get; set; }   //Contiene l'immagine, non e' memorizzato nel database
    }
}
