# API Endpoints

https://documenter.getpostman.com/view/25136747/2s8ZDd1LKw#intro

# Piccola spiegazione dell'architettura del progetto

Per poter garantire a chiunque di caricare/eliminare/modificare immagini è necessario avere una tabella contenente qualche metadata riguardo l'immagine. Le immagini vere e proprie vengono salvate non all'interno del database ma nella cartella wwwroot per motivi prestazionali.

Inizialmente forse e' meglio sviluppare l'applicazione in maniera sincrona per avere qualcosa di almeno funzionante, solo successivamente provero' a realizzare una queue-base workers.
Per poter realizzare una queue-base workers, prendo ispirazione da questa guida: 

https://blog.elmah.io/async-processing-of-long-running-tasks-in-asp-net-core/
	
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
