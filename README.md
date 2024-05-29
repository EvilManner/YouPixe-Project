# Plateforme de partage de vidéos - YouPixe

Ce dépôt héberge une application de microservices conçue à l'aide de .NET 7 et d'Angular 14, s'apparentant à une version clonée de la plateforme YouTube.

### Aperçu des technologies utilisées

- ASP.NET Core 7
- Modèles CQRS et DDD
- Entity Framework Core
- MongoDB
- Elasticsearch
- RabbitMQ
- Bus d'événements
- Modèle de boîte d'envoi transactionnel
- Transaction distribuée
- Redis
- Mise en cache distribuée
- SignalR
- Duende identity server
- OAuth 2.0 avec OpenID Connect
- FFmpeg pour le traitement vidéo
- Optimistic et pessimistic concurrency
- Réplication des données entre les services
- YARP reverse proxy
- Journalisation distribuée utilisant Serilog et ELK stack (ElasticSearch, Logstash et Kibana)
- Traçage distribué à l'aide d'OpenTelemetry et Jaeger
- Vérifications de fonctionnement
- Résilience aux pannes
- JSON Web Token
- Prometheus
- Docker
- Angular 14        
        
### Caractéristiques

- Traitement asynchrone pour générer des miniatures d'aperçu et différentes résolutions des vidéos téléchargées
- Téléchargement et gestion de vidéos
- Création et gestion de playlists
- Lecture 
- Commentaires et notes 
- Visibilité des vidéos et des playlists (publiques, non répertoriées, privées)
- Marquage des vidéos
- Recommandations et suggestions 
- Recherche et découverte 
- Authentification et autorisation des utilisateurs
- Connexion externe (Google)
- Profil utilisateur, chaîne et abonnement
- Personnalisation du canal utilisateur (mise en page, vignette, bannière)
- Historique des vidéos regardées par l'utilisateur
- Message de notification des chaînes abonnées
- Flux d'abonnement
- Et plus
    
## Architecture de microservices en .NET

Cette application utilise une architecture de microservices basée sur ASP.NET Core 7, offrant une solution évolutive et flexible. 
Chaque microservice est conçu pour être horizontalement scalable, permettant une allocation granulaire des ressources 
et une utilisation efficace de la puissance de calcul.

### Éléments clés

- Modèle de conception basé sur le domaine (DDD) : Ce modèle permet de clarifier les domaines complexes 
    et d'assurer la cohérence des règles métier.
  
- Bus d'événements RabbitMQ : Ce bus permet une communication asynchrone entre les différents microservices.

- Boîte d'envoi transactionnel : Implémenté pour les couches de persistance d'Entity Framework Core et MongoDB, 
    il garantit une publication de messages fiable et ordonnée.
  
- Gestion des erreurs d'événements d'intégration : Les événements sont remis en file d'attente en cas d'erreurs temporaires 
    et envoyés vers une file d'attente de lettres mortes pour les erreurs irrécupérables.
  
- Routing slip pattern : Utilisé dans les systèmes de bus d'événements comme MassTransit, 
    il permet une cohérence éventuelle dans les transactions distribuées.
  
- Implémentation du modèle d'unité de travail : Permet de valider les modifications en un seul lot 
    transactionnel pour Entity Framework Core et MongoDB.

### Service de gestion des vidéos (Video Manager Service)

Le service de gestion des vidéos joue un rôle crucial dans la plateforme de partage de vidéos, en gérant les vidéos et leurs métadonnées.

- Création de vidéos : Le service de gestion de vidéos gère les requêtes des utilisateurs pour créer une nouvelle vidéo.
  
- Autorisation de téléversement de vidéos : Le service de gestion de vidéos génère des jetons de téléversement qui autorisent
  les utilisateurs à télécharger des fichiers vidéo.
  
- Enregistrement de vidéos : Une fois le fichier vidéo téléchargé, le service de gestion de vidéos enregistre la vidéo auprès d'autres microservices,
  notamment les services de stockage de vidéos, de bibliothèque, de communauté et d'historique.
  
- Déclenchement du traitement vidéo : Une fois la vidéo enregistrée auprès de différents services, le service de gestion de vidéos publie
  un événement d'intégration au service de traitement vidéo (Video Processor Service) pour lancer le traitement de la vidéo.
  
