# API Endpoints
## UploadImage()

permette l'inserimento di una nuova immagine

## ListImages()

restituisce le immagini memorizzate

## DeleteImage()

elimina una specifica immagine

## ResizeImage()

modifica le dimensioni di una immagine


# Piccola spiegazione dell'architettura del progetto

Per poter garantire a chiunque di caricare/eliminare/modificare immagini è necessario avere:
	
### Tabella Figures:

Descrizione: Contiene i metadata relativi a una immagine

#### Attributi:

* Id: Chiave identificativa dell'immagine;
* Name: Assegnato durante il caricamento, non possono sussistere nomi equivalenti
* ImageURL: Contiene il path dell'immagine;

*Se possono sussistere piu' nomi equivalenti*:

* Non sarebbe possibile determinare quale immagine restituire al momento della ricerca;
* Potrei implementare un attributo NomeReale contenente un nome univoco per l'immagine ma sarebbe necessario implementare per la gestione degli utenti.

		
###	Cartella wwwroot: 
Contenitore delle immagini.
Non memorizzo le immagini all'interno del database per motivi prestazionali.

# Librearie Aggiunte

## System.ComponentModel.DataAnnotations.Schema;

Contiene la parola chiave NotMapped, permette di NON memorizzare 
l'immagine all'interno della
tabella Images.

## Microsoft.EntityFrameworkCore

Contiene la classe DbContext, permette la realizzazione di un database applicando il concetto di code first approach.

## Microsoft.EntityFrameworkCore.SqlServer

Contiene il servisio AddDbContext

## Microsoft.EntityFrameworkCore.Tools

Contiene gli strumenti per poter effettuare Migration

## System.ComponentModel.DataAnnotations

Contiene la parola chiave Required, i parametri con questa parola chiave non possono essere nulli

## Magick.NET-Q16-AnyCPU

Contiene i metodi per la modifica delle immagini
