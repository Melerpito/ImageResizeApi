# API Endpoints
## UploadImage()
## ListImage()
## DeleteImage()
## ResizeImage()

# Piccola spiegazione dell'architettura del progetto
Per poter impelementare una HTTP API per poter garantire a chiunque
	di caricare/eliminare/modificare immagini è necessario avere:
	
	Tabella Images:
		Descrizione: Contiene i metadata relativi a una immagine
		Attributi:
			- Id: 		Chiave identificativa dell'immagine;
			- Name: 	Assegnato durante il caricamento,
					Non possono sussistere nomi equivalenti*
			- ImageURL: 	Contiene il path dell'immagine;
			- Width:	Larghezza dell'immagine;
			- Height:	Altezza dell'immagine.

		*Se possono sussistere piu' nomi equivalenti:
			1) Non sarebbe possibile determinare quale immagine 
				restituire al momento della ricerca;
			2) Potrei implementare un attributo NomeReale 
				contenente un nome univoco per l'immagine 
				ma sarebbe necessario implementare un sistema
				per la gestione degli utenti.

		
	Cartella wwwroot:
		Contenitore delle immagini.
		Non memorizzo le immagini all'interno del database per motivi
			prestazionali.

# Librearie Aggiunte
##	System.ComponentModel.DataAnnotations.Schema;
Per poter utilizzare la parola chiave NotMapped all'interno della classe Image,
permette di NON memorizzare l'immagine all'interno del database
##	Microsoft.EntityFrameworkCore
Per poter utilizzare la classe DbContext che permette la realizzazione di un
database applicando il concetto di code first approach
		
