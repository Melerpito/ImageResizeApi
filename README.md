# API Endpoints

https://documenter.getpostman.com/view/25136747/2s8ZDd1LKw#intro

# Piccola spiegazione sull'architettura del progetto

Per poter garantire a chiunque di caricare/eliminare/modificare immagini è necessario avere una tabella contenente qualche metadato riguardo l'immagine. Le immagini vere e proprie vengono salvate non all'interno del database ma nella cartella wwwroot per motivi prestazionali. 
Scelgo di implementare un database sql, non e' particolarmente veloce ma e' un modo semplice per mantenere le informazioni relative le immagini anche dopo il riavvio.

Inizialmente ho sviluppato l'applicazione in maniera sincrona per avere qualcosa di almeno funzionante, solo successivamente ho provato ad implementare una queue-base workers.
Dato che non ho esperienza a riguardo, ho preso ispirazione dalla seguente guida:
https://blog.elmah.io/async-processing-of-long-running-tasks-in-asp-net-core/

Non sono purtroppo riuscito ad applicarla, ho notato che il codice che inserirsco all'interno di QueueBackgrounWorkItem non funziona per qualche motivo. Comunque lascio il branch aperto.

Ho realizzato infine un branch equivalente al master(sincrono) ma asincrono, sembra funzionara ma penso che possa presentare problemi nel caso in cui piu' client vadano ad interagire con lo stesso file.

### Tabella Figures:

Descrizione: Contiene i metadata relativi a una immagine

#### Attributi:

* Id: Chiave identificativa dell'immagine;
* Name: Assegnato durante il caricamento, non possono sussistere nomi equivalenti
* ImageURL: Contiene il path dell'immagine;

*Se possono sussistere piu' nomi equivalenti*:

1) Non sarebbe possibile determinare quale immagine restituire al momento della ricerca;
2) Potrei implementare un attributo NomeReale contenente un nome univoco per l'immagine ma sarebbe necessario implementare per la gestione degli utenti.

### Cartella wwwroot: 
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
