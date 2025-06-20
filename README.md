# Titre & Description

**Nom du projet** : Info‑Académie – Backend

**Description** :  
API développée avec **ASP.NET Core (.NET 7)** pour servir de backend à l'application Info‑Académie. Elle gère l’**authentification**, la **gestion des utilisateurs**, des **réunions** et d’autres entités liées à la plateforme. Ce backend fournit une API RESTful sécurisée et modulaire, conçue pour une application éducative complète.

---

## Prérequis

- **.NET 7 SDK** : nécessaire pour compiler et exécuter le projet. Télécharger sur [dotnet.microsoft.com](https://dotnet.microsoft.com/).
- **Entity Framework Core** : intégré au projet pour gérer les migrations et la base de données.
- **SQL Server** ou autre SGBD compatible avec EF Core.
- **Git** : pour cloner le dépôt.

---

## Configuration

**Cloner le projet** :
```bash
git clone https://github.com/soukaina203/info-back.git
cd info-back
```
**Installation des dependances**
```bash
dotnet restore
```
## Configurer la base de données :
Modifier le fichier appsettings.json :
```bash
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=InfoAcademieDb;User Id=sa;Password=YourPassword;"
}
```
## creation de fichier .env 
ou vous definissez ces variables : 
# .env

## for database credentials
DefaultConnection="Server=localhost;Port=5432;Database=infoAcademie;Username=postgres;Password=pwd"

## JWT credentials

JWT_ISSUER=https://app.com
JWT_AUDIENCE=https://app.com
JWT_SECRET=codeSecret=

## CORS
AllowedOrigins=http://localhost:4200


SMTP_HOST=smtp.gmail.com
SMTP_PORT=587
SenderName=Info-academie Support
SenderEmail=votreEmail

SMTP_USER=votreEmail
SMTP_PASS=codeApplication


## Generer les migrations pour la base de donnees : 
dotnet ef migrations add "First Migration"
dotnet ef migrations script -o Migrations/sample.sql
puis un fichier sample.sql est generer vous prenez le requettes generers et vous les inserer dans votre SGBD et les executer

## Lancement 
executer cette fonction : 
dotnet watch run

Accès à l’interface Swagger :
http://localhost:5000/swagger

### Configuration de la base de données

Concernant la base de données, le projet utilise PostgreSQL , mais  vous pouvez utiliser n’importe quel SGBD relationnel.

Dans le fichier `.env`, remplacez la ligne suivante par vos propres identifiants de connexion :

```
env
DefaultConnection="Server=localhost;Port=5432;Database=infoAcademie;Username=postgres;Password=maRaNV"

```

---

### Génération des migrations

Une fois la configuration terminée, il faut générer les fichiers de migration avec la commande suivante :

```bash

dotnet ef migrations add InitialCreate
```

Cela va créer un dossier `Migrations/` avec les fichiers nécessaires pour construire la base de données.

---

### Génération du script SQL

Pour obtenir un script SQL complet à exécuter dans votre SGBD, utilisez la commande suivante :

```bash
dotnet ef migrations script -o Migrations/sample.sql
```

Vous trouverez ensuite le fichier `sample.sql` dans le dossier `Migrations`. Il vous suffit de copier-coller ce code dans votre outil de gestion de base de données (comme pgAdmin, etc.) et d’exécuter les requêtes.

---

### Lancer l'application

Une fois la base de données prête, tout est en place pour tester l’application.

Dans le dossier backend, lancez le projet avec la commande suivante :

```bash
dotnet watch run
```

Et voilà, l’API est maintenant opérationnelle

## Fonctionnalités principales
- **Authentification avec JWT**

- **Création, mise à jour, suppression d’utilisateurs**

- **Gestion des réunions**

- **Envoi d’e-mails via Razor templates**

- **Gestion des rôles (admin, utilisateur, etc.)**

- **API RESTful sécurisée**

- **Gestion de fichiers statiques (CV, pièces jointes, etc.)**

- **Architecture modulaire avec services et interfaces**

## Structure du projet

L'application suit une architecture modulaire et orientée services. Voici les principaux dossiers :

**/Controllers/** : Contient les endpoints API pour chaque ressource (User, Reunion, etc.)

**/Services/** : Logique métier principale

**/Models/** : Modèles de données (entités) et le DbContext

**/Dtos/** : Objets de transfert utilisés entre client et serveur

**/Interfaces/** : Interfaces associées aux services

**/Enums/** : Énumérations utilisées dans les modèles et la logique

**/Utilities/** : Fonctions utilitaires (ex : génération de tokens)

**/Templates/Emails/** : Templates Razor pour les mails envoyés

**/Data/** : Initialisation et seed de la base de données

**/Migrations/** : Dossiers générés par EF Core pour la gestion du schéma

**/wwwroot/** : Fichiers statiques (ex : documents uploadés)

Fichiers principaux :

**Program.cs** : Point d’entrée de l’application, configuration des services

**appsettings.json** : Configuration globale (connexion BD, JWT, etc.)

**info-back.csproj** : Fichier de projet .NET

**info-back.sln** : Fichier de solution Visual Studio

## Choix technologiques
**ASP.NET Core (.NET 7)** : Framework backend moderne, robuste et performant

**Entity Framework Core** : ORM pour gérer les entités et la base de données

**JWT (JSON Web Token)** : Authentification sécurisée basée sur tokens

**Swagger** : Documentation interactive de l’API

**Architecture orientée services** : séparation claire entre la logique métier, les contrôleurs et les interfaces


