using ImageMagick;
using ImageResize.Data;
using ImageResize.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Metrics;

namespace ImageResize.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private ImageResizeDbContext _dbContext;
        public ImagesController(ImageResizeDbContext dbContext) => _dbContext = dbContext;

        //summary: permette l'inserimento di una nuova immagine
        //params:
        //      - imageIn: contiene l'immagine da caricare
        //returns:
        //  BadRequest():
        //      - se non e' stato inserito un nome o ha piu' di 100 caratteri
        //      - se non e' stata caricata un immagine
        //      - se esiste un immagine con lo stesso nome
        //  201: se l'immagine e' stata caricata con successo
        [HttpPost]
        public IActionResult UploadImage([FromForm] Figure imageIn)
        {
            //verifico se e' stato inserito un nome per l'immagine e se la sua dimensione e' minore di 100
            if(imageIn.Name == null || imageIn.Name.Length == 0 || imageIn.Name.Length > 100)
                return BadRequest("Necessario inserire un nome con un numero di caratteri inferiore a 100");

            //controllo se esiste un'immagine nella richiesta
            if(imageIn.realFigure == null)
                return BadRequest("Necessario caricare un immagine");

            try
            {
                //verifico se il nome e' stato gia' utilizzato
                var imageDb = _dbContext.Figures.Where(u => u.Name == imageIn.Name).SingleOrDefault();
                if(imageDb != null)
                    return BadRequest("Nome gia' utilizzato");
            
                //creo un path per la memorizzazione dell'immagine in wwwroot
                var newPath = Path.Combine("wwwroot", imageIn.Name + ".jpg");


                //creo un nuovo file
                var fileStream = new FileStream(newPath, FileMode.Create);
                //copio l'immagine nel nuovo file
                imageIn.realFigure.CopyTo(fileStream);
                //chiudo il file
                fileStream.Close();

                //memorizzo l'url dell'immagine senza wwwroot
                imageIn.ImageUrl = newPath.Remove(0, 8);
            
                //memorizzo l'immagine all'interno del database
                _dbContext.Figures.Add(imageIn);
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return StatusCode(StatusCodes.Status201Created);
        }

        //summary:  restituisce i nomi delle immagini memorizzate
        //              in ordine alfabetico
        //returns:
        //          - NotFound() se non esistono immagini;
        //          - Ok(imagesList) altrimenti.
        [HttpGet]
        public IActionResult ListImages()
        {
            try 
            { 
                //crea la lista delle immagini da restituire
                var imagesList =
                (
                    from images in _dbContext.Figures
                    orderby images.Name
                    select images.Name
                );

                //controllo se esistono immagini
                if(imagesList == null)
                    return NotFound("Nessuna immagine");

                return Ok(imagesList);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        //summary: Elimina una immagine relativa a name
        //params:
        //          - name: Contiene il nome dell'immagine da eliminare
        //returns:
        //          - BadRequest():   Se name e' null
        //          - NotFound():     Se l'immagine non e' stata trovata
        //          - Ok():           Se l'immagine e' stata eliminata
        [HttpDelete("{name}")]
        public IActionResult DeleteImage(string name)
        {
            //controllo se e' stato inserito un nome, non dovrebbe capitare
            if(name == null)
                return BadRequest("Necessario inserire il nome dell'immagine da eliminare");

            //controllo se il nome esiste
            try
            {
                var imageDb = _dbContext.Figures.Where(u => u.Name == name).SingleOrDefault();
                if (imageDb == null || imageDb.ImageUrl == null)
                    return NotFound("Immagine non trovata");

                //elimino l'immagine contenuta in wwwroot
                var imagePath = Path.Combine("wwwroot", imageDb.ImageUrl);
                //var imagePath = "wwwroot/" + imageDb.ImageUrl;
                if (!System.IO.File.Exists(imagePath))
                    NotFound("Immagine non trovata");

                System.IO.File.Delete(imagePath);

                //Rimuovo i metadati relativi dal database
                _dbContext.Figures.Remove(imageDb);
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return Ok("Immagine rimossa correttamente");
        }

        //summary: Permette la modifica della dimensione di una specifica immagine
        //params:
        //          - name:     Nome dell'immagine che si vuole modificare
        //          - width:    Rappresenta la nuova larghezza dell'immagine
        //          - height:   Rappresenta la nuova altezza dell'immagine
        //returns:
        //          - BadRequest():   se name == null o width < 1 o height < 1
        //          - NoFound():      se l'immagine non e' stata trovata
        //          - Ok():           se l'immagine e' stata modificata con successo
        [HttpPut("{name}")]
        public IActionResult ResizeImage(string name, int width, int height)
        {
            //controllo dei parametri di ingresso
            if (name == null || width < 1 || height < 1)
                return BadRequest("Parametri non sono inseriti corretamente");
            try
            {
                //verifico l'esistenza dell'immagine
                var imageDb = _dbContext.Figures.Where(u => u.Name == name).SingleOrDefault();
                if (imageDb == null || imageDb.Name == null) 
                    return NotFound("Immagine non trovata");

                var imagePath = Path.Combine("wwwroot", imageDb.Name + ".jpg");

                //Ottengo il nome completo del file
                var f = new FileInfo(imagePath);
                var fullName = f.FullName;

                // Source: https://github.com/dlemstra/Magick.NET/blob/main/docs/ResizeImage.md
                // Read from file
                using (var image = new MagickImage(fullName))
                {
                    var size = new MagickGeometry(100, 100);
                    // This will resize the image to a fixed size without maintaining the aspect ratio.
                    // Normally an image will be resized to fit inside the specified size.
                    size.IgnoreAspectRatio = true;

                    image.Resize(size);

                    // Save the result
                    image.Write(fullName);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return Ok("Immagine modificata correttamente");
        }
    }
}