- Notification de fin de traitement vidéo : Une fois la vidéo traitée, le service de gestion de vidéos publie des événements d'intégration
  pour informer les autres microservices de la fin du traitement.
  
- Gestion des métadonnées vidéo : Le service de gestion de vidéos gère et stocke les métadonnées originales des vidéos, telles que le titre, la description,
  la visibilité, les tags, etc. Les métadonnées mises à jour sont synchronisées avec le service de stockage de vidéos pour la diffusion.
  
- Suppression de vidéo : Le service de gestion de vidéos a également la capacité de supprimer une vidéo en réponse aux demandes des utilisateurs.

### Service Gestionnaire de vidéos : SignalR Hub 
SignalR Hub Service enrichit le service de gestion des vidéos en fournissant des notifications en temps réel aux utilisateurs sur l'état de traitement de leurs vidéos.

### Services de la plateforme de partage de vidéos

La plateforme de partage de vidéos est composée de plusieurs microservices qui travaillent ensemble pour offrir une expérience fluide et riche aux utilisateurs. Voici une description de chaque service :

### Service de traitement vidéo (Video Processor Service)

- Utilise FFmpeg pour traiter les fichiers vidéo et générer différentes résolutions et miniatures.

- Gère les interruptions temporaires et effectue des tentatives de retraitement automatique.

- Mise à l'échelle horizontale automatique via Kubernetes en fonction de la charge de travail.

### Service de stockage de vidéos (Video Store Service)

- Stocke et diffuse les métadonnées des vidéos aux utilisateurs.

- Contrôle la publication des vidéos et assure la réplication des métadonnées avec d'autres services.

### Service de stockage (Storage Service)

- Stocke et diffuse les fichiers vidéo et image.
  
- Gère les uploads utilisateurs via des jetons d'autorisation (JWT) contenant des informations de sécurité et de taille maximale.
  
- Retourne des jetons aux utilisateurs pour les images téléchargées, utilisés par d'autres services pour vérifier leur validité.
  
- Publie des événements d'intégration pour le traitement des vidéos.

- Dispose d'un service de nettoyage des fichiers inutilisés.
  
- Offre la possibilité d'intégrer un antivirus et un stockage cloud (Azure Blob Storage par exemple).

### Service d'authentification (Identity Provider Service)

- Gère l'authentification et l'autorisation des utilisateurs via Duende Identity Server.

- Utilise les flux d'autorisation "code d'autorisation" et "凭据客户端 (píngjù kehuduàn)" (client credential flow).

- Peut être remplacé par des solutions d'identité cloud comme Auth0 ou Azure AD.

### Service des Utilisateurs (Users Service)

Gère et stocke les profils utilisateurs et les données de chaîne.

- Gestion des profils utilisateurs (création, stockage, modification) via des transactions distribuées asynchrones.
  
- Gestion des chaînes des utilisateurs (mise en page, bannière, etc.).

### Service d'Abonnements (Subscriptions Service)

Gère les abonnements des utilisateurs et stocke leurs notifications.

- Permet aux utilisateurs de s'abonner aux chaînes de leur choix.

- Stocke les notifications des utilisateurs en relation avec leurs abonnements (base Redis).

### Service de Bibliothèque (Library Service)

Gère les playlists et diffuse les playlists et les métadonnées vidéo de base aux utilisateurs.

- Gère les playlists des utilisateurs (préférées, "à regarder plus tard", personnalisées).
  
- Permet le vote sur les vidéos (via l'ajout aux playlists "aimées" ou "pas aimées").
  
### Service de Communauté (Community Service)

Fournit une plateforme de discussion pour les utilisateurs.

- Commentaire de vidéos (ajout, réponse, modification, suppression).
        
- Vote sur les commentaires (pour identifier les commentaires les plus utiles).

### Service d'Historique (History Service)

- Gère et stocke l'historique de visionnage des utilisateurs et comptabilise le nombre de vues par vidéo.
  
- Utilise Elasticsearch pour un stockage efficace et des analyses de tags favoris.

- Stocke initialement les vues en mémoire (Redis) puis les transfère vers MongoDB.

### Service de recherche (Search Service)

Intégré à Elasticsearch pour offrir des recherches en texte intégral sur les vidéos.

- Recherche en texte intégral (titres, noms de créateurs, mots-clés, etc.)

- Recherche par tags

- Recherche par créateur

- Classement par pertinence basé sur les vues et les mentions positives

- Recherche de tags tendance et pertinents via Elasticsearch
        
### Service de passerelle d'API (API Gateway Service)

- Point d'entrée centralisé pour les requêtes client, les redirigeant vers les services backend appropriés (sauf Stockage et Fournisseur d'identité).
  
