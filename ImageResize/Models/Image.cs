using System.ComponentModel.DataAnnotations.Schema;

namespace ImageResize.Models
{
    public class Image
    {
        public int Id { get; set; }                 //Codice univoco rappresentante l'immagine
        public string? Name { get; set; }           //Nome assegnato all'immagine dall'utente
        public string? ImageUrl { get; set; }       //Path dell'immagine
        public int Width { get; set; }              //Larghezza
        public int Height { get; set; }             //Altezza

        [NotMapped]
        public FormFile? realFigure { get; set; }   //Contiene l'immagine, non e' memorizzato nel database
    }
}