- Utilise YARP pour la répartition de la charge et la haute disponibilité.
  
- Applique une limitation du débit pour protéger les services backend contre un trafic excessif.

### Service de surveillance web (Web Status Service)

- Fournit une interface web pour effectuer des contrôles d'intégrité de la plateforme.
  
- Pour la découverte automatique des services Kubernetes, référez-vous à la documentation :
  https://github.com/Xabaril/AspNetCore.Diagnostics.HealthChecks/blob/master/doc/k8s-ui-discovery.md

### Journalisation distribuée (Distributed Logging)

- La plateforme utilise ELK (Elasticsearch, Logstash et Kibana) avec Serilog pour stocker, traiter et analyser les journaux d'activité.
  
- Toutes les données de journalisation sont collectées et stockées dans une base de données centralisée Elasticsearch, facilitant la recherche et l'analyse des journaux provenant de tous les composants du système.

### Traçage distribué (Distributed Tracing)

OpenTelemetry est utilisé pour instrumenter les microservices, générer des données de traçage et les exporter vers Jaeger. Cela permet d'obtenir une meilleure visibilité sur les performances et le comportement de l'application.

### Dépendances

- Autofac
- AutoMapper
- AspNetCore.Diagnostics.HealthChecks
- Duende.IdentityServer
- FluentValidation
- IdentityModel
- MediatR
- Microsoft.EntityFrameworkCore
- MongoDB.Driver
- NEST
- Newtonsoft.Json
- Npgsql.EntityFrameworkCore.PostgreSQL
- OpenTelemetry
- Polly
- RabbitMQ.Client
- Serilog
- SixLabors.ImageSharp
- Yarp.ReverseProxy

### Requis

- FFmpeg

### Application SPA (Angular)

L'application SPA (Single Page Application) est développée avec Angular, Angular Material, NgRx et Videogular. Ces technologies permettent la création d'interfaces utilisateur responsives, offrant une expérience utilisateur fluide.

### Dépendances

- AppAuth
- Angular Material
- Color Thief
- Microsoft SignalR Client
- Moment.js
- NgRx
- Ngx Scrollbar
- Ngx Videogular
- Swiper

## Exécuter avec Docker

Pour exécuter l'application et son infrastructure nécessaire, assurez-vous que Docker et Docker Compose sont installés sur votre ordinateur.

Accédez au répertoire **Scripts** et utilisez les commandes suivantes :

##### Windows

```
backend_build.bat
backend_up.bat

front_build.bat
front_up.bat
```

##### Linux

```
./backend_build.sh
./backend_up.sh

./front_build.sh
./front_up.sh
```

Les scripts lanceront 26 conteneurs, dont le client Web, les microservices et l'infrastructure.

Une fois l'application complètement lancée, vous devriez pouvoir parcourir les différents composants en visitant

```
Web SPA : http://localhost:4200/
Web Status : http://localhost:16050/
Kibana : http://localhost:5601/
Jaeger UI : http://localhost:16686/
```

```
Cette application utilise les ports suivants :

    PostgreSQL: 5432
    Adminer: 8080
    PgAdmin: 8081
    RabbitMQ: 5672
    RabbitMQ Management: 15672
    MongoDB: 27017
    MongoExpress: 8082
    Elasticsearch: 9200
    Logstash: 9600, 9250
    Kibana: 5601
    Redis: 6379
    RedisCommander: 8083
    Jaeger: 5565/udp, 6831/udp, 6832/udp, 5778, 14268, 14250, 9411
    Jaeger UI: 16686

    Storage: 14200
    VideoManager: 15000
    VideoManagerSignalRHub: 15050
    IdentityProvider: 15100
    VideoProcessor: 15200
    VideoStore: 15300
    Users: 15400
    Community: 15500
    Library: 15600
    Search: 15700
    Subscriptions: 15800
    History: 15900
    APIGateway: 16000
    WebStatus: 16050

    WebClient: 4200
```
